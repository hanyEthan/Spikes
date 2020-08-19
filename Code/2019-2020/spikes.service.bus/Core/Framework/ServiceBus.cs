using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Core.Framework
{
    public class ServiceBus
    {
        #region props.

        public bool Initialized { get; protected set; }

        public string ServerUri { get; set; }
        public string ServerUsername { get; set; }
        public string ServerPassword { get; set; }
        public string ComponentEndPoint { get; set; }

        private IBusControl _BusControl { get; set; }
        private Dictionary<string, ISendEndpoint> SendEndpoints { get; set; }

        #endregion
        #region cst.

        public ServiceBus(string endpointName) : this(endpointName, null)
        {
        }
        public ServiceBus(string endpointName, Action<IReceiveEndpointConfigurator> config)
        {
            this.ComponentEndPoint = endpointName;
            this.Initialized = Initialize(config);
        }

        #endregion
        #region publics.

        public async Task Start()
        {
            await this._BusControl.StartAsync();
        }
        public async Task Stop()
        {
            await this._BusControl.StopAsync();
        }

        public async Task Send<TContract, TMessage>(TMessage message, string endpoint)
               where TMessage : class, TContract
               where TContract : class
        {
            if (!this.Initialized) throw new Exception("not initialized properly.");

            var sendEndPoint = this.GetSenderEndpoint(endpoint);
            await sendEndPoint.Send<TContract>(message);
        }
        public async Task Publish<TContract, TMessage>(TMessage message)
            where TMessage : class, TContract
            where TContract : class
        {
            await this._BusControl.Publish<TContract>(message);
        }

        #endregion
        #region helpers.

        private bool Initialize(Action<IReceiveEndpointConfigurator> config)
        {
            bool state = true;

            this.ServerUri = "rabbitmq://localhost/";
            this.ServerUsername = "guest";
            this.ServerPassword = "guest";
            Uri rabbitMqRootUri = new Uri(ServerUri);
            this.SendEndpoints = new Dictionary<string, ISendEndpoint>();

            this._BusControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(rabbitMqRootUri, settings =>
                {
                    settings.Username(this.ServerUsername);
                    settings.Password(this.ServerPassword);
                });

                if (config != null)
                {
                    x.ReceiveEndpoint(this.ComponentEndPoint, config);
                }
            });

            return state;
        }
        private ISendEndpoint GetSenderEndpoint(string endpoint)
        {
            if (this.SendEndpoints.TryGetValue(endpoint, out ISendEndpoint sentEndpoint))
            {
                return sentEndpoint;
            }
            else
            {
                string server = this.ServerUri.EndsWith("/") ? this.ServerUri : this.ServerUri + "/";

                Task<ISendEndpoint> sendEndpointTask = this._BusControl.GetSendEndpoint(new Uri(string.Concat(server, endpoint)));
                ISendEndpoint sendEndpoint = sendEndpointTask?.Result;

                return sendEndpoint;
            }
        }

        #endregion
    }
}
