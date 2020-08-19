using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using XCore.Framework.Framework.ServiceBus.Contracts;

namespace XCore.Framework.Framework.ServiceBus.MST.Host
{
    public class ServiceBusHostedService : IHostedService
    {
        #region props.

        public bool Initialized { get; protected set; }
        public bool Started { get; protected set; }
        public IServiceBus ServiceBus { get; protected set; }

        #endregion
        #region cst.

        public ServiceBusHostedService(IServiceBus serviceBus)
        {
            this.ServiceBus = serviceBus;
            this.Initialized = Initialize();
        }

        #endregion

        #region IHostedService

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!this.Initialized) throw new Exception("Service bus is not configured correctly");
            await this.ServiceBus.Start();
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (this.Started)
            {
                await this.ServiceBus.Stop();
            }
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.ServiceBus != null;

            return isValid;
        }

        #endregion
    }
}
