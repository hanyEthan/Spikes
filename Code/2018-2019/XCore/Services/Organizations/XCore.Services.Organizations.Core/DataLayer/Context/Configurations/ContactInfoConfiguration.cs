using Microsoft.EntityFrameworkCore;
using XCore.Services.Organizations.Core.Constants;
using XCore.Services.Organizations.Core.Models;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.Core.DataLayer.Context.Configurations
{
    public static  class ContactInfoConfiguration
    {
        public static void HandleDBConfigurationForContactInfo(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<ContactInfo>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableContactInfo, DBConstants.Schema);
                entity.HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.Description).HasColumnName("Description");
                entity.Property(x => x.Address).HasColumnName("Address");
                entity.Property(x => x.Email).HasColumnName("Email");
                entity.Property(x => x.PostalCode).HasColumnName("PostalCode");
                entity.Property(x => x.Phone).HasColumnName("Phone");
                entity.Property(x => x.Mobile).HasColumnName("Mobile");

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
