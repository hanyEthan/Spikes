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
    public class LoggingDataContext : DataContext
    {
        #region cst.

        public LoggingDataContext()
        {
            base.HandleReferences();
            base.Database.Connection.ConnectionString = XDB.Settings.Connections( "Core.Components.LogData" );
        }

        #endregion
        #region base

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            #region entities

            #endregion
            #region settings

            base.OnModelCreating( modelBuilder );

            #endregion
        }

        #endregion
    }
}
