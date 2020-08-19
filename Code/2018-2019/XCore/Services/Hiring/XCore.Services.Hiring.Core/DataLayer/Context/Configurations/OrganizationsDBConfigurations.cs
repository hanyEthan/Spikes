using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.DataLayer.Context.Configurations
{
    public static class OrganizationsDBConfigurations
    {
        public static void HandleOrganizationsDBConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.OrganizationsTable, DBConstants.Schema).HasKey(x => x.Id);

                #endregion
                #region relations.

                entity.HasMany(x => x.HiringProcesses)
                      .WithOne(x => x.Organization)
                      .HasForeignKey(x => x.OrganizationId);

                entity.HasMany(x => x.Roles)
                      .WithOne(x => x.Organization)
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
                entity.Property(x => x.AppId).HasColumnName("AppId");
                entity.Property(x => x.ModuleId).HasColumnName("ModuleId");
                entity.Property(x => x.OrganizationReferenceId).HasColumnName("OrganizationReferenceId");

                #endregion
            });
        }
    }
}
