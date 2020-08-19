using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Utilities;

namespace XCore.Services.Security.Core.DataLayer.Context.Configurations
{
    public static class ActorClaimConfiguration
    {
        public static void HandleActorClaimConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<ActorClaim>(entity =>
            {
                #region table.

                entity.ToTable(Constants.TableActorClaims, Constants.DBSchema)
                    .HasKey(bc => new { bc.ActorId, bc.ClaimId });

                #endregion
                #region relations.

                entity.HasOne(x => x.Actor)
                       .WithMany(y => y.Claims)
                       .HasForeignKey(z => z.ActorId);

                entity.HasOne(x => x.Claim)
                       .WithMany()
                       .HasForeignKey(z => z.ClaimId);

                #endregion

            });


            #endregion
        }

    }
}
