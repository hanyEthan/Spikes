using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XCore.Framework.Infrastructure.Entities.Repositories.Helpers;
using XCore.Framework.Infrastructure.Messaging.Queues.Models;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Context
{
    public class QueueDataContextConfigurations
    {
        #region MQMessage

        public class MQMessageConfiguration : BaseEntityTypeFluentMapperConfiguration<MQMessage>
        {
            #region cst.

            public MQMessageConfiguration(string tableName) : base(tableName)
            {
            }

            #endregion
            #region BaseEntityTypeFluentMapperConfiguration

            public override void Configure(EntityTypeBuilder<MQMessage> builder)
            {
                #region table.

                builder.ToTable(base.TableName).HasKey(x => x.Id);
                builder.Property(x => x.Id).UseSqlServerIdentityColumn();

                #endregion
                #region props.

                builder.Property(x => x.Type).HasColumnName("Type");
                builder.Property(x => x.Status).HasColumnName("Status").IsRequired();
                builder.Property(x => x.Priority).HasColumnName("Priority").IsRequired();
                builder.Property(x => x.Complexity).HasColumnName("Complexity").IsRequired();
                builder.Property(x => x.TargetCode).HasColumnName("TargetCode");
                builder.Property(x => x.ContentType).HasColumnName("ContentType");
                builder.Property(x => x.TargetType).HasColumnName("TargetType");
                builder.Property(x => x.SubscribersTokens).HasColumnName("SubscribersTokens");
                builder.Property(x => x.RetrialsCounter).HasColumnName("RetrialsCounter");

                // common.
                builder.Property(x => x.Name).HasColumnName("Name");
                builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
                builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
                builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
                builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
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
