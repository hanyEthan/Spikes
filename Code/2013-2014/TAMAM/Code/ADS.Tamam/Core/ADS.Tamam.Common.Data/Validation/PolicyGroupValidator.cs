using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using FluentValidation;
using System;

namespace ADS.Tamam.Common.Data.Validation
{
    public class PolicyGroupValidator : AbstractModelValidator<PolicyGroup>
    {
        #region cst.

        public PolicyGroupValidator ( PolicyGroup model , TamamConstants.ValidationMode mode )
            : base(model, new ValidationContext(mode))
        {
        }

        #endregion

        #region classes

        internal class ValidationContext : AbstractValidator<PolicyGroup>
        {
            public ValidationContext ( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor ( group => group.Name ).NotEmpty ().WithMessage ( ValidationResources.PolicyGroupNameEmpty );
                    RuleFor ( group => group.NameCultureVarient ).NotEmpty ().WithMessage ( ValidationResources.PolicyGroupArabicNameEmpty );
                    RuleFor(group => group.Name).Must(IsNameUnique).WithMessage(ValidationResources.NameNotUnique);
                    RuleFor(group => group.NameCultureVarient).Must(IsNameCultureVariantUnique).WithMessage(ValidationResources.NameCultureVariantNotUnique);
                }
                if ( mode == TamamConstants.ValidationMode.Deactivate )
                {
                    // check if Policy Group have associations with department, personnel, or policies.
                    RuleFor(group => group.Id).Must(CanDeactivateGroup).WithMessage(ValidationResources.PolicyGroupHaveAssociations);
                }
                
            }

            #region Helpers

            private bool IsNameUnique(PolicyGroup instance, string Name)
            {
                return new SchedulesDataHandler().IsPolicyGroupNameUnique(instance);
            }
            private bool IsNameCultureVariantUnique(PolicyGroup instance, string nameCultureVariant)
            {
                return new SchedulesDataHandler().IsPolicyGroupNameCultureVariantUnique(instance);
            }

            private bool CanDeactivateGroup(PolicyGroup instance, Guid id)
            {
                // check if Policy Group have associations with department, personnel, or policies.

                // 1- get Departments associated with this policy group
                var noDepartments = new OrganizationDataHandler().GetDepartmentsCountAssociatedWithPolicyGroup(id) == 0;

                // 2- get Personnel associated with this policy group
                var noPersonnel = new PersonnelDataHandler().GetPersonnelCountAssociatedWithPolicyGroup(id) == 0;

                // 3- get Policies associated with this policy group
                var noPolicies = instance.Policies.Count == 0;


                return noDepartments && noPersonnel && noPolicies;
            }

            #endregion
        }

        #endregion

        # region Enums

        

        # endregion
    }
}
