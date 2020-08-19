using Microsoft.EntityFrameworkCore;
using XCore.Services.Organizations.Core.Constants;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.Core.DataLayer.Context.Configurations
{
    public static class OrganizationDelegationConfiguration
    {
        public static void HandleOrganizationDelegationConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<OrganizationDelegation>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableOrganizationDelegation, DBConstants.Schema);
                entity.HasKey(sc => new { sc.DelegateId, sc.DelegatorId });

                #endregion
                #region props.

                // model


                // Entity
                entity.Ignore(x => x.Id);
                entity.Property(x => x.Code).HasColumnName("Code").IsRequired();
                entity.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region Relationship.

                entity.HasOne<Organization>(x => x.Delegate)
                      .WithMany(x => x.OrganizationDelegates)
                      .HasForeignKey(x => x.DelegateId);

                entity.HasOne<Organization>(x => x.Delegator)
                    .WithMany(x => x.OrganizationDelegators)
                    .HasForeignKey(x => x.DelegatorId);

                #endregion
            });

            #endregion
        }
    }
}
