using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models;
using XCore.Services.Organizations.Core.Constants;

namespace XCore.Services.Organizations.Core.DataLayer.Context.Configurations
{
    public static class VenueDepartmentConfiguration
    {
        public static void HandleDBConfigurationForVenueDepartment(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<VenueDepartment>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableVenueDepartment, DBConstants.Schema);
                entity.HasKey(sc => new { sc.VenueId, sc.DepartmentId });

                #endregion
                #region props.

                entity.Property(x => x.Capacity).HasColumnName("Capacity");

                #endregion
                #region Relationship.

                entity.HasOne<Venue>(x => x.Venue)
                      .WithMany(x => x.Departments)
                      .HasForeignKey(x => x.VenueId);

                entity.HasOne<Department>(x => x.Department)
                    .WithMany(x => x.Venues)
                    .HasForeignKey(x => x.DepartmentId);

                #endregion
            });

            #endregion
        }
    }
}
