using System;
using System.Collections.Generic;
using System.Text;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support
{
    public class AsyncConfiguration
    {
        #region props.

        public TransportEndPoint Transport { get; set; }
        public string EndPointName { get; set; }

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
