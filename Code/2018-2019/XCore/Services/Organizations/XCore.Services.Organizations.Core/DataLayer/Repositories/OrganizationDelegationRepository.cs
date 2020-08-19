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
    class OrganizationDelegationRepository : Repository<OrganizationDelegation>, IOrganizationDelegationRepository
    {
        #region props.
        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public OrganizationDelegationRepository(OrganizationDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IContactInfoRepository

        public async Task<bool> AnyAsync(OrganizationDelegationSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<OrganizationDelegation>> GetAsync(OrganizationDelegationSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<OrganizationDelegation>()
            {
                Results =await queryPaged.ToListAsync(),
                TotalCount =await query.CountAsync(),
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

        private ExpressionStarter<OrganizationDelegation> GetQuery(OrganizationDelegationSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<OrganizationDelegation>(true);

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
            #region delegate Id

            if (criteria.DelegateId.HasValue)
            {
                predicate = predicate.And(x => criteria.DelegateId.Value == x.DelegateId);
            }

            #endregion
            #region Delegator Id

            if (criteria.DelegatorId.HasValue)
            {
                predicate = predicate.And(x => criteria.DelegatorId.Value == x.DelegatorId);
            }

            #endregion

            return predicate;
        }
        private void ApplyPaging(OrganizationDelegationSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<OrganizationDelegation>, IOrderedQueryable<OrganizationDelegation>> ApplySorting(OrganizationDelegationSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != OrganizationDelegationSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<OrganizationDelegation>, IOrderedQueryable<OrganizationDelegation>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case OrganizationDelegationSearchCriteria.OrderByExpression.CreationDate:
                    default:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
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
