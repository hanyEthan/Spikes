using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Repositories;
using XCore.Utilities.Logger.Framework.Logging.Mappers;

namespace XCore.Utilities.Logger.Framework.Logging.Queues.Support
{
    public class LogQueueRepository : QueueRepository
    {
        #region cst.

        public LogQueueRepository( DbContext context ) : base( context )
        {
        }

        #endregion
        #region QueueRepository

        public override bool Send( MQMessage message )
        {
            return Send( new List<MQMessage>( 1 ) { message } );
        }
        public override bool Send( IList<MQMessage> messages )
        {
            var logsDT = VMappers.Logging.MapToMetaData( messages );
            if ( logsDT == null || logsDT.Rows == null || logsDT.Rows.Count == 0 ) return false;

            var parameter = new SqlParameter( "@logs" , SqlDbType.Structured );
            parameter.TypeName = "dbo.LogQueueTable";
            parameter.Value = logsDT;

            var result = context.Database.ExecuteSqlCommand( "exec AddQueueLogs @logs" , parameter );
            return result > 0;
        }

        #endregion
    }
}
