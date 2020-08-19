using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.DataLayer.Context;
using XCore.Services.Security.Core.DataLayer.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.DataLayer.Repositories
{
   public class ActorRepository : Repository<Actor>, IActorRepository
    {
        #region props.
        public bool? Initialized { get; protected set; }
        #endregion
        #region cst.

        public ActorRepository(SecurityDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IConfigsRepository

        public async Task<bool> AnyAsync(ActorSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<Actor>> GetAsync(ActorSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Actor>()
            {
                Results = await queryPaged.ToListAsync(),
                TotalCount = await query.CountAsync(),
            };
        }
        public void DeletedAssociatedPrivilege(ActorPrivilege model)
        {
            this.Delete<ActorPrivilege>(model);
        }
        public void DeletedAssociatedRole(ActorRole model)
        {
            this.Delete<ActorRole>(model);
        }


        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }
        private ExpressionStarter<Actor> GetQuery(ActorSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Actor>(true); 

            #region Id

            if (criteria.Id != null && criteria.Id != 0)
            {
                predicate = predicate.And(x => x.Id == criteria.Id);
            }

            #endregion
            #region Name

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                predicate = predicate.And(x => x.Name.Contains(criteria.Name));
            }

            #endregion
            #region Active

            if (criteria.IsActive.HasValue)
            {
                predicate = predicate.And(x => x.IsActive == criteria.IsActive.Value);
            }

            #endregion
            #region Code

            if (!string.IsNullOrWhiteSpace(criteria.Code))
            {
                predicate = predicate.And(x => x.Code == criteria.Code);
            }

            #endregion
            #region AppId

            if (criteria.AppId != null && criteria.AppId != 0)
            {
                predicate = predicate.And(x => x.AppId == criteria.AppId && x.App.IsActive);
            }

            #endregion
            #region AppCode

            if (!string.IsNullOrWhiteSpace(criteria.AppCode))
            {
                predicate = predicate.And(x => x.App.Code == criteria.AppCode);
            }

            #endregion
            #region RoleId

            if (criteria.RoleId != null)
            {
                predicate = predicate.And(x => this.context.Set<ActorRole>().Any(y => y.ActorId == x.Id &&
                                                   this.context.Set<Role>().Any(z => z.Id == y.RoleId && z.Id == criteria.RoleId && z.IsActive == true)));
            }


            #endregion
            #region RoleCode

            if (!string.IsNullOrWhiteSpace(criteria.RoleCode))
            {
                predicate = predicate.And(x => this.context.Set<ActorRole>().Any(y => y.ActorId == x.Id &&
                                               this.context.Set<Role>().Any(z => z.Id == y.RoleId && z.Code == criteria.RoleCode && z.IsActive == true)));
            }

            #endregion
            
            

            return predicate;
        }
        private void ApplyPaging(ActorSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Actor>, IOrderedQueryable<Actor>> ApplySorting(ActorSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != ActorSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Actor>, IOrderedQueryable<Actor>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case ActorSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case ActorSearchCriteria.OrderByExpression.Name:
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
