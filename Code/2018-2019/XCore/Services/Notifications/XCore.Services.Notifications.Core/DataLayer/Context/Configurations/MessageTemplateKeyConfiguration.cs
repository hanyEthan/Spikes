using Microsoft.EntityFrameworkCore;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Utilities;

namespace XCore.Services.Notifications.Core.DataLayer.Context.Configurations
{
    public static class MessageTemplateKeyConfiguration
    {
        public static void HandleMessageTemplateKeyConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration.
            modelBuilder.Entity<MessageTemplateKey>(entity =>
            {
                #region table.

                entity.ToTable(Constants.MessageTemplateKey, Constants.DBSchema);
                entity.HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.Description).HasColumnName("Description");
                entity.Property(x => x.Key).HasColumnName("Key").IsRequired();

                // Entity
                entity.Ignore(x => x.Code);
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.NameCultured);
                entity.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                entity
                      .HasOne(a => a.MessageTemplate)
                      .WithMany(b => b.Keys)
                      .HasForeignKey(b => b.MessageTemplateId);

                #endregion
            });

            #endregion
        }
    }
}
