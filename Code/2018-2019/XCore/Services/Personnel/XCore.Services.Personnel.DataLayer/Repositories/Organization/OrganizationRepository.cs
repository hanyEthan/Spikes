using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.DataLayer.Context;
using XCore.Services.Personnel.DataLayer.Contracts.Organizations;
using XCore.Services.Personnel.Models.Organizations;

namespace XCore.Services.Personnel.DataLayer.Repositories.Organizations
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public OrganizationRepository(PersonnelDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IOrganizationsRepository

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
            var order = OrganizationlySorting(criteria);
            OrganizationlyPaging(criteria, out int? skip, out int? take);

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

            #region Code

            if (criteria.Code != null && !string.IsNullOrEmpty(criteria.Code))
            {
                predicate = predicate.And(x => x.Code.Contains(criteria.Code));
            }

            #endregion
            #region Name

            if (criteria.Name != null && !string.IsNullOrEmpty(criteria.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(criteria.Name) || x.NameCultured.Contains(criteria.Name));
            }

            #endregion
            #region IsActive

            if (criteria.IsActive != null && criteria.IsActive.HasValue)
            {
                predicate = predicate.And(x => x.IsActive == criteria.IsActive);
            }

            #endregion
            #region OrganizationIds

            if (criteria.OrganizationReferenceId != null && criteria.OrganizationReferenceId.Count > 0)
            {
                predicate = predicate.And(x => criteria.OrganizationReferenceId.Contains(x.OrganizationReferenceId));
            }

            #endregion
          
            return predicate;
        }
        private void OrganizationlyPaging(OrganizationSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Organization>, IOrderedQueryable<Organization>> OrganizationlySorting(OrganizationSearchCriteria criteria)
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
