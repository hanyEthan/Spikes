using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Notifications.Core.Utilities;
using XCore.Services.Notifications.Core.Models.Domain;

namespace XCore.Services.Notifications.Core.DataLayer.Context.Configurations
{
    public static class InternalNotificationConfiguration
    {
        public static void HandleInternalNotificationConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration.
            modelBuilder.Entity<InternalNotification>(entity =>
            {
                #region table.

                entity.ToTable(Constants.InternalNotification, Constants.NotIntSchema);
                entity.HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.IsRead).HasColumnName("IsRead");
                entity.Property(x => x.IsDismissed).HasColumnName("IsDismissed");
                entity.Property(x => x.IsDeleted).HasColumnName("IsDeleted");
                entity.Property(x => x.DateRead).HasColumnName("DateRead");
                entity.Property(x => x.DateDismissed).HasColumnName("DateDismissed");
                entity.Property(x => x.ActorId).HasColumnName("ActorId").IsRequired();
                entity.Property(x => x.ActionId).HasColumnName("ActionId").IsRequired();
                entity.Property(x => x.TargetId).HasColumnName("TargetId");
                entity.Property(x => x.Content).HasColumnName("Content").IsRequired();

                // Entity
                entity.Property(x => x.Code).HasColumnName("Code").IsRequired();
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.NameCultured);
                entity.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion

            });
            #endregion
        }
    } 
}

