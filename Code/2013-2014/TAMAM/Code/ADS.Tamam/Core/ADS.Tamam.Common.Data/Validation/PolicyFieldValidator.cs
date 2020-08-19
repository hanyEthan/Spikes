using ADS.Common.Validation;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Resources.Culture;
using FluentValidation;

namespace ADS.Tamam.Common.Data.Validation
{
    public class PolicyFieldValidator : AbstractModelValidator<PolicyField>
    {
        public PolicyFieldValidator( PolicyField model , TamamConstants.ValidationMode mode )
            : base(model, new ValidationContext(mode))
        {
        }

        #region classes

        internal class ValidationContext : AbstractValidator<PolicyField>
        {
            public ValidationContext( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor ( policyField => policyField.Name ).NotEmpty ().WithMessage ( ValidationResources.NameEmpty );
                    RuleFor ( policyField => policyField.NameCultureVariant ).NotEmpty ().WithMessage ( ValidationResources.ArabicNameEmpty );

                    RuleFor ( policyField => policyField.Name ).Length ( 0 , 50 ).WithMessage ( ValidationResources.NameLength );
                    RuleFor ( policyField => policyField.NameCultureVariant ).Length ( 0 , 50 ).WithMessage ( ValidationResources.ArabicNameLength );                   

                    RuleFor ( policyField => policyField.DataTypeId ).Must ( IsDataTypeValid ).WithMessage ( ValidationResources.PolicyFieldDataTypeInvalid );
                    
                    RuleFor ( policyField => policyField.DatasetReferenceTypeName ).Must ( IsReferenceTypeValid ).
                        WithMessage ( ValidationResources.PolicyFieldReferenceTypeInvalid );

                    RuleFor ( policyField => policyField.DataCollectionReferenceId ).Must ( IsReferenceIdValid ).
                        WithMessage ( ValidationResources.PolicyFieldReferenceIdInvalid );

                    RuleFor ( shift => shift.PolicyTypeId ).NotEmpty ().WithMessage ( ValidationResources.PolicyTypeNotValid );
                    RuleFor ( shift => shift.Sequence ).GreaterThan ( 0 ).WithMessage ( ValidationResources.PolicyFieldSequanceInValid );
                    RuleFor ( shift => shift.Sequence ).Must ( IsSequenceValid ).WithMessage ( ValidationResources.PolicyFieldSequanceInValid );


   
                }
                else if ( mode == TamamConstants.ValidationMode.Delete || mode == TamamConstants.ValidationMode.Deactivate )
                {
                }
            }

            

            #region Helpers

            private bool IsSequenceValid( PolicyField instance , int arg )
            {
                var handler = new OrganizationDataHandler ();
                return handler.IsSequenceUnique ( instance );
            }

            private bool IsReferenceIdValid( PolicyField instance , string arg )
            {
                if ( instance.DatasetReferenceTypeName == PolicyFieldListDataType.MasterCode.ToString () ||
                    instance.DatasetReferenceTypeName == PolicyFieldListDataType.PolicyType.ToString () )
                    return !string.IsNullOrEmpty ( arg );
                return true;
            }
            private bool IsReferenceTypeValid( PolicyField instance , string arg )
            {
                if ( instance.DataTypeId == ( int ) PolicyFieldDataType.List )
                {
                    if ( arg == PolicyFieldListDataType.Department.ToString () || arg == PolicyFieldListDataType.MasterCode.ToString ()
                        || arg == PolicyFieldListDataType.PolicyType.ToString () )
                        return true;
                    else
                        return false;
                }
                return true;

            }

            private bool IsDataTypeValid( int arg )
            {
                if ( arg == ( int ) PolicyFieldDataType.Bool || arg == ( int ) PolicyFieldDataType.Date || arg == ( int ) PolicyFieldDataType.Float
                    || arg == ( int ) PolicyFieldDataType.Int || arg == ( int ) PolicyFieldDataType.List || arg == ( int ) PolicyFieldDataType.String )
                    return true;
                return false;
            }
            #endregion
        }

        #endregion
    }
}
