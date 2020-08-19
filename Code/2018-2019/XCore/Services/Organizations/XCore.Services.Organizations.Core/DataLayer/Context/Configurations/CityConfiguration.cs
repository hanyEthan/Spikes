using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Organizations.Core.Constants;
using XCore.Services.Organizations.Core.Models;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.Core.DataLayer.Context.Configurations
{
    public static class CityConfiguration
    {
        public static void HandleDBConfigurationForCity(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<City>(entity =>
            {
                #region Fluent API Configuration

                #region table.

                entity.ToTable(DBConstants.TableCity, DBConstants.Schema);
                entity.HasKey(x => x.Id);

                #endregion
                #region props.

                // model

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
                #region Relationship.
             
                #endregion

                #endregion
            });

            #endregion
        }
    }
}
