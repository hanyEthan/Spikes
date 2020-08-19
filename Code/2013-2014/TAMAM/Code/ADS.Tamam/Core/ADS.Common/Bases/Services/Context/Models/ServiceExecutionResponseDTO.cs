using System;

namespace ADS.Common.Bases.Services.Context.Models
{
    public class ServiceExecutionResponseDTO<T> //where T : IServiceExecutionContent
    {
        #region props.

        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string Metadata { get; set; }
        public string RequestTime { get; set; }
        public TimeSpan? ResponseProcessingTime { get; set; }
        public T Content { get; set; }

        #endregion
    }
}
