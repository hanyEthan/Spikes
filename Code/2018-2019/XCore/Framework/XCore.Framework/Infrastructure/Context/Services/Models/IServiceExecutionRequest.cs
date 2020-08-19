using XCore.Framework.Infrastructure.Context.Execution.Support;

namespace XCore.Framework.Infrastructure.Context.Services.Models
{
    public interface IServiceExecutionRequest
    {
        #region props.

        string ClientToken { get; set; }
        string UserCode { get; set; }
        string SessionCode { get; set; }
        string CorrelationCode { get; set; }
        string Metadata { get; set; }
        string Environment { get; set; }
        string RequestTime { get; set; }
        string Culture { get; set; }
        string AppId { get; set; }
        string ModuleId { get; set; }

        #endregion
    }
}