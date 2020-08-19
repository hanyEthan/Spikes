using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.Core.DataLayer.Context;
using XCore.Services.Notifications.Core.DataLayer.Contracts;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.DataLayer.Repositories
{
    public class InternalNotificationRepository : Repository<InternalNotification>, IInternalNotificationRepository
    {
        #region props

        public bool? Initialized { get; protected set; }
        
        #endregion
        #region cst.

        public InternalNotificationRepository(NotificationsDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region ITemplateMessagesRepository

        public async Task<bool> AnyAsync(InternalNotificationSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<InternalNotification>> GetAsync(InternalNotificationSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = TemplateMessagelySorting(criteria);
            TemplateMessagelyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<InternalNotification>()
            {
                Results = await queryPaged.ToListAsync(),
                TotalCount = await query.CountAsync(),
            };
        }
        public async Task SetIsDismissedAsync(InternalNotification item, bool isActive)
        {
            
            var Context = this.context as NotificationsDataContext;
            if (Context!=null)
            {
                //var entity = await Context.InternalNotification.FindAsync(id);
                item.IsDismissed = isActive;
                Context.Update(item);
            }
            else
            {
                throw new Exception();
            }
            
           
        }
        public async Task SetIsDeletedAsync(InternalNotification item, bool isActive)
        {

            var Context = this.context as NotificationsDataContext;
            if (Context != null)
            {
                //var entity = await Context.InternalNotification.FindAsync(id);
                item.IsDeleted = isActive;
                Context.Update(item);
            }
            else
            {
                throw new Exception();
            }


        }

        public async Task SetIsReadAsync(List<InternalNotification> items, bool isActive)
        {
            var Context = this.context as NotificationsDataContext;
            // List<InternalNotification> entity = await context.Set<List<InternalNotification>>().FindAsync(id);
            //var entities = item.Select(X => X.IsActive = isActive);
            if (Context != null)
            {
                foreach (var item in items)
                {
                    item.IsRead = isActive;
                    Context.Update(item);
                }
            }
            else
            {
                throw new Exception();
            }
           
        }
     
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        private ExpressionStarter<InternalNotification> GetQuery(InternalNotificationSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<InternalNotification>(true);

            #region Id

            if (criteria.Id != null && criteria.Id.All(x => x!=0))
            {
                predicate = predicate.And(x => criteria.Id.Contains(x.Id) );
            }
           

            #endregion
            #region Name

            //if (!string.IsNullOrWhiteSpace(criteria.Name))
            //{
            //    predicate = predicate.And(x => x.Name.Contains(criteria.Name.ToLower()));
            //}

            #endregion
            #region Code

            if (criteria.Code != null && criteria.Code.All(x => x != ""))
            {
                predicate = predicate.And(x => criteria.Code.Contains(x.Code));
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
        private void TemplateMessagelyPaging(InternalNotificationSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<InternalNotification>, IOrderedQueryable<InternalNotification>> TemplateMessagelySorting(InternalNotificationSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != MessageTemplateSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<InternalNotification>, IOrderedQueryable<InternalNotification>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case InternalNotificationSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case InternalNotificationSearchCriteria.OrderByExpression.Name:
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
