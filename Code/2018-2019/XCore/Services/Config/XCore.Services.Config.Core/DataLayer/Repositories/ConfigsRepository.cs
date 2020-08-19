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
    public class ConfigsRepository : Repository<ConfigItem>, IConfigsRepository
    {
        #region cst.

        public ConfigsRepository(DbContext context) : base(context)
        {
        }

        #endregion
        #region IConfigsRepository

        public bool Any(ConfigSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return query.Any();
        }
        public SearchResults<ConfigItem> Get(ConfigSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<ConfigItem>()
            {
                Results = queryPaged.ToList(),
                TotalCount = query.Count(),
            };
        }

        #endregion
        #region helpers.

        private ExpressionStarter<ConfigItem> GetQuery(ConfigSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<ConfigItem>(true);

            #region Id

            if (criteria.Id != null)
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
            #region AppId

            if (criteria.AppId != null)
            {
                predicate = predicate.And(x => x.AppId == criteria.AppId);
            }

            #endregion
            #region ModuleId

            if (criteria.ModuleId != null)
            {
                predicate = predicate.And(x => x.ModuleId == criteria.ModuleId);
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
        private void ApplyPaging(ConfigSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<ConfigItem>, IOrderedQueryable<ConfigItem>> ApplySorting(ConfigSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != AppSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<ConfigItem>, IOrderedQueryable<ConfigItem>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case ConfigSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = OrderByProperty(isCultured, isDesc, x => x.CreatedDate, x => x.CreatedDate);
                        }
                        break;
                    case ConfigSearchCriteria.OrderByExpression.Name:
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
        private Func<IQueryable<ConfigItem>, IOrderedQueryable<ConfigItem>> OrderByProperty<TKey>(bool isCultured, bool isDesc, Func<ConfigItem, TKey> property, Func<ConfigItem, TKey> culturedProperty)
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
