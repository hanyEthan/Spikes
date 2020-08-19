using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Organizations.Core.Constants;
using XCore.Services.Organizations.Core.Models;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.Core.DataLayer.Context.Configurations
{
    public static class OrganizationConfiguration
    {
        public static void HandleDBConfigurationForOrganization(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<Organization>(entity =>
            {
                #region Fluent API Configuration

                #region table.

                entity.ToTable(DBConstants.TableOrganization, DBConstants.Schema);
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
                #region Relationship.
             
                entity.HasMany<Department>(org => org.Departments)
                       .WithOne(x=>x.Organization)
                       .HasForeignKey(dep => dep.OrganizationId);

                entity.HasMany<Event>(x => x.Events)
                    .WithOne(x => x.Organization)
                    .HasForeignKey(x => x.OrganizationId);

                entity.HasMany<Settings>(org => org.Settings)
                      .WithOne(x=>x.Organization)
                      .HasForeignKey(s => s.OrganizationId);

                entity.HasMany<ContactInfo>(org => org.ContactInfo)
                      .WithOne(x=>x.Organization)
                      .HasForeignKey(s => s.OrganizationId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany<ContactPerson>(org => org.ContactPersonnel)
                      .WithOne(x=>x.Organization)
                      .HasForeignKey(s => s.OrganizationId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(org => org.SubOrganizations)
                      .WithOne(x=>x.ParentOrganization)
                      .HasForeignKey(org => org.ParentOrganizationId);

                #endregion

                #endregion
            });

            #endregion
        }
    }
}
