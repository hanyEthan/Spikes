using ADS.Common.Bases.Services.Context.Models;
using ADS.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS.Common.Bases.Services.Context.Models.Enums;
using ADS.Common.Context;

namespace ADS.Common.Bases.Services.Context.Handlers
{
    public static class ServiceExecutionContext
    {
        #region props.

        private const string Token = "542C5B48-C2C3-419C-95F5-E955A9AAAE87";

        #endregion
        #region publics.
        public static ServiceExecutionResponse<TResContent> HandleRequest<TReqContent, TResContent>(ServiceExecutionRequest<TReqContent> request) //where T : IServiceExecutionContent , where D : IServiceExecutionContent
        {
            var response = new ServiceExecutionResponse<TResContent>();

            try
            {
                if (request.ClientToken == null || request.ClientToken != Token)
                {
                    response.ResponseCode = (int)ResponseCode.AccessDenied;
                }
                else if (request.Content == null)
                {
                    response.ResponseCode = (int)ResponseCode.InvalidInput;
                }
                else
                {
                    response.ResponseCode = (int)ResponseCode.Success;
                }

                // TODO : handle


                return response;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;

                // TODO : handle
            }
        }

        public static bool Map<TDTO, TDomain>(ServiceExecutionRequestDTO<TDTO> from, out ServiceExecutionRequest<TDomain> to) //where TDTO : IServiceExecutionContent
        {
            to = null;
            if (from == null) return false;

            try
            {
                to = new ServiceExecutionRequest<TDomain>();

                to.ClientToken = from.RequestClientToken;
                to.Culture = from.RequestCulture;
                to.Metadata = from.RequestMetadata;
                to.RequestTime = from.RequestTime;
                to.SessionCode = from.RequestSessionCode;
                to.UserCode = from.RequestUserCode;

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }
        public static bool Map<TDTO, TDomain>(ServiceExecutionResponse<TDomain> from, out ServiceExecutionResponseDTO<TDTO> to)
        {
            to = null;
            if (from == null) return false;

            try
            {
                to = new ServiceExecutionResponseDTO<TDTO>();

                to.Metadata = from.Metadata;
                to.RequestTime = from.RequestTime;
                to.ResponseCode = from.ResponseCode;
                to.ResponseMessage = from.ResponseMessage;
                to.ResponseProcessingTime = from.ResponseProcessingTime;

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return false;
            }
        }

        #endregion
        #region helpers.

        #endregion
    }
}
