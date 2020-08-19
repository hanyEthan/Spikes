using Microsoft.EntityFrameworkCore;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Utilities;

namespace XCore.Services.Notifications.Core.DataLayer.Context.Configurations
{
    public static class MessageTemplateConfiguration
    {
        public static void HandleMessageTemplateConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration.
            modelBuilder.Entity<MessageTemplate>(entity =>
            {
                #region table.

                entity.ToTable(Constants.MessageTemplate, Constants.DBSchema);
                entity.HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.Title).HasColumnName("Title");
                entity.Property(x => x.Body).HasColumnName("Body").IsRequired();
                entity.Property(x => x.AppId).HasColumnName("AppId").IsRequired();
                entity.Property(x => x.ModuleId).HasColumnName("ModuleId");

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
                #region relations.

                entity.HasMany(MessageTemplate => MessageTemplate.Keys)
                      .WithOne(Keys => Keys.MessageTemplate);

                #endregion
            });

            #endregion
        }
    }
}
