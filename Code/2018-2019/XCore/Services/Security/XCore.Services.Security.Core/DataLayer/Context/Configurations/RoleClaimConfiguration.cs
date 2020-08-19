using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Utilities;

namespace XCore.Services.Security.Core.DataLayer.Context.Configurations
{
    public static class RoleClaimConfiguration
    {
        public static void HandleRoleClaimConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                #region table.

                entity.ToTable(Constants.TableRolesClaims, Constants.DBSchema)
                    .HasKey(bc => new { bc.RoleId, bc.ClaimId });

                #endregion
                #region relations.

                entity.HasOne(x => x.Role)
                       .WithMany(y => y.Claims)
                       .HasForeignKey(z => z.RoleId);

                entity.HasOne(x => x.Claim)
                       .WithMany()
                       .HasForeignKey(z => z.ClaimId);

                #endregion

            });


            #endregion
        }

    }
}
