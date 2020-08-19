using ADS.Tamam.Common.Data.Model.Enums;
using System.Collections.Generic;
using OpenAccessRuntime.DataObjects;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class ApprovalPolicy : AbstractSpecialPolicy
    {
        public bool? EnableIterations { get; private set; }
        public int? MaxIterationsCount { get; private set; }
        public bool? IncludePassedStepsPerIteration { get; private set; }

        public bool? DirectManager { get; private set; }
        public int? DirectManagerSequance { get; private set; }
        public int? DirectManagerCondition { get; private set; }

        public bool? SuperManager { get; private set; }
        public int? SuperManagerSequance { get; private set; }
        public int? SuperManagerCondition { get; private set; }

        public bool? HR { get; private set; }
        public int? HRSequance { get; private set; }
        public int? HRCondition { get; private set; }

        public List<ApprovalStep> ApprovalSteps
        {
            get
            {
                var steps = new List<ApprovalStep>();
                if ( DirectManager ?? false )
                    steps.Add( new ApprovalStep { Sequance = DirectManagerSequance , Condition = DirectManagerCondition ?? 0 , StepType = StepType.Manager } );
                if ( SuperManager ?? false )
                    steps.Add( new ApprovalStep { Sequance = SuperManagerSequance , Condition = SuperManagerCondition ?? 0 , StepType = StepType.SuperManager } );
                if ( HR ?? false )
                    steps.Add( new ApprovalStep { Sequance = HRSequance , Condition = HRCondition ?? 0 , StepType = StepType.HR } );
                return steps;
            }
        }

        public ApprovalPolicy( PolicyModel policy ) : base( policy )
        {
            this.EnableIterations = GetBool( PolicyFields.ApprovalsPolicy.EnableIterations );
            this.MaxIterationsCount = GetInt( PolicyFields.ApprovalsPolicy.MaxIterationsCount );
            this.IncludePassedStepsPerIteration = true;

            this.DirectManager = GetBool( PolicyFields.ApprovalsPolicy.DirectManager );
            this.DirectManagerSequance = GetInt( PolicyFields.ApprovalsPolicy.DirectManagerSequance );
            this.DirectManagerCondition = GetInt( PolicyFields.ApprovalsPolicy.DirectManagerCondition );
            this.SuperManager = GetBool( PolicyFields.ApprovalsPolicy.SuperManager );
            this.SuperManagerSequance = GetInt( PolicyFields.ApprovalsPolicy.SuperManagerSequance );
            this.SuperManagerCondition = GetInt( PolicyFields.ApprovalsPolicy.SuperManagerCondition );
            this.HR = GetBool( PolicyFields.ApprovalsPolicy.HR );
            this.HRSequance = GetInt( PolicyFields.ApprovalsPolicy.HRSequance );
            this.HRCondition = GetInt( PolicyFields.ApprovalsPolicy.HRCondition );
        }
    }

    public class ApprovalStep
    {
        public int? Sequance { get; set; }
        public int Condition { get; set; }
        public StepType StepType { get; set; }
    }
    public enum StepType { Manager = 1 , SuperManager = 2 , HR = 3 }
}
