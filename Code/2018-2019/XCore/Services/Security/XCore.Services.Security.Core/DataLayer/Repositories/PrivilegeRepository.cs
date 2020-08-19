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
   public class PrivilegeRepository : Repository<Privilege>, IPrivilegeRepository
    {
        #region props.
        public bool? Initialized { get; protected set; }
        #endregion
        #region cst.

        public PrivilegeRepository(SecurityDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IPrivilegeRepository

        public async Task<bool> AnyAsync(PrivilegeSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<Privilege>> GetAsync(PrivilegeSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Privilege>()
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
        private ExpressionStarter<Privilege> GetQuery(PrivilegeSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Privilege>(true).And(p=>p.IsActive);

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
            #region Code

            if (!string.IsNullOrWhiteSpace(criteria.Code))
            {
                predicate = predicate.And(x => x.Code == criteria.Code);
            }

            #endregion
            #region Active

            if (criteria.IsActive.HasValue)
            {
                predicate = predicate.And(x => x.IsActive == criteria.IsActive);
            }

            #endregion

            #region AppId

            if (criteria.AppId != null)
            {
                predicate = predicate.And(x => x.AppId == criteria.AppId && x.App.IsActive);
            }

            #endregion
            #region AppCode

            if (!string.IsNullOrWhiteSpace(criteria.AppCode))
            {
                predicate = predicate.And(x => x.App.Code == criteria.AppCode && x.App.IsActive);
            }

            #endregion

            #region ActorId

            if (criteria.ActorId != null)
            {
                if (criteria.IsRecursive)
                {
                    predicate = predicate.And(x => (this.context.Set<ActorPrivilege>().Any(ap => ap.PrivilegeId == x.Id &&
                                          this.context.Set<Actor>().Any(a => a.Id == ap.ActorId && a.Id == criteria.ActorId && a.IsActive == true)))
                                          ||
                                          (this.context.Set<RolePrivilege>().Any(rp => rp.PrivilegeId == x.Id &&
                                          this.context.Set<Role>().Any(r => r.Id == rp.RoleId && r.IsActive == true &&
                                          this.context.Set<ActorRole>().Any(ar => ar.RoleId == r.Id &&
                                          this.context.Set<Actor>().Any(a => a.Id == ar.ActorId && a.IsActive == true && a.Id == criteria.ActorId)))))
                                          );
                }
                else
                {
                    predicate = predicate.And(x => this.context.Set<ActorPrivilege>().Any(y => y.PrivilegeId == x.Id &&
                                                   this.context.Set<Actor>().Any(z => z.Id == y.ActorId && z.Id == criteria.ActorId && z.IsActive == true)));
                }
            }

            #endregion
            #region ActorCode

            if (!string.IsNullOrWhiteSpace(criteria.ActorCode))
            {
                if (criteria.IsRecursive)
                {
                    predicate = predicate.And(x => (this.context.Set<ActorPrivilege>().Any(ap => ap.PrivilegeId == x.Id &&
                                          this.context.Set<Actor>().Any(a => a.Id == ap.ActorId && a.Code == criteria.ActorCode && a.IsActive == true)))
                                          ||
                                          (this.context.Set<RolePrivilege>().Any(rp => rp.PrivilegeId == x.Id &&
                                          this.context.Set<Role>().Any(r => r.Id == rp.RoleId && r.IsActive == true &&
                                          this.context.Set<ActorRole>().Any(ar => ar.RoleId == r.Id &&
                                          this.context.Set<Actor>().Any(a => a.Id == ar.ActorId && a.IsActive == true && a.Code == criteria.ActorCode)))))
                                          );
                }
                else
                {
                    predicate = predicate.And(x => this.context.Set<ActorPrivilege>().Any(y => y.PrivilegeId == x.Id &&
                                                   this.context.Set<Actor>().Any(z => z.Id == y.ActorId && z.Code == criteria.ActorCode && z.IsActive == true)));
                }
            }

            #endregion

            #region RoleId

            if (criteria.RoleId != null)
            {
                predicate = predicate.And(x => this.context.Set<RolePrivilege>().Any(y => y.PrivilegeId == x.Id &&
                                               this.context.Set<Role>().Any(z => z.Id == y.RoleId && z.Id == criteria.RoleId && z.IsActive == true)));
            }

            #endregion
            #region RoleCode

            if (!string.IsNullOrWhiteSpace(criteria.RoleCode))
            {
                predicate = predicate.And(x => this.context.Set<RolePrivilege>().Any(y => y.PrivilegeId == x.Id &&
                                               this.context.Set<Role>().Any(z => z.Id == y.RoleId && z.Code == criteria.RoleCode && z.IsActive == true)));
            }

            #endregion

            return predicate;
        }
        private void ApplyPaging(PrivilegeSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Privilege>, IOrderedQueryable<Privilege>> ApplySorting(PrivilegeSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != PrivilegeSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Privilege>, IOrderedQueryable<Privilege>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case PrivilegeSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case PrivilegeSearchCriteria.OrderByExpression.Name:
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
