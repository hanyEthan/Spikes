using System;
using System.Collections.Generic;
using System.Text;
using GreenPipes;
using MassTransit;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAsyncCommunication(this IServiceCollection services, IConfiguration configuration, Action<MessageConsumersConfigurator> consumersConfig = null, bool configureHost = false)
        {
            var asyncConfig = GetConfig(services, configuration);
            return services.AddMassTransit(asyncConfig, consumersConfig, configureHost)
                           .AddAsyncWrapper();
        }

        #region config.

        private static AsyncConfiguration GetConfig(IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                #region Load from IOC
                
                var serviceProvider = services.BuildServiceProvider();
                var config = serviceProvider.GetService<AsyncConfiguration>();
                if (config == null) throw new Exception("service bus : MassTransit : couldn't find configurations in service container.");

                return config;

                #endregion
            }
            catch
            {
                #region Load from config.

                var asyncConfig = new AsyncConfiguration();
                configuration.GetSection(ConfigurationConstants.AsyncSectionKey).Bind(asyncConfig);

                services.AddSingleton(asyncConfig);

                return asyncConfig;

                #endregion
            }
        }

        #endregion
        #region MassTransit

        private static IServiceCollection AddMassTransit(this IServiceCollection services, AsyncConfiguration config, Action<MessageConsumersConfigurator> consumersConfig = null, bool configureHost = false)
        {
            #region Service Bus

            services.AddMassTransit(configure =>
                     {
                         #region consumers.

                         var consumersData = new MessageConsumersConfigurator();
                         if (consumersConfig != null)
                         {
                             consumersConfig(consumersData);
                             consumersData.ConsumerDefinitions.ForEach(consumer => configure.AddConsumer(consumer.ConsumerType));
                         }

                         #endregion

                         configure.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                         {
                             #region transport : RabbitMQ

                             var host = cfg.Host(new Uri(config.Transport.Uri), hostConfigurator =>
                             {
                                 hostConfigurator.Username(config.Transport.CredentialsUsername);
                                 hostConfigurator.Password(config.Transport.CredentialsPassword);
                             });

                             #endregion
                             #region protocol : endpoints

                             if (consumersConfig == null)
                             {
                                 // do nothing : no endpoint to be configured.
                             }
                             else
                             {
                                 consumersData.ConsumerDefinitions.ForEach(consumerDef =>

                                     cfg.ReceiveEndpoint(consumerDef.QueueName, endpoint =>
                                     {
                                         #region Concurrent Messages

                                         if (consumerDef.MaxConcurrentMessages.HasValue)
                                         {
                                             endpoint.PrefetchCount = consumerDef.MaxConcurrentMessages.Value;
                                         }

                                         #endregion
                                         #region Circuit Breaker

                                         if (consumerDef.CircuitBreakerTrackingPeriodInSeconds.HasValue &&
                                             consumerDef.CircuitBreakerTripThreshold.HasValue &&
                                             consumerDef.CircuitBreakerActiveThreshold.HasValue &&
                                             consumerDef.CircuitBreakerResetIntervalInSeconds.HasValue)
                                         {
                                             endpoint.UseCircuitBreaker(cb =>
                                             {
                                                 cb.TrackingPeriod = TimeSpan.FromSeconds(consumerDef.CircuitBreakerTrackingPeriodInSeconds.Value);
                                                 cb.TripThreshold = consumerDef.CircuitBreakerTripThreshold.Value;
                                                 cb.ActiveThreshold = consumerDef.CircuitBreakerActiveThreshold.Value;
                                                 cb.ResetInterval = TimeSpan.FromSeconds(consumerDef.CircuitBreakerResetIntervalInSeconds.Value);
                                             });
                                         }

                                         #endregion
                                         #region Retry Policy

                                         if (consumerDef.RetryCount.HasValue && consumerDef.RetryIntervalInMilliseconds.HasValue)
                                         {
                                             endpoint.UseMessageRetry(x => x.SetRetryPolicy(policy => policy.Interval(consumerDef.RetryCount.Value, TimeSpan.FromMilliseconds(consumerDef.RetryIntervalInMilliseconds.Value))));
                                         }

                                         #endregion
                                         #region Consumer

                                         endpoint.ConfigureConsumer(provider, consumerDef.ConsumerType);

                                         #endregion
                                     })
                                 );
                             }

                             #endregion
                         }));
                     });

            #endregion
            #region Host

            if (configureHost)
            {
                services.AddMassTransitHostedService();
            }

            #endregion

            return services;
        }

        #endregion
        #region wrapper.

        private static IServiceCollection AddAsyncWrapper(this IServiceCollection services)
        {
            return services.AddSingleton<IAsyncClient, Handlers.AsyncClient>();
        }

        #endregion
    }
}
