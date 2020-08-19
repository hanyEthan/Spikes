using Microsoft.EntityFrameworkCore;
using XCore.Services.Personnel.DataLayer.Constants;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.DataLayer.Context.Configurations.Settings
{
    public static class SettingDBConfiguration
    {
        public static void HandleDBConfigurationForSetting(this ModelBuilder modelBuilder)
        {
            #region SettingModel.

            modelBuilder.Entity<Setting>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableSetting, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // model

                // Entity
                entity.Property(x => x.IsActive).HasColumnName("IsActive");
                entity.Property(x => x.Code).HasColumnName("Code");
                entity.Property(x => x.Name).HasColumnName("Name");
                entity.Property(x => x.NameCultured).HasColumnName("NameCultured");
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.
                // one-many : Account ( two sided ).
                entity.HasOne(config => config.Account)
                      .WithMany(x => x.Settings)
                      .HasForeignKey(config => config.AccountId);
                #endregion
            });

            #endregion
        }
    }

}
