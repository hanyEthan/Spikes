using System.Linq;
using System.Collections.Generic;
using ADS.Tamam.Common.Data.Model.Enums;
using FluentValidation;
using ADS.Common.Validation;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Resources.Culture;
//using ADS.Tamam.Common.Data.Handlers;

namespace ADS.Tamam.Common.Data.Validation
{
    public class ApprovalPolicyValidator : IModelValidator
    {
        #region props ...

        private ApprovalPolicy _policy { get; set; }

        #endregion
        #region cst.

        public ApprovalPolicyValidator( Policy model , TamamConstants.ValidationMode mode )
        {
            var context = new ValidationContext ( mode );
            _policy = new ApprovalPolicy( model );
            var result = context.Validate( _policy );
            IsValid = result.IsValid;
            Errors = result.Errors.Select( e => e.ErrorMessage ).ToList();
            ErrorsDetailed = result.Errors.Select( e => new ModelMetaPair( e.PropertyName , e.ErrorMessage ) ).ToList();
        }

        #endregion

        internal class ValidationContext : AbstractValidator<ApprovalPolicy>
        {
            public ValidationContext( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor ( app => app.DirectManagerSequance ).NotEmpty ().When ( app => app.DirectManager.Value ).WithMessage ( ValidationResources.DirectManagerSequanceEmpty );
                    RuleFor ( app => app.SuperManagerSequance ).NotEmpty ().When ( app => app.SuperManager.Value ).WithMessage ( ValidationResources.SuperManagerSequanceEmpty );
                    RuleFor ( app => app.HRSequance ).NotEmpty ().When ( app => app.HR.Value ).WithMessage ( ValidationResources.HRSequanceEmpty );

                    RuleFor ( app => app.DirectManagerSequance ).GreaterThanOrEqualTo ( 1 ).When ( app => app.DirectManager.Value ).WithMessage ( ValidationResources.DirectManagerSequanceEmpty );
                    RuleFor ( app => app.SuperManagerSequance ).GreaterThanOrEqualTo ( 1 ).When ( app => app.SuperManager.Value ).WithMessage ( ValidationResources.SuperManagerSequanceEmpty );
                    RuleFor ( app => app.HRSequance ).GreaterThanOrEqualTo ( 1 ).When ( app => app.HR.Value ).WithMessage ( ValidationResources.HRSequanceEmpty );

                    RuleFor ( app => app.DirectManagerSequance ).LessThanOrEqualTo ( 3 ).When ( app => app.DirectManager.Value ).WithMessage ( ValidationResources.DirectManagerSequanceEmpty );
                    RuleFor ( app => app.SuperManagerSequance ).LessThanOrEqualTo ( 3 ).When ( app => app.SuperManager.Value ).WithMessage ( ValidationResources.SuperManagerSequanceEmpty );
                    RuleFor ( app => app.HRSequance ).LessThanOrEqualTo ( 3 ).When ( app => app.HR.Value ).WithMessage ( ValidationResources.HRSequanceEmpty );
        
                    RuleFor ( app => app.ApprovalSteps ).Must ( IsValidApprovalSteps ).WithMessage ( ValidationResources.ApprovalStepsUnique ); 
                }
                if ( mode == TamamConstants.ValidationMode.Delete )
                {
                    RuleFor ( app => app.Code ).Must ( CheckRelation ).WithMessage ( ValidationResources.PolicyDeleteFail );
                }
            }

            private bool CheckRelation(ApprovalPolicy instance, string arg )
            {
                //OrganizationDataHandler handler = new OrganizationDataHandler ();
                //return handler.GetPolicyRelationCount ( instance.Policy.Id ) > 0 ? false : true;
                return true;
            }

            private bool IsValidApprovalSteps ( List<ApprovalStep> steps )
            {
                foreach ( var item in steps )
                {
                    if (
                        steps.Any(
                            s => (s.Sequance == item.Sequance) && s.Sequance.HasValue && (s.StepType != item.StepType)))
                        return false;
                }
                return true;
            }
        }

        #region IModelValidator

        public bool? IsValid { get; private set; }
        public List<string> Errors { get; private set; }
        public List<ModelMetaPair> ErrorsDetailed { get; private set; }

        #endregion
    }
}
