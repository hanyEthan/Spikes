using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS.Common.Handlers;
using ADS.Common.Models.Domain.Authorization;
using ADS.Common.Validation;
using FluentValidation;

namespace ADS.Common.Validators
{
    public class RoleValidator : AbstractModelValidator<Role>
    {
        #region cst.

        public RoleValidator(Role model, ValidationMode validationMode) : base(model, new ValidationContext(validationMode))
        {
        }

        # endregion
        #region classes

        public enum ValidationMode { Create, Edit }

        internal class ValidationContext : AbstractValidator<Role>
        {
            #region cst ...

            public ValidationContext(ValidationMode validationMode)
            {
                // Required
                RuleFor(role => role.Name).Must(IsNameValid).WithMessage("Name is not provided");
                RuleFor(role => role.Code).Must(IsCodeValid).WithMessage("Code is not provided");

                // Uniqueness
                RuleFor(role => role.Name).Must(IsNameUnique).WithMessage("Name must be unique");
                RuleFor(role => role.Code).Must(IsCodeUnique).WithMessage("Code must be unique");

                if (validationMode == ValidationMode.Create)
                {

                }
                else if (validationMode == ValidationMode.Edit)
                {
                    RuleFor( R => R.SystemRole ).Equal( false ).WithMessage( "System Role cannot be updated" );
                }
            }

            #endregion
            #region Helpers

            private bool IsNameValid(Role instance, string name)
            {
                return string.IsNullOrWhiteSpace(name) == false;
            }
            private bool IsCodeValid(Role instance, string code)
            {
                return string.IsNullOrWhiteSpace(code) == false;
            }

            private bool IsNameUnique(Role instance, string name)
            {
                var roles = Broker.AuthorizationHandler.GetRoles();
                var isRepeatedName = roles.Count(x => x.Id != instance.Id && string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)) > 0;
                return !isRepeatedName;
            }
            private bool IsCodeUnique(Role instance, string code)
            {
                var roles = Broker.AuthorizationHandler.GetRoles();
                var isRepeatedCode =  roles.Count(x =>  x.Id != instance.Id && string.Equals(x.Code, code, StringComparison.InvariantCultureIgnoreCase)) > 0;
                return !isRepeatedCode;
            }

            #endregion
        }

        #endregion
    }
}
