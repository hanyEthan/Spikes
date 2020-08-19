using System;
using System.Runtime.Serialization;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Actions
{
    [DataContract( IsReference = true )]
    public class InitializationWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                if ( Step == null ) return false;

                // for any future actions ..

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