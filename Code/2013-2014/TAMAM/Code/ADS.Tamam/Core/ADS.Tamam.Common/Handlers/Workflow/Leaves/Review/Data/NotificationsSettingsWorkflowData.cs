using System.Runtime.Serialization;
using ADS.Common.Workflow.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Data
{
    [DataContract( IsReference = true )]
    public class NotificationsSettingsWorkflowData : WorkflowBaseData
    {
        [DataMember] public int MaxIterations { get; set; }
        [DataMember] public bool IncludePastStepsPerIteration { get; set; }
        
        [DataMember] public int CurrentIterationNumber { get; set; }

        #region cst

        public NotificationsSettingsWorkflowData() { }
        public NotificationsSettingsWorkflowData( int maxIterations , bool includePastStepsPerIteration ) : this()
        {
            this.MaxIterations = maxIterations;
            this.IncludePastStepsPerIteration = includePastStepsPerIteration;
        }

        #endregion
        #region Helpers

        public int IncrementIterations()
        {
            return ++CurrentIterationNumber;
        }

        #endregion
    }
}