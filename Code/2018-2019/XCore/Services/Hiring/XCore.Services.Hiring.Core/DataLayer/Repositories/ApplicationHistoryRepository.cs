using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.DataLayer.Context;
using XCore.Services.Hiring.Core.DataLayer.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.DataLayer.Repositories
{
    public class ApplicationHistoryRepository : Repository<ApplicationHistory>, IApplicationHistoryRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public ApplicationHistoryRepository(HiringDataContext dataContext) : base(dataContext)
        {
            this.Initialized = Initialize();
        }

        #endregion

        #region IApplicationHistoryRepository
        
        public async Task<SearchResults<ApplicationHistory>> Get(ApplicationHistorySearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<ApplicationHistory>()
            {
                Results = await queryPaged.ToListAsync(),
                TotalCount = await query.CountAsync(),
            };
        }
        
        #endregion
        #region helpers.

        private ExpressionStarter<ApplicationHistory> GetQuery(ApplicationHistorySearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<ApplicationHistory>(true);

            #region DateCreatedFrom

            if (criteria.DateCreatedFrom.HasValue)
            {
                predicate = predicate.And(x => x.CreatedDate >= criteria.DateCreatedFrom);
            }

            #endregion
            #region DateCreatedTo

            if (criteria.DateCreatedTo.HasValue)
            {
                predicate = predicate.And(x => x.CreatedDate <= criteria.DateCreatedTo);
            }

            #endregion            

            return predicate;
        }
        private Func<IQueryable<ApplicationHistory>, IOrderedQueryable<ApplicationHistory>> ApplySorting(ApplicationHistorySearchCriteria criteria)
        {
            if (criteria.Order == null) return null;
            bool isCultured = criteria.OrderByCultureMode != SearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<ApplicationHistory>, IOrderedQueryable<ApplicationHistory>> orderBy = null;

            switch (criteria.Order.Value)
            {
                case ApplicationHistorySearchCriteria.OrderByExpression.CreationDate:
                    {
                        orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                     : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                     : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                     : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                     : null;
                    }
                    break;
                case ApplicationHistorySearchCriteria.OrderByExpression.Name:
                    {
                        orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.Name)
                                     : isDesc && isCultured ? x.OrderByDescending(y => y.NameCultured)
                                     : !isDesc && !isCultured ? x.OrderBy(y => y.Name)
                                     : !isDesc && isCultured ? x.OrderBy(y => y.NameCultured)
                                     : null;
                    }
                    break;
                default:
                    break;
            }

            return orderBy;
        }
        private void ApplyPaging(ApplicationHistorySearchCriteria criteria, out int? skip, out int? take)
        {

            skip = (criteria.PageNumber - 1) * criteria.PageSize;
            take = criteria.PageSize;
        }
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }


        #endregion
    }
}
