using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Utilities;

namespace XCore.Services.Security.Core.DataLayer.Context.Configurations
{
    public static class AppConfiguration
    {
        public static void HandleAppConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<App>(entity =>
            {
                #region table.

                entity.ToTable(Constants.TableApps, Constants.DBSchema);
                entity.HasKey(x => x.Id);

                #endregion
                #region props.

                // model
                entity.Property(x => x.Description).HasColumnName("Description");

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

                entity.HasMany(app => app.Actors)
                       .WithOne(actor => actor.App);
                //.OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(app => app.Roles)
                       .WithOne(role => role.App);
                //.OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(app => app.Privileges)
                       .WithOne(privilege => privilege.App);
                //.OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(app => app.Targets)
                       .WithOne(target => target.App);
                //.OnDelete(DeleteBehavior.SetNull);

                #endregion
            });


            #endregion
        }

    }
}
