using System;
using System.Runtime.Serialization;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;
using ADS.Tamam.Common.Workflow.Leaves.Review.Data;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Workflow.Leaves.Review.Actions
{
    [DataContract( IsReference = true )]
    public class LeaveReviewWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                if( Step == null ) return false;

                var dataLocal = this.Data as LeaveReviewWorkflowData;
                var dataPassed = data as LeaveReviewWorkflowData;

                //dataPassed = dataPassed ?? dataLocal;   // if nothing's passed, use locally stored data ...

                if ( dataLocal == null || dataPassed == null) return false;

                if ( data.Command != WorkflowLeaveReviewStatus.SystemApproved.ToString() )
                {
                    // check if the reviewer is the expected one or can act for ...
                    var canActForResponse = TamamServiceBroker.PersonnelHandler.CanActFor( new Guid( dataPassed.PersonId ) , new Guid( dataLocal.ApproverIdExpected ) , SystemRequestContext.Instance );
                    if ( canActForResponse.Type != ResponseState.Success ) return false;
                    if ( !canActForResponse.Result ) return false;
                }
                WorkflowLeaveReviewStatus command;
                if ( !Enum.TryParse<WorkflowLeaveReviewStatus>( dataPassed.Command , true , out command ) ) return false;   // check if the command is valid ...

                dataLocal.Command = dataPassed.Command;
                dataLocal.TargetStatus = dataPassed.Command;
                dataLocal.PersonId = dataPassed.PersonId;
                dataLocal.Metadata = dataPassed.Metadata;

                #region cache ...

                bool isExcuse = true;   // TODO fill this bool correctly, according to the target type ...
                if ( isExcuse )
                {
                    // excuse ...
                    Broker.Cache.Invalidate( TamamCacheClusters.Excuses );
                    Broker.Cache.Invalidate( TamamCacheClusters.Leaves );
                }
                else
                {
                    // leave ...
                    Broker.Cache.Invalidate( TamamCacheClusters.Leaves );
                }

                #endregion
                
                Step.Data = dataLocal;   // data is valid ...
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
    }
}
