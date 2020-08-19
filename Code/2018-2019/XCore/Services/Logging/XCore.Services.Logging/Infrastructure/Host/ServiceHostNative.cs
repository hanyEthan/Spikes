using System;
using System.Threading.Tasks;
using MassTransit;
using XCore.Framework.Framework.ServiceBus.Handlers;
using XCore.Framework.Framework.ServiceBus.MST.Support;
using XCore.Framework.Utilities;
using XCore.Services.Logging.Handlers;

namespace XCore.Services.Logging.Infrastructure.Host
{
    public class ServiceHostNative : IDisposable
    {
        #region props.

        public bool Initialized { get; private set; }
        private ServiceBus ServiceBus { get; set; }

        #endregion
        #region cst.

        public ServiceHostNative()
        {
            this.Initialized = Initialize().GetAwaiter().GetResult();
        }

        #endregion
        #region helpers.

        private async Task<bool> Initialize()
        {
            //this.ServiceBus = new ServiceBus(ServiceBusConfigurationProvider.Instance, e =>
            //{
            //    e.Consumer<LogHandler>();
            //});

            return true;
        }

        #endregion
        #region publics.

        public async Task Start()
        {
            try
            {
                #region LOG
                //XLogger.Trace("");
                #endregion

                await this.ServiceBus.Start();
            }
            #region catch
            catch (Exception x)
            {
                //XLogger.Error("Exception : " + x);
                throw;
            }
            #endregion
        }
        public async Task Stop()
        {
            try
            {
                await this.ServiceBus.Stop();
            }
            #region catch
            catch (Exception x)
            {
                //XLogger.Error("Exception : " + x);
                throw;
            }
            #endregion
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            try
            {
            }
            catch
            {
            }
        }

        #endregion
    }
}
