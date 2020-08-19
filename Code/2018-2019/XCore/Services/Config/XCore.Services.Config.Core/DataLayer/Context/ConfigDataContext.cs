using Microsoft.EntityFrameworkCore;
using XCore.Framework.Utilities;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Utilities;

namespace XCore.Services.Config.Core.DataLayer.Context
{
    public class ConfigDataContext: DbContext
    {
        #region props.

        protected const string DefaultConnectionString = Constants.ConnectionString;

        #endregion
        #region DbSets

        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ConfigItem> Configs { get; set; }

        #endregion
        #region cst.
        public ConfigDataContext() : this(DefaultConnectionString)
        {
        }
        public ConfigDataContext(string connectionString) : this(new DbContextOptionsBuilder<ConfigDataContext>().UseSqlServer(XDB.GetConnectionString(connectionString)).Options)
        {
        }
        public ConfigDataContext(DbContextOptions options) : base(options)
        {
        }

        #endregion
        #region base
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigDataContextConfigrations.AppsConfiguration(Constants.TableApps));
            modelBuilder.ApplyConfiguration(new ConfigDataContextConfigrations.ModulesConfiguration(Constants.TableModules));
            modelBuilder.ApplyConfiguration(new ConfigDataContextConfigrations.ConfigsConfiguration(Constants.TableConfigs));
        }

        #endregion
    }
}
