using System;

namespace ADS.Common.Context
{
    [Serializable]
    public class RequestContextBase
    {
        public Guid SecurityToken { get; set; }
        public string CultureName { get; set; }
    }
}
