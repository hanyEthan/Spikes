using System.Collections.Generic;
using System.Linq;
using XCore.Framework.Infrastructure.Messaging.Pools.Models;
using XCore.Framework.Infrastructure.Messaging.Pools.Service.Models;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Service.Mappers
{
    public static class Mapper
    {
        public static PoolMessage Map(MPoolMessageDataContract from)
        {
            if (from == null) return null;
            var to = new PoolMessage()
            {
                Code=from.Code,
                AppId = from.AppId,
                MessageContent=from.MessageContent,
                MessageType=from.MessageType,
                MetaData=from.MetaData,
                Periority=from.Periority,
                Size=from.Size,
                Status = from.Status,

                CreatedBy = from.CreatedBy,
            };

            return to;
        }
        public static MPoolMessageDataContract Map(PoolMessage from)
        {
            if (from == null) return null;
            var to = new MPoolMessageDataContract()
            {
                Code = from.Code,
                AppId = from.AppId,
                MessageContent = from.MessageContent,
                MessageType = from.MessageType,
                Periority = from.Periority,
                Size = from.Size,
                Status = from.Status,
                MetaData = from.MetaData,

                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
            };

            return to;
        }
        public static List<MPoolMessageDataContract> Map(List<PoolMessage> from)
        {
            if (from == null) return null;
            return from.Select(x => Map(x)).ToList();
        }
        public static PoolMessageSearchCriteria Map(MPoolCriteriaDataContract from)
        {
            if (from == null) return null;
            var to = new PoolMessageSearchCriteria()
            {
                CreatedDate = from.CreatedDate,
                MessageType = from.MessageType,
                PopMessages = from.PopMessages,
                AppId = from.AppId,
            };
            return to;
        }
    }
}
