using ADS.Common.Context;
using ADS.Common.Validation;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Reports;
using ADS.Tamam.Resources.Culture;
using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ADS.Tamam.Common.Data.Validation
{
    public class ScheduledReportEventValidator : AbstractModelValidator<ScheduledReportEvent>
    {
        #region cst.

        public ScheduledReportEventValidator(ScheduledReportEvent model)
            : base(model, new ValidationContext())
        {
        }

        # endregion

        #region classes

        internal class ValidationContext : AbstractValidator<ScheduledReportEvent>
        {
            Regex emailRegex = new Regex(@"^[\W]*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4}[\W]*,{1}[\W]*)*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4})[\W]*$");

            public ValidationContext()
            {
                RuleFor(sre => sre.DepartmentId).Must(IsValidDepartmentId).WithMessage(ValidationResources.DepartmentIdEmpty);
                RuleFor(sre => sre.ReportDefinitionId).Must(IsValidReportId).WithMessage(ValidationResources.ReportIdEmpty);

                RuleFor(sre => sre.Id).Must(IsValidDayData).WithMessage(ValidationResources.InvalidDay);
                RuleFor(sre => sre.Id).Must(IsValidReportPrivilage).WithMessage(ValidationResources.InvalidPersonReportPrivilage);

                RuleFor(sre => sre.Id).Must(IsValidEmail).WithMessage(ValidationResources.InvalidEmail);
                RuleFor(sre => sre.Id).Must(IsValidCCs).WithMessage(ValidationResources.InvalidCCs);

                RuleFor(sre => sre.IncludeSupervisor).Must(IsValidIncludeSupervisor).WithMessage(ValidationResources.InvalidIncludeSupervisor);
                RuleFor(sre => sre.Id).Must(IsThereAtLeastOneTarget).WithMessage(ValidationResources.InvalidTarget);
            
            }

            #region Helpers

            private bool IsValidDepartmentId(ScheduledReportEvent scheduledReportEvent, Guid? guidId)
            {
                return (guidId != Guid.Empty);
            }

            private bool IsValidReportId(ScheduledReportEvent scheduledReportEvent, Guid guidId)
            {
                return guidId != Guid.Empty;
            }

            private bool IsValidDayData(ScheduledReportEvent scheduledReportEvent, Guid id)
            {
                if (scheduledReportEvent.Repeates == Repeates.Daily)
                {
                    return scheduledReportEvent.Day == null && scheduledReportEvent.DayNumber == null;
                }

                if (scheduledReportEvent.Repeates == Repeates.Weekly)
                {
                    return scheduledReportEvent.Day != null && scheduledReportEvent.DayNumber == null;
                }

                if (scheduledReportEvent.Repeates == Repeates.Monthly)
                {
                    return scheduledReportEvent.Day == null
                        && scheduledReportEvent.DayNumber != null
                        && scheduledReportEvent.DayNumber >= 1
                        && scheduledReportEvent.DayNumber <= 31;
                }

                return false;
            }

            private bool IsValidReportPrivilage(ScheduledReportEvent scheduledReportEvent, Guid id)
            {
                // if (!scheduledReportEvent.PersonId.HasValue) return true;

                var organizationHandler = new OrganizationDataHandler();
                var reportReponse = organizationHandler.GetReportDefinition(scheduledReportEvent.ReportDefinitionId);
                if (reportReponse.Type != ResponseState.Success) return false;
                var report = reportReponse.Result;

                // return Broker.AuthorizationHandler.Authorize(scheduledReportEvent.PersonId.Value, report.Privilege);
                return true;
            }

            private bool IsValidEmail(ScheduledReportEvent scheduledReportEvent, Guid id)
            {
                if (string.IsNullOrWhiteSpace(scheduledReportEvent.Email)) return true;
                return emailRegex.IsMatch(scheduledReportEvent.Email);
            }

            private bool IsValidCCs(ScheduledReportEvent scheduledReportEvent, Guid id)
            {
                if (string.IsNullOrWhiteSpace(scheduledReportEvent.CCs)) return true;
                return emailRegex.IsMatch(scheduledReportEvent.CCs);
            }

            private bool IsValidIncludeSupervisor(ScheduledReportEvent scheduledReportEvent, bool includeSupervisor)
            {
                if (!includeSupervisor) return true;
                if (includeSupervisor && scheduledReportEvent.DepartmentId != Guid.Empty && scheduledReportEvent.DepartmentId != null) return true;

                return false;
            }

            private bool IsThereAtLeastOneTarget(ScheduledReportEvent scheduledReportEvent, Guid id)
            {
                if (scheduledReportEvent.IncludeSupervisor) return true;
                if (!string.IsNullOrWhiteSpace(scheduledReportEvent.Email)) return true;
                if (scheduledReportEvent.Personnel.Any()) return true;
                if (!string.IsNullOrWhiteSpace(scheduledReportEvent.CCs)) return true;

                return false;
            }

            #endregion
        }

        #endregion
    }
}
