using XCore.Framework.Framework.ServiceBus.MST.Models;

namespace XCore.Framework.Framework.ServiceBus.MST.Support
{
    public class ServiceBusConfiguration
    {
        #region props.

        public TransportType TransportType { get; set; }
        public TransportEndPoint Transport { get; set; }
        public string EndPointName { get; set; }
        public ushort? MaxConcurrentMessages { get; set; }

        #endregion
        #region nested.

        public class TransportEndPoint
        {
            public string Uri { get; set; }
            public string CredentialsUsername { get; set; }
            public string CredentialsPassword { get; set; }
        }

        #endregion
    }
}
