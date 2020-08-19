using System;
using ADS.Common.Validation;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using FluentValidation;

namespace ADS.Tamam.Common.Data.Validation
{
    public class SchedulePersonValidator : AbstractModelValidator<SchedulePerson>
    {
        #region cst.

        public SchedulePersonValidator(SchedulePerson model, TamamConstants.ValidationMode mode)
            : base(model, new ValidationContext(mode))
        {
        }

        #endregion

        #region classes

        internal class ValidationContext : AbstractValidator<SchedulePerson>
        {
            public ValidationContext(TamamConstants.ValidationMode mode)
            {
                RuleFor(s => s.ScheduleId).Must(isValidGuid).WithMessage(Resources.Culture.ValidationResources.ScheduleRequired);
                RuleFor(s => s.StartDate).NotEmpty().WithMessage(Resources.Culture.ValidationResources.StartDateEmpty);
                RuleFor(s => s.PersonId).Must(isValidGuid).WithMessage(Resources.Culture.ValidationResources.PersonIdEmpty);
                RuleFor( s => s.StartDate ).Must( IsPersonnelValid_HireDate ).WithMessage( ADS.Tamam.Resources.Culture.ValidationResources.InvalidSchedulePersonnel_HireDate );
                RuleFor(s => s.EndDate).Must(isValidEndDate).WithMessage(Resources.Culture.ValidationResources.TransferScheduleInvalidDate);
            }

            # region Helpers

            private bool isValidGuid(SchedulePerson instance, Guid id)
            {
                return id != Guid.Empty;
            }
            private bool isValidEndDate(SchedulePerson instance, DateTime? endDate)
            {
                var schedulesDataHandler = new SchedulesDataHandler();
                return schedulesDataHandler.ValidateSchedulePersonDates(instance);
            }
            private bool IsPersonnelValid_HireDate( SchedulePerson instance , DateTime startDate )
            {
                var dataHandler = new PersonnelDataHandler();

                var person = dataHandler.GetPerson( instance.PersonId ).Result;
                if ( person.AccountInfo.JoinDate.HasValue && person.AccountInfo.JoinDate.Value.Date > instance.StartDate.Date )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


            # endregion
        }

        #endregion
    }
}
