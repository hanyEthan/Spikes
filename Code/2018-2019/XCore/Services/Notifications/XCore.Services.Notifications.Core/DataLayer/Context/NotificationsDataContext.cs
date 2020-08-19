using Microsoft.EntityFrameworkCore;
using XCore.Services.Notifications.Core.DataLayer.Context.Configurations;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Utilities;

namespace XCore.Services.Notifications.Core.DataLayer.Context
{
    public class NotificationsDataContext : DbContext
    {
        #region props.

        protected const string DefaultConnectionString = Constants.ConnectionString;

        #endregion
        #region DbSets
        public virtual DbSet<MessageTemplate> MessageTemplate { get; set; }
        public virtual DbSet<MessageTemplateKey> MessageTemplateKey { get; set; }
        public virtual DbSet<InternalNotification> InternalNotification { get; set; }
        public virtual DbSet<MessageTemplateAttachment> Document { get; set; }

        #endregion
        #region cst.

        public NotificationsDataContext(DbContextOptions options) : base(options)
        {
        }

        #endregion                      
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleMessageTemplateConfiguration();
            modelBuilder.HandleMessageTemplateKeyConfiguration();
            modelBuilder.HandleInternalNotificationConfiguration();
            modelBuilder.HandleDocumentConfiguration();
        }

        #endregion
    }
}
