using System.Net;
using System.Threading.Tasks;
using Grpc.Core;
using Mcs.Invoicing.Core.Framework.Infrastructure.Context.Models;
using Mcs.Invoicing.Core.Framework.Infrastructure.Exceptions;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Context.Mappers
{
    public static class ResponseContextMapper<TContent>
    {
        #region IModelMapper

        public static async Task<TContent> MapContent(BaseResponseContext domainResponse, TContent domainContent, HttpResponse httpResponse)
        {
            if (domainResponse.Header?.StatusCode == ResponseCode.Success)
            {
                httpResponse.StatusCode = (int)HttpStatusCode.OK;
                httpResponse.ContentType = "application/json";

                httpResponse.Headers["correlationId"] = domainResponse.Header.CorrelationId;
                httpResponse.Headers["requestTimeUTC"] = domainResponse.Header.RequestTimeUTC.ToString();
                httpResponse.Headers["responseProcessingTimeInTicks"] = domainResponse.Header.ResponseProcessingTimeInTicks?.ToString();

                //await httpResponse.WriteAsync(JsonConvert.SerializeObject(domainContent));
                return domainContent;
            }
            else
            {
                throw new ContextException(domainResponse);
            }
        }
        public static async Task MapContext(BaseResponseContext domainResponse, HttpResponse httpResponse)
        {
            httpResponse.StatusCode = (int)MapHttpCode(domainResponse.Header?.StatusCode);
            httpResponse.ContentType = "application/json";

            httpResponse.Headers["correlationId"] = domainResponse.Header.CorrelationId;
            httpResponse.Headers["requestTimeUTC"] = domainResponse.Header.RequestTimeUTC.ToString();
            httpResponse.Headers["responseProcessingTimeInTicks"] = domainResponse.Header.ResponseProcessingTimeInTicks?.ToString();

            await httpResponse.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse() { error = domainResponse.Header }, Formatting.Indented, new JsonSerializerSettings
            {
                //NullValueHandling = NullValueHandling.Ignore
            }));
        }
        public static void Map(BaseResponseContext domainResponse, ServerCallContext grpcResponseContext)
        {
            if (domainResponse?.Header == null || grpcResponseContext == null) return;
            if (domainResponse.Header.StatusCode == ResponseCode.Success) return;

            grpcResponseContext.Status = new Status(MapGrpcCode(domainResponse.Header.StatusCode), domainResponse.Header.MessagesSummary ?? "");
        }

        #endregion
        #region helpers.

        private static HttpStatusCode MapHttpCode(ResponseCode? domainStatus)
        {
            switch (domainStatus)
            {
                case ResponseCode.Success:
                    {
                        return HttpStatusCode.OK;
                    }
                case ResponseCode.NotFound:
                    {
                        return HttpStatusCode.NotFound;
                    }
                case ResponseCode.ValidationError:
                case ResponseCode.InvalidInput:
                    {
                        return HttpStatusCode.BadRequest;
                    }
                case ResponseCode.AuthenticationError:
                    {
                        return HttpStatusCode.Unauthorized;
                    }
                case ResponseCode.AccessLocked:
                    {
                        return HttpStatusCode.Locked;
                    }
                case ResponseCode.AccessDenied:
                    {
                        return HttpStatusCode.Forbidden;
                    }
                case ResponseCode.SystemError:
                default:
                    {
                        return HttpStatusCode.InternalServerError;
                    }
            }
        }
        private static Grpc.Core.StatusCode MapGrpcCode(ResponseCode domainStatus)
        {
            switch (domainStatus)
            {
                case ResponseCode.Success:
                    {
                        return Grpc.Core.StatusCode.OK;
                    }
                case ResponseCode.ValidationError:
                    {
                        return Grpc.Core.StatusCode.FailedPrecondition;
                    }
                case ResponseCode.NotFound:
                    {
                        return Grpc.Core.StatusCode.NotFound;
                    }
                case ResponseCode.InvalidInput:
                    {
                        return Grpc.Core.StatusCode.InvalidArgument;
                    }
                case ResponseCode.AuthenticationError:
                    {
                        return Grpc.Core.StatusCode.Unauthenticated;
                    }
                case ResponseCode.AccessLocked:
                case ResponseCode.AccessDenied:
                    {
                        return Grpc.Core.StatusCode.PermissionDenied;
                    }
                case ResponseCode.SystemError:
                default:
                    {
                        return Grpc.Core.StatusCode.Unknown;
                    }
            }
        }

        #endregion
    }
}
