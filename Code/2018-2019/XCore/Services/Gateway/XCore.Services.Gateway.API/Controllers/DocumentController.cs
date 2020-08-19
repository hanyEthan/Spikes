using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Docs.SDK.Contracts;
using XCore.Services.Docs.SDK.Models;

namespace XCore.Services.Gateway.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class DocumentController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        protected IDocumentClient DocumentClient { get; set; }

        #endregion
        #region cst.

        public DocumentController(IDocumentClient documentClient)
        {
            this.DocumentClient = documentClient;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<DocumentDTO>>> Get(ServiceExecutionRequestDTO<DocumentSearchCriteriaDTO> request)
        {
            try
            {
                Check();
                var response = await this.DocumentClient.Get(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<DocumentDTO>> Create(ServiceExecutionRequestDTO<DocumentDTO> request)
        {
            try
            {
                Check();
                var response = await this.DocumentClient.Create(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        [HttpPost]
        [Route("CreateList")]
        public async Task<ServiceExecutionResponseDTO<List<DocumentDTO>>> CreateList(ServiceExecutionRequestDTO<List<DocumentDTO>> request)
        {
            try
            {
                Check();
                var response = await this.DocumentClient.Create(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<int> request)
        {
            try
            {
                Check();
                var response = await this.DocumentClient.Delete(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<DocumentDTO>> Edit(ServiceExecutionRequestDTO<DocumentDTO> request)
        {
            try
            {
                Check();
                var response = await this.DocumentClient.Edit(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.DocumentClient?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault()) throw new Exception("gateway service is not initialized correctly.");
        }
        #endregion
    }
}
