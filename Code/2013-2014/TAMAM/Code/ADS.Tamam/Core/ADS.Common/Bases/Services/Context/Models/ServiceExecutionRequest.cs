namespace ADS.Common.Bases.Services.Context.Models
{
    public class ServiceExecutionRequest<T> //where T : IServiceExecutionContent
    {
        #region props.

        public string ClientToken { get; set; }
        public string UserCode { get; set; }
        public string SessionCode { get; set; }
        public string Metadata { get; set; }
        public string RequestTime { get; set; }
        public string Culture { get; set; }
        public T Content { get; set; }

        #endregion
    }
}
