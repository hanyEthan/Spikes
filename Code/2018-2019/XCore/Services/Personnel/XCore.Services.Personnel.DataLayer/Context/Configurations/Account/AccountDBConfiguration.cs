using Microsoft.EntityFrameworkCore;
using XCore.Services.Personnel.DataLayer.Constants;
using XCore.Services.Personnel.Models.Accounts;

namespace XCore.Services.Personnel.DataLayer.Context.Configurations.Accounts
{
    public static class AccountDBConfiguration
    {
        public static void HandleDBConfigurationForAccount(this ModelBuilder modelBuilder)
        {
            #region PersonnelTrail.

            modelBuilder.Entity<AccountBase>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableAccount, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.AccountType).HasColumnName("AccountType");
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
                #region Configuration.
                entity.HasDiscriminator<string>("AccountType")
                .HasValue<PersonnelAccount>(Models.Constants.Constants.AccountTypes.Personnel)
                .HasValue<OrganizationAccount>(Models.Constants.Constants.AccountTypes.Organization);
                #endregion
            });

            #endregion
        }
    }

}
