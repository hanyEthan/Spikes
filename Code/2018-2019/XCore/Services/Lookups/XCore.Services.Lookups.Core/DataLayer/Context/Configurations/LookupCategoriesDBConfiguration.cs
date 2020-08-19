using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using XCore.Services.Lookups.Core.Constants;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.Core.DataLayer.Context.Configurations
{
    public static class LookupCategoriesDBConfiguration
    {
        public static void HandleDBConfigurationForLookupCategories(this ModelBuilder modelBuilder)
        {
            #region LookupCategory.

            modelBuilder.Entity<LookupCategory>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableLookupCategories, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                // Entity
                entity.Property(x => x.Code).HasColumnName("Code").IsRequired();
                entity.Property(x => x.Name).HasColumnName("Name").IsRequired();
                entity.Property(x => x.NameCultured).HasColumnName("NameCultured");
                entity.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                #endregion
            });

            #endregion
        }
    }
}
