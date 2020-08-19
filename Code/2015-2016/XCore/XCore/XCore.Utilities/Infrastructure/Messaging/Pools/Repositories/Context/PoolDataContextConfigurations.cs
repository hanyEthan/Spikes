using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using XCore.Utilities.Infrastructure.Messaging.Pools.Models;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Repositories.Context
{
    public class PoolDataContextConfigurations
    {
        #region PoolMessage

        public class PoolMessageConfiguration : EntityTypeConfiguration<PoolMessage>
        {
            public PoolMessageConfiguration(string poolMessagesTableName )
            {
                #region table.

                this.ToTable( poolMessagesTableName )
                    .HasKey( x => x.Id );

                this.Property( x => x.Id ).HasDatabaseGeneratedOption( DatabaseGeneratedOption.Identity );

                #endregion
                #region props.
                this.Property( x => x.MessageContent ).HasColumnName( "MessageContent" ).IsOptional();
                this.Property( x => x.AppId ).HasColumnName( "AppId" ).IsRequired();
                this.Property( x => x.MessageType ).HasColumnName( "MessageType" ).IsRequired();
                this.Property( x => x.Periority ).HasColumnName( "Periority" ).IsRequired();
                this.Property( x => x.Size ).HasColumnName( "Size" ).IsOptional();
                this.Property( x => x.Status ).HasColumnName( "Status" ).IsOptional();

                // common props.
                this.Ignore( x => x.Name );
                this.Ignore( x => x.NameCultured );
                this.Property( x => x.Code ).HasColumnName( "Code" ).IsRequired();
                this.Property( x => x.CreatedBy ).HasColumnName( "CreatedBy" ).IsOptional();
                this.Property( x => x.IsActive ).HasColumnName( "IsActive" ).IsRequired();
                this.Property( x => x.CreatedDate ).HasColumnName( "CreatedDate" ).IsOptional();
                this.Property( x => x.ModifiedDate ).HasColumnName( "ModifiedDate" ).IsOptional();
                this.Property( x => x.ModifiedBy ).HasColumnName( "ModifiedBy" ).IsOptional();
                this.Property( x => x.MetaData ).HasColumnName( "MetaData" ).IsOptional();

                #endregion
            }
        }

        #endregion
    }
}
