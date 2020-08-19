using System;
using System.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.Core.DataLayer.Contracts;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.Core.DataLayer.Repositories
{
    public class AppsRepository: Repository<App>, IAppsRepository
    {
        #region cst.

        public AppsRepository(DbContext context) : base(context)
        {
        }

        #endregion
        #region IAppsRepository

        public bool Any(AppSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return query.Any();
        }
        public SearchResults<App> Get(AppSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<App>()
            {
                Results = queryPaged.ToList(),
                TotalCount = query.Count(),
            };
        }

        #endregion
        #region helpers.

        private ExpressionStarter<App> GetQuery(AppSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<App>(true);

            #region Id

            if (criteria.Id != null && criteria.Id != 0)
            {
                predicate = predicate.And(x => x.Id == criteria.Id);
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
        private void ApplyPaging(AppSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<App>, IOrderedQueryable<App>> ApplySorting(AppSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != AppSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<App>, IOrderedQueryable<App>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case AppSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = OrderByProperty(isCultured, isDesc, x => x.CreatedDate, x => x.CreatedDate);
                        }
                        break;
                    case AppSearchCriteria.OrderByExpression.Name:
                    default:
                        {
                            orderBy = OrderByProperty(isCultured, isDesc, x => x.Name, x => x.NameCultured);
                        }
                        break;
                }
            }
            else
            {
                orderBy = OrderByProperty(isCultured, isDesc, x => x.Name, x => x.NameCultured);
            }

            return orderBy;
        }
        private Func<IQueryable<App>, IOrderedQueryable<App>> OrderByProperty<TKey>(bool isCultured , bool isDesc, Func<App, TKey> property, Func<App, TKey> culturedProperty)
        {
            if (isDesc && !isCultured)
            {
                return x => x.OrderByDescending(y => property);
            }
            else if (isDesc && isCultured)
            {
                return x => x.OrderByDescending(y => culturedProperty);
            }
            else if (!isDesc && !isCultured)
            {
                return x => x.OrderBy(y => property);
            }
            else
            {
                return x => x.OrderBy(y => culturedProperty);
            }
        }

        #endregion
    }
}
