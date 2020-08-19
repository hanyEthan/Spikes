using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XCore.Framework.Infrastructure.Entities.Repositories.Helpers;
using XCore.Framework.Infrastructure.Messaging.Pools.Models;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Repositories.Context
{
    public class PoolDataContextConfigurations
    {
        #region PoolMessage

        public class PoolMessageConfiguration : BaseEntityTypeFluentMapperConfiguration<PoolMessage>
        {
            #region cst.

            public PoolMessageConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<PoolMessage> builder)
            {
                #region table.

                builder.ToTable(base.TableName).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.
                builder.Property(x => x.MessageContent).HasColumnName("MessageContent");
                builder.Property(x => x.AppId).HasColumnName("AppId").IsRequired();
                builder.Property(x => x.MessageType).HasColumnName("MessageType").IsRequired();
                builder.Property(x => x.Periority).HasColumnName("Periority").IsRequired();
                builder.Property(x => x.Size).HasColumnName("Size");
                builder.Property(x => x.Status).HasColumnName("Status");

                // common.
                builder.Ignore(x => x.Name);
                builder.Ignore(x => x.NameCultured);
                builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
                builder.Property(x => x.ModifiedDate).HasColumnName("ModifiedDate");
                builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                builder.Property(x => x.MetaData).HasColumnName("MetaData");

                #endregion
            }

            #endregion
        }

        #endregion
    }
}
