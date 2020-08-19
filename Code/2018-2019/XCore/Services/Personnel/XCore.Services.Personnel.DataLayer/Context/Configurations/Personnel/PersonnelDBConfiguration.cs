using Microsoft.EntityFrameworkCore;
using XCore.Services.Personnel.DataLayer.Constants;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.Personnels;

namespace XCore.Services.Personnel.DataLayer.Context.Configurations.Personnels
{
    public static class PersonnelDBConfiguration
    {
        public static void HandleDBConfigurationForPersonnel(this ModelBuilder modelBuilder)
        {
            #region PersonnelModel.

            modelBuilder.Entity<Person>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TablePersonnel, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
            #region props.

            // model
    
                entity.Property(x => x.AppId).HasColumnName("AppId");
                entity.Property(x => x.ModuleId).HasColumnName("ModuleId");

                entity.Property(x => x.ManagerId).HasColumnName("ManagerId");
                entity.Property(x => x.DepartmentId).HasColumnName("DepartmentId");
                // entity.Property(x => x.AccountId).HasColumnName("AccountId");
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
                      .WithOne(ad => ad.Person)
                      .HasForeignKey<PersonnelAccount>(ad => ad.PersonId);

                // one-many : Department ( one sided ).
                entity.HasOne(config => config.Department)
                      .WithMany()
                      .HasForeignKey(config => config.DepartmentId);

                // one-many : Manager ( one sided ).
                entity.HasOne(config => config.Manager)
                      .WithMany()
                      .HasForeignKey(config => config.ManagerId);
                #endregion
            });
            #endregion
        }
    }

}
