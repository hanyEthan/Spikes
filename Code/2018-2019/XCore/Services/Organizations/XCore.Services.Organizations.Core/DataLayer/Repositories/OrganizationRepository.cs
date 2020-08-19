using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.DataLayer.Context;
using XCore.Services.Organizations.Core.DataLayer.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Repositories
{
    public  class OrganizationRepository :Repository<Organization>, IOrganizationRepository
    {
        #region props.
        
        public bool? Initialized { get; protected set; }
        
        #endregion
        #region cst.

        public OrganizationRepository(OrganizationDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IOrganizationRepository

        public async Task<bool> AnyAsync(OrganizationSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<Organization>> GetAsync(OrganizationSearchCriteria criteria, string includeProperties = null)
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

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        private ExpressionStarter<Organization> GetQuery(OrganizationSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Organization>(true);

            #region Id

            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(x => criteria.Ids.Contains(x.Id));
            }

            #endregion 
            #region parent Organization Id

            if (criteria.ParentOrganizationId.HasValue)
            {
                predicate = predicate.And(x => criteria.ParentOrganizationId.Value == x.ParentOrganizationId);
            }

            #endregion
            #region Delegate Id

            if (criteria.DelegateId.HasValue)
            {
                predicate = predicate.And(x => x.OrganizationDelegates.Any(y => y.DelegateId == criteria.DelegateId));
            }

            #endregion
            #region Delegator Id

            if (criteria.DelegatorId.HasValue)
            {
                predicate = predicate.And(x => x.OrganizationDelegators.Any(y => y.DelegatorId == criteria.DelegatorId));
            }

            #endregion
            #region Name

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(criteria.Name.Trim()));
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
        private void ApplyPaging(OrganizationSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Organization>, IOrderedQueryable<Organization>> ApplySorting(OrganizationSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != OrganizationSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Organization>, IOrderedQueryable<Organization>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case OrganizationSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case OrganizationSearchCriteria.OrderByExpression.Name:
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
