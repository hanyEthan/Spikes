using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XCore.Framework.Infrastructure.Entities.Repositories.Helpers;
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.Core.DataLayer.Context
{
    public class GeoDataContextConfigurations
    {
        #region LocationEvent

        public class LocationEventsConfiguration : BaseEntityTypeFluentMapperConfiguration<LocationEvent>
        {
            #region cst.

            public LocationEventsConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<LocationEvent> builder)
            {
                #region table.

                builder.ToTable(base.TableName).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                builder.Property(x => x.EntityCode).HasColumnName("EntityCode");
                builder.Property(x => x.EntityType).HasColumnName("EntityType");
                builder.Property(x => x.EventCode).HasColumnName("EventCode");
                builder.Property(x => x.Longitude).HasColumnName("Longitude").IsRequired();
                builder.Property(x => x.Latitude).HasColumnName("Latitude").IsRequired();

                // common.

                builder.Ignore(x => x.Code);
                builder.Ignore(x => x.Name);
                builder.Ignore(x => x.NameCultured);
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Ignore(x => x.CreatedBy);
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Ignore(x => x.ModifiedBy);
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
            }

            #endregion
        }

        #endregion
        #region LocationEventLatest

        public class LocationEventsLatestConfiguration : BaseEntityTypeFluentMapperConfiguration<LocationEventLatest>
        {
            #region cst.

            public LocationEventsLatestConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<LocationEventLatest> builder)
            {
                #region table.

                builder.ToTable(base.TableName).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                builder.Property(x => x.EntityCode).HasColumnName("EntityCode");
                builder.Property(x => x.EntityType).HasColumnName("EntityType");
                builder.Property(x => x.EventCode).HasColumnName("EventCode");
                builder.Property(x => x.Longitude).HasColumnName("Longitude").IsRequired();
                builder.Property(x => x.Latitude).HasColumnName("Latitude").IsRequired();

                // common.

                builder.Ignore(x => x.Code);
                builder.Ignore(x => x.Name);
                builder.Ignore(x => x.NameCultured);
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                builder.Ignore(x => x.CreatedBy);
                builder.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                builder.Ignore(x => x.ModifiedBy);
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
            }

            #endregion
        }

        #endregion
    }
}
