using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using FluentValidation;

namespace ADS.Tamam.Common.Data.Validation
{
    public class OrganizationDetailValidator : AbstractModelValidator<OrganizationDetail>
    {
        internal class ValidationContext : AbstractValidator<OrganizationDetail>
        {
            public ValidationContext()
            {
                RuleFor(org => org.Code).NotEmpty().WithMessage(ValidationResources.OrganizationDetailCodeEmpty);
                RuleFor(org => org.Name).NotEmpty().WithMessage(ValidationResources.OrganizationDetailNameEmpty);
                RuleFor(org => org.NameCultureVarient).NotEmpty().WithMessage(ValidationResources.OrganizationDetailNameCultureVariantEmpty);
                RuleFor(org => org.Email).EmailAddress().When(org => !string.IsNullOrEmpty(org.Email)).WithMessage(ValidationResources.InvalidEmail);
                RuleFor(org => org.Phone).Must(IsValidPhone).WithMessage(ValidationResources.InvalidPhone);
                RuleFor(org => org.Phone).Must(IsValidPhoneLength).WithMessage(ValidationResources.InvalidPhoneLength);
                RuleFor(org => org.Fax).Must(IsValidFaxLength).WithMessage(ValidationResources.InvalidFaxLength);
            }


            # region Helpers

            private bool IsValidPhone(string Phone)
            {
                if (string.IsNullOrEmpty(Phone.Trim()))
                    return true;
                return XString.MatchPattern(Phone,
                    @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
            }

            private bool IsValidPhoneLength(string Phone)
            {
                return Phone.Trim().Length <= 20;
            }

            private bool IsValidFaxLength(string Fax)
            {
                return Fax.Trim().Length <= 20;
            }

            # endregion
        }
        # region Constructor

        public OrganizationDetailValidator(OrganizationDetail model)
            : base(model, new ValidationContext())
        {
        }

        # endregion

    }
}
