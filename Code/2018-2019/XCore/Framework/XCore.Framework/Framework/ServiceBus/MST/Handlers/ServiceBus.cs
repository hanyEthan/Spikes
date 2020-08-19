using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.ServiceBus.MST.Models;
using XCore.Framework.Framework.ServiceBus.MST.Support;
using XCore.Framework.Infrastructure.Config.Contracts;

namespace XCore.Framework.Framework.ServiceBus.Handlers
{
    public class ServiceBus : IServiceBus
    {
        #region props.

        public bool Initialized { get; protected set; }
        public bool Started { get; private set; }
        public ServiceBusConfiguration Configurations { get; protected set; }

        protected virtual IBusControl Bus { get; set; }
        protected virtual IServiceBusTransportConfigurator TransportConfig { get; set; }
        protected virtual Dictionary<string, ISendEndpoint> SendEndpoints { get; set; } = new Dictionary<string, ISendEndpoint>();  // cache.

        #endregion
        #region cst.

        public ServiceBus(IConfigProvider<ServiceBusConfiguration> configProvider) : this(configProvider, null)
        {
        }
        public ServiceBus(IConfigProvider<ServiceBusConfiguration> configProvider, Action<IReceiveEndpointConfigurator> nativeConfig)
        {
            this.Initialized = Initialize(configProvider, nativeConfig);
        }

        #endregion

        #region publics.

        public async Task Start()
        {
            #region initialized ?
            if (!Initialized) throw new Exception("ServiceBus is not initialized correctly.");
            #endregion

            await this.Bus.StartAsync();
            this.Started = true;
        }
        public async Task Stop()
        {
            #region initialized ?
            if (!Initialized) throw new Exception("ServiceBus is not initialized correctly.");
            if (!Started) throw new Exception("ServiceBus is not running.");
            #endregion

            await this.Bus.StopAsync();
            this.Started = false;
        }

        public async Task Send<TContract, TMessage>(TMessage message, string endpoint) where TMessage : class, TContract
                                                                                       where TContract : class
        {
            #region initialized ?
            if (!Initialized) throw new Exception("ServiceBus is not initialized correctly.");
            #endregion

            var sendEndPoint = await this.GetSenderEndpoint(endpoint);
            await sendEndPoint.Send<TContract>(message);
        }
        public async Task Publish<TContract, TMessage>(TMessage message) where TMessage : class, TContract
                                                                         where TContract : class
        {
            #region initialized ?
            if (!Initialized) throw new Exception("ServiceBus is not initialized correctly.");
            #endregion

            await this.Bus.Publish<TContract>(message);
        }

        public void Subscribe<TConsumer, TMessage>() where TConsumer : class, IConsumer<TMessage>, new()
                                                     where TMessage : class
        {
            this.Bus.ConnectConsumer<TConsumer>();
        }

        public IBusControl GetBusControl(IServiceProvider serviceProvider)
        {
            return this.Initialized
                 ? this.Bus
                 : null;
        }

        #endregion
        #region helpers.

        protected virtual bool Initialize()
        {
            bool state = true;

            state = state && this.Bus != null;
            if (state == false) return state;

            return state;
        }
        protected virtual bool Initialize(IConfigProvider<ServiceBusConfiguration> configProvider, Action<IReceiveEndpointConfigurator> nativeConfig)
        {
            bool state = true;

            this.Configurations = configProvider?.GetConfigAsync().GetAwaiter().GetResult();
            state = state && this.Configurations != null;
            if (state == false) return state;

            this.TransportConfig = GetTransportConfigurator(this.Configurations.TransportType);
            state = state && this.TransportConfig != null;
            if (state == false) return state;

            this.Bus = this.TransportConfig.GetServiceBus(this.Configurations, nativeConfig);
            state = state && this.Bus != null;
            if (state == false) return state;

            return state;
        }
        protected virtual IServiceBusTransportConfigurator GetTransportConfigurator(TransportType transportType)
        {
            switch (transportType)
            {
                case TransportType.RabbitMQ:
                    {
                        return new RabbitMQServiceBusTransportConfigurator();
                    }
                default:
                    {
                        return null;
                    }
            }
        }
        protected virtual async Task<ISendEndpoint> GetSenderEndpoint(string endpoint)
        {
            if (this.SendEndpoints.TryGetValue(endpoint, out ISendEndpoint sentEndpoint))
            {
                return sentEndpoint;
            }
            else
            {
                var uri = new Uri(this.TransportConfig.GetComponentEndPoint(this.Configurations.Transport.Uri, endpoint));
                
                var sendEndpoint = await this.Bus.GetSendEndpoint(uri);
                this.SendEndpoints.TryAdd(endpoint, sendEndpoint);

                return sendEndpoint;
            }
        }

        #endregion
    }
}
