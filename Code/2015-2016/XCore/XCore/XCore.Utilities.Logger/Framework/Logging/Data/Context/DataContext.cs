using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Logger.Framework.Logging.Data.Context
{
    public class DataContext : DbContext
    {
        #region Props.

        #endregion
        #region cst.

        public DataContext()
        {
            HandleReferences();
        }

        #endregion

        #region base

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            #region entities.

            #endregion
            #region settings

            base.OnModelCreating( modelBuilder );

            #endregion
        }

        #endregion
        #region helpers.

        protected void HandleReferences()
        {
            var x = typeof( SqlProviderServices );
            var y = x.ToString();
        }
        protected string GetConnectionString( string connectionString )
        {
            return connectionString.ToLower().Contains( "data" )
                 ? connectionString
                 : XCrypto.DecryptFromAES( connectionString );
        }

        #endregion
    }
}
