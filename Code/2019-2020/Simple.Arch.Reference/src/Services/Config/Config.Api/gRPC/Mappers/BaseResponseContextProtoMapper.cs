using System.Collections.Generic;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Api.gRPC.Mappers
{
    public class BaseResponseContextProtoMapper : IModelMapper<BaseResponseContext.HeaderContent, BaseResponseContextProto>
    {
        #region statics.

        public static BaseResponseContextProtoMapper Instance = new BaseResponseContextProtoMapper();

        #endregion
        #region IModelMapper

        public BaseResponseContextProto Map(BaseResponseContext.HeaderContent from, object metadata = null)
        {
            if (from == null) return null;

            var to = new BaseResponseContextProto()
            {
                Code = (((int?)from.StatusCode) ?? (int?)ResponseCode.SystemError).Value,
                CorrelationId = from.CorrelationId,
                Messages = Map(from.details),
                RequestTimeUTC = from.RequestTimeUTC?.ToString(),
                ResponseProcessingTimeInTicks = (from.ResponseProcessingTimeInTicks).GetValueOrDefault(),
                StatusCode = from.code,
            };

            return to;
        }
        TDestinationAlt IModelMapper<BaseResponseContext.HeaderContent, BaseResponseContextProto>.Map<TDestinationAlt>(BaseResponseContext.HeaderContent from, object metadata)
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
        #region helpers.

        private string Map(List<MetaPair> from)
        {
            string to = "";
            from.ForEach(x => to += $" ({x.target}:{x.message})" );

            return to;
        }

        #endregion
    }
}
