using System.Collections.Generic;
using System.Linq;
using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Resources.Culture;
using FluentValidation;

namespace ADS.Tamam.Common.Data.Validation
{
    public class HRPolicyValidator : IModelValidator
    {
        #region props ...

        private HRPolicy _policy { get; set; }

        #endregion
        #region cst.

        public HRPolicyValidator( Policy model , TamamConstants.ValidationMode mode )
        {
            var context = new HRPolicyValidator.ValidationContext( mode );
            _policy = new HRPolicy( model );
            var result = context.Validate( _policy );
            IsValid = result.IsValid;
            Errors = result.Errors.Select( e => e.ErrorMessage ).ToList();
            ErrorsDetailed = result.Errors.Select( e => new ModelMetaPair( e.PropertyName , e.ErrorMessage ) ).ToList();
        }

        #endregion

        internal class ValidationContext : AbstractValidator<HRPolicy>
        {
            public ValidationContext( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor( app => app.HRRole ).NotEmpty().WithMessage( ValidationResources.HRRoleEmpty );
                    RuleFor( app => app.CCs ).Must( IsCCsValid ).WithMessage( ValidationResources.InvalidCCs );
                }
            }

            # region internals

            private bool IsCCsValid( HRPolicy instance , string CCs )
            {
                if ( string.IsNullOrWhiteSpace( CCs ) ) return true;
                return XString.MatchPattern( CCs.Trim() , @"^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*,?)*$" );
            }

            # endregion
        }

        #region IModelValidator

        public bool? IsValid { get; private set; }
        public List<string> Errors { get; private set; }
        public List<ModelMetaPair> ErrorsDetailed { get; private set; }

        #endregion
    }
}
