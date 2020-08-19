using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Repositories.Context
{
    public class QueueDataContext : DbContext
    {
        #region Props.

        public string MQMessagesTableName { get; set; }

        #endregion
        #region cst.

        public QueueDataContext() : this( null , null )
        {
        }
        public QueueDataContext( string queueTableName , string dbConnectionName )
        {
            HandleReferences();

            this.MQMessagesTableName = queueTableName ?? "MQMessage";
            dbConnectionName = dbConnectionName ?? "XCore.DB";

            Database.Connection.ConnectionString = GetConnectionString( XDB.Settings.Connections( dbConnectionName ) );
        }

        #endregion

        #region base

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            #region entities.

            modelBuilder.Configurations.Add( new QueueDataContextConfigurations.MQMessageConfiguration( MQMessagesTableName ) );   // MQ

            #endregion
            #region settings

            base.OnModelCreating( modelBuilder );

            #endregion
        }

        #endregion
        #region helpers.

        private void HandleReferences()
        {
            var x = typeof( SqlProviderServices );
            var y = x.ToString();
        }
        private string GetConnectionString( string connectionString )
        {
            return connectionString.ToLower().Contains( "data" )
                  ? connectionString
                  : XCrypto.DecryptFromAES( connectionString );
        }

        #endregion
    }
}
