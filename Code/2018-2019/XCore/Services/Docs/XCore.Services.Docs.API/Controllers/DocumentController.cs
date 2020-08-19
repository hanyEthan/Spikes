using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Docs.API.Mappers;
using XCore.Services.Docs.API.Models;
using XCore.Services.Docs.Core.Contracts;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class DocumentController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private NativeMapper<bool> BoolMapper { get; set; }

        private readonly IDocumentHandler _DocumentHandler;

        #endregion
        #region cst.

        public DocumentController(IDocumentHandler documentHandler)
        {
            this._DocumentHandler = documentHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<DocumentDTO>>> Get(ServiceExecutionRequestDTO<DocumentSearchCriteriaDTO> request)
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
                ServiceExecutionRequestDTO<DocumentSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<DocumentSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Document>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<DocumentDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DocumentSearchCriteriaDTO, DocumentSearchCriteria, SearchResultsDTO<DocumentDTO>, SearchResults<Document>>(requestDTO, DocumentGetRequestMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._DocumentHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<DocumentSearchCriteriaDTO, SearchResults<Document>, SearchResultsDTO<DocumentDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DocumentGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DocumentSearchCriteriaDTO, SearchResultsDTO<DocumentDTO>>(request, serviceName);
            }

            #endregion
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<DocumentDTO>> Create(ServiceExecutionRequestDTO<DocumentDTO> request)
        {
            string serviceName = "xcore.document.create";

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
                ServiceExecutionRequestDTO<DocumentDTO> requestDTO = request;
                ServiceExecutionRequest<Document> requestDMN;
                ServiceExecutionResponse<Document> responseDMN;
                ServiceExecutionResponseDTO<DocumentDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DocumentDTO, Document, DocumentDTO, Document>(requestDTO, DocumentMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._DocumentHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<DocumentDTO, Document, DocumentDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DocumentMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DocumentDTO, DocumentDTO>(request, serviceName);
            }

            #endregion
        }

        [HttpPost]
        [Route("createList")]
        public async Task<ServiceExecutionResponseDTO<List<DocumentDTO>>> CreateList(ServiceExecutionRequestDTO<List<DocumentDTO>> request)
        {
            string serviceName = "xcore.document.create.list";

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
                ServiceExecutionRequestDTO<List<DocumentDTO>> requestDTO = request;
                ServiceExecutionRequest<List<Document>> requestDMN;
                ServiceExecutionResponse<List<Document>> responseDMN;
                ServiceExecutionResponseDTO<List<DocumentDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<List<DocumentDTO>, List<Document>, List<DocumentDTO>, List<Document>>(requestDTO, DocumentListMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._DocumentHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<List<DocumentDTO>, List<Document>, List<DocumentDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DocumentListMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<List<DocumentDTO>, List<DocumentDTO>>(request, serviceName);
            }

            #endregion
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<DocumentDTO>> Edit(ServiceExecutionRequestDTO<DocumentDTO> request)
        {
            string serviceName = "xcore.document.edit";

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
                ServiceExecutionRequestDTO<DocumentDTO> requestDTO = request;
                ServiceExecutionRequest<Document> requestDMN;
                ServiceExecutionResponse<Document> responseDMN;
                ServiceExecutionResponseDTO<DocumentDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DocumentDTO, Document, DocumentDTO, Document>(requestDTO, DocumentMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._DocumentHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<DocumentDTO, Document, DocumentDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DocumentMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DocumentDTO, DocumentDTO>(request, serviceName);
            }

            #endregion
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.document.delete";

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
                ServiceExecutionRequestDTO<int> requestDTO = request;
                ServiceExecutionRequest<int> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<int, int, bool, bool>(requestDTO, NativeMapper<int>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._DocumentHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<int, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<int, bool>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("DeleteList")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeleteList(ServiceExecutionRequestDTO<List<int>> request)
        {
            string serviceName = "xcore.document.DeleteList";

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
                ServiceExecutionRequestDTO<List<int>> requestDTO = request;
                ServiceExecutionRequest<List<int>> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<List<int>, List<int>, bool, bool>(requestDTO, NativeMapper<List<int>>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._DocumentHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<List<int>, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<List<int>, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && ( this._DocumentHandler?.Initialized ?? false );

            return isValid;
        }

        #endregion
    }
}
