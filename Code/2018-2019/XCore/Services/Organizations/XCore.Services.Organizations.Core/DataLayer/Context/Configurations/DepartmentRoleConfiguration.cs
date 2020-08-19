using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models;
using XCore.Services.Organizations.Core.Constants;

namespace XCore.Services.Organizations.Core.DataLayer.Context.Configurations
{
    public static class DepartmentRoleConfiguration
    {
        public static void HandleDBConfigurationForDepartmentRole(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<DepartmentRole>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.TableDepartmentRole, DBConstants.Schema);
                entity.HasKey(sc => new { sc.RoleId, sc.DepartmentId });

                #endregion
                #region props.

                #endregion
                #region Relationship.

                entity.HasOne<Role>(x => x.Role)
                      .WithMany(x => x.Departments)
                      .HasForeignKey(x => x.RoleId);

                entity.HasOne<Department>(x => x.Department)
                    .WithMany(x => x.Roles)
                    .HasForeignKey(x => x.DepartmentId);

                #endregion
            });

            #endregion
        }
    }
}
