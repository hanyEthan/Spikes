using System;

using ADS.Common.Context;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;

namespace ADS.Tamam.Common.Data.Context
{
    [Serializable]
    public class RequestContext : RequestContextBase
    {
        public string CallerUsername { get; set; }
        public Guid? PersonId { get; set; }
        public Person Person { get; set; }
        public string MachineName { get; set; }
        public string IpAddress { get; set; }
        public bool IgnoreCache { get; set; }

        private static object syncRoot = new Object();

        private volatile SecurityContext _SecurityContext;
        public SecurityContext SecurityContext
        {
            get
            {
                if ( _SecurityContext == null )
                {
                    lock ( syncRoot )
                    {
                        if (_SecurityContext == null) _SecurityContext = this is SystemRequestContext ? SystemSecurityContext.Instance : new SecurityContext(PersonId, CallerUsername);
                    }
                }

                return _SecurityContext;
            }
        }

        public override string ToString()
        {
            if (!PersonId.HasValue) return string.Empty;
            return PersonId.Value.ToString();
        }
    }
}