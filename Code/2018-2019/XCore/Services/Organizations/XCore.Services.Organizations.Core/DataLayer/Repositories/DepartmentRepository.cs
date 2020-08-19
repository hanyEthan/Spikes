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
    public  class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        #region props.
        public bool? Initialized { get; protected set; }
        #endregion
        #region cst.

        public DepartmentRepository(OrganizationDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IDepartmentRepository

        public async Task<bool> AnyAsync(DepartmentSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<Department>> GetAsync(DepartmentSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...


            return new SearchResults<Department>()
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

        private ExpressionStarter<Department> GetQuery(DepartmentSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Department>(true);

            #region Id

            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(x => criteria.Ids.Contains(x.Id));
            }

            #endregion
            #region Name

            if (!string.IsNullOrEmpty(criteria.Name))
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
            #region Organization Id

            if (criteria.OrganizationId.HasValue)
            {
                predicate = predicate.And(x => x.OrganizationId == criteria.OrganizationId.Value);
            }

            #endregion
            #region Parent Department Id

            if (criteria.ParentDepartmentId.HasValue)
            {
                predicate = predicate.And(x => x.ParentDepartmentId == criteria.ParentDepartmentId.Value);
            }

            #endregion

            return predicate;
        }
        private void ApplyPaging(DepartmentSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Department>, IOrderedQueryable<Department>> ApplySorting(DepartmentSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != DepartmentSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Department>, IOrderedQueryable<Department>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case DepartmentSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case DepartmentSearchCriteria.OrderByExpression.Name:
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
