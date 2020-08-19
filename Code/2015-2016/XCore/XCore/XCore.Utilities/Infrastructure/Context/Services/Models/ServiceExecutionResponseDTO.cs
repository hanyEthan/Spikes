using System;

namespace XCore.Utilities.Infrastructure.Context.Services.Models
{
    public class ServiceExecutionResponseDTO<T> //where T : IServiceExecutionContent
    {
        #region props.

        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string Metadata { get; set; }
        public string RequestTime { get; set; }
        public TimeSpan? ResponseProcessingTime { get; set; }
        public long? ResponseProcessingTimeInTicks { get { return ResponseProcessingTime?.Ticks; } set { ResponseProcessingTime = new TimeSpan( value.GetValueOrDefault() ); } }
        public T Content { get; set; }

        #endregion
    }
}