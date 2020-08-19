﻿using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.DataLayer.Context;
using XCore.Services.Personnel.DataLayer.Contracts.Settings;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.DataLayer.Repositories.Settings
{
    public class SettingRepository : Repository<Setting>, ISettingRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public SettingRepository(PersonnelDataContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region ISettingsRepository

        public async Task<bool> AnyAsync(SettingSearchCriteria criteria)
        {
            var predicate = GetQuery(criteria);
            var query = base.GetQueryable(true, predicate);

            return await query.AnyAsync();
        }
        public async Task<SearchResults<Setting>> GetAsync(SettingSearchCriteria criteria, string includeProperties = null)
        {
            // prep ...
            var predicate = GetQuery(criteria);
            var order = SettinglySorting(criteria);
            SettinglyPaging(criteria, out int? skip, out int? take);

            var query = base.GetQueryable(true, predicate);
            var queryPaged = base.GetQueryable(true, predicate, order, includeProperties, skip, take);

            // query ...
            return new SearchResults<Setting>()
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

        private ExpressionStarter<Setting> GetQuery(SettingSearchCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Setting>(true);

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
            #region AccountIds

            if (criteria.AccountIds != null && criteria.AccountIds.Count > 0)
            {
                predicate = predicate.And(x => criteria.AccountIds.Contains(x.AccountId));
            }

            #endregion
           

            return predicate;
        }
        private void SettinglyPaging(SettingSearchCriteria criteria, out int? skip, out int? take)
        {
            skip = (criteria.PageNumber == null ? null : (int?)criteria.PageNumber - 1) * (criteria.PageSize == null ? null : (int?)criteria.PageSize);
            take = criteria.PageSize == null ? null : (int?)criteria.PageSize;
        }
        private Func<IQueryable<Setting>, IOrderedQueryable<Setting>> SettinglySorting(SettingSearchCriteria criteria)
        {
            bool isCultured = criteria.OrderByCultureMode != SettingSearchCriteria.OrderByCulture.Default;
            bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

            Func<IQueryable<Setting>, IOrderedQueryable<Setting>> orderBy = null;

            if (criteria.Order != null)
            {
                switch (criteria.Order.Value)
                {
                    case SettingSearchCriteria.OrderByExpression.CreationDate:
                        {
                            orderBy = x => isDesc && !isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : isDesc && isCultured ? x.OrderByDescending(y => y.CreatedDate)
                                         : !isDesc && !isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : !isDesc && isCultured ? x.OrderBy(y => y.CreatedDate)
                                         : null;
                        }
                        break;
                    case SettingSearchCriteria.OrderByExpression.Name:
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
