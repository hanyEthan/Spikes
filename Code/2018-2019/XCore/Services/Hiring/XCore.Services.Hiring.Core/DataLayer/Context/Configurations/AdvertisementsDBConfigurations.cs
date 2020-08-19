using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.DataLayer.Context.Configurations
{
    public static class AdvertisementsDBConfigurations
    {
        public static void HandleAdvertisementsDBConfigurations(this ModelBuilder modelBuilder)
        {          
            modelBuilder.Entity<Advertisement>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.AdvertisementsTable, DBConstants.Schema).HasKey(x => x.Id);

                #endregion
                #region relations.

                entity.HasMany(x => x.Positions)
                      .WithOne(x => x.Advertisement)
                      .HasForeignKey(x => x.AdvertisementId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Questions)
                      .WithOne(x => x.Advertisement)
                      .HasForeignKey(x => x.AdvertisementId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(x => x.Role)
                      .WithMany()
                      .HasForeignKey(x => x.RoleId);

                entity.HasOne(x => x.Organization)
                      .WithMany()
                      .HasForeignKey(x => x.OrganizationId);

                entity.HasOne(x => x.HiringProcces)
                      .WithMany()
                      .HasForeignKey(x => x.HiringProccesId);

                #endregion
                #region props.

                // common
                entity.Property(x => x.Code).HasColumnName("Code");
                entity.Property(x => x.Title).HasColumnName("Title");
                entity.Property(x => x.IsActive).HasColumnName("IsActive");
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                // ignore
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.NameCultured);

                // model
                entity.Property(x => x.AppId).HasColumnName("AppId");
                entity.Property(x => x.ModuleId).HasColumnName("ModuleId");

                #endregion
            });
        }
    }
}
