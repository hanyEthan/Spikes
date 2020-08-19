using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models;
using XCore.Services.Organizations.Core.Constants;

namespace XCore.Services.Organizations.Core.DataLayer.Context.Configurations
{
    public static class VenueCityConfiguration
    {
        public static void HandleDBConfigurationForVenueCity(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<VenueCity>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableVenueCity, DBConstants.Schema);
                entity.HasKey(sc => new { sc.VenueId, sc.CityId });

                #endregion
                #region props.

                #endregion
                #region Relationship.

                entity.HasOne<Venue>(x => x.Venue)
                      .WithMany(x => x.Cities)
                      .HasForeignKey(x => x.CityId);

                entity.HasOne<City>(x => x.City)
                    .WithMany(x => x.Venues)
                    .HasForeignKey(x => x.VenueId);

                #endregion
            });

            #endregion
        }
    }
}
