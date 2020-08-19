﻿using LinqKit;
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
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public OrganizationRepository(HiringDataContext dataContext) : base(dataContext)
        {
            this.Initialized = Initialize();
        }

        #endregion

        #region IOrganizationRepository

        public async Task<SearchResults<Organization>> Get(OrganizationsSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Organization>()
            {
                Results = await queryPaged.ToListAsync(),
                TotalCount = await query.CountAsync(),
            };
        }
        public async Task<bool> AnyAsync(OrganizationsSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        #endregion
        #region helpers.

        private ExpressionStarter<Organization> GetQuery(OrganizationsSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Organization>(true);
            #region Id

            if (criteria.Id.HasValue && criteria.Id > 0)
            {
                predicate = predicate.And(x => criteria.Id.Equals(x.Id));
            }

            #endregion
            #region Name

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                predicate = predicate.And(x => criteria.Name.Contains(x.Name));
            }

            #endregion
            #region Code

            if (criteria.Codes != null && criteria.Codes.Count > 0)
            {
                predicate = predicate.And(x => criteria.Codes.Contains(x.Code));
            }

            #endregion  
            #region Activation

            if (criteria.IsActive.HasValue)
            {
                predicate = predicate.And(x => x.IsActive == criteria.IsActive);
            }

            #endregion

            #region Apps

            if (criteria.Apps != null && criteria.Apps.Count > 0)
            {
                predicate = predicate.And(x => criteria.Apps.Contains(x.AppId));
            }

            #endregion
            #region Modules

            if (criteria.Modules != null && criteria.Modules.Count > 0)
            {
                predicate = predicate.And(x => criteria.Modules.Contains(x.ModuleId));
            }

            #endregion

            return predicate;
        }
        private Func<IQueryable<Organization>, IOrderedQueryable<Organization>> ApplySorting(OrganizationsSearchCriteria criteria)
        {
            if (criteria.Order == null) return null;
            bool isCultured = criteria.OrderByCultureMode != SearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Organization>, IOrderedQueryable<Organization>> orderBy = null;

            switch (criteria.Order.Value)
            {
                case OrganizationsSearchCriteria.OrderByExpression.CreationDate:
                    {
                        orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                     : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                     : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                     : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                     : null;
                    }
                    break;
                case OrganizationsSearchCriteria.OrderByExpression.Name:
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
        private void ApplyPaging(OrganizationsSearchCriteria criteria, out int? skip, out int? take)
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