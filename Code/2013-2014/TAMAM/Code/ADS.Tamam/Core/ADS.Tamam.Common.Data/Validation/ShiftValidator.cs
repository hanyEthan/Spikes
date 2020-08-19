using System;
using System.Linq;

using FluentValidation;

using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;

namespace ADS.Tamam.Common.Data.Validation
{
    public class ShiftValidator : AbstractModelValidator<Shift>
    {
        #region cst.

        public ShiftValidator( Shift model , TamamConstants.ValidationMode mode ) : base( model , new ValidationContext( mode ) )
        {
        }

        #endregion
        #region classes

        internal class ValidationContext : AbstractValidator<Shift>
        {
            #region props.

            private SchedulesDataHandler DataHandler { get; set; }
            private OrganizationDataHandler OrganizationDataHandler { get; set; }

            #endregion

            public ValidationContext()
            {
                this.DataHandler = new SchedulesDataHandler();
                this.OrganizationDataHandler = new OrganizationDataHandler();
            }
            public ValidationContext( TamamConstants.ValidationMode mode ) : this()
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor( shift => shift.Name ).NotEmpty().WithMessage( ValidationResources.NameEmpty );
                    RuleFor( shift => shift.NameCultureVarient ).NotEmpty().WithMessage( ValidationResources.NameEmpty );
                    RuleFor( shift => shift.Name ).Must( IsNameUnique ).WithMessage( ValidationResources.NameNotUnique );
                    RuleFor( shift => shift.NameCultureVarient ).Must( IsNameCultureVariantUnique ).WithMessage( ValidationResources.NameCultureVariantNotUnique );

                    RuleFor( shift => shift.Name ).Length( 0 , 100 ).WithMessage( string.Format( ValidationResources.NameLengthInvalid , 100 ) );
                    RuleFor( shift => shift.NameCultureVarient ).Length( 0 , 100 ).WithMessage( string.Format( ValidationResources.ArabicNameLengthInvalid , 100 ) );

                    RuleFor( shift => shift.ShiftPolicyId ).Must( IsValidShiftPolicy ).WithMessage( ValidationResources.InvalidShiftPolicy );

                    RuleFor( shift => shift.IsNightShift ).Must( IsValidNightFlexibleCombination ).WithMessage( ValidationResources.InValidNightFlexibleCombination );
                    RuleFor( shift => shift.IsNightShift ).Must( IsValidDuration ).WithMessage( ValidationResources.InValidDuration );
                    RuleFor( shift => shift.StartTime ).Must( ValidateShiftTimes ).WithMessage( ValidationResources.InvalidShiftTimes );

                    if ( mode == TamamConstants.ValidationMode.Edit )
                    {
                        RuleFor( shift => shift.Id ).Must( ValidateTemplates ).WithMessage( ValidationResources.ShiftInTemplatesOverlap );
                    }
                }
                else if ( mode == TamamConstants.ValidationMode.Delete || mode == TamamConstants.ValidationMode.Deactivate )
                {
                    RuleFor( shift => shift.Id ).Must( ShiftNotUsedInSchedules ).WithMessage( ValidationResources.ShiftHaveTemplates );
                    RuleFor( shift => shift.Id ).Must( ShiftNotUsedInPolicies ).WithMessage( ValidationResources.ShiftHavePolicies );
                }
            }

            #region Helpers

            private bool IsNameUnique( Shift instance , string Name )
            {
                return this.DataHandler.IsShiftNameUnique( instance );
            }
            private bool IsNameCultureVariantUnique( Shift instance , string NameCultureVariant )
            {
                return this.DataHandler.IsShiftNameCultureVariantUnique( instance );
            }
            private bool IsValidShiftPolicy( Shift instance , Guid policyId )
            {
                if ( policyId == Guid.Empty ) return false;

                var handler = new OrganizationDataHandler();
                var policy = handler.GetPolicy( policyId ).Result;
                var isValid = policy != null && policy.PolicyTypeId == Guid.Parse( PolicyTypes.ShiftPolicyType );

                return isValid;
            }

            private bool ShiftNotUsedInSchedules( Shift instance , Guid shiftId )
            {
                var shift = this.DataHandler.GetShiftWithScheduleTemplates( shiftId );
                return shift.TemplateDetails == null || shift.TemplateDetails.Count == 0;
            }
            private bool ShiftNotUsedInPolicies( Shift instance , Guid shiftId )
            {
                var policies = this.OrganizationDataHandler.GetPolicies( Guid.Parse( PolicyTypes.IslamicPolicyType ) ).Result.Select( x => new IslamicPolicy( x ) );
                return !policies.Any( x => x.RamadanShift.Id == shiftId );
            }

            private bool ValidateShiftTimes( Shift instance , DateTime? start )
            {
                if ( instance.IsFlexible ) return !instance.StartTime.HasValue && !instance.EndTime.HasValue;
                if ( instance.IsNightShift ) return instance.StartTime.HasValue && instance.EndTime.HasValue;

                // normal shift
                return instance.StartTime.HasValue && 
                       instance.EndTime.HasValue &&
                       instance.EndTime.Value.TimeOfDay > instance.StartTime.Value.TimeOfDay;
            }

            private bool IsValidDuration( Shift shift , bool isNightShift )
            {
                if ( shift.Duration <= 0 ) return false;

                var maxDuration = 24;//shift.IsNightShift ? 48 : 24;
                if ( shift.Duration > maxDuration ) return false;

                if ( !shift.IsFlexible )
                {
                    var durationCalculated = ( shift.EndTime.Value.AddDays( shift.IsNightShift ? 1 : 0 ) - shift.StartTime.Value ).TotalHours;
                    var duration = shift.Duration;

                    if ( Math.Abs( Convert.ToDouble( duration ) - durationCalculated ) > 0.05 ) return false;
                }

                return true;
            }
            private bool IsValidNightFlexibleCombination( Shift shift , bool isNightShift )
            {
                return !( shift.IsNightShift && shift.IsFlexible );
            }
            private bool ValidateTemplates( Shift arg , Guid id )
            {
                //Make sure the new shift values doesn't overlap in any schedule template.
                try
                {
                    var days = this.DataHandler.GetTemplateDaysByShiftId( arg.Id ).Result;
                    foreach ( var day in days )
                    {
                        foreach ( var shift in day.DayShifts.Where( s => s.ShiftId == arg.Id ) )
                        {
                            shift.Shift.StartTime = arg.StartTime;
                            shift.Shift.EndTime = arg.EndTime;
                        }
                        if ( !Shift.CheckOverlap( day.DayShifts.Select( s => s.Shift ).ToList() ) )
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

            #endregion
        }

        #endregion
    }
}
