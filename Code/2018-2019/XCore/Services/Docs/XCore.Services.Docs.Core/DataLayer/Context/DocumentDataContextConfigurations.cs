using Microsoft.EntityFrameworkCore;
using XCore.Services.Docs.Core.Constants;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.Core.DataLayer.Context
{
    public static class DocumentDataContextConfigurations
    {
       
        public static void HandleDBConfigurationForDocument (this ModelBuilder modelBuilder)
        {
           #region Document.

        modelBuilder.Entity<Document>(entity =>
          {
                #region table.

                entity.ToTable(DBConstants.TableDocument, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.
             
                    entity.Property(x => x.UserId).HasColumnName("UserId");
                    entity.Property(x => x.UserName).HasColumnName("UserName");
                    entity.Property(x => x.App).HasColumnName("App");
                    entity.Property(x => x.Module).HasColumnName("Module");
                    entity.Property(x => x.Action).HasColumnName("Action");
                    entity.Property(x => x.Entity).HasColumnName("Entity");
                    entity.Property(x => x.AttachId).HasColumnName("AttachId");
                    entity.Property(x => x.Category).HasColumnName("Category");
              

                    entity.Property(x => x.Code).HasColumnName("Code");
                    entity.Ignore(x => x.Name);
                    entity.Ignore(x => x.NameCultured);
                    entity.Ignore(x => x.IsActive);
                    entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                    entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                    entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                    entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                    entity.Property(x => x.MetaData).HasColumnName("MetaData");

        #endregion
          });

        #endregion
        }

    }
}
