using Microsoft.EntityFrameworkCore;
using XCore.Services.Lookups.Core.Constants;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.Core.DataLayer.Context.Configurations
{
    public static class LookupsDBConfiguration
    {
        public static void HandleDBConfigurationForLookups(this ModelBuilder modelBuilder)
        {
            #region LookupCategory.

            modelBuilder.Entity<Lookup>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableLookups, DBConstants.Schema)
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

                entity.HasOne(x => x.Category)
                       .WithMany(x => x.Lookups)
                       .HasForeignKey(x => x.CategoryId);

                #endregion
            });

            #endregion
        }
    }
}
