using Microsoft.EntityFrameworkCore;
using XCore.Services.Audit.Core.Constants;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.Core.DataLayer.Context.Configurations
{
    public static class AuditDBConfiguration
    {
        public static void HandleDBConfigurationForAudit(this ModelBuilder modelBuilder)
        {
            #region AuditTrail.

            modelBuilder.Entity<AuditTrail>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableAudits, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.UserId).HasColumnName("UserId");
                entity.Property(x => x.UserName).HasColumnName("UserName");
                entity.Property(x => x.App).HasColumnName("App");
                entity.Property(x => x.Module).HasColumnName("Module");
                entity.Property(x => x.Action).HasColumnName("Action");
                entity.Property(x => x.Entity).HasColumnName("Entity");
                entity.Property(x => x.Text).HasColumnName("Text");
                entity.Property(x => x.SourceIP).HasColumnName("SourceIP");
                entity.Property(x => x.SourcePort).HasColumnName("SourcePort");
                entity.Property(x => x.SourceOS).HasColumnName("SourceOS");
                entity.Property(x => x.SourceClient).HasColumnName("SourceClient");
                entity.Property(x => x.DestinationIP).HasColumnName("DestinationIP");
                entity.Property(x => x.DestinationPort).HasColumnName("DestinationPort");
                entity.Property(x => x.DestinationAddress).HasColumnName("DestinationAddress");
                entity.Property(x => x.ConnectionMethod).HasColumnName("ConnectionMethod");
                entity.Property(x => x.Level).HasColumnName("Level");
                entity.Property(x => x.SyncStatus).HasColumnName("SyncStatus");

                // Entity
                entity.Property(x => x.Code).HasColumnName("Code");
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.NameCultured);
                entity.Ignore(x => x.IsActive);
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                #endregion
            });

            #endregion
        }
    }

}
