using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Geo.Core.DataLayer.Contracts;
using XCore.Services.Geo.Core.Models.Domain;
using XCore.Services.Geo.Core.Models.Search;

namespace XCore.Services.Geo.Core.DataLayer.Repositories
{
    public class LocationEventRepository : Repository<LocationEvent>, ILocationEventRepository
    {
        #region cst.

        public LocationEventRepository(DbContext context) : base(context)
        {
           
        }

        #endregion

        #region ILocationEventRepository

        public virtual bool AddLocationEvent(LocationEvent locationEvent)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@EntityCode", locationEvent.EntityCode),
                    new SqlParameter("@EntityType", locationEvent.EntityType),
                    new SqlParameter("@EventCode", locationEvent.EventCode),
                    new SqlParameter("@Longitude", locationEvent.Longitude),
                    new SqlParameter("@Latitude", locationEvent.Latitude),
                    new SqlParameter("@ModifiedDate", locationEvent.ModifiedDate),
                    new SqlParameter("@MetaData",locationEvent.MetaData),
                    new SqlParameter("@IsActive", locationEvent.IsActive)
                };

                var sql = "AddLocationEvent @EntityCode, @EntityType, @EventCode, @Longitude, @Latitude, @ModifiedDate, @MetaData, @IsActive";
                XDB.ExecuteProcedure(base.context, sql, parameters);

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        public LocationEventsSearchResults GetLocations(LocationEventSearchCriteria criteria, string includeProperties = "", bool detached = false)
        {
            try
            {
                #region dynamic query.

                var predicate = GetLocationsQuery(criteria, includeProperties);
                var query = base.GetQueryable(detached, predicate);

                #endregion
                #region sorting

                Func<IQueryable<LocationEvent>, IOrderedQueryable<LocationEvent>> orderBy = null;
                bool isDesc = criteria.OrderByDirection == SearchCriteria.OrderDirection.Descending;

                if (criteria.OrderBy != null)
                {
                    switch (criteria.OrderBy.Value)
                    {
                        case LocationEventSearchCriteria.OrderByExepression.CreatedDate:
                            {
                                #region order

                                if (isDesc)
                                {
                                    orderBy = x => x.OrderByDescending(y => y.CreatedDate);
                                }
                                else if (!isDesc)
                                {
                                    orderBy = x => x.OrderBy(y => y.CreatedDate);
                                }

                                #endregion
                            }
                            break;
                        case LocationEventSearchCriteria.OrderByExepression.Default:
                        default:
                            break;
                    }
                }
                else
                {
                    //orderBy = x => x.OrderByDescending(y => y.CreatedDate);
                }

                #endregion
                #region paging

                var skip = (criteria.PageNumber - 1 ?? 0) * (criteria.PageSize ?? 0);
                var take = criteria.PageSize ?? 0;

                #endregion

                #region execute.

                var queryPaged = base.GetQueryable(detached, predicate, orderBy, includeProperties, skip, take);
                var result = new LocationEventsSearchResults();

                result.Results = queryPaged.ToList();
                result.TotalCount = query.Count();

                return result;

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region helpers

        private ExpressionStarter<LocationEvent> GetLocationsQuery(LocationEventSearchCriteria criteria, string includeProperties = "" )
        {
            #region conditions

            var predicate = PredicateBuilder.New<LocationEvent>(true);

            #region EntityCode

            if (!string.IsNullOrWhiteSpace(criteria.EntityCode))
            {
                predicate = predicate.And(x => x.EntityCode == criteria.EntityCode);
            }

            #endregion

            #endregion

            return predicate;
        }

        #endregion
    }
}
