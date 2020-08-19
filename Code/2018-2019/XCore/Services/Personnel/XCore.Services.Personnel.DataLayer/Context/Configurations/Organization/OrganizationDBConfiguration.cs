using Microsoft.EntityFrameworkCore;
using XCore.Services.Personnel.DataLayer.Constants;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.Organizations;

namespace XCore.Services.Personnel.DataLayer.Context.Configurations.Organizations
{
    public static class OrganizationDBConfiguration
    {
        public static void HandleDBConfigurationForOrganization(this ModelBuilder modelBuilder)
        {
            #region PersonnelTrail.

            modelBuilder.Entity<Organization>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableOrganization, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.AppId).HasColumnName("AppId");
                entity.Property(x => x.ModuleId).HasColumnName("ModuleId");
                entity.Property(x => x.OrganizationReferenceId).HasColumnName("OrganizationReferenceId");
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
                // one-one : Account ( one sided ).
                entity.HasOne(s => s.Account)
                .WithOne(ad => ad.Organization)
                .HasForeignKey<OrganizationAccount>(ad => ad.OrganizationId);
                #endregion
            });

            #endregion
        }
    }

}
