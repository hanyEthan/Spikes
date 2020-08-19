using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using XCore.Framework.Infrastructure.Context.Services.Models.Enums;

namespace XCore.Framework.Infrastructure.Context.Services.Models
{
    public class ServiceExecutionResponse<T> //where T : IServiceExecutionContent
    {
        #region cst.

        public ServiceExecutionResponse()
        {
            this.RequestTime = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public ServiceExecutionResponse(string requestTime) : this()
        {
            this.RequestTime = !string.IsNullOrWhiteSpace(requestTime) ? requestTime : this.RequestTime;
        }

        #endregion
        #region props.

        private int _responseCode;
        public int ResponseCode
        {
            get { return _responseCode; }
            set
            {
                _responseCode = value;
                this.ResponseMessage = this.ResponseMessage 
                                    ?? Enum.GetName( typeof( ResponseCode ) , _responseCode );
            }
        }

        public string ResponseMessage { get; set; }
        public string Metadata { get; set; }
        public string RequestTime { get; set; }
        public TimeSpan? ResponseProcessingTime
        {
            get
            {
                return CalculateProcessingTime(RequestTime);
            }
            set
            {
            }
        }
        public T Content { get; set; }

        [JsonIgnore] private Dictionary<string, string> responseMessageDetails;
        [JsonIgnore] public Dictionary<string, string> ResponseMessageDetails
        {
            get
            {
                return this.responseMessageDetails;
            }
            set
            {
                try
                {
                    this.responseMessageDetails = value;
                    this.Metadata = JsonConvert.SerializeObject(this.responseMessageDetails);
                }
                catch (Exception x)
                {
                    // ...
                    throw;
                }
            }
        }

        #endregion
        #region helpers.

        private static TimeSpan? CalculateProcessingTime(string RequestTime)
        {
            DateTime dateTime;
            if (string.IsNullOrWhiteSpace(RequestTime)) return null;
            if (!DateTime.TryParseExact(RequestTime, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime)) return null;

            return DateTime.UtcNow.Subtract(dateTime);
        }

        #endregion
    }
}