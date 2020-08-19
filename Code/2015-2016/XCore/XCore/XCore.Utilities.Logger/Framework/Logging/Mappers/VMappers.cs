using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;
using XCore.Utilities.Logger.Framework.Logging.Models;
using XCore.Utilities.Logger.Framework.Logging.Models.Enums;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Logger.Framework.Logging.Mappers
{
    public static class VMappers
    {
        #region Logging

        public static class Logging
        {
            #region publics.

            #region LoggingEvent => LogMessage

            public static List<LogMessage> Map( List<LoggingEvent> from )
            {
                try
                {
                    if ( from == null ) return null;
                    var to = new List<LogMessage>();

                    foreach ( var item in from )
                    {
                        to.Add( Map( item ) );
                    }

                    return to;
                }
                catch ( Exception x )
                {
                    NLogger.Error( "Exception : " + x );
                    throw;
                }
            }
            public static LogMessage Map( LoggingEvent from )
            {
                try
                {
                    if ( from == null ) return null;
                    var to = new LogMessage();

                    to.Message = from.MessageObject?.ToString();
                    to.Exception = from.ExceptionObject?.ToString();
                    to.Level = from.Level.Name;
                    to.LevelCode = from.Level.Value;
                    to.Logger = GetSourceLogger( from );
                    to.Thread = from.ThreadName;
                    to.LogDate = from.TimeStamp;
                    to.AppId = from.Properties[Properties.AppId]?.ToString();
                    to.ModuleId = from.Properties[Properties.ModuleId]?.ToString();
                    to.Context = from.Properties[Properties.Context]?.ToString();

                    return to;
                }
                catch ( Exception x )
                {
                    NLogger.Error( "Exception : " + x );
                    throw;
                }
            }

            #endregion
            #region LogMessage => MQMessage

            public static List<MQMessage> Map( List<LogMessage> from )
            {
                try
                {
                    if ( from == null ) return null;
                    var to = new List<MQMessage>();

                    foreach ( var item in from )
                    {
                        to.Add( Map( item ) );
                    }

                    return to;
                }
                catch ( Exception ex )
                {
                    NLogger.Error( ex.Message , ex );
                    throw;
                }
            }
            public static MQMessage Map( LogMessage from )
            {
                try
                {
                    if ( from == null ) return null;
                    var to = new MQMessage();

                    to.Code = from.Code;
                    to.MetaData = XSerialize.JSON.Serialize( from );
                    to.CreatedBy = from.CreatedBy;
                    to.ModifiedBy = from.ModifiedBy;
                    to.ModifiedDate = from.ModifiedDate;
                    to.CreatedDate = from.CreatedDate;
                    to.Name = from.Name;
                    to.NameCultured = from.NameCultured;
                    to.SubscribersTokens = XSerialize.JSON.Serialize( new List<string> { "logging" } );
                    to.Status = MQMessageStatus.UnProcessed;
                    to.Type = QueueMessageType.Logging.ToString();

                    return to;
                }
                catch ( Exception x )
                {
                    NLogger.Error( "Exception : " + x );
                    throw;
                }
            }

            #endregion
            #region LogMessage => DataTable

            public static DataTable MapToDataTable( List<LogMessage> logs )
            {
                var dt = DeclareTable();
                DataRow row;

                foreach ( var log in logs )
                {
                    row = dt.NewRow();
                    row["Thread"] = log.Thread;
                    row["Level"] = log.Level;
                    row["LevelCode"] = log.LevelCode;
                    row["Logger"] = log.Logger;
                    row["Message"] = log.Message;
                    row["Exception"] = log.Exception;
                    row["AppId"] = log.AppId;
                    row["ModuleId"] = log.ModuleId;
                    row["Context"] = log.Context;
                    row["LogDate"] = log.LogDate;
                    row["CreatedDate"] = DateTime.Now;

                    dt.Rows.Add( row );
                }

                return dt;
            }
            public static DataTable MapToMetaData( IList<MQMessage> logs )
            {
                var dt = DeclareQueueTable();
                DataRow row;
                foreach ( var log in logs )
                {
                    row = dt.NewRow();
                    row["MetaData"] = log.MetaData;

                    dt.Rows.Add( row );
                }

                return dt;
            }

            #endregion

            #endregion
            #region helpers.

            private static string GetSourceLogger( LoggingEvent log )
            {
                if ( log.LocationInformation == null ) return log.LoggerName;

                foreach ( var item in log.LocationInformation.StackFrames )
                {
                    if ( !item.ClassName.StartsWith( "XCore.Utilities.Logger" ) && !item.ClassName.StartsWith( "XCore.Utilities.Utilities.NativeLogger" ) )
                    {
                        return $"{item.ClassName}.{item.Method.Name} ... [line: {item.LineNumber}]";
                    }
                }
                return log.LoggerName;
            }
            private static DataTable DeclareTable()
            {
                var dt = new DataTable();

                dt.Columns.Add( "Thread" );
                dt.Columns.Add( "Level" );
                dt.Columns.Add( "LevelCode" );
                dt.Columns.Add( "Logger" );
                dt.Columns.Add( "Message" );
                dt.Columns.Add( "Exception" );
                dt.Columns.Add( "AppId" );
                dt.Columns.Add( "ModuleId" );
                dt.Columns.Add( "Context" );
                dt.Columns.Add( "LogDate" , typeof( DateTime ) );
                dt.Columns.Add( "CreatedDate" , typeof( DateTime ) );

                return dt;
            }
            private static DataTable DeclareQueueTable()
            {
                var dt = new DataTable();
                dt.Columns.Add( "MetaData" );
                return dt;
            }

            #endregion
        }

        #endregion
    }
}
