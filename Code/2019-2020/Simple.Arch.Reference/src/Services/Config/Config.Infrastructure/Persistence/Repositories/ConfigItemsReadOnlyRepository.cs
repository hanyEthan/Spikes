using System;
using System.Linq;
using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Context;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Handlers;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Repositories
{
    public class ConfigItemsReadOnlyRepository : DbRepositoryRead<ConfigItem>, IConfigItemsReadOnlyRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }
        
        #endregion
        #region cst.

        public ConfigItemsReadOnlyRepository(ConfigReadOnlyDbContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IConfigItemsRepository

        public async Task<bool> AnyAsync(ConfigItemsSearchCriteria criteria)
        {
            var query = base.GetQueryable(true);
            query = ApplyFilter(query, criteria);
            return await query.AnyAsync();
        }
        public async Task<SearchResults<ConfigItem>> GetAsync(ConfigItemsSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);
            var query = base.GetQueryable(true, query: null, null, includeProperties);
            query = ApplyFilter(query, criteria);
            var queryPaged = base.GetQueryable(true, query, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<ConfigItem>()
            {
                Results = await queryPaged.ToListAsync(),
                Metadata = new SearchResults<ConfigItem>.MetadataHeader()
                {
                    TotalCount = await query.CountAsync(),
                    PageIndex = criteria.PageNumber,
                    PageSize = criteria.PageSize,
                },
            };
        }
        public async Task<ConfigItem> GetAsync(int moduleId, string key, string includeProperties = null)
        {
            return await base.GetFirstAsync(x => x.ModuleId == moduleId &&
                                                 x.Key == key &&
                                                 x.IsDeleted == false,
                                                 null, includeProperties);
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        private IQueryable<ConfigItem> ApplyFilter(IQueryable<ConfigItem> query, ConfigItemsSearchCriteria criteria)
        {
            #region Keys

            if (!string.IsNullOrWhiteSpace(criteria.Key))
            {
                query = query.Where(x => x.Key.Contains(criteria.Key));
            }

            #endregion
            #region ModuleId

            if (criteria.ModuleId.HasValue)
            {
                query = query.Where(x => x.ModuleId == criteria.ModuleId.Value);
            }

            #endregion
            #region Active

            if (criteria.IsActive.HasValue)
            {
                query = query.Where(x => x.IsDeleted != criteria.IsActive.Value);
            }

            #endregion

            return query;
        }
        private void ApplyPaging(ConfigItemsSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<ConfigItem>, IOrderedQueryable<ConfigItem>> ApplySorting(ConfigItemsSearchCriteria criteria)
        {
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<ConfigItem>, IOrderedQueryable<ConfigItem>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case ConfigItemsSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc
                                         ? x.OrderByDescending(y => y.CreationDateTimeUtc)
                                         : x.OrderBy(y => y.CreationDateTimeUtc);
                        }
                        break;
                    case ConfigItemsSearchCriteria.OrderByExpression.Key:
                    default:
                        {
                            orderBy = x => isDesc
                                         ? x.OrderByDescending(y => y.Key)
                                         : x.OrderBy(y => y.Key);
                        }
                        break;
                }
            }

            return orderBy;
        }

        #endregion
    }
}
