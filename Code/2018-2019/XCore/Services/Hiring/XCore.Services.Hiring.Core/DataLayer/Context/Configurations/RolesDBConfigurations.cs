using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.DataLayer.Context.Configurations
{
    public static class RolesDBConfigurations
    {
        public static void HandleRolesDBConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.RolesTable, DBConstants.Schema).HasKey(x => x.Id);

                #endregion
                #region relations.

                entity.HasOne(x => x.Organization)
                      .WithMany(x => x.Roles)
                      .HasForeignKey(x => x.OrganizationId);

                #endregion
                #region props.

                // common
                entity.Property(x => x.Code).HasColumnName("Code");
                entity.Property(x => x.Name).HasColumnName("Name");
                entity.Property(x => x.NameCultured).HasColumnName("NameCultured");
                entity.Property(x => x.IsActive).HasColumnName("IsActive");
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                // model

                #endregion
            });
        }
    }
}
