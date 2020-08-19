using System;

namespace XCore.Framework.Infrastructure.Context.Execution.Support
{
    [Serializable]
    public class RequestContext
    {
        public Guid SecurityToken { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public string Culture { get; set; }
        public string SourceIP { get; set; }
        public string SourceMachine { get; set; }
        public string Metadata { get; set; }
        public string Environment { get; set; }
        public string AppId { get; set; } = null;
        public string ModuleId { get; set; } = null;
    }
}
