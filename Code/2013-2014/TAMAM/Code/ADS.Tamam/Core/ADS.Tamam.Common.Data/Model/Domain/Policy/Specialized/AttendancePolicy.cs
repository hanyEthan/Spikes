using ADS.Common.Context;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using System;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class AttendancePolicy : AbstractSpecialPolicy
    {
        #region props ...

        public bool EnableManualEditApprovals { get; private set; }
        public ApprovalPolicy ManualEditApprovalPolicy { get; private set; }
        public ApprovalPolicy ViolationsApprovalPolicy { get; private set; }

        public bool ConsiderEarlyLeaveAsViolation { get; private set; }
        public bool ConsiderAbsentAsViolation { get; private set; }
        public bool ConsiderInLateAsViolation { get; private set; }
        public bool ConsiderLateAbsentAsViolation { get; private set; }
        public bool ConsiderMissedPunchAsViolation { get; private set; }
        public bool ConsiderWorkingLessAsViolation { get; private set; }

        #endregion
        #region cst ...

        public AttendancePolicy( PolicyModel policy ) : base ( policy )
        {
            var enableManualEdit = GetBool( PolicyFields.AttendancePolicy.EnableManualEditApprovals );

            var considerEarlyLeaveAsViolation = GetBool(PolicyFields.AttendancePolicy.ConsiderEarlyLeaveAsViolation);
            var considerAbsentAsViolation = GetBool(PolicyFields.AttendancePolicy.ConsiderAbsentAsViolation);
            var considerInLateAsViolation = GetBool(PolicyFields.AttendancePolicy.ConsiderInLateAsViolation);
            var considerLateAbsentAsViolation = GetBool(PolicyFields.AttendancePolicy.ConsiderLateAbsentAsViolation);
            var considerMissedPunchAsViolation = GetBool(PolicyFields.AttendancePolicy.ConsiderMissedPunchAsViolation);
            var considerWorkingLessAsViolation = GetBool(PolicyFields.AttendancePolicy.ConsiderWorkingLessAsViolation);

            EnableManualEditApprovals = enableManualEdit.HasValue && enableManualEdit.Value;

            ConsiderEarlyLeaveAsViolation = considerEarlyLeaveAsViolation.HasValue && considerEarlyLeaveAsViolation.Value;
            ConsiderAbsentAsViolation = considerAbsentAsViolation.HasValue && considerAbsentAsViolation.Value;
            ConsiderInLateAsViolation = considerInLateAsViolation.HasValue && considerInLateAsViolation.Value;
            ConsiderLateAbsentAsViolation = considerLateAbsentAsViolation.HasValue && considerLateAbsentAsViolation.Value;
            ConsiderMissedPunchAsViolation = considerMissedPunchAsViolation.HasValue && considerMissedPunchAsViolation.Value;
            ConsiderWorkingLessAsViolation = considerWorkingLessAsViolation.HasValue && considerWorkingLessAsViolation.Value;

            InitializeApprovalPolicy();
        }

        #endregion
        #region Helpers

        private void InitializeApprovalPolicy()
        {
            var manualEditApprovalPolicyId = GetGuid( PolicyFields.AttendancePolicy.ManualEditApprovalPolicy );
            ManualEditApprovalPolicy = manualEditApprovalPolicyId.HasValue ? GetApprovlPolicy( manualEditApprovalPolicyId.Value ) : null;

            var violationsApprovalPolicyId = GetGuid( PolicyFields.AttendancePolicy.ViolationsApprovalPolicy );
            ViolationsApprovalPolicy = violationsApprovalPolicyId.HasValue ? GetApprovlPolicy( violationsApprovalPolicyId.Value ) : null;
        }
        private ApprovalPolicy GetApprovlPolicy( Guid id )
        {
            //var dataHandler = new OrganizationDataHandler();
            //var response = dataHandler.GetPolicy( id );
            //if ( response.Type != ResponseState.Success ) return null;
            //if ( response.Result.PolicyTypeId != new Guid( PolicyTypes.ApprovalPolicyType ) ) return null;

            //return new ApprovalPolicy( response.Result );
            return null;
        }

        #endregion
    }
}