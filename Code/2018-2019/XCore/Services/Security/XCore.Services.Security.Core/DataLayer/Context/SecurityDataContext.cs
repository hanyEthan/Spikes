using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Utilities;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Utilities;
using XCore.Services.Security.Core.DataLayer.Context.Configurations;

namespace XCore.Services.Security.Core.DataLayer.Context
{
   public class SecurityDataContext : DbContext
    {
        #region props.

        protected const string DefaultConnectionString = Constants.ConnectionString;

        #endregion
        #region DbSets

        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Target> Targets { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }


        #endregion

        #region cst.

        public SecurityDataContext(DbContextOptions options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging(true);
        //    optionsBuilder.EnableDetailedErrors(true);


        //    base.OnConfiguring(optionsBuilder);
        //}

        #endregion
        #region base
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleAppConfiguration();
            modelBuilder.HandlePrivilegeConfiguration();
            modelBuilder.HandleActorConfiguration();
            modelBuilder.HandleActorPrivilegeConfiguration();
            modelBuilder.HandleActorRoleConfiguration();
            modelBuilder.HandleRoleConfiguration();
            modelBuilder.HandleTargetConfiguration();
            modelBuilder.HandleRolePrivilegeConfiguration();
            modelBuilder.HandleActorClaimConfiguration();
            modelBuilder.HandleRoleClaimConfiguration();
            modelBuilder.HandleClaimConfiguration();

            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.AppsConfiguration("Apps"));
            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.RolesConfiguration("Roles"));
            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.ActorsConfiguration("Actors"));
            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.PrivilegesConfiguration("Privileges"));
            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.TargetsConfiguration("Targets"));
            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.ActorPrivilegeConfiguration("ActorsPrivileges"));
            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.ActorRoleConfiguration("ActorsRoles"));
            //modelBuilder.ApplyConfiguration(new SecurityDataContextConfigrations.RolePrivilegeConfiguration("RolesPrivileges"));









        }

        #endregion
    }
}
