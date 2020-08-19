using System.Linq;
using System.Collections.Generic;

using FluentValidation;

using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using System;

namespace ADS.Tamam.Common.Data.Validation
{
    public class ExcusesPolicyValidator : IModelValidator
    {
        #region props ...

        private ExcusePolicy _policy { get; set; }

        # endregion
        # region cst.

        public ExcusesPolicyValidator(Policy model, TamamConstants.ValidationMode mode)
        {
            var context = new ValidationContext(mode);
            _policy = new ExcusePolicy(model);
            var result = context.Validate(_policy);
            IsValid = result.IsValid;
            Errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            ErrorsDetailed = result.Errors.Select(e => new ModelMetaPair(e.PropertyName, e.ErrorMessage)).ToList();
        }

        # endregion

        internal class ValidationContext : AbstractValidator<ExcusePolicy>
        {
            public ValidationContext(TamamConstants.ValidationMode mode)
            {
                if (mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit)
                {
                    // with credit
                    RuleFor(policy => policy.MaxExcusesPerDay).Must(IsValidMaxExcusesPerDay_WithCredit).WithMessage(ValidationResources.InvalidExcusePolicyMaxExcusesPerDay);
                    RuleFor(policy => policy.MaxExcusesPerMonth).Must(IsValidMaxExcusesPerMonth_WithCredit).WithMessage(ValidationResources.InvalidExcusePolicyMaxExcusesPerMonth);
                    RuleFor(policy => policy.AllowedHoursPerDay).Must(IsValidAllowedHoursPerDay_WithCredit).WithMessage(ValidationResources.InvalidExcusePolicyAllowedHoursPerDay);
                    RuleFor(policy => policy.AllowedHoursPerMonth).Must(IsValidAllowedHoursPerMonth_WithCredit).WithMessage(ValidationResources.InvalidExcusePolicyAllowedHoursPerMonth);
                    RuleFor(policy => policy.MinExcuseDuration).Must(IsValidMinExcuseDuration_WithCredit).WithMessage(ValidationResources.InvalidExcusePolicyMinDurationPerExcuse);

                    // without credit
                    RuleFor(policy => policy.MaxExcusesPerDay).Must(IsValidMaxExcusesPerDay_NoCredit).WithMessage(ValidationResources.InvalidExcusePolicyMaxExcusesPerDay_NoCredit);
                    RuleFor(policy => policy.MaxExcusesPerMonth).Must(IsValidMaxExcusesPerMonth_NoCredit).WithMessage(ValidationResources.InvalidExcusePolicyMaxExcusesPerMonth_NoCredit);
                    RuleFor(policy => policy.AllowedHoursPerDay).Must(IsValidAllowedHoursPerDay_NoCredit).WithMessage(ValidationResources.InvalidExcusePolicyAllowedHoursPerDay_NoCredit);
                    RuleFor(policy => policy.AllowedHoursPerMonth).Must(IsValidAllowedHoursPerMonth_NoCredit).WithMessage(ValidationResources.InvalidExcusePolicyAllowedHoursPerMonth_NoCredit);
                    RuleFor(policy => policy.MinExcuseDuration).Must(IsValidMinExcuseDuration_NoCredit).WithMessage(ValidationResources.InvalidExcusePolicyMinDurationPerExcuse_NoCredit);
                
                    RuleFor(policy => policy.ApprovalPolicy).Must(IsValidApprovalPolicy).WithMessage(ValidationResources.InvalidApprovalPolicy);
                }
            }

