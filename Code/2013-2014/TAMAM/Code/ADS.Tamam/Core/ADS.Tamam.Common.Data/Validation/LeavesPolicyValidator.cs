using System.Linq;
using System.Collections.Generic;

using FluentValidation;

using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Validation
{
    public class LeavesPolicyValidator : IModelValidator
    {
        #region props ...

        private LeavePolicy _policy { get; set; }

        #endregion
        #region cst.

        public LeavesPolicyValidator( Policy model , TamamConstants.ValidationMode mode )
        {
            var context = new ValidationContext ( mode );
            _policy = new LeavePolicy( model );
            var result = context.Validate( _policy );
            IsValid = result.IsValid;
            Errors = result.Errors.Select( e => e.ErrorMessage ).ToList();
            ErrorsDetailed = result.Errors.Select( e => new ModelMetaPair( e.PropertyName , e.ErrorMessage ) ).ToList();
        }

        #endregion

        internal class ValidationContext : AbstractValidator<LeavePolicy>
        {
            public ValidationContext( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor ( policy => policy.AllowedAmount ).Must ( IsValidAmount ).WithMessage ( ValidationResources.InvalidLeavePolicyAmount );
                    RuleFor ( policy => policy.DaysBeforeRequest ).Must ( IsValidRequest ).WithMessage ( ValidationResources.InvalidLeavePolicyRequest );
                    RuleFor ( policy => policy.MaxCarryOverDays ).Must ( IsValidCarryover ).WithMessage ( ValidationResources.InvalidLeavePolicyCarryOver );

                    RuleFor ( policy => policy.LeaveType ).Must ( IsValidLeaveType ).WithMessage ( ValidationResources.InvalidLeaveType );
                    RuleFor ( policy => policy.ApprovalPolicy ).Must ( IsValidApprovalPolicy ).WithMessage ( ValidationResources.InvalidApprovalPolicy );
                }
            }

            private bool IsValidAmount( LeavePolicy instance , int? allowedAmount )
            {
                return allowedAmount.HasValue && allowedAmount.Value >= 0 && allowedAmount.Value <= 300;
            }
            private bool IsValidRequest( LeavePolicy instance , int? daysBeforeRequest )
            {
                if ( !instance.AllowRequests.HasValue ) return false;
                if ( !instance.AllowRequests.Value ) return true;
                if ( instance.DaysBeforeRequest.HasValue && instance.DaysBeforeRequest.Value >= 0 ) return true;

                return false;
            }
            private bool IsValidCarryover( LeavePolicy instance , int? maxCarryOver )
            {
                if ( !instance.SupportCarryOver.HasValue ) return false;
                if ( !instance.SupportCarryOver.Value ) return true;
                if ( instance.MaxCarryOverDays.HasValue && instance.MaxCarryOverDays.Value >= 0 ) return true;

                return false;
            }

            private bool IsValidLeaveType( LeavePolicy instance , LeaveTypes? leaveType )
            {
                return leaveType.HasValue;
            }
            private bool IsValidApprovalPolicy( LeavePolicy instance , ApprovalPolicy approvalPolicy )
            {
                return approvalPolicy != null;
            }
        }

        #region IModelValidator

        public bool? IsValid { get; private set; }
        public List<string> Errors { get; private set; }
        public List<ModelMetaPair> ErrorsDetailed { get; private set; }

        #endregion
    }
}