using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Services.Geo.Core.DataLayer.Contracts;
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.Core.DataLayer.Repositories
{
    public class LocationEventLatestRepository : Repository<LocationEventLatest>, ILocationEventLatestRepository
    {
        #region cst.
        public LocationEventLatestRepository(DbContext context) : base(context)
        {
        }

        #endregion
        #region Publics


        #endregion
    }
}
