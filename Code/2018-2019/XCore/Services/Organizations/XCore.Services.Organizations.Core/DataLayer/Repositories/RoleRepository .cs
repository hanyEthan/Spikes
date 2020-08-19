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
    public  class RoleRepository : Repository<Role>, IRoleRepository
    {
        #region props.
        public bool? Initialized { get; protected set; }
        #endregion
        #region cst.

        public RoleRepository(OrganizationDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IRoleRepository

        public async Task<bool> AnyAsync(RoleSearchCriteria criteria, string includeProperties = null)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate,null, includeProperties);

            return await query.AnyAsync();
        }
       

        public async Task<SearchResults<Role>> GetAsync(RoleSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Role>()
            {
                Results =await queryPaged.ToListAsync(),
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

        private ExpressionStarter<Role> GetQuery(RoleSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Role>(true);

            #region Id

            if (criteria.Id.HasValue)
            {
                predicate = predicate.And(x => x.Id == criteria.Id.Value);
            }

            #endregion
            #region Name

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(criteria.Name.Trim()));
            }

            #endregion
          
      
            return predicate;
        }
        private void ApplyPaging(RoleSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Role>, IOrderedQueryable<Role>> ApplySorting(RoleSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != RoleSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case RoleSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case RoleSearchCriteria.OrderByExpression.Name:
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
