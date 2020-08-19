using ADS.Common.Context;
using ADS.Common.Validation;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Resources.Culture;
using FluentValidation;

namespace ADS.Tamam.Common.Data.Validation
{
    public class HolidayValidator : AbstractModelValidator<Holiday>
    {
        #region cst.

        public HolidayValidator(Holiday model, TamamConstants.ValidationMode mode) : base(model, new ValidationContext(mode))
        {
        }

        # endregion

        #region classes

        internal class ValidationContext : AbstractValidator<Holiday>
        {
            public ValidationContext( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor( holiday => holiday.Name ).NotEmpty().WithMessage( ValidationResources.NameEmpty );
                    RuleFor( holiday => holiday.NameCultureVariant ).NotEmpty().WithMessage( ValidationResources.ArabicNameEmpty );
                    RuleFor( holiday => holiday.StartDate ).NotEmpty().WithMessage( ValidationResources.StartDateEmpty );
                    RuleFor( holiday => holiday.EndDate ).NotEmpty().WithMessage( ValidationResources.EndDateEmpty );
                    RuleFor( holiday => holiday.StartDate ).LessThanOrEqualTo( holiday => holiday.EndDate ).WithMessage( ValidationResources.StartDateLessEndDate );

                    RuleFor( holiday => holiday.Code ).NotEmpty().WithMessage( ValidationResources.HolidayCodeEmpty );
                    RuleFor( holiday => holiday.Code ).Must( IsCodeUnique ).WithMessage( ValidationResources.HolidayCodeUnique );
                    RuleFor( holiday => holiday.Code ).Length( 1 , 50 ).WithMessage( ValidationResources.HolidayCodeLength );
                }
            }

            #region helpers

            private bool IsCodeUnique( Holiday instance , string code )
            {
                var dataHandler = new OrganizationDataHandler();
                var response = dataHandler.IsNativeHolidayCodeUnique( instance );

                return response.Result && response.Type == ResponseState.Success;
            }
            
            #endregion
        }

        #endregion
    }
}
