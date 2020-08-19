using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.DataLayer.Context;
using XCore.Services.Organizations.Core.DataLayer.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public EventRepository(OrganizationDataContext dataContext) : base(dataContext)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region ICityRepository
        public async Task<SearchResults<Event>> GetAsync(EventSearchCriteria criteria , string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Event>()
            {
                Results = await queryPaged.ToListAsync(),
                TotalCount = await query.CountAsync(),
            };
        }

        public async Task<bool> AnyAsync(EventSearchCriteria criteria, string includeProperties =null)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate,null, includeProperties);

            return await query.AnyAsync();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }


        private ExpressionStarter<Event> GetQuery(EventSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Event>(true);

            #region Id

            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(x => criteria.Ids.Contains(x.Id));
            }

            #endregion 
            #region Name

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                predicate = predicate.And(x => x.Name == criteria.Name);
            }

            #endregion
            #region Code

            if (!string.IsNullOrWhiteSpace(criteria.Code))
            {
                predicate = predicate.And(x => x.Code == criteria.Code);
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
        private void ApplyPaging(EventSearchCriteria criteria, out int? skip, out int? take)
        {

            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Event>, IOrderedQueryable<Event>> ApplySorting(EventSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != EventSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Event>, IOrderedQueryable<Event>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case EventSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
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

            }

            return orderBy;
        }




        #endregion
    }
}
