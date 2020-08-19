using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XCore.Framework.Infrastructure.Entities.Repositories.Helpers;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Utilities;

namespace XCore.Services.Config.Core.DataLayer.Context
{
    public class ConfigDataContextConfigrations
    {
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
                builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
                builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
            }
            #endregion
        }
        #endregion
        #region Module.

        public class ModulesConfiguration : BaseEntityTypeFluentMapperConfiguration<Module>
        {
            #region cst.

            public ModulesConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<Module> builder)
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
                builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
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
                builder.HasOne(x => x.App)
                       .WithMany()
                       .HasForeignKey(x => x.AppId);

                #endregion
            }
            #endregion
        }

        #endregion
        #region ConfigItem.

        public class ConfigsConfiguration : BaseEntityTypeFluentMapperConfiguration<ConfigItem>
        {
            #region cst.

            public ConfigsConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<ConfigItem> builder)
            {
                #region table.

                builder.ToTable(base.TableName, Constants.DBSchema).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                // model
                builder.Property(x => x.Key).HasColumnName("Key").IsRequired();
                builder.Property(x => x.Value).HasColumnName("Value");
                builder.Property(x => x.Description).HasColumnName("Description");
                builder.Property(x => x.Type).HasColumnName("Type");
                builder.Property(x => x.ReadOnly).HasColumnName("ReadOnly").IsRequired();
                builder.Property(x => x.Version).HasColumnName("Version");

                // Entity
                builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
                builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
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
                builder.HasOne(x => x.App)
                       .WithMany()
                       .HasForeignKey(x => x.AppId);

                // one-many : Module ( one sided ).
                builder.HasOne(x => x.Module)
                       .WithMany()
                       .HasForeignKey(x => x.ModuleId);

                #endregion
            }

            #endregion
        }

        #endregion
    }
}
