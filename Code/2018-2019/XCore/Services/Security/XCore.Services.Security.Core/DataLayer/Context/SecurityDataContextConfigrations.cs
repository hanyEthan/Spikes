using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Helpers;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Utilities;

namespace XCore.Services.Security.Core.DataLayer.Context
{
   public class SecurityDataContextConfigrations
    {
        #region Role.Privilege.

        public class RolePrivilegeConfiguration : BaseEntityTypeFluentMapperConfiguration<RolePrivilege>
        {
            #region cst.

            public RolePrivilegeConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<RolePrivilege> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema)
                    .HasKey(bc => new { bc.RoleId, bc.PrivilegeId });

                #endregion
                #region relations.

                builder.HasOne(x => x.Role)
                       .WithMany(y => y.Privileges)
                       .HasForeignKey(z => z.RoleId);

                builder.HasOne(x => x.Privilege)
                       .WithMany(y => y.Roles)
                       .HasForeignKey(z => z.PrivilegeId);

                #endregion
            }

            #endregion
        }

        #endregion
        #region Actor.Privilege.

        public class ActorPrivilegeConfiguration : BaseEntityTypeFluentMapperConfiguration<ActorPrivilege>
        {
            #region cst.

            public ActorPrivilegeConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<ActorPrivilege> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(bc => new { bc.ActorId, bc.PrivilegeId });

                #endregion
                #region relations.

                builder.HasOne(x => x.Actor)
                       .WithMany(y => y.Privileges)
                       .HasForeignKey(z => z.ActorId);

                builder.HasOne(x => x.Privilege)
                       .WithMany(y => y.Actors)
                       .HasForeignKey(z => z.PrivilegeId);

                #endregion
            }

            #endregion
        }

        #endregion
        #region Actor.Role.

        public class ActorRoleConfiguration : BaseEntityTypeFluentMapperConfiguration<ActorRole>
        {
            #region cst.

            public ActorRoleConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<ActorRole> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(bc => new { bc.RoleId, bc.ActorId });

                #endregion
                #region relations.

                builder.HasOne(x => x.Role)
                       .WithMany(y => y.Actors)
                       .HasForeignKey(z => z.RoleId);

                builder.HasOne(x => x.Actor)
                       .WithMany(y => y.Roles)
                       .HasForeignKey(z => z.ActorId);

                #endregion
            }

            #endregion
        }

        #endregion

        #region App.

        public class AppsConfiguration : BaseEntityTypeFluentMapperConfiguration<App>
        {
            #region cst.

            public AppsConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<App> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                // model
                builder.Property(x => x.Description).HasColumnName("Description");

                // Entity
                builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
                builder.Property(x => x.Name).HasColumnName("Name");
                builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                builder.HasMany(app => app.Actors)
                       .WithOne(actor => actor.App);
                       //.OnDelete(DeleteBehavior.SetNull);

                builder.HasMany(app => app.Roles)
                       .WithOne(role => role.App);
                       //.OnDelete(DeleteBehavior.SetNull);

                builder.HasMany(app => app.Privileges)
                       .WithOne(privilege => privilege.App);
                        //.OnDelete(DeleteBehavior.SetNull);

                builder.HasMany(app => app.Targets)
                       .WithOne(target => target.App);
                       //.OnDelete(DeleteBehavior.SetNull);

                #endregion
            }

            #endregion
        }

        #endregion
        #region Actor.

        public class ActorsConfiguration : BaseEntityTypeFluentMapperConfiguration<Actor>
        {
            #region cst.

            public ActorsConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<Actor> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                // model
                builder.Property(x => x.Description).HasColumnName("Description");

                // Entity
                builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
                builder.Property(x => x.Name).HasColumnName("Name");
                builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");


                #endregion
                #region relations.

                // one-many : App ( one sided ).
                //builder.HasOne(x => x.App)
                //       .WithMany()
                //       .HasForeignKey(x => x.AppId);

                #endregion
            }

            #endregion
        }

        #endregion
        #region Target.

        public class TargetsConfiguration : BaseEntityTypeFluentMapperConfiguration<Target>
        {
            #region cst.

            public TargetsConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<Target> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();
                builder.Property(x => x.PrivilegeId).HasColumnName("PrivilegeId");


                #endregion
                #region props.

                // model
                builder.Property(x => x.Description).HasColumnName("Description");

                // Entity
                builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
                builder.Property(x => x.Name).HasColumnName("Name");
                builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                builder
                      .HasOne(a => a.Privilege)
                      .WithOne(b => b.Target)
                      .HasForeignKey<Target>(b => b.PrivilegeId);

                #endregion
            }

            #endregion
        }

        #endregion
        #region Role.

        public class RolesConfiguration : BaseEntityTypeFluentMapperConfiguration<Role>
        {
            #region cst.

            public RolesConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<Role> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                // model
                builder.Property(x => x.Description).HasColumnName("Description");

                // Entity
                builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
                builder.Property(x => x.Name).HasColumnName("Name");
                builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                #endregion
            }

            #endregion
        }

        #endregion
        #region Privilege.

        public class PrivilegesConfiguration : BaseEntityTypeFluentMapperConfiguration<Privilege>
        {
            #region cst.

            public PrivilegesConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<Privilege> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                // model
                builder.Property(x => x.Description).HasColumnName("Description");

                // Entity
                builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
                builder.Property(x => x.Name).HasColumnName("Name");
                builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
                #region relations.

                // one-many : App ( one sided ).
                //builder.HasOne(x => x.App)
                //       .WithMany()
                //       .HasForeignKey(x => x.AppId);

                #endregion
            }
            #endregion
        }

        #endregion
    }
}
