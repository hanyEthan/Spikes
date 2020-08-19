using System.Reflection;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Context
{
    public class ConfigDbContext : DbContext
    {
        #region props.

        #endregion
        #region DbSets

        public virtual DbSet<Domain.Entities.Module> Modules { get; set; }
        public virtual DbSet<ConfigItem> Configs { get; set; }

        #endregion
        #region cst.

        public ConfigDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region base.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
