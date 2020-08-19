using System;
using XCore.Utilities.Infrastructure.Context.Execution.Models;
using XCore.Utilities.Infrastructure.Context.Services.Contracts;
using XCore.Utilities.Infrastructure.Context.Services.Models;
using XCore.Utilities.Infrastructure.Context.Services.Models.Enums;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Infrastructure.Context.Services.Handlers
{
    public static class ServiceExecutionContext
    {
        #region props.

        private const string Token = "634a5d14-81c1-4c9b-9484-0b3c21bb2299";

        #endregion
        #region publics.

        public static ServiceExecutionResponse<TResContent> HandleRequest<TReqContent, TResContent>( ServiceExecutionRequest<TReqContent> request ) //where T : IServiceExecutionContent , where D : IServiceExecutionContent
        {
            var response = new ServiceExecutionResponse<TResContent>();

            try
            {
                if ( request.ClientToken == null || request.ClientToken != Token )
                {
                    response.ResponseCode = ( int ) ResponseCode.AccessDenied;
                }
                else if ( request.Content == null )
                {
                    response.ResponseCode = ( int ) ResponseCode.InvalidInput;
                }
                else
                {
                    response.ResponseCode = ( int ) ResponseCode.Success;
                }

                return response;
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public static ServiceExecutionResponseDTO<TResContentDTO> HandleRequestDTO<TReqContentDTO, TReqContentDMN, TResContentDTO , TResContentDMN>( ServiceExecutionRequestDTO<TReqContentDTO> request , 
                                                                                                                                                     IModelMapper<TReqContentDMN , TReqContentDTO> modelMapper ,
                                                                                                                                                     out ServiceExecutionRequest<TReqContentDMN> requestDMN ,
                                                                                                                                                     out bool isValid , 
                                                                                                                                                     string serviceName = "Service" ) //where T : IServiceExecutionContent , where D : IServiceExecutionContent
        {
            isValid = false;
            requestDMN = null;

            try
            {
                // map request context ( DTO -> DMN ) ...
                bool status = ServiceExecutionContext.Map( request , out requestDMN );

                // map request content ( DTO -> DMN ) ...
                requestDMN.Content = modelMapper != null ? modelMapper.Map( request.Content ) : default( TReqContentDMN );

                // validate request ...
                var responseDMN = ServiceExecutionContext.HandleRequest<TReqContentDMN , TResContentDMN>( requestDMN );
                isValid = ServiceExecutionContext.IsValidRequest( responseDMN );

                // return response ...
                return ServiceExecutionContext.PrepareResponse<TReqContentDTO , TResContentDMN , TResContentDTO>( request , responseDMN , EnableLogging: false );
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return ServiceExecutionContext.PrepareResponseError<TReqContentDTO,TResContentDTO>( request , serviceName );
            }
        }

        public static bool Map<TDTO, TDomain>( ServiceExecutionRequestDTO<TDTO> from , out ServiceExecutionRequest<TDomain> to ) //where TDTO : IServiceExecutionContent
        {
            to = null;
            if ( from == null ) return false;

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
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public static bool Map<TDTO, TDomain>( ServiceExecutionResponse<TDomain> from , out ServiceExecutionResponseDTO<TDTO> to )
        {
            to = null;
            if ( from == null ) return false;

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
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }

        public static bool IsValidRequest<TResContent>( ServiceExecutionResponse<TResContent> response )
        {
            return ( response != null && response.ResponseCode == ( int ) ResponseState.Success );
        }
        public static ServiceExecutionResponseDTO<TResContentDTO> PrepareResponse<TReqContentDTO, TResContentDomain, TResContentDTO>( ServiceExecutionRequestDTO<TReqContentDTO> requestDTO , ServiceExecutionResponse<TResContentDomain> response , string serviceName = "Service" , bool EnableLogging = true )
        {
            ServiceExecutionResponseDTO<TResContentDTO> responseDTO;
            ServiceExecutionContext.Map( response , out responseDTO );

            #region LOG
            if ( EnableLogging )
            {
                NLogger.Info( serviceName + " : Request  : " + XSerialize.JSON.Serialize( requestDTO ) );
                NLogger.Info( serviceName + " : Response : " + XSerialize.JSON.Serialize( responseDTO ) );
            }
            #endregion

            return responseDTO;
        }
        public static ServiceExecutionResponseDTO<TResContentDTO> PrepareResponse<TReqContentDTO, TResContentDomain, TResContentDTO>( ServiceExecutionRequestDTO<TReqContentDTO> requestDTO , ServiceExecutionResponse<TResContentDomain> response , ResponseState status , string message , TResContentDomain content , IModelMapper<TResContentDomain , TResContentDTO> modelMapper = null , string serviceName = "Service" , bool EnableLogging = true )
        {
            response.ResponseCode = ( int ) status;
            response.ResponseMessage = message;
            response.Content = content;

            // ...
            ServiceExecutionResponseDTO<TResContentDTO> responseDTO;
            ServiceExecutionContext.Map( response , out responseDTO );

            // ...
            responseDTO.Content = modelMapper != null ? modelMapper.Map( response.Content ) : default( TResContentDTO );
            responseDTO.ResponseMessage = responseDTO.Content != null ? responseDTO.ResponseMessage
                                        : status != ResponseState.Success ? responseDTO.ResponseMessage
                                        : "Error";
            responseDTO.ResponseCode = responseDTO.Content != null ? responseDTO.ResponseCode : ( int ) ResponseState.Error;

            #region LOG
            if ( EnableLogging )
            {
                NLogger.Info( serviceName + " : Request  : " + XSerialize.JSON.Serialize( requestDTO ) );
                NLogger.Info( serviceName + " : Response : " + XSerialize.JSON.Serialize( responseDTO ) );
            }
            #endregion

            return responseDTO;
        }
        public static ServiceExecutionResponseDTO<TResContentDTO> PrepareResponse<TReqContentDTO, TResContentDomain, TResContentDTO>( ServiceExecutionRequestDTO<TReqContentDTO> requestDTO , ServiceExecutionResponseDTO<TResContentDTO> responseDTO , ResponseState status , string message , TResContentDomain content , IModelMapper<TResContentDomain , TResContentDTO> modelMapper = null , string serviceName = "Service" , bool EnableLogging = true )
        {
            responseDTO.Content = modelMapper != null ? modelMapper.Map( content ) : default( TResContentDTO );
            responseDTO.ResponseMessage = responseDTO.Content != null  ? message 
                                        : status != ResponseState.Success ? message
                                        : "Error";
            responseDTO.ResponseCode = responseDTO.Content != null ? ( int ) status : ( int ) ResponseState.Error;

            #region LOG
            if ( EnableLogging )
            {
                NLogger.Info( serviceName + " : Request  : " + XSerialize.JSON.Serialize( requestDTO ) );
                NLogger.Info( serviceName + " : Response : " + XSerialize.JSON.Serialize( responseDTO ) );
            }
            #endregion

            return responseDTO;
        }
        public static ServiceExecutionResponseDTO<TResContentDTO> PrepareResponseError<TReqContentDTO, TResContentDTO>( ServiceExecutionRequestDTO<TReqContentDTO> requestDTO , string serviceName = "Service" , bool EnableLogging = true )
        {
            var response = new ServiceExecutionResponseDTO<TResContentDTO>()
            {
                Content = default( TResContentDTO ) ,
                ResponseCode = ( int ) ResponseState.Error ,
            };

            #region LOG
            if ( EnableLogging )
            {
                NLogger.Info( serviceName + " : Request  : " + XSerialize.JSON.Serialize( requestDTO ) );
                NLogger.Info( serviceName + " : Response : " + XSerialize.JSON.Serialize( response ) );
            }
            #endregion

            return response;
        }

        #endregion
        #region helpers.

        #endregion
    }
}
