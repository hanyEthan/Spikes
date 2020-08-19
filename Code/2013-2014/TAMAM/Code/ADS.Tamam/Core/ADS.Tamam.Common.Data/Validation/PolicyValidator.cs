using System;
using System.Linq;
using System.Collections.Generic;

using FluentValidation;

using ADS.Common.Validation;
using ADS.Common.Handlers;
using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Context;

namespace ADS.Tamam.Common.Data.Validation
{
    public class PolicyValidator : AbstractModelValidator<Policy>
    {
        #region classes

        protected class ValidationContext : AbstractValidator<Policy>
        {
            #region props

            private bool _initialized;
            private ValidationMode _validationMode;
            private OrganizationDataHandler _dataHandler;
            private SchedulesDataHandler _SchedulesDataHandler;

            #endregion

            public ValidationContext( ValidationMode mode )
            {
                _validationMode = mode;
                _initialized = InitializeDataLayer();

                RuleFor( policy => policy.Id ).Must( IsValidId );
                RuleFor( policy => policy.Id ).Must( CheckPolicyGroupsAssociation ).WithMessage( ValidationResources.PolicyAssociationsCheck );

                RuleFor( policy => policy.Name ).Length( 0 , 200 ).WithMessage( ValidationResources.PolicyNameLength );
                RuleFor( policy => policy.Name ).Must( IsNameUnique ).WithMessage( ValidationResources.PolicyNameUnique );

                RuleFor( policy => policy.NameCultureVarient ).Length( 0 , 200 ).WithMessage( ValidationResources.PolicyNameCultureVariantLength );
                RuleFor( policy => policy.NameCultureVarient ).Must( IsNameCultureVariantUnique ).WithMessage( ValidationResources.PolicyNameCultureVariantUnique );

                RuleFor( policy => policy.Code ).NotEmpty().WithMessage( ValidationResources.PolicyCodeEmpty );
                RuleFor( policy => policy.Code ).Must( IsCodeUnique ).WithMessage( ValidationResources.PolicyCodeUnique );
                RuleFor( policy => policy.Code ).Length( 1 , 50 ).WithMessage( ValidationResources.PolicyCodeLength );

                RuleFor( policy => policy.PolicyTypeId ).NotEmpty().WithMessage( ValidationResources.PolicyTypeNotValid );
                RuleFor( policy => policy.PolicyTypeId ).Must( IsPolicyTypeValid ).WithMessage( ValidationResources.PolicyTypeNotValid );
                RuleFor( policy => policy.Values ).Must( CheckValues ).WithMessage( ValidationResources.PolicyValuesCheck );
                RuleFor( policy => policy.Id ).Must( CheckPolicyAssociationWithShift ).WithMessage( ValidationResources.PolicyAssociationsWithShift );
            }

            #region Helpers

