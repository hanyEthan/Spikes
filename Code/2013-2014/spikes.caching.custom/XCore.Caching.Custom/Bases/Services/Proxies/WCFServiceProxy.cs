using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using XCore.Caching.Custom.Utilities;

namespace XCore.Caching.Custom.Bases.Services.Proxies
{
    public class WCFServiceProxy<T> : IDisposable
    {
        #region Properties

        protected T client;
        public bool Initialized { get; protected set; }
        public bool RealtimeStatusChecking { get; protected set; }

        private const int INTEGRITYCHECKINTERVAL = 5;   // in seconds ...

        #endregion
        #region cst

        public WCFServiceProxy()
        {
            RealtimeStatusChecking = true;

            Initialized = InitiateConnection();
            CheckStatusIntegrityAsync();
        }

        #endregion

        #region Helpers

        protected bool CheckStatus()
        {
            return CheckStatus(false);
        }
        protected bool CheckStatus(bool reconnect)
        {
            if (client == null) return Initialized = false;

            var status = ((ICommunicationObject)client).State;
            if (status == CommunicationState.Faulted) return Initialized = InitiateConnection();
            if (status == CommunicationState.Closed && reconnect) return Initialized = InitiateConnection();
            if (status != CommunicationState.Opened) return Initialized = false;

            return Initialized;
        }

        private bool InitiateConnection()
        {
            try
            {
                //var binding = new NetTcpBinding( "CacheServiceBinding" );
                //var endpoint = new EndpointAddress( Broker.ConfigurationHandler.GetValue( Constants.CentralizedCacheService , Constants.CentralizedCacheServiceEndPoint ) );

                var channelFactory = new ChannelFactory<T>("CacheServiceEndPoint");

                client = channelFactory.CreateChannel();
                channelFactory.Closed += ChannelFactory_Closed;
                channelFactory.Faulted += ChannelFactory_Faulted;
                channelFactory.Closing += ChannelFactory_Closing;

                ((ICommunicationObject)client).Open();

                return true;
            }
            catch
            {
                if (client != null)
                {
                    ((ICommunicationObject)client).Abort();
                }
                return false;
            }
        }
        private async void CheckStatusIntegrityAsync()
        {
            await Task.Run(() => CheckStatusIntegrity());
        }
        private async Task CheckStatusIntegrity()
        {
            try
            {
                while (true)
                {
                    await Task.Delay(new TimeSpan(0, 0, INTEGRITYCHECKINTERVAL));
                    if (RealtimeStatusChecking) Initialized = CheckStatus(true);
                    //XLogger.Trace( "Checking ... ... ..." );
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
            }
        }

        private void ChannelFactory_Faulted(object sender, EventArgs e)
        {
            Initialized = InitiateConnection();   // try to initiate the connection again ...
        }
        private void ChannelFactory_Closed(object sender, EventArgs e)
        {
            Initialized = false;
        }
        private void ChannelFactory_Closing(object sender, EventArgs e)
        {
            Initialized = false;
        }

        #endregion
        #region IDisposable

        public void Dispose()
        {
            try
            {
                if (client != null && ((ICommunicationObject)client).State == CommunicationState.Opened) ((ICommunicationObject)client).Close();
                Initialized = false;
            }
            catch (Exception x)
            {
                XLogger.Error(x.ToString());
            }
        }

        #endregion
    }
}
