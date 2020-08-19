using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Context.Mappers;
using Mcs.Invoicing.Core.Framework.Infrastructure.Exceptions;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Context.APIs.Middleware
{
    #region Middleware

    public class CustomExceptionHandlerMiddleware
    {
        #region props.

        private readonly RequestDelegate _next;
        private readonly IModelMapper<BaseRequestContext, BaseRequestContext> _requestContextMapper;

        #endregion
        #region cst.

        public CustomExceptionHandlerMiddleware(RequestDelegate next,
                                                IModelMapper<BaseRequestContext, BaseRequestContext> requestContextMapper)
        {
            this._next = next;
            this._requestContextMapper = requestContextMapper;
        }

        #endregion

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception x)
            {
                await HandleExceptionAsync(context, x);
            }
        }

        #region helpers.

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            switch (exception)
            {
                case ContextException contextException:
                    {
                        var errorResponse = contextException.BaseResponseContext;
                        await ResponseContextMapper<object>.MapContext(errorResponse, context.Response);
                        break;
                    }
                case ValidationException validationException:
                    {
                        var errorResponse = GetErrorResponse();
                        errorResponse.Header.StatusCode = ResponseCode.ValidationError;
                        errorResponse.Header.details = validationException.Failures?.Select(x => new MetaPair(x.Item1, x.Item2)).ToList();
                        await ResponseContextMapper<object>.MapContext(errorResponse, context.Response);
                        break;
                    }
                case NotFoundException _:
                    {
                        var errorResponse = GetErrorResponse();
                        errorResponse.Header.details = new List<MetaPair>() { new MetaPair("exception", exception.Message) };
                        errorResponse.Header.StatusCode = ResponseCode.NotFound;
                        await ResponseContextMapper<object>.MapContext(errorResponse, context.Response);
                        break;
                    }
                default:
                    {
                        var errorResponse = GetErrorResponse();
                        errorResponse.Header.details = new List<MetaPair>() { new MetaPair("exception", exception.Message) };
                        await ResponseContextMapper<object>.MapContext(errorResponse, context.Response);
                        break;
                    }
            }
        }
        private BaseResponseContext GetErrorResponse()
        {
            var requestContext = this._requestContextMapper.Map(new BaseRequestContext());
            var responseContext  = requestContext.SetResponseNative<BaseResponseContext>(ResponseCode.SystemError);

            return responseContext;
        }

        #endregion
    }

    #endregion
    #region IOC

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }

    #endregion
}
