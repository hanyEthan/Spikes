using System;
using ADS.Common.Validation;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using FluentValidation;
using ADS.Tamam.Resources.Culture;

namespace ADS.Tamam.Common.Data.Validation
{
    public class DeleteShiftValidator : AbstractModelValidator<Shift>
    {
        #region cst.

        public DeleteShiftValidator(Shift model)
            : base ( model , new ValidationContext ( ) )
        {
        }

        #endregion
        #region classes

        internal class ValidationContext : AbstractValidator<Shift>
        {
            public ValidationContext ( )
            {
                RuleFor(shift => shift.Id).Must(CanDeleteShift).WithMessage(ValidationResources.ShiftHaveTemplates);
            }

            #region Helpers

            private bool CanDeleteShift ( Shift instance , Guid shiftId )
            {
                var handler = new SchedulesDataHandler();
                var shift = handler.GetShiftWithScheduleTemplates(shiftId);
                return shift.TemplateDetails == null || shift.TemplateDetails.Count == 0;
            }
            
            #endregion 
        }

        #endregion
    }
}
