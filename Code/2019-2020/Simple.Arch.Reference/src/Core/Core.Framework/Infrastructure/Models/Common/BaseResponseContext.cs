using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common
{
    #region BaseResponseContext

    public class BaseResponseContext
    {
        #region props.

        public virtual HeaderContent Header { get; set; }

        #endregion
        #region statics.

        public static BaseResponseContext Error = new BaseResponseContext()
        {
            Header = new HeaderContent()
            {
                StatusCode = ResponseCode.SystemError,
            },
        };

        #endregion
        #region nested.

        public class HeaderContent
        {
            [JsonIgnore] public virtual ResponseCode StatusCode { get; set; }
            public virtual string code { get { return StatusCode.ToString(); } }
            public virtual string message { get; set; }
            public virtual string target { get; set; }
            public virtual List<MetaPair> details { get; set; }
            [JsonIgnore] public virtual string MessagesSummary
            {
                get
                {
                    var summary = code;

                    if ((details?.Count).GetValueOrDefault() != 0)
                    {
                        details.ForEach(x=> summary += $" ({x.target}:{x.message})");
                    }

                    return summary;
                }
            }
            [JsonIgnore] public virtual string CorrelationId { get; set; }
            [JsonIgnore] public virtual DateTime? RequestTimeUTC { get; set; }
            [JsonIgnore] public virtual long? ResponseProcessingTimeInTicks
            {
                get
                {
                    return this.RequestTimeUTC == null
                         ? null
                         : DateTime.UtcNow.Subtract(this.RequestTimeUTC.Value).Ticks as long?;
                }
            }
        }

        #endregion
    }

    #endregion
    #region BaseResponseContext<TContent>

    public class BaseResponseContext<TContent> : BaseResponseContext
    {
        public virtual TContent Content { get; set; }
    }

    #endregion
}
