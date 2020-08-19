using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Api.gRPC.Mappers
{
    public class CreateConfigItemResponseProtoMapper : IModelMapper<BaseResponseContext<int>, CreateConfigItemResponseProto>
    {
        #region IModelMapper

        public CreateConfigItemResponseProto Map(BaseResponseContext<int> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new CreateConfigItemResponseProto()
            {
                EntityId = from.Content,
                Header = BaseResponseContextProtoMapper.Instance.Map(from.Header),
            };

            return to;
        }
        TDestinationAlt IModelMapper<BaseResponseContext<int>, CreateConfigItemResponseProto>.Map<TDestinationAlt>(BaseResponseContext<int> from, object metadata)
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
