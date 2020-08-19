using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Data;
using ADS.Common.Context;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Definitions;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Actions
{
    [DataContract( IsReference = true )]
    public class ApprovalWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                var Definition = new AttendanceManualEditApprovalWorkflowDefinition();
                var dataLocal = PrepareData();

                var historyItem = GetHistoryItem( new Guid( dataLocal.TargetId ) );

                var previousStatus = historyItem.ManualAttendanceStatus;
                var newStatus = GetUpdatedStatusForAttendanceRawData( dataLocal );

                if ( IsStatusValidForApproval( previousStatus , newStatus ) )
                {
                    if ( newStatus == ManualAttendanceStatus.Approved )
                    {
                        // 1. Mark Associated History Item as Approved ...
                        historyItem.ManualAttendanceStatus = ManualAttendanceStatus.Approved;
                        UpdateHistoryItem( historyItem );

                        // 2. Mark RawData as Approved ...
                        AttendanceRawData rawData = null;
                        for ( int i = 0 ; i < 3 ; i++ )   // had to use a loop, in order to try overcoming a thread race issue ...
                        {
                            rawData = GetAttendanceRawData( historyItem.AttendanceRawDataId );
                            rawData.ManualAttendanceStatus = ManualAttendanceStatus.Approved;
                            if ( UpdateTarget( rawData ) ) break;
                        }
                        if ( rawData == null ) return false;

                        // 3. Mark Associated SE as Dirty ...
                        var SEs = GetScheduleEvents( rawData );
                        foreach ( var SE in SEs )
                        {
                            SE.IsDirty = true;
                            TamamServiceBroker.AttendanceHandler.UpdateScheduleEvent( SE );
                        }
                    }
                    else if ( newStatus == ManualAttendanceStatus.Denied )
                    {
                        var rawData = GetAttendanceRawData( historyItem.AttendanceRawDataId );

                        // 1. Check if there are Previous Approved Manual Edits..
                        var previousApprovedHistoryItems = rawData.DataHistoryItems.Where( x => x.ManualAttendanceStatus == ManualAttendanceStatus.Approved && x.Id != this.Step.Instance.TargetId ).ToList();
                        if (previousApprovedHistoryItems != null && previousApprovedHistoryItems.Count > 0 )
                        {
                            // if exist previous Approved History Item, use the last one value to update the RAW Data and change its status..
                            var lastHistoryItem = previousApprovedHistoryItems.OrderByDescending( x => x.UpdateTime ).First();

                            // NOTE : this loop is in place to avoid a race issue, and because of a caching service issue, we will have to update ...
                            // ... the raw attendance coming alone, and the one injected inside the history item model.
                            for ( int i = 0 ; i < 3 ; i++ )
                            {
                                #region TMP : working around the ORM conflict with the cache
                                historyItem.AttendanceRawData.AttendanceDateTime = lastHistoryItem.ValueNew;
                                historyItem.AttendanceRawData.ManualAttendanceStatus = ManualAttendanceStatus.Approved;
                                #endregion

                                rawData = GetAttendanceRawData( historyItem.AttendanceRawDataId );
                                rawData.AttendanceDateTime = lastHistoryItem.ValueNew;
                                rawData.ManualAttendanceStatus = ManualAttendanceStatus.Approved;
                                if ( UpdateTarget( rawData ) ) break;
                                else rawData = null;
                            }
                            if ( rawData == null ) return false;

                            // Update this History As Denied ...
                            historyItem.ManualAttendanceStatus = ManualAttendanceStatus.Denied;
                            UpdateHistoryItem( historyItem );
                        }
                        else
                        {
                            // if no exist previous Approved History Item, check if the current one, it's old value is null or not..
                            if ( !historyItem.ValueOld.HasValue )
                            {
                                // Delete Associated WF instances ..
                                //foreach ( AttendanceRawDataHistoryItem item in rawData.DataHistoryItems )
                                //{
                                //    Broker.WorkflowEngine.DeleteInstance( item , Definition );
                                //}

                                // if NULL Delete this RAW Data and it's own History..
                                SystemBroker.AttendanceHandler.DeleteAttendanceRawData( rawData );
                            }
                            else
                            {
                                // ELSE use its old Value to update the RAW Data
                                for ( int i = 0 ; i < 3 ; i++ )
                                {
                                    #region TMP : working around the ORM conflict with the cache
                                    historyItem.AttendanceRawData.AttendanceDateTime = historyItem.ValueOld.Value;
                                    historyItem.AttendanceRawData.ManualAttendanceStatus = ManualAttendanceStatus.NotSet;
                                    #endregion

                                    rawData = GetAttendanceRawData( historyItem.AttendanceRawDataId );
                                    rawData.AttendanceDateTime = historyItem.ValueOld.Value;
                                    rawData.ManualAttendanceStatus = ManualAttendanceStatus.NotSet;
                                    if ( UpdateTarget( rawData ) ) break;
                                    else rawData = null;
                                }
                                if ( rawData == null ) return false;

                                // Update this History As Denied ...
                                historyItem.ManualAttendanceStatus = ManualAttendanceStatus.Denied;
                                UpdateHistoryItem( historyItem );
                            }
                        }
                        
                        // 3. Mark Associated SE as Dirty ...
                        var SEs = GetScheduleEvents( rawData );
                        foreach ( var SE in SEs )
                        {
                            SE.IsDirty = true;
                            TamamServiceBroker.AttendanceHandler.UpdateScheduleEvent( SE );
                        }
                    }
                }
                else
                {
                    return false;
                }
                
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #region Helpers

        private AttendanceManualEditReviewWorkflowData PrepareData()
        {
            var data = this.Data as AttendanceManualEditReviewWorkflowData;
            if ( data == null || string.IsNullOrWhiteSpace( data.TargetId ) || string.IsNullOrWhiteSpace( data.TargetStatus ) ) throw new Exception( "invalid action data" );

            return data;
        }
        private bool IsStatusValidForApproval( ManualAttendanceStatus old , ManualAttendanceStatus neo )
        {
            // cannot approve AttendanceRawData unless it's status ( NotSet, Pending)

            return ( ( old == ManualAttendanceStatus.NotSet && neo == ManualAttendanceStatus.Approved ) ||
                     ( old == ManualAttendanceStatus.Pending && neo == ManualAttendanceStatus.Approved ) ||
                     ( old == ManualAttendanceStatus.Pending && neo == ManualAttendanceStatus.Denied ) )
                   && ( old != ManualAttendanceStatus.Approved && old != ManualAttendanceStatus.Denied );
        }
        private ManualAttendanceStatus GetUpdatedStatusForAttendanceRawData( AttendanceManualEditReviewWorkflowData data )
        {
            WorkflowAttendanceManualEditReviewStatus command;
            if ( !Enum.TryParse<WorkflowAttendanceManualEditReviewStatus>( data.TargetStatus , true , out command ) ) throw new Exception( "invalid command for target status" );

            switch ( command )
            {
                case WorkflowAttendanceManualEditReviewStatus.Approved: return ManualAttendanceStatus.Approved;
                case WorkflowAttendanceManualEditReviewStatus.Denied: return ManualAttendanceStatus.Denied;
                case WorkflowAttendanceManualEditReviewStatus.Cancelled: return ManualAttendanceStatus.NotSet;      // TODO : need more discussion..
                case WorkflowAttendanceManualEditReviewStatus.Pending:
                default: return ManualAttendanceStatus.NotSet;
            }
        }
        private AttendanceRawDataHistoryItem GetHistoryItem( Guid id )
        {
            var response = TamamServiceBroker.AttendanceHandler.GetAttendanceRawDataHistoryItem( id , SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success ) throw new Exception( "error getting target model" );
            return response.Result;
        }
        private AttendanceRawData GetAttendanceRawData( Guid id )
        {
            var response = TamamServiceBroker.AttendanceHandler.GetAttendanceRaw( id , SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success ) throw new Exception( "error getting target model" );
            return response.Result;
        }
        private bool UpdateTarget( AttendanceRawData rawData )
        {
            var response = TamamServiceBroker.AttendanceHandler.UpdateAttendanceRawData( rawData );
            return response.Type == ResponseState.Success;
        }
        private void UpdateHistoryItem( AttendanceRawDataHistoryItem item )
        {
            var response = TamamServiceBroker.AttendanceHandler.UpdateAttendanceRawDataHistoryItem( item );
        }
        private List<ScheduleEvent> GetScheduleEvents( AttendanceRawData rawData )
        {
            return TamamServiceBroker.AttendanceHandler.GetScheduleEvent( rawData.PersonId , rawData.AttendanceDateTime ).Result;
        }

        #endregion
    }
}