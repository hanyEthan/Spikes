using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using XCore.Utilities.Infrastructure.Context.Execution.Models;
using XCore.Utilities.Infrastructure.Entities.Validation.Contracts;
using XCore.Utilities.Infrastructure.Entities.Validation.Models;

namespace XCore.Utilities.Infrastructure.Entities.Validation.Handlers
{
    public class AbstractModelValidator<T> : IModelValidator<T>
    {
        #region props.

        protected AbstractValidator<T> DefaultValidationContext { get; set; }
        protected Dictionary<string , AbstractValidator<T>> ValidationContexts { get; set; }
        protected ValidationResult _Result { get; set; }
        public bool? IsValid { get; private set; }
        public List<MetaPair> Errors { get; private set; }

        #endregion
        #region cst.

        public AbstractModelValidator()
        {
            this.ValidationContexts = new Dictionary<string , AbstractValidator<T>>();
        }

        #endregion
        #region publics.

        public bool Validate( T model )
        {
            if ( model == null || this.DefaultValidationContext == null ) return ( IsValid = false ).Value;

            _Result = this.DefaultValidationContext.Validate( model );
            IsValid = _Result.IsValid;

            if ( !IsValid.Value )
            {
                Errors = new List<MetaPair>();
                for ( int i = 0 ; i < _Result.Errors.Count ; i++ )
                {
                    Errors.Add( new MetaPair( _Result.Errors[i].PropertyName , _Result.Errors[i].ErrorMessage ) );
                }
            }

            return _Result.IsValid;
        }
        public bool Validate( T model , ValidationMode mode )
        {
            AbstractValidator<T> context;

            if ( model == null ||
                 this.ValidationContexts == null ||
                 !this.ValidationContexts.TryGetValue( mode.ToString() , out context ) ) return ( IsValid = false ).Value;

            _Result = context.Validate( model );
            IsValid = _Result.IsValid;

            if ( !IsValid.Value )
            {
                Errors = new List<MetaPair>();
                for ( int i = 0 ; i < _Result.Errors.Count ; i++ )
                {
                    Errors.Add( new MetaPair( _Result.Errors[i].PropertyName , _Result.Errors[i].ErrorMessage ) );
                }
            }

            return _Result.IsValid;
        }

        #endregion
    }
}
