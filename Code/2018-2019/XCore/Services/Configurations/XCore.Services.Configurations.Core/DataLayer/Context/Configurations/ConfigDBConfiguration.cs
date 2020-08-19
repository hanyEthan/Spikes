using Microsoft.EntityFrameworkCore;
using XCore.Services.Configurations.Core.Models;
using XCore.Services.Configurations.Core.Models.Domain;

namespace XCore.Services.Configurations.Core.DataLayer.Context.Configurations
{
    public static class ConfigDBConfiguration
    {
        public static void HandleDBConfigurationForConfig(this ModelBuilder modelBuilder)
        {
            #region Configuration.

            modelBuilder.Entity<ConfigItem>(entity =>
            {
                #region table.

                entity.ToTable(Constants.DB.TableConfigs, Constants.DB.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.Key).HasColumnName("Key").IsRequired();
                entity.Property(x => x.Value).HasColumnName("Value");
                entity.Property(x => x.Description).HasColumnName("Description");
                entity.Property(x => x.Type).HasColumnName("Type");
                entity.Property(x => x.ReadOnly).HasColumnName("ReadOnly").IsRequired();
                entity.Property(x => x.Version).HasColumnName("Version");

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
                #region relations.

                // one-many : App ( one sided ).
                entity.HasOne(config => config.App)
                      .WithMany()
                      .HasForeignKey(config => config.AppId);

                // one-many : Module.
                entity.HasOne(config => config.Module)
                      .WithMany(module => module.Configurations)
                      .HasForeignKey(config => config.ModuleId);

                #endregion
            });

            #endregion
        }
    }
}
