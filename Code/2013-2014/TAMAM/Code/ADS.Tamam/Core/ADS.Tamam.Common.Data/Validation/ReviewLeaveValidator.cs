using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using FluentValidation;

namespace ADS.Tamam.Common.Data.Validation
{
    public class ReviewLeaveValidator : AbstractModelValidator<Leave>
    {
        #region cst.

        public ReviewLeaveValidator(Leave model)
            : base(model, new ValidationContext())
        {
        }

        # endregion
        #region classes

        internal class ValidationContext : AbstractValidator<Leave>
        {
            public ValidationContext()
            {
                RuleFor(leave => leave.LeaveStatusId)
                    .Must(CanReviewLeave)
                    .WithMessage(ValidationResources.LeaveCanReviewed);
            }

            #region Helpers

            private bool CanReviewLeave(Leave instance, int statusId)
            {
                return statusId != (int) LeaveStatus.Cancelled && statusId != (int) LeaveStatus.Taken;
            }

            #endregion
        }

        # endregion
    }
}
