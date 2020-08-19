using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telerik.OpenAccess.Data.Common;

using ADS.Common.Utilities;
using ADS.Tamam.Services.DataAcquisition.Models;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Data.ORM;
using ADS.Common.Models.Enums;

namespace ADS.Tamam.Services.DataAcquisition.DataHandler
{
    public class PullMechanismDataHandler
    {
        public List<PullMechanismModel> GetUnProcessedEvents()
        {
            using ( var context = new DataContext() )
            {
                try
                {

                    if ( IsOracle() )
                    {
                        var result = context.ExecuteQuery<PullMechanismModel>( "GetUnProcessedEvents" );
                        context.SaveChanges();   // dummy save to let the ORM know the operation has completed and commit any transaction.

                        //return result2.Select( x => new PullMechanismModel
                        //{
                        //    Id = x[0].ToString() ,
                        //    PersonId = x[1].ToString() ,
                        //    EventDate = Convert.ToDateTime( x[2] ) ,
                        //    TerminalId = x[3].ToString() ,
                        //    EventType = Convert.ToInt32( x[4] ) ,
                        //    LogOrgin = x[5] != null? x[5].ToString():null,
                        //    Logsource = x[6] != null ? x[6].ToString():null,
                        //    ConsiderLogForAttendance =x[7] !=null? x[7].ToString():null
                        //} ).ToList();
                        return result.ToList();
                    }
                    else
                    {
                        var result = context.ExecuteStoredProcedure<object[]>( "GetUnProcessedEvents" , null , null );
                        context.SaveChanges();   // dummy save to let the ORM know the operation has completed and commit any transaction.
                        return result.Select( x => new PullMechanismModel
                        {
                            Id = x[0].ToString() ,
                            PersonId = x[1].ToString() ,
                            EventDate = Convert.ToDateTime( x[2] ) ,
                            TerminalId = x[3].ToString() ,
                            EventType = Convert.ToInt32( x[4] ) ,
                            LogOrgin = x[5] != null ? x[5].ToString() : null ,
                            Logsource = x[6] != null ? x[6].ToString() : null ,
                            ConsiderLogForAttendance = x[7] != null ? x[7].ToString() : null,
                            //LocationName = x[8]?.ToString(),
                        }).ToList();
                    }
                }
                catch ( Exception x )
                {
                    XLogger.Error( "Exception : " + x );
                    return new List<PullMechanismModel>();
                }
            }
        }
        public bool MarkRawEventAsProcessed( string rawEventId )
        {
            using ( var context = new DataContext() )
            {
                OAParameter paramId = new OAParameter();
                paramId.ParameterName = IsOracle() ? "rowEventId" : "@rowId";
                paramId.DbType = DbType.String;
                paramId.Direction = ParameterDirection.Input;
                paramId.Value = rawEventId;

                try
                {
                    var result = context.ExecuteNonQuery( "MarkRawEventAsProcessed" , System.Data.CommandType.StoredProcedure ,

                                  new[]
                    {
                         paramId
                    } );
                    context.SaveChanges();   // dummy save to let the ORM know the operation has completed and commit any transaction.
                    return true;
                }
                catch ( Exception x )
                {
                    XLogger.Error( "Exception : " + x );
                    return false;
                }
            }
        }

        public static bool IsOracle()
        {
            //return false;
            return Broker.Settings.Datastore.Type == DatastoreType.Oracle;
        }
    }
}
