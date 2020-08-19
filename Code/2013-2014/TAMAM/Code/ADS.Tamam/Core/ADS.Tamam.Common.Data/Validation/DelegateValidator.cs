using ADS.Common.Validation;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Resources.Culture;
using FluentValidation;
using System;

namespace ADS.Tamam.Common.Data.Validation
{
    public class DelegateValidator : AbstractModelValidator<PersonDelegate>
    {
        #region classes
        #region cst.

        public DelegateValidator(PersonDelegate model, TamamConstants.ValidationMode mode): base(model, new ValidationContext(mode))
        {
        }

        #endregion

        internal class ValidationContext : AbstractValidator<PersonDelegate>
        {
            public ValidationContext(TamamConstants.ValidationMode mode)
            {
                RuleFor(personDelegate => personDelegate.Code).NotEmpty().WithMessage(ValidationResources.PersonCodeEmpty);
                RuleFor(personDelegate => personDelegate.PersonId).Must(IsValidId).WithMessage(ValidationResources.PersonIdEmpty);
                RuleFor(personDelegate => personDelegate.DelegateId).Must(IsValidId).WithMessage(ValidationResources.DelegateIdEmpty);
            }
            
            #region Helpers

            private bool IsValidId(Guid Id)
            {
                return Id != Guid.Empty ? true : false;
            }

            #endregion
        }

        #endregion
    }
}
