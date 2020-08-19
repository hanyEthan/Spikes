using System.Data.Entity;
using System.Data.Entity.SqlServer;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Repositories.Context
{
    public class PoolDataContext : DbContext
    {
        #region Props.

        public string PoolMessagesTableName { get; set; }

        #endregion
        #region cst.

        public PoolDataContext() : this( null , null )
        {
        }
        public PoolDataContext( string poolTableName , string dbConnectionName )
        {
            HandleReferences();

            this.PoolMessagesTableName = poolTableName ?? "MessagesPool";
            dbConnectionName = dbConnectionName ?? "XCore.DB";

            Database.Connection.ConnectionString = GetConnectionString( XDB.Settings.Connections( dbConnectionName ) );
        }

        #endregion

        #region base

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            #region entities.

            modelBuilder.Configurations.Add( new PoolDataContextConfigurations.PoolMessageConfiguration( PoolMessagesTableName ) );   // MQ

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
