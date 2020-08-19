using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Repositories.Context
{
    public class QueueDataContextConfigurations
    {
        #region MQMessage

        public class MQMessageConfiguration : EntityTypeConfiguration<MQMessage>
        {
            public MQMessageConfiguration() : this( "MQMessage" )
            {

            }
            public MQMessageConfiguration( string tableName )
            {
                #region table.

                this.ToTable( tableName )
                    .HasKey( x => x.Id );

                this.Property( x => x.Id ).HasDatabaseGeneratedOption( DatabaseGeneratedOption.Identity );

                #endregion
                #region props.
                this.Property( x => x.Type ).HasColumnName( "Type" ).IsOptional();
                this.Property( x => x.Status ).HasColumnName( "Status" ).IsRequired();
                this.Property( x => x.Priority ).HasColumnName( "Priority" ).IsRequired();
                this.Property( x => x.Complexity ).HasColumnName( "Complexity" ).IsRequired();
                this.Property( x => x.TargetCode ).HasColumnName( "TargetCode" ).IsOptional();
                this.Property( x => x.ContentType ).HasColumnName( "ContentType" ).IsOptional();
                this.Property( x => x.TargetType ).HasColumnName( "TargetType" ).IsOptional();
                this.Property( x => x.SubscribersTokens ).HasColumnName( "SubscribersTokens" ).IsOptional();
                this.Property( x => x.RetrialsCounter ).HasColumnName( "RetrialsCounter" ).IsOptional();

                // common props.
                this.Property( x => x.Name ).HasColumnName( "Name" ).IsOptional();
                this.Property( x => x.NameCultured ).HasColumnName( "NameCultured" ).IsOptional();
                this.Property( x => x.IsActive ).HasColumnName( "IsActive" ).IsRequired();
                this.Property( x => x.CreatedDate ).HasColumnName( "CreatedDate" ).IsOptional();
                this.Property( x => x.CreatedBy ).HasColumnName( "CreatedBy" ).IsOptional();
                this.Property( x => x.ModifiedDate ).HasColumnName( "ModifiedDate" ).IsOptional();
                this.Property( x => x.ModifiedBy ).HasColumnName( "ModifiedBy" ).IsOptional();
                this.Property( x => x.MetaData ).HasColumnName( "MetaData" ).IsOptional();

                #endregion
            }
        }

        #endregion
    }
}
