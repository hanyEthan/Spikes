using Microsoft.EntityFrameworkCore;
using XCore.Services.Configurations.Core.Models;
using XCore.Services.Configurations.Core.Models.Domain;

namespace XCore.Services.Configurations.Core.DataLayer.Context.Configurations
{
    public static class AppDBConfiguration
    {
        public static void HandleDBConfigurationForApp(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<App>(entity =>
            {
                #region table.

                entity.ToTable(Constants.DB.TableApps, Constants.DB.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.Description).HasColumnName("Description");

                // Entity
                entity.Property(x => x.Code).HasColumnName("Code").IsRequired();
                entity.Property(x => x.Name).HasColumnName("Name").IsRequired();
                entity.Property(x => x.NameCultured).HasColumnName("NameCultured");
                entity.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
            });

            #endregion
        }
    }
}
