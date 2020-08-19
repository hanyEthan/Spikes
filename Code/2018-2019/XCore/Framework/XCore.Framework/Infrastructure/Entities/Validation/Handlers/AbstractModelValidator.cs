using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;

namespace XCore.Framework.Infrastructure.Entities.Validation.Handlers
{
    public class AbstractModelValidator<T> : IModelValidator<T>
    {
        #region props.

        protected AbstractValidator<T> DefaultValidationContext { get; set; }
        protected Dictionary<string , AbstractValidator<T>> ValidationContexts { get; set; }

        #endregion
        #region cst.

        public AbstractModelValidator()
        {
            this.ValidationContexts = new Dictionary<string , AbstractValidator<T>>();
        }

        #endregion
        #region publics.

        public async Task<ValidationResponse> ValidateAsync(T model, ValidationMode? mode = null)
        {
            var validator = GetValidator(mode);
            if (model == null || validator == null) return ValidationResponse.Error;

            var result = await validator.ValidateAsync(model);
            return Map(result);
        }

        #endregion
        #region helpers.

        private AbstractValidator<T> GetValidator(ValidationMode? mode = null)
        {
            if (mode == null)
            {
                return this.DefaultValidationContext;
            }
            else
            {
                return this.ValidationContexts == null ? null
                     : this.ValidationContexts.TryGetValue(mode.ToString(), out AbstractValidator<T> validator) ? validator
                     : null;
            }
        }

        private ValidationResponse Map(ValidationResult from)
        {
            var to = new ValidationResponse();

            if (from != null)
            {
                to.IsValid = from.IsValid;
                to.Errors = Map(from.Errors);
            }

            return to;
        }
        private List<MetaPair> Map(IList<ValidationFailure> from)
        {
            if (from == null) return null;

            var to = new List<MetaPair>();
            foreach (var fromItem in from)
            {
                var toItem = Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private MetaPair Map(ValidationFailure from)
        {
            if (from == null) return null;

            return new MetaPair()
            {
                Property = from.PropertyName,
                Meta = from.ErrorMessage,
            };
        }

        #endregion
    }
}
