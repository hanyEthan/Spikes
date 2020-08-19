using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Entities.Repositories.Handlers;
using XCore.Utilities.Logger.Framework.Logging.Contracts;
using XCore.Utilities.Logger.Framework.Logging.Mappers;
using XCore.Utilities.Logger.Framework.Logging.Models;

namespace XCore.Utilities.Logger.Framework.Logging.Handlers
{
    public class LoggingRepository : Repository<LogMessage>, ILoggingRepository
    {
        #region cst.

        public LoggingRepository( DbContext context ) : base( context )
        {
        }

        #endregion
        #region publics.

        public bool AddLogs( List<LogMessage> logs )
        {
            var logsDT = VMappers.Logging.MapToDataTable( logs );
            if ( logsDT == null || logsDT.Rows == null || logsDT.Rows.Count == 0 ) return false;

            var parameter = new SqlParameter( "@logs" , SqlDbType.Structured );
            parameter.TypeName = "dbo.LogTable";
            parameter.Value = logsDT;

            var result = context.Database.ExecuteSqlCommand( "exec AddLogs  @logs" , parameter );
            return result > 0;
        }

        #endregion
    }
}
