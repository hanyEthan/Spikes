using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Geo.Core.Models.Search
{
    public class LocationEventSearchCriteria : SearchCriteria
    {
        #region nested.

        public enum OrderByExepression
        {
            Default = 0,
            CreatedDate = 1,
        }

        #endregion
        #region criteria.

        public string EntityCode { get; set; }
        public OrderByExepression? OrderBy { get; set; }

        #endregion
    }
}
