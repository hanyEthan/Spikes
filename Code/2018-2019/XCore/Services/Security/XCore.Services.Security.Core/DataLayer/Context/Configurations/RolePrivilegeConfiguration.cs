using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Utilities;

namespace XCore.Services.Security.Core.DataLayer.Context.Configurations
{
   public static class RolePrivilegeConfiguration
    {
        public static void HandleRolePrivilegeConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<RolePrivilege>(entity =>
            {
                #region table.

                entity.ToTable(Constants.TableRolesPrivileges, Constants.DBSchema)
                    .HasKey(bc => new { bc.RoleId, bc.PrivilegeId });

                #endregion
                #region relations.

                entity.HasOne(x => x.Role)
                       .WithMany(y => y.Privileges)
                       .HasForeignKey(z => z.RoleId);

                entity.HasOne(x => x.Privilege)
                       .WithMany(y => y.Roles)
                       .HasForeignKey(z => z.PrivilegeId);

                #endregion

            });


            #endregion
        }

    }
}
