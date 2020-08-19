using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Utilities;


namespace XCore.Services.Security.Core.DataLayer.Context.Configurations
{
   public static class ActorPrivilegeConfiguration
    {
        public static void HandleActorPrivilegeConfiguration(this ModelBuilder modelBuilder)
        {
            #region Configuration..

            modelBuilder.Entity<ActorPrivilege>(entity =>
            {
                #region table.

                entity.ToTable(Constants.TableActorPrivilege, Constants.DBSchema)
                    .HasKey(bc => new { bc.ActorId, bc.PrivilegeId });

                #endregion
                #region relations.

                entity.HasOne(x => x.Actor)
                       .WithMany(y => y.Privileges)
                       .HasForeignKey(z => z.ActorId);

                entity.HasOne(x => x.Privilege)
                       .WithMany(y => y.Actors)
                       .HasForeignKey(z => z.PrivilegeId);

                #endregion

            });


            #endregion
        }

    }
}
