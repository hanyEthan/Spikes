using System;
using System.Collections.Generic;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common
{
    public static class BaseContextExtensions
    {
        #region statics.

        public static BaseResponseContext<TContent> SetResponse<TContent>(this BaseRequestContext request)
        {
            return Map<TContent>(request);
        }
        public static BaseResponseContext<TContent> SetResponse<TContent>(this BaseRequestContext request, TContent content)
        {
            var response = Map<TContent>(request);
            response.Content = content;
            return response;
        }
        public static TResponse SetResponseNative<TResponse>(this BaseRequestContext request, ResponseCode code = ResponseCode.Success, List<MetaPair> messages = null, string message = null, string target = null, Exception exception = null, string metadata = null) where TResponse : new()
        {
            return MapNative<TResponse>(request, code, messages, message, target, exception, metadata);
        }
        public static BaseResponseContext<TContentDestination> SetResponse<TContentSource, TContentDestination>(this BaseResponseContext<TContentSource> baseResponse, TContentDestination content)
        {
            var response = Map<TContentSource, TContentDestination>(baseResponse);
            response.Content = content;
            return response;
        }

        #endregion
        #region helpers.

        private static BaseResponseContext<TContent> Map<TContent>(BaseRequestContext from)
        {
            return from?.Header == null
                 ? null
                 : new BaseResponseContext<TContent>()
                 {
                     Header = new BaseResponseContext.HeaderContent()
                     {
                         StatusCode = ResponseCode.Success,

                         CorrelationId = from.Header.CorrelationId,

                         RequestTimeUTC = from.Header.RequestTimeUTC,
                     },
                 };
        }
        private static BaseResponseContext<TContentDestination> Map<TContentSource, TContentDestination>(BaseResponseContext<TContentSource> from)
        {
            return from?.Header == null
                 ? null
                 : new BaseResponseContext<TContentDestination>()
                 {
                     Header = from.Header,
                 };
        }
        private static TResponse MapNative<TResponse>(BaseRequestContext from, ResponseCode code, List<MetaPair> messages, string message, string target, Exception exception, string metadata) where TResponse : new()
        {
            var resultNative = from?.Header == null ? default : new TResponse();
            var result = resultNative as BaseResponseContext;
            if (result == null) return default;
            result.Header = new BaseResponseContext.HeaderContent()
            {
                StatusCode = code,
                details = messages,
                message = message,
                target = target,
                CorrelationId = from.Header.CorrelationId,
                RequestTimeUTC = from.Header.RequestTimeUTC,
            };

            return resultNative;
        }

        #endregion
    }
}
