using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Utilities;


namespace XCore.Services.Security.Core.DataLayer.Context.Configurations
{
   public static class ActorRoleConfiguration
    {
        public static void HandleActorRoleConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<ActorRole>(entity =>
            {
                #region table.

                entity.ToTable(Constants.TableActorRole, Constants.DBSchema)
                    .HasKey(bc => new { bc.RoleId, bc.ActorId });

                #endregion
                #region relations.

                entity.HasOne(x => x.Role)
                       .WithMany(y => y.Actors)
                       .HasForeignKey(z => z.RoleId);

                entity.HasOne(x => x.Actor)
                       .WithMany(y => y.Roles)
                       .HasForeignKey(z => z.ActorId);

                #endregion

            });


            #endregion
        }
    }
}
