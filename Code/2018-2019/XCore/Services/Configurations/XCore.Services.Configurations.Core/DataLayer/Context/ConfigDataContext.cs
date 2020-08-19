using Microsoft.EntityFrameworkCore;
using XCore.Services.Configurations.Core.DataLayer.Context.Configurations;
using XCore.Services.Configurations.Core.Models.Domain;

namespace XCore.Services.Configurations.Core.DataLayer.Context
{
    public class ConfigDataContext: DbContext
    {
        #region props.

     

        #endregion
        #region DbSets

        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ConfigItem> Configs { get; set; }

        #endregion
        #region cst.
       
        public ConfigDataContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region base.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleDBConfigurationForApp();
            modelBuilder.HandleDBConfigurationForModule();
            modelBuilder.HandleDBConfigurationForConfig();
        }

        #endregion
    }
}
