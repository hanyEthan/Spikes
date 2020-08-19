using Microsoft.EntityFrameworkCore;
using XCore.Services.Audit.Core.DataLayer.Context.Configurations;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.Core.DataLayer.Context
{
    public class AuditDataContext : DbContext
    {
        #region Props.

        #endregion
        #region DbSets

        public virtual DbSet<AuditTrail> AuditTrail { get; set; }

        #endregion

        #region cst.

        public AuditDataContext(DbContextOptions options) : base(options)
        {
        }
      
        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleDBConfigurationForAudit();
        }

        #endregion
    }
}
