using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Attachments.API.Mappers;
using XCore.Services.Attachments.API.Models;
using XCore.Services.Attachments.Core.Contracts;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Support;

namespace XCore.Services.Attachments.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class AttachmentController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }

        private readonly IAttachmentsHandler _AttachmentsHandler;

        #endregion
        #region cst.

        public AttachmentController(IAttachmentsHandler attachmentsHandler)
        {
            this._AttachmentsHandler = attachmentsHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<AttachmentDTO>>> Get(ServiceExecutionRequestDTO<AttachmentSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.attachments.get";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<AttachmentSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<AttachmentSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Attachment>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<AttachmentDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AttachmentSearchCriteriaDTO, AttachmentSearchCriteria, SearchResultsDTO<AttachmentDTO>, SearchResults<Attachment>>(requestDTO, AttachmentGetRequestMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._AttachmentsHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AttachmentSearchCriteriaDTO, SearchResults<Attachment>, SearchResultsDTO<AttachmentDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AttachmentGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AttachmentSearchCriteriaDTO, SearchResultsDTO<AttachmentDTO>>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<AttachmentDTO>> Create(ServiceExecutionRequestDTO<AttachmentDTO> request)
        {
            string serviceName = "xcore.attachments.create";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<AttachmentDTO> requestDTO = request;
                ServiceExecutionRequest<Attachment> requestDMN;
                ServiceExecutionResponse<Attachment> responseDMN;
                ServiceExecutionResponseDTO<AttachmentDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AttachmentDTO, Attachment, AttachmentDTO, Attachment>(requestDTO, AttachmentMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._AttachmentsHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AttachmentDTO, Attachment, AttachmentDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AttachmentMapper.Instance, serviceName);
           
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AttachmentDTO, AttachmentDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("CreateConfirm")]
        public async Task<ServiceExecutionResponseDTO<List<AttachmentDTO>>> CreateConfirm(ServiceExecutionRequestDTO<List<AttachmentDTO>> request)
        {
            string serviceName = "xcore.Attachment.CreateConfirm";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<List<AttachmentDTO>> requestDTO = request;
                ServiceExecutionRequest<List<Attachment>> requestDMN;
                ServiceExecutionResponse<List<Attachment>> responseDMN;
                ServiceExecutionResponseDTO<List<AttachmentDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<List<AttachmentDTO>, List<Attachment>, List<AttachmentDTO>, List<Attachment>>(requestDTO, AttachmentListMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._AttachmentsHandler.CreateConfirm(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<List<AttachmentDTO>, List<Attachment>, List<AttachmentDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AttachmentListMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<List<AttachmentDTO>, List<AttachmentDTO>>(request, serviceName);
            }

            #endregion
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.attachment.delete";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

               

                #endregion
                #region BL.

                var domainResponse = await this._AttachmentsHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("DeleteList")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeleteListConfirm(ServiceExecutionRequestDTO<List<string>> request)
        {
            string serviceName = "xcore.attachment.DeleteListConfirm";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<List<string>> requestDTO = request;
                ServiceExecutionRequest<List<string>> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<List<string>, List<string>, bool, bool>(requestDTO, NativeMapper<List<string>>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;



                #endregion
                #region BL.

                var domainResponse = await this._AttachmentsHandler.DeleteListConfirm(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<List<string>, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<List<string>, bool>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("DeleteSoft")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeleteSoft(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.attachment.DeleteSoft";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;



                #endregion
                #region BL.

                var domainResponse = await this._AttachmentsHandler.DeleteSoft(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }
        //[HttpPost]
        //[Route("ConfirmStatus")]
        //public async Task<ServiceExecutionResponseDTO<bool>> ConfirmStatus(ServiceExecutionRequestDTO<AttachmentConfirmationAction> request)
        //{
        //    string serviceName = "xcore.attachment.Convert";
        //    try
        //    {
        //        #region request.

        //        // ...
        //        if (!this.Initialized.GetValueOrDefault())
        //        {
        //            throw new Exception("Service is not properly initialized.");
        //        }

        //        // ...
        //        bool isValidRequest;
        //        ServiceExecutionRequestDTO<AttachmentConfirmationAction> requestDTO = request;
        //        ServiceExecutionRequest<AttachmentConfirmationAction> requestDMN;
        //        ServiceExecutionResponse<bool> responseDMN;
        //        ServiceExecutionResponseDTO<bool> responseDTO;

        //        // ...
        //        responseDTO = ServiceExecutionContext.HandleRequestDTO<AttachmentConfirmationAction, AttachmentConfirmationAction, bool, bool>(requestDTO, NativeMapper<AttachmentConfirmationAction>.Instance, out requestDMN, out isValidRequest);
        //        if (!isValidRequest) return responseDTO;



        //        #endregion
        //        #region BL.

        //        var domainResponse = await this._AttachmentsHandler.ConfirmStatus(requestDMN.Content, requestDMN.ToRequestContext());

        //        #endregion
        //        #region response.

        //        return ServiceExecutionContext.PrepareResponse<AttachmentConfirmationAction, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

        //        #endregion
        //    }
        //    #region Exception

        //    catch (Exception x)
        //    {
        //        XLogger.Error($"Exception : {x}");
        //        return ServiceExecutionContext.PrepareResponseError<AttachmentConfirmationAction, bool>(request, serviceName);
        //    }

        //    #endregion
        //}
        //
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && ( this._AttachmentsHandler?.Initialized ?? false );

            return isValid;
        }

        #endregion
    }
}
