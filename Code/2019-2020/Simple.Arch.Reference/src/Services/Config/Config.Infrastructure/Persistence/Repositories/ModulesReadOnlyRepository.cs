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
    public class ModulesReadOnlyRepository : DbRepositoryRead<Module>, IModulesReadOnlyRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public ModulesReadOnlyRepository(ConfigReadOnlyDbContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IModulesReadOnlyRepository

        public async Task<bool> AnyAsync(ModulesSearchCriteria criteria)
        {
            var query = base.GetQueryable(true);
            query = ApplyFilter(query, criteria);
            return await query.AnyAsync();
        }
        public async Task<SearchResults<Module>> GetAsync(ModulesSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);
            var query = base.GetQueryable(true, query: null, null, includeProperties);
            query = ApplyFilter(query, criteria);
            var queryPaged = base.GetQueryable(true, query, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Module>()
            {
                Results = await queryPaged.ToListAsync(),
                Metadata = new SearchResults<Module>.MetadataHeader()
                {
                    TotalCount = await query.CountAsync(),
                    PageIndex = criteria.PageNumber,
                    PageSize = criteria.PageSize,
                },
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

        private IQueryable<Module> ApplyFilter(IQueryable<Module> query, ModulesSearchCriteria criteria)
        {
            #region Id

            if (criteria.Id != null && criteria.Id != 0)
            {
                query = query.Where(x => criteria.Id == x.Id);
            }

            #endregion
            #region Name

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                query = query.Where(x => criteria.Name == x.Name ||
                                         criteria.Name == x.NameCultured);
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
        private void ApplyPaging(ModulesSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Module>, IOrderedQueryable<Module>> ApplySorting(ModulesSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != ModulesSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Module>, IOrderedQueryable<Module>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case ModulesSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc
                                         ? x.OrderByDescending(y => y.CreationDateTimeUtc)
                                         : x.OrderBy(y => y.CreationDateTimeUtc);
                        }
                        break;
                    case ModulesSearchCriteria.OrderByExpression.Name:
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

            return orderBy;
        }

        #endregion
    }
}
