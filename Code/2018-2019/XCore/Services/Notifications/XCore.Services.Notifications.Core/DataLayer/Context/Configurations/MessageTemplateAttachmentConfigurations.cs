using Microsoft.EntityFrameworkCore;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Utilities;

namespace XCore.Services.Notifications.Core.DataLayer.Context.Configurations
{
    public static class MessageTemplateAttachmentConfigurations
    {
        public static void HandleDocumentConfiguration(this ModelBuilder modelBuilder)
        {
            #region Document.

            modelBuilder.Entity<MessageTemplateAttachment>(entity =>
            {
                #region table.

                entity.ToTable(Constants.MessageTemplateAttachment, Constants.DBSchema)
                      .HasKey(x => x.Id);

                #endregion
                #region props.

                entity.Property(x => x.UserId).HasColumnName("UserId");
                entity.Property(x => x.UserName).HasColumnName("UserName");
                entity.Property(x => x.App).HasColumnName("App");
                entity.Property(x => x.Module).HasColumnName("Module");
                entity.Property(x => x.Action).HasColumnName("Action");
                entity.Property(x => x.AttachmentReferenceId).HasColumnName("AttachmentReferenceId").IsRequired();
                entity.Property(x => x.DocumentReferenceId).HasColumnName("DocumentReferenceId").IsRequired();
                entity.Property(x => x.Category).HasColumnName("Category");

                entity.Property(x => x.Code).HasColumnName("Code").IsRequired();
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.NameCultured);
                entity.Ignore(x => x.IsActive);
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion

                #region relations.

                entity
                      .HasOne(a => a.MessageTemplate)
                      .WithMany(b => b.Attachments)
                      .HasForeignKey(b => b.MessageTemplateId);

                #endregion
            });

            #endregion
        }
    }
}
