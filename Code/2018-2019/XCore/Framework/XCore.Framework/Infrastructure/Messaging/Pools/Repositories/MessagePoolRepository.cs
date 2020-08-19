using System.Collections.Generic;
using System.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Messaging.Pools.Contracts;
using XCore.Framework.Infrastructure.Messaging.Pools.Models;
using XCore.Framework.Infrastructure.Messaging.Pools.Models.Enums;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Repositories
{
    public class MessagePoolRepository : Repository<PoolMessage>, IMessagePoolRepository
    {
        #region cst.

        public MessagePoolRepository(DbContext context) : base(context)
        {
        }

        #endregion
        #region IMessagePoolRepository

        public List<PoolMessage> Get(PoolMessageSearchCriteria filters)
        {
            var predicate = GetQuery(filters);
            var query = GetQueryable(true, predicate);
            return query.ToList();
        }
        public bool Hold(List<PoolMessage> Messages)
        {
            foreach (var message in Messages)
            {
                message.Status = PoolMessageStatus.OnHold;
                base.Update(message);
            }

            return true;
        }
        public bool Restore(List<string> MessageIds)
        {
            var messages = base.GetAsync(x => MessageIds.Contains(x.Code)).GetAwaiter().GetResult();
            foreach (var message in messages)
            {
                message.Status = PoolMessageStatus.New;
                base.Update(message);
            }

            return true;
        }
        public bool Delete(List<string> MessageIds)
        {
            var messages = base.GetAsync(x => MessageIds.Contains(x.Code)).GetAwaiter().GetResult();
            foreach (var message in messages)
            {
                base.Delete(message);
            }

            return true;
        }

        #endregion

        #region helpers.

        private ExpressionStarter<PoolMessage> GetQuery(PoolMessageSearchCriteria filters)
        {
            var predicate = PredicateBuilder.New<PoolMessage>(true);

            predicate = predicate.And(x => x.IsActive &&
                                           x.Status != PoolMessageStatus.OnHold &&
                                           x.Status != PoolMessageStatus.Deleted &&
                                           x.AppId == filters.AppId);

            if (filters.CreatedDate.HasValue)
            {
                predicate = predicate.And(x => x.CreatedDate > filters.CreatedDate.Value);
            }
            if (!string.IsNullOrWhiteSpace(filters.MessageType))
            {
                predicate = predicate.And(x => x.MessageType == filters.MessageType);
            }

            return predicate;
        }

        #endregion
    }
}
