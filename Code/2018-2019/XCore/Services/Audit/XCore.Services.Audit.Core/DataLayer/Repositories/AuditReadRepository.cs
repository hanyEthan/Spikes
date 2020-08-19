using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Audit.Core.DataLayer.Context;
using XCore.Services.Audit.Core.DataLayer.Contracts;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Config.Core.DataLayer.Repositories
{
    public class AuditReadRepository : RepositoryRead<AuditTrail> , IAuditReadRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public AuditReadRepository(AuditReadDataContext readdataContext) : base(readdataContext)
        {
            this.Initialized = Initialize();
        }

        #endregion

        #region IAuditRepository

        public async Task<SearchResults<AuditTrail>> GetAsync(AuditSearchCriteria criteria)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = ApplySorting(criteria);
            ApplyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, null, skip, take);

            // query ...
            return new SearchResults<AuditTrail>()
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

        private ExpressionStarter<AuditTrail> GetQuery(AuditSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<AuditTrail>(true);

            #region UserIds

            if (criteria.UserIds != null)
            {
                predicate = predicate.And(x => criteria.UserIds.Contains(x.UserId));
            }

            #endregion
            #region UserNames

            if (criteria.UserNames != null)
            {
                predicate = predicate.And(x => criteria.UserNames.Contains(x.UserName));
            }

            #endregion
            #region Apps

            if (criteria.Apps != null)
            {
                predicate = predicate.And(x => criteria.Apps.Contains(x.App));
            }

            #endregion
            #region Modules

            if (criteria.Modules != null)
            {
                predicate = predicate.And(x => criteria.Modules.Contains(x.Module));
            }

            #endregion
            #region Actions

            if (criteria.Actions != null)
            {
                predicate = predicate.And(x => criteria.Actions.Contains(x.Action));
            }

            #endregion
            #region Entities

            if (criteria.Entities != null)
            {
                predicate = predicate.And(x => criteria.Entities.Contains(x.Entity));
            }

            #endregion
            #region Text

            if (!string.IsNullOrWhiteSpace(criteria.Text))
            {
                predicate = predicate.And(x => x.Text.Contains(criteria.Text));
            }

            #endregion

            return predicate;
        }
        private void ApplyPaging(AuditSearchCriteria criteria, out int? skip, out int? take)
        {
           
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<AuditTrail>, IOrderedQueryable<AuditTrail>> ApplySorting(AuditSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != AuditSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<AuditTrail>, IOrderedQueryable<AuditTrail>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case AuditSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured  ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
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
                //orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                //             : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                //             : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                //             : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                //             : null;
            }

            return orderBy;
        }

        #endregion
    }
}
