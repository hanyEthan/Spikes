using System.Linq;
using System.Collections.Generic;

using FluentValidation;

using ADS.Common.Validation;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;


namespace ADS.Tamam.Common.Data.Validation
{
    public class NotificationsPolicyValidator : IModelValidator
    {
        #region props ...

        private NotificationsPolicy _policy { get; set; }

        #endregion
        #region cst.

        public NotificationsPolicyValidator( Policy model , TamamConstants.ValidationMode mode )
        {
            var context = new ValidationContext( mode );
            _policy = new NotificationsPolicy( model );
            var result = context.Validate( _policy );
            IsValid = result.IsValid;
            Errors = result.Errors.Select( e => e.ErrorMessage ).ToList();
            ErrorsDetailed = result.Errors.Select( e => new ModelMetaPair( e.PropertyName , e.ErrorMessage ) ).ToList();
        }

        #endregion

        internal class ValidationContext : AbstractValidator<NotificationsPolicy>
        {
            public ValidationContext( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    // any related validations ..
                }
            }
        }

        #region IModelValidator

        public bool? IsValid { get; private set; }
        public List<string> Errors { get; private set; }
        public List<ModelMetaPair> ErrorsDetailed { get; private set; }

        #endregion
    }
}