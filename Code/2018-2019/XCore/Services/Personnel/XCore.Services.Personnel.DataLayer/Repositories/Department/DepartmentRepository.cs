using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.DataLayer.Context;
using XCore.Services.Personnel.DataLayer.Contracts.Departments;
using XCore.Services.Personnel.Models.Departments;

namespace XCore.Services.Personnel.DataLayer.Repositories.Departments
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public DepartmentRepository(PersonnelDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IDepartmentsRepository

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
            var order = DepartmentlySorting(criteria);
            DepartmentlyPaging(criteria, out int? skip, out int? take);

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
            #region DepartmentReferenceId

            if (criteria.DepartmentReferenceId != null && criteria.DepartmentReferenceId.Count > 0)
            {
                predicate = predicate.And(x => criteria.DepartmentReferenceId.Contains(x.DepartmentReferenceId));
            }

            #endregion
            #region HeadDepartmentId

            if (criteria.HeadDepartmentId != null && criteria.HeadDepartmentId.Count > 0)
            {
                predicate = predicate.And(x => criteria.HeadDepartmentId.Contains(x.HeadDepartmentId));
            }

            #endregion

            return predicate;
        }
        private void DepartmentlyPaging(DepartmentSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Department>, IOrderedQueryable<Department>> DepartmentlySorting(DepartmentSearchCriteria criteria)
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
