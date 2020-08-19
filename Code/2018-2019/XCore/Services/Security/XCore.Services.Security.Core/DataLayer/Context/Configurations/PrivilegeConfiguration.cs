using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Utilities;


namespace XCore.Services.Security.Core.DataLayer.Context.Configurations
{
    public static class PrivilegeConfiguration
    {
        public static void HandlePrivilegeConfiguration(this ModelBuilder modelentity)
        {
            modelentity.Entity<Privilege>(entity =>
            {
                #region table.

                entity.ToTable(Constants.TablePrivileges, Constants.DBSchema);
                entity.HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.Description).HasColumnName("Description");

                // Entity
                entity.Property(x => x.Code).HasColumnName("Code").IsRequired();
                entity.Property(x => x.Name).HasColumnName("Name");
                entity.Property(x => x.NameCultured).HasColumnName("NameCultured");
                entity.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                // one-many : App ( one sided ).
                //entity.HasOne(x => x.App)
                //       .WithMany()
                //       .HasForeignKey(x => x.AppId);

                #endregion
            });
        }
    }
}