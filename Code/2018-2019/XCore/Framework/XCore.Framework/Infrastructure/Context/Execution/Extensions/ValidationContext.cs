using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Components.Framework.Context.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;

namespace XCore.Framework.Infrastructure.Context.Execution.Extensions
{
    [Serializable]
    public class ValidationContext<T> : ValidationContextBase
    {
        #region props.

        public IModelValidator<T> Validator { get; set; }
        public List<T> Models { get; set; }
        public ValidationMode Mode { get; set; }

        #endregion
        #region cst.

        public ValidationContext() { }
        public ValidationContext( IModelValidator<T> validator , T model , ValidationMode mode ) : this()
        {
            this.Validator = validator;
            this.Models = new List<T>() { model };
            this.Mode = mode;
        }
        public ValidationContext( IModelValidator<T> validator , List<T> models , ValidationMode mode ) : this()
        {
            this.Validator = validator;
            this.Models = models;
            this.Mode = mode;
        }

        #endregion
        #region IContextStep

        public override async Task<IResponse> Process( IActionContext context )
        {
            var response = new ExecutionResponseBasic() { State = ResponseState.Success };
            if ( this.Validator == null ) return response;

            var errors = new List<MetaPair>();
            var errorOccured = false;

            foreach ( var model in this.Models )
            {
                var validationResponse = await this.Validator.ValidateAsync(model, this.Mode);
                if ( !validationResponse.IsValid )
                {
                    errorOccured = true;
                    errors.AddRange(validationResponse.Errors );
                }
            }

            if ( errorOccured ) response.Set( ResponseState.ValidationError , errors , "" , null );
            return response;
        }

        #endregion
    }
}
