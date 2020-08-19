using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using FluentValidation;

namespace ADS.Tamam.Common.Data.Validation
{
    public class AttendanceCodeValidator : AbstractModelValidator<AttendanceCode>
    {
        #region cst.

        public AttendanceCodeValidator(AttendanceCode model, AttendanceCodeValidationMode mode)
            : base(model, new ValidationContext(mode))
        {
        }

        # endregion

        #region classes

        internal class ValidationContext : AbstractValidator<AttendanceCode>
        {
            public ValidationContext(AttendanceCodeValidationMode mode)
            {
                // validation rules
                RuleFor(code => code.Code).Must(IsNotEmpty).WithMessage(ValidationResources.AttendanceCodeIsEmpty);

                // Edit Mode
                if (mode == AttendanceCodeValidationMode.Edit)
                {
                    RuleFor(code => code.Code).Must(IsCodeUnique).WithMessage(ValidationResources.AttendanceCodeIsCodeUnique);
                }
            }

            #region Helpers

            private bool IsNotEmpty(AttendanceCode attCode, string code)
            {
                return !string.IsNullOrEmpty(code.Trim());
            }
            private bool IsCodeUnique(AttendanceCode attCode, string code)
            {
                var handler = new OrganizationDataHandler();
                var isCodeUnique = handler.IsAttendanceCodeUnique(attCode);
                return isCodeUnique;
            }
            
            #endregion
        }

        #endregion

        # region Enums

        public enum AttendanceCodeValidationMode
        {
            Edit
        }

        # endregion
    }
}
