using Microsoft.EntityFrameworkCore;
using XCore.Services.Personnel.DataLayer.Constants;
using XCore.Services.Personnel.Models.Departments;

namespace XCore.Services.Personnel.DataLayer.Context.Configurations.Departments
{
    public static class DepartmentDBConfiguration
    {
        public static void HandleDBConfigurationForDepartment(this ModelBuilder modelBuilder)
        {
            #region DepartmentModel.

            modelBuilder.Entity<Department>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableDepartment, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.AppId).HasColumnName("AppId");
                entity.Property(x => x.ModuleId).HasColumnName("ModuleId");
                entity.Property(x => x.DepartmentReferenceId).HasColumnName("DepartmentReferenceId");
                entity.Property(x => x.HeadDepartmentId).HasColumnName("HeadDepartmentId");

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
                // one-many : HeadDepartment ( one sided ).
                entity.HasOne(config => config.HeadDepartment)
                      .WithMany()
                      .HasForeignKey(config => config.HeadDepartmentId);
                #endregion
            });

            #endregion
        }
    }

}
