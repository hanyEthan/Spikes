using System.Collections.Generic;

using FluentValidation;
using FluentValidation.Results;

namespace ADS.Common.Validation
{
    public class AbstractModelValidator<T> : IModelValidator
    {
        #region props

        private AbstractValidator<T> _Context { get; set; }
        private ValidationResult _Result { get; set; }
        private T _Model { get; set; }

        public bool? IsValid { get; private set; }
        public List<string> Errors { get; private set; }
        public List<ModelMetaPair> ErrorsDetailed { get; private set; }

        #endregion
        #region cst.

        public AbstractModelValidator( T model , AbstractValidator<T> context )
        {
            _Model = model;
            _Context = context;

            ValidateModel();
        }

        #endregion
        #region Helpers

        private bool ValidateModel()
        {
            if ( _Model == null ) return ( IsValid = false ).Value;

            _Result = _Context.Validate( _Model );
            IsValid = _Result.IsValid;

            if ( !IsValid.Value )
            {
                //Errors = _Result.Errors.Select( x => x.ErrorMessage ).ToList();

                Errors = new List<string>();
                ErrorsDetailed = new List<ModelMetaPair>();

                for ( int i = 0 ; i < _Result.Errors.Count ; i++ )
                {
                    Errors.Add( _Result.Errors[i].ErrorMessage );
                    ErrorsDetailed.Add( new ModelMetaPair( _Result.Errors[i].PropertyName , _Result.Errors[i].ErrorMessage ) );
                }
            }

            return true;
        }

        #endregion
    }
}
