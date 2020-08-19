using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.API.Models.City;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.API.Mappers;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class CityController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly ICityHandler CityHandler;

        #endregion
        #region cst.

        public CityController(ICityHandler CityHandler)
        {
            this.CityHandler = CityHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.
        #region City : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<CityDTO>> CityCreate(ServiceExecutionRequestDTO<CityDTO> request)
        {
            string serviceName = "xcore.Organizations.City.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<CityDTO> requestDTO = request;
                ServiceExecutionRequest<City> requestDMN;
                ServiceExecutionResponse<City> responseDMN;
                ServiceExecutionResponseDTO<CityDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<CityDTO, City, CityDTO, City>(requestDTO, CityMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.CityHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<CityDTO, City, CityDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, CityMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<CityDTO, CityDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region City : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<CityDTO>> CityEdit(ServiceExecutionRequestDTO<CityDTO> request)
        {
            string serviceName = "xcore.Organizations.City.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<CityDTO> requestDTO = request;
                ServiceExecutionRequest<City> requestDMN;
                ServiceExecutionResponse<City> responseDMN;
                ServiceExecutionResponseDTO<CityDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<CityDTO, City, CityDTO, City>(requestDTO, CityMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.CityHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<CityDTO, City, CityDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, CityMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<CityDTO, CityDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region City : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> CityDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.Organizations.City.delete";

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
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.CityHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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

        #endregion
        #region City : Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<CityDTO>>> CityGet(ServiceExecutionRequestDTO<CitySearchCriteriaDTO> request)
        {
            string serviceName = "xcore.Organizations.City.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<CitySearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<CitySearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<City>> responseDMN = new ServiceExecutionResponse<SearchResults<City>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<CityDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<CitySearchCriteriaDTO, CitySearchCriteria, SearchResultsDTO<CityDTO>, SearchResults<City>>(requestDTO, CitySearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.CityHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<CitySearchCriteriaDTO, SearchResults<City>, SearchResultsDTO<CityDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, CitySearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<CitySearchCriteriaDTO, SearchResultsDTO<CityDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region City : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateCity(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.Organizations.City.activate";

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
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.CityHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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

        #endregion
        #region City : DeActivate

        [HttpPost]
        [Route("Deactivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateCity(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.Organizations.City.deactivate";

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
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.CityHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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

        #endregion




        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.CityHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}