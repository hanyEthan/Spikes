using XCore.Framework.Infrastructure.Config.Contracts;

namespace XCore.Services.Audit.SDK.Models
{
    public class AuditClientConfig : IConfigData
    {
        public string Endpoint { get; set; }
    }
}
