using Microsoft.EntityFrameworkCore;
using XCore.Framework.Utilities;
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.Core.DataLayer.Context
{
    public class GeoDataContext : DbContext
    {
        #region Props.

        protected const string DefaultConnectionString = "XCore.Geo";

        #endregion
        #region DbSets

        public virtual DbSet<LocationEvent> LocationEvents { get; set; }
        public virtual DbSet<LocationEventLatest> LocationEventsLatest { get; set; }

        #endregion

        #region cst.

        public GeoDataContext() : this(DefaultConnectionString)
        {
        }
        public GeoDataContext(string connectionString) : this(new DbContextOptionsBuilder<GeoDataContext>().UseSqlServer(XDB.GetConnectionString(connectionString)).Options)
        {
        }
        public GeoDataContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GeoDataContextConfigurations.LocationEventsConfiguration("LocationEvents"));
            modelBuilder.ApplyConfiguration(new GeoDataContextConfigurations.LocationEventsLatestConfiguration("LocationEventsLatest"));
        }

        #endregion
        #region helpers.

        //private void HandleReferences()
        //{
        //    var x = typeof( SqlProviderServices );
        //    var y = x.ToString();
        //}

        #endregion
    }
}
