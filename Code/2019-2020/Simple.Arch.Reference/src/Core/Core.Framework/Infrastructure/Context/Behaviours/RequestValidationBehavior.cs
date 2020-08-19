using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Exceptions;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Context.Behaviours
{
    public class RequestValidationBehavior<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
                                                                  where TRequest : MediatR.IRequest<TResponse>
                                                                  where TResponse : new()
    {
        #region props.

        private readonly IEnumerable<FluentValidation.IValidator<TRequest>> _validators;

        #endregion
        #region cst.

        public RequestValidationBehavior(IEnumerable<FluentValidation.IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        #endregion
        #region IPipelineBehavior

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MediatR.RequestHandlerDelegate<TResponse> next)
        {
            var context = new FluentValidation.ValidationContext(request);

            var failures = _validators
                           .Select(v => v.Validate(context))
                           .SelectMany(result => result.Errors)
                           .Where(f => f != null)
                           .ToList();

            if (failures.Count != 0)
            {
                if (request is BaseRequestContext)
                {
                    return (request as BaseRequestContext).SetResponseNative<TResponse>(ResponseCode.ValidationError, 
                                                                                        failures?.Select(x=>new MetaPair() { target = !string.IsNullOrEmpty(x.PropertyName)? x.PropertyName:"Key" , message = x.ErrorMessage }).ToList(),
                                                                                        "Message in EN/AR to describe one or more validation errors in the submitted data.",
                                                                                        request.GetType().Name);
                }
                else
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }

        #endregion
    }
}
