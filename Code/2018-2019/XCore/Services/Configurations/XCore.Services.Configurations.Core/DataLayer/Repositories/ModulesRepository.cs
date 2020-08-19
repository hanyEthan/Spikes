﻿using System;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Configurations.Core.DataLayer.Context;
using XCore.Services.Configurations.Core.DataLayer.Contracts;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Support;

namespace XCore.Services.Configurations.Core.DataLayer.Repositories
{
    public class ModulesRepository : Repository<Module>, IModulesRepository
    {
        #region props.
        public bool? Initialized { get; protected set; }
        #endregion
        #region cst.

        public ModulesRepository(ConfigDataContext context) : base(context)
        {
            this.Initialized = Initialize();

        }

        #endregion
        #region IModulesRepository

        public async Task<bool> AnyAsync(ModuleSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<Module>> GetAsync(ModuleSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Module>()
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

        private ExpressionStarter<Module> GetQuery(ModuleSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Module>(true);

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

            if (criteria.AppIds != null && criteria.AppIds[0] != default)
            {
                predicate = predicate.And(x => criteria.AppIds.Contains(x.AppId));
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
        private void ApplyPaging(ModuleSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Module>, IOrderedQueryable<Module>> ApplySorting(ModuleSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != AppSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Module>, IOrderedQueryable<Module>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case ModuleSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case ModuleSearchCriteria.OrderByExpression.Name:
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
                orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.Name)
                             : isDesc && isCultured ? x.OrderByDescending(y => y.NameCultured)
                             : !isDesc && !isCultured ? x.OrderBy(y => y.Name)
                             : !isDesc && isCultured ? x.OrderBy(y => y.NameCultured)
                             : null;
            }

            return orderBy;
        }

        #endregion
    }
}