            private bool IsValidMaxExcusesPerDay_WithCredit( ExcusePolicy instance, int? maxExcusesPerDay )
            {
                if ( instance.HasCredit )
                {
                    return maxExcusesPerDay.HasValue && maxExcusesPerDay.Value >= 0 && maxExcusesPerDay.Value <= 48;
                }

                return true;
            }
            private bool IsValidMaxExcusesPerMonth_WithCredit(ExcusePolicy instance, int? maxExcusesPerMonth)
            {
                if ( instance.HasCredit )
                {
                    // 1488 = 31 (max days per month) * 48 (max excuses per day)
                    return maxExcusesPerMonth.HasValue && maxExcusesPerMonth.Value >= 0 && maxExcusesPerMonth.Value <= 1488;
                }

                return true;
            }
            private bool IsValidAllowedHoursPerDay_WithCredit(ExcusePolicy instance, double? allowedHoursPerDay)
            {
                if ( instance.HasCredit )
                {
                    // 24 =  day
                    double Fractionalpart = allowedHoursPerDay.HasValue ? allowedHoursPerDay.Value - Math.Floor( allowedHoursPerDay.Value ) : 0;
                    return allowedHoursPerDay.HasValue && allowedHoursPerDay.Value >= 0 && (Fractionalpart == 0.0 || Fractionalpart == 0.5) && allowedHoursPerDay.Value <= 24;
                }

                return true;
            }
            private bool IsValidAllowedHoursPerMonth_WithCredit(ExcusePolicy instance, double? allowedHoursPerMonth)
            {
                if ( instance.HasCredit )
                {
                    // 744 = 24 (max hours per day) * 31 (max month days)
                    double Fractionalpart = allowedHoursPerMonth.HasValue ? allowedHoursPerMonth.Value - Math.Floor( allowedHoursPerMonth.Value ) : 0;
                    return allowedHoursPerMonth.HasValue && allowedHoursPerMonth.Value >= 0 && (Fractionalpart == 0.0 || Fractionalpart == 0.5) && allowedHoursPerMonth.Value <= 744;
                }

                return true;
            }
            private bool IsValidMinExcuseDuration_WithCredit(ExcusePolicy instance, double? minExcuseDuration)
            {
                if ( instance.HasCredit )
                {
                    //double Fractionalpart = minExcuseDuration.Value - Math.Floor(minExcuseDuration.Value);
                    //return minExcuseDuration.HasValue && minExcuseDuration.Value >= 0.5 && ( Fractionalpart == 0.0 || Fractionalpart == 0.5 );
                    return minExcuseDuration.HasValue;
                }

                return true;
            }

            private bool IsValidMaxExcusesPerDay_NoCredit( ExcusePolicy instance, int? maxExcusesPerDay )
            {
                if ( instance.HasCredit )
                {
                    return true;
                }

                return !maxExcusesPerDay.HasValue || maxExcusesPerDay.Value == 0;
            }
            private bool IsValidMaxExcusesPerMonth_NoCredit(ExcusePolicy instance, int? maxExcusesPerMonth)
            {
                if (instance.HasCredit)
                {
                    return true;
                }

                return !maxExcusesPerMonth.HasValue || maxExcusesPerMonth.Value == 0;

            }
            private bool IsValidAllowedHoursPerDay_NoCredit(ExcusePolicy instance, double? allowedHoursPerDay)
            {
                if (instance.HasCredit)
                {
                    return true;
                }

                return  !allowedHoursPerDay.HasValue || allowedHoursPerDay.Value == 0;
            }
            private bool IsValidAllowedHoursPerMonth_NoCredit(ExcusePolicy instance, double? allowedHoursPerMonth)
            {
                if (instance.HasCredit)
                {
                    return true;
                }

                return !allowedHoursPerMonth.HasValue || allowedHoursPerMonth.Value == 0;
            }
            private bool IsValidMinExcuseDuration_NoCredit(ExcusePolicy instance, double? minExcuseDuration)
            {
                if (instance.HasCredit)
                {
                    return true;
                }

                return !minExcuseDuration.HasValue || minExcuseDuration.Value == 0;
            }
            
            private bool IsValidApprovalPolicy(ExcusePolicy instance, ApprovalPolicy approvalPolicy)
            {
                return approvalPolicy != null;
            }
        }

        #region IModelValidator

        public bool? IsValid { get; private set; }
        public List<string> Errors { get; private set; }
        public List<ModelMetaPair> ErrorsDetailed { get; private set; }

        # endregion
    }
}
