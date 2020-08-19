using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Utilities;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.API.Mappers.Advertisements;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Services.Advertisements.API.Mappers.Advertisements;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;

namespace XCore.Services.Hiring.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class AdvertisementsController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IAdvertisementsHandler AdvertisementsHandler;

        #endregion
        #region cst.

        public AdvertisementsController(IAdvertisementsHandler AdvertisementsHandler)
        {
            this.AdvertisementsHandler = AdvertisementsHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions. 
        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<AdvertisementDTO>>> Get(ServiceExecutionRequestDTO<AdvertisementsSearchCriteriaDTO> request)
        {
            string serviceName = "Xcore.Hiring.Advertisement.Get";

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
                ServiceExecutionRequestDTO<AdvertisementsSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<AdvertisementsSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Advertisement>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<AdvertisementDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AdvertisementsSearchCriteriaDTO, AdvertisementsSearchCriteria, SearchResultsDTO<AdvertisementDTO>, SearchResults<Advertisement>>(requestDTO, AdvertisementGetRequestMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.AdvertisementsHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AdvertisementsSearchCriteriaDTO, SearchResults<Advertisement>, SearchResultsDTO<AdvertisementDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AdvertisementGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AdvertisementsSearchCriteriaDTO, SearchResultsDTO<AdvertisementDTO>>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<AdvertisementDTO>> Create(ServiceExecutionRequestDTO<AdvertisementDTO> request)
        {
            string serviceName = "Xcore.Hiring.Advertisements.Create";

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
                ServiceExecutionRequestDTO<AdvertisementDTO> requestDTO = request;
                ServiceExecutionRequest<Advertisement> requestDMN;
                ServiceExecutionResponse<Advertisement> responseDMN;
                ServiceExecutionResponseDTO<AdvertisementDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AdvertisementDTO, Advertisement, AdvertisementDTO, Advertisement>(requestDTO, AdvertisementMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.AdvertisementsHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AdvertisementDTO, Advertisement, AdvertisementDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AdvertisementMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AdvertisementDTO, AdvertisementDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<AdvertisementDTO>> Edit(ServiceExecutionRequestDTO<AdvertisementDTO> request)
        {
            string serviceName = "xcore.Hiring.Advertisements.Edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<AdvertisementDTO> requestDTO = request;
                ServiceExecutionRequest<Advertisement> requestDMN;
                ServiceExecutionResponse<Advertisement> responseDMN;
                ServiceExecutionResponseDTO<AdvertisementDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AdvertisementDTO, Advertisement, AdvertisementDTO, Advertisement>(requestDTO, AdvertisementMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.AdvertisementsHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AdvertisementDTO, Advertisement, AdvertisementDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AdvertisementMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AdvertisementDTO, AdvertisementDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("ActivateById")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateById(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "Xcore.Hiring.Advertisements.ActivateById";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<int> requestDTO = request;
                ServiceExecutionRequest<int> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<int, int, bool, bool>(requestDTO, NativeMapper<int>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await AdvertisementsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<int, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

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
        [Route("ActivateByCode")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateByCode(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "Xcore.Hiring.Advertisements.ActivateByCode";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await AdvertisementsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

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
        [Route("DeActivateById")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateById(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "Xcore.Hiring.Advertisements.DeActivateById";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<int> requestDTO = request;
                ServiceExecutionRequest<int> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<int, int, bool, bool>(requestDTO, NativeMapper<int>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await AdvertisementsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<int, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

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
        [Route("DeActivateByCode")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateByCode(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "Xcore.Hiring.Advertisements.DeActivateByCode";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await AdvertisementsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

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
        //[Route("DeleteById")]
        //public async Task<ServiceExecutionResponseDTO<bool>> DeleteById(ServiceExecutionRequestDTO<int> request)
        //{
        //    string serviceName = "Xcore.Hiring.Advertisements.DeleteById";

        //    try
        //    {
        //        #region request.

        //        // ...
        //        bool isValidRequest;
        //        ServiceExecutionRequestDTO<int> requestDTO = request;
        //        ServiceExecutionRequest<int> requestDMN;
        //        ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
        //        ServiceExecutionResponseDTO<bool> responseDTO;

        //        // ...
        //        responseDTO = ServiceExecutionContext.HandleRequestDTO<int, int, bool, bool>(requestDTO, NativeMapper<int>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
        //        if (!isValidRequest) return responseDTO;

        //        #endregion
        //        #region BL.

        //        var domainResponse = await this.AdvertisementsHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

        //        #endregion
        //        #region response.

        //        return ServiceExecutionContext.PrepareResponse<int, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

        //        #endregion
        //    }
        //    #region Exception

        //    catch (Exception x)
        //    {
        //        XLogger.Error($"Exception : {x}");
        //        return ServiceExecutionContext.PrepareResponseError<int, bool>(request, serviceName);
        //    }

        //    #endregion
        //}
        //[HttpPost]
        //[Route("DeleteByCode")]
        //public async Task<ServiceExecutionResponseDTO<bool>> DeleteByCode(ServiceExecutionRequestDTO<string> request)
        //{
        //    string serviceName = "Xcore.Hiring.Advertisements.DeleteByCode";

        //    try
        //    {
        //        #region request.

        //        // ...
        //        bool isValidRequest;
        //        ServiceExecutionRequestDTO<string> requestDTO = request;
        //        ServiceExecutionRequest<string> requestDMN;
        //        ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
        //        ServiceExecutionResponseDTO<bool> responseDTO;

        //        // ...
        //        responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
        //        if (!isValidRequest) return responseDTO;

        //        #endregion
        //        #region BL.

        //        var domainResponse = await this.AdvertisementsHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

        //        #endregion
        //        #region response.

        //        return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

        //        #endregion
        //    }
        //    #region Exception

        //    catch (Exception x)
        //    {
        //        XLogger.Error($"Exception : {x}");
        //        return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
        //    }

        //    #endregion
        //}        
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;
            isValid = isValid && ( this.AdvertisementsHandler?.Initialized ?? false );
            return isValid;
        }

        #endregion
    }
}
