using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.Core.DataLayer.Context;
using XCore.Services.Notifications.Core.DataLayer.Contracts;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.DataLayer.Repositories
{
    public class MessageTemplateRepository : Repository<MessageTemplate>, IMessageTemplateRepository
    {
        #region props

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public MessageTemplateRepository(NotificationsDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion

        #region ITemplateMessagesRepository

        public async Task<bool> AnyAsync(MessageTemplateSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<MessageTemplate>> GetAsync(MessageTemplateSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = TemplateMessagelySorting(criteria);
            TemplateMessagelyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<MessageTemplate>()
            {
                Results = await queryPaged.ToListAsync(),
                TotalCount = await query.CountAsync(),
            };
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        private ExpressionStarter<MessageTemplate> GetQuery(MessageTemplateSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<MessageTemplate>(true);

            #region Id

            if (criteria.Id != null && criteria.Id != 0)
            {
                predicate = predicate.And(x => x.Id == criteria.Id);
            }

            #endregion
            #region Name

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(criteria.Name.ToLower()));
            }

            #endregion
            #region Code

            if (!string.IsNullOrWhiteSpace(criteria.Code))
            {
                predicate = predicate.And(x => x.Code == criteria.Code.Trim());
            }

            #endregion
            #region Active

            if (criteria.IsActive.HasValue)
            {
                predicate = predicate.And(x => x.IsActive == criteria.IsActive.Value);
            }

            #endregion

            return predicate;
        }
        private void TemplateMessagelyPaging(MessageTemplateSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<MessageTemplate>, IOrderedQueryable<MessageTemplate>> TemplateMessagelySorting(MessageTemplateSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != MessageTemplateSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<MessageTemplate>, IOrderedQueryable<MessageTemplate>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case MessageTemplateSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case MessageTemplateSearchCriteria.OrderByExpression.Name:
                    default:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.Name)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.NameCultured)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.Name)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.NameCultured)
                                         : null;
                        }
                        break;
                }
            }
            else
            {
                //orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.Name)
                //             : isDesc && isCultured ? x.OrderByDescending(y => y.NameCultured)
                //             : !isDesc && !isCultured ? x.OrderBy(y => y.Name)
                //             : !isDesc && isCultured ? x.OrderBy(y => y.NameCultured)
                //             : null;
            }

            return orderBy;
        }

        #endregion
    }
}
