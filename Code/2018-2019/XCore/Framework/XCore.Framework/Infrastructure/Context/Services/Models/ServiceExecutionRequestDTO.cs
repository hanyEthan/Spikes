namespace XCore.Framework.Infrastructure.Context.Services.Models
{
    public class ServiceExecutionRequestDTO<T> //where T : IServiceExecutionContent
    {
        #region props.

        public string RequestClientToken { get; set; }
        public string RequestUserCode { get; set; }
        public string RequestSessionCode { get; set; }
        public string RequestCorrelationCode { get; set; }
        public string RequestMetadata { get; set; }
        public string RequestTime { get; set; }
        public string RequestCulture { get; set; }
        public string RequestAppId { get; set; } = null;
        public string RequestModuleId { get; set; } = null;
        public T Content { get; set; }

        #endregion
    }
}