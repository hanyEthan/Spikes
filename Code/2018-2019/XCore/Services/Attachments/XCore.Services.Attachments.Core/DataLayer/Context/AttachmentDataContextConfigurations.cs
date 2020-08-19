using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XCore.Framework.Infrastructure.Entities.Repositories.Helpers;
using XCore.Services.Attachments.Core.Constants;
using XCore.Services.Attachments.Core.Models;

namespace XCore.Services.Attachments.Core.DataLayer.Context
{
    public static class AttachmentDataContextConfigurations
    {
        public static void HandleDBConfigurationForAttachment(this ModelBuilder modelBuilder)
        {
            #region Attachment.

            modelBuilder.Entity<Attachment>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableAttachment, DBConstants.Schema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                entity.Property(x => x.MimeType).HasColumnName("MimeType");
                entity.Property(x => x.Body).HasColumnName("Body");
                entity.Property(x => x.Extension).HasColumnName("Extension");
                entity.Property(x => x.Status).HasColumnName("Status");
                entity.Property(x => x.Code).HasColumnName("Code");
                entity.Property(x => x.Name).HasColumnName("Name");
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
