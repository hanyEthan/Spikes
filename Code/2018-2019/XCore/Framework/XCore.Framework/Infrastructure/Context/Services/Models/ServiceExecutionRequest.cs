using XCore.Framework.Infrastructure.Context.Execution.Support;

namespace XCore.Framework.Infrastructure.Context.Services.Models
{
    public class ServiceExecutionRequest<T> : IServiceExecutionRequest  //where T : IServiceExecutionContent
    {
        #region props.

        public string ClientToken { get; set; }
        public string UserCode { get; set; }
        public string SessionCode { get; set; }
        public string CorrelationCode { get; set; }
        public string Metadata { get; set; }
        public string Environment { get; set; }
        public string RequestTime { get; set; }
        public string Culture { get; set; }
        public string AppId { get; set; } = null;
        public string ModuleId { get; set; } = null;
        public T Content { get; set; }

        #endregion
        #region helpers.

        public RequestContext ToRequestContext()
        {
            var to = new RequestContext()
            {
                Metadata = this.Metadata,
                Culture = this.Culture,
                Environment = this.Environment,
                AppId = this.AppId,
                ModuleId = this.ModuleId,
            };

            return to;
        }

        #endregion
    }
}