            private bool IsValidId( Guid Id )
            {
                if ( _validationMode == ValidationMode.Create ) return true;
                return Id != Guid.Empty;
            }
            private bool IsCodeUnique( Policy instance , string code )
            {
                if ( _validationMode != ValidationMode.Edit && _validationMode != ValidationMode.Create ) return true;
                return _initialized && _dataHandler.CheckPolicyCodeUniqueness( instance ).Result;
            }
            private bool IsNameUnique( Policy instance , string name )
            {
                if ( _validationMode != ValidationMode.Edit && _validationMode != ValidationMode.Create ) return true;
                return _initialized && _dataHandler.CheckPolicyNameUniqueness( instance ).Result;
            }
            private bool IsNameCultureVariantUnique( Policy instance , string nameCultureVariant )
            {
                if ( _validationMode != ValidationMode.Edit && _validationMode != ValidationMode.Create ) return true;
                return _initialized && _dataHandler.CheckPolicyNameCultureVariantUniqueness( instance ).Result;
            }
            private bool IsPolicyTypeValid( Policy instance , Guid policyTypeId )
            {
                if ( _validationMode != ValidationMode.Edit && _validationMode != ValidationMode.Create ) return true;
                return _dataHandler.CheckPolicyTypeExistance( policyTypeId ).Result;
            }
            private bool CheckValues( Policy instance , IList<PolicyFieldValue> values )
            {
                // skip ...
                if ( _validationMode != ValidationMode.Edit && _validationMode != ValidationMode.Create ) return true;

                // TODO : redesign the checkout of master codes and policy types ...

                // get policy type (and its schema) from the db ...
                var response = _dataHandler.GetPolicyType( instance.PolicyTypeId );
                var policyType = response.Type == ResponseState.Success ? response.Result : null;

                // check for counts ...
                bool state = policyType != null && policyType.Fields != null && policyType.Fields.Count( x => x.Active ) <= values.Count && policyType.Fields.Count >= values.Count;
                if ( !state ) return false;

                // check each field and its validation rules ...
                foreach ( var field in policyType.Fields )
                {
                    // getting the field value ...
                    var possibleValues = values.Where( x => x.PolicyFieldId == field.Id ).ToList();
                    if ( possibleValues.Count > 1 ) return false;
                    var value = possibleValues.Count > 0 ? possibleValues[0] : null;

                    // skip non active fields ...
                    if ( !field.Active )
                    {
                        if ( value != null ) value.Value = "";
                        continue;
                    }

                    // check if it doesn't have any value ...
                    if ( value == null ) return false;

                    value.Value = value.Value.Trim();

                    // check if it doesn't have any value ...
                    if ( value.Value != null && value.Value.Length > 255 ) return false;

                    // validate its regular expression (if any) ...
                    if ( !string.IsNullOrEmpty( field.ValidationRegularExpression ) && !XString.MatchPattern( value.Value , field.ValidationRegularExpression ) ) return false;

                    // validate if it references complex data type (system code or policy type) ...
                    if ( field.DataTypeId == ( int ) PolicyFieldDataType.List )
                    {
                        if ( field.DatasetReferenceTypeName == "MasterCode" )
                        {
                            int detailCodeId;
                            if ( !int.TryParse( value.Value , out detailCodeId ) ) return false;

                            var detailCode = Broker.DetailCodeHandler.GetDetailCode( detailCodeId );
                            if ( detailCode == null ) return false;
                            if ( detailCode.MasterCodeId.ToString() != field.DataCollectionReferenceId ) return false;
                        }
                        else if ( field.DatasetReferenceTypeName == "PolicyType" )
                        {
                            Guid policyId;
                            if ( !Guid.TryParse( value.Value , out policyId ) ) return false;

                            var policyResult = _dataHandler.GetPolicy( policyId );
                            if ( policyResult.Type != ResponseState.Success ) return false;
                            if ( policyResult.Result.PolicyTypeId.ToString() != field.DataCollectionReferenceId ) return false;
                        }
                        else if ( field.DatasetReferenceTypeName == "Department" )
                        {
                            Guid DepartmentId;
                            if ( !Guid.TryParse( value.Value , out DepartmentId ) ) return false;
                            var departmentResult = _dataHandler.GetDepartment( DepartmentId , SystemSecurityContext.Instance );
                            if ( departmentResult.Type != ResponseState.Success ) return false;
                        }
                        else if ( field.DatasetReferenceTypeName == "Shift" )
                        {
                            Guid shiftId;
                            if ( !Guid.TryParse( value.Value , out shiftId ) ) return false;
                            var shiftExistsResponse = _SchedulesDataHandler.ShiftExists( shiftId , true );
                            if ( !shiftExistsResponse.Result ) return false;
                        }

                        else return false;  // can't have "List" data type of any other types than master codes and policy types ...
                    }
                }

                return state;
            }
            private bool CheckPolicyGroupsAssociation( Policy instance , Guid id )
            {
                // skip ...
                if ( _validationMode != ValidationMode.Delete ) return true;

                return _dataHandler.CheckPolicyAssociations( id ).Result;
            }

            // check if Policy instance is Shift Policy and associated with Shits..
            private bool CheckPolicyAssociationWithShift( Policy instance , Guid id )
            {
                // skip ...
                if ( _validationMode != ValidationMode.Delete ) return true;
                if ( instance.PolicyTypeId != Guid.Parse( PolicyTypes.ShiftPolicyType ) ) return true;

                var shifts = _SchedulesDataHandler.GetShifts( true ).Result;
                var shift = shifts.FirstOrDefault( x => x.ShiftPolicyId == instance.Id );

                return shift == null;
            }

            private bool InitializeDataLayer()
            {
                _dataHandler = new OrganizationDataHandler();
                _SchedulesDataHandler = new SchedulesDataHandler();
                return _dataHandler.Initialized && _SchedulesDataHandler.Initialized;
            }

            #endregion
        }
        public enum ValidationMode { Get , Create , Edit , Delete , }

        #endregion
        #region cst.

        public PolicyValidator( Policy model , ValidationMode mode ) : base( model , new ValidationContext( mode ) )
        {
        }

        #endregion
    }
}
