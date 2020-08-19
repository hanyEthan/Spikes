using System;

using Quartz;

using ADS.Common.Context;
using ADS.Common.Utilities;

namespace ADS.Tamam.Common.Handlers.Listeners
{
    public class ScheduleEventListener : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            XLogger.Info( "ScheduleEventListener.Execute ... " );

            try
            {
                // extract required info ...
                //var scheduleId = new Guid( context.JobDetail.Key.Group );
                //var scheduleEventId = new Guid( context.JobDetail.Key.Name );
                //var triggerTime = context.JobDetail.JobDataMap.GetDateTime( "FireTime" );

                //XLogger.Info( "ScheduleEventListener.Execute ... schedule : {0} / event : {1} / trigger time : {2}" , scheduleId.ToString() , scheduleEventId.ToString() , triggerTime.ToString() );

                // check schedule event status ...
                //var response = TamamServiceBroker.SchedulesHandler.CheckScheduleEventStatus( scheduleEventId );
                //if ( response.Type != ResponseState.Success )
                //{
                //     if event not found or system error ...
                //     ...

                //    return;
                //}
                //if ( !response.Result )
                //{
                //     if not attended (needs more logic than that when it's time to delve into this module) ...
                //     change schedule event status  ...
                //     ...

                //     change attendance status (missed punch ?) ...
                //     ...
                //}

                // get schedule event ...
                //var eventResponse = TamamServiceBroker.SchedulesHandler.GetScheduleEvent( scheduleEventId );
                //if ( eventResponse.Type != ResponseState.Success )
                //{
                //     if event not found or system error ...
                //     ...

                //    return;
                //}

                // schedule event model ...
                //var scheduleEvent = eventResponse.Result;

                // create next schedule event ...
                //response = TamamServiceBroker.SchedulesHandler.CreateNextScheduleEvent( scheduleEvent.Schedule , triggerTime );
                //if ( eventResponse.Type != ResponseState.Success )
                //{
                //     if system error ...
                //     ...

                //    return;
                //}
            }
            catch(Exception x)
            {
                XLogger.Error( "ScheduleEventListener.Execute ... Exception : " + x );
            }
        }
    }
}
