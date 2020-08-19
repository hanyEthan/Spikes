using System;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Geo.API.Handlers;
using XCore.Services.Geo.API.Mappers;
using XCore.Services.Geo.API.Models;
using XCore.Services.Geo.Core.Models.Commands;

namespace XCore.Services.Geo.API.Controllers
{
    [ApiController]
    public class LocationsController : ControllerBase
    {
        #region props.

        private AddLocationRequestMapper AddLocationRequestMapper { get; set; }
        private AddLocationResponseMapper AddLocationResponseMapper { get; set; }
        private GetLocationsRequestMapper GetLocationsRequestMapper { get; set; }
        private GetLocationsResponseMapper GetLocationsResponseMapper { get; set; }
        private GetCurrentLocationRequestMapper GetCurrentLocationRequestMapper { get; set; }
        private GetCurrentLocationResponseMapper GetCurrentLocationResponseMapper { get; set; }
        private GeoServiceHandler Handler { get; set; }

        #endregion

        #region cst.
        public LocationsController()
        {
            Initialize();
        }

        #endregion

        #region actions

        #region add location

        [Route("api/AddLocation"), HttpPost]
        public ServiceExecutionResponseDTO<AddLocationResponseDTO> AddLocation([FromBody] ServiceExecutionRequestDTO<AddLocationRequestDTO> request)
        {
            const string serviceName = "XCore.Geo.AddLocation";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<AddLocationRequestDTO> requestDTO = request;
                ServiceExecutionRequest<AddLocationRequestDomain> requestDMN;
                ServiceExecutionResponse<AddLocationResponseDomain> responseDMN;
                ServiceExecutionResponseDTO<AddLocationResponseDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AddLocationRequestDTO, AddLocationRequestDomain, AddLocationResponseDTO, AddLocationResponseDomain>(requestDTO, this.AddLocationRequestMapper, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = this.Handler.AddLocationEvent(requestDMN?.Content);

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse, this.AddLocationResponseMapper, serviceName);

                #endregion
            }
            catch (Exception e)
            {
                // TODO : log
                XLogger.Error("Exception : " + e);
                return ServiceExecutionContext.PrepareResponseError<AddLocationRequestDTO, AddLocationResponseDTO>(request, serviceName);
            }
        }

        #endregion

        #region get locations

        [Route("api/GetLocations"), HttpPost]
        public ServiceExecutionResponseDTO<GetLocationsResponseDTO> GetLocations([FromBody] ServiceExecutionRequestDTO<GetLocationsRequestDTO> request)
        {
            const string serviceName = "XCore.Geo.GetLocations";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<GetLocationsRequestDTO> requestDTO = request;
                ServiceExecutionRequest<GetLocationsRequestDomain> requestDMN;
                ServiceExecutionResponse<GetLocationsResponseDomain> responseDMN;
                ServiceExecutionResponseDTO<GetLocationsResponseDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<GetLocationsRequestDTO, GetLocationsRequestDomain, GetLocationsResponseDTO, GetLocationsResponseDomain>(requestDTO, this.GetLocationsRequestMapper, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = this.Handler.GetLocations(requestDMN?.Content?.Criteria);

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse, this.GetLocationsResponseMapper, serviceName);

                #endregion
            }
            catch (Exception e)
            {
                // TODO : log
                XLogger.Error("Exception : " + e);
                return ServiceExecutionContext.PrepareResponseError<GetLocationsRequestDTO, GetLocationsResponseDTO>(request, serviceName);
            }
        }

        #endregion

        #region get current location

        [Route("api/GetCurrentLocation"), HttpPost]
        public ServiceExecutionResponseDTO<GetCurrentLocationResponseDTO> GetCurrentLocation([FromBody] ServiceExecutionRequestDTO<GetCurrentLocationRequestDTO> request)
        {
            const string serviceName = "XCore.Geo.GetCurrentLocation";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<GetCurrentLocationRequestDTO> requestDTO = request;
                ServiceExecutionRequest<GetCurrentLocationRequestDomain> requestDMN;
                ServiceExecutionResponse<GetCurrentLocationResponseDomain> responseDMN;
                ServiceExecutionResponseDTO<GetCurrentLocationResponseDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<GetCurrentLocationRequestDTO, GetCurrentLocationRequestDomain, GetCurrentLocationResponseDTO, GetCurrentLocationResponseDomain>(requestDTO, this.GetCurrentLocationRequestMapper, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = this.Handler.GetCurrentLocation(requestDMN?.Content.EntityCode);

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse, this.GetCurrentLocationResponseMapper, serviceName);

                #endregion
            }
            catch (Exception e)
            {
                // TODO : log
                XLogger.Error("Exception : " + e);
                return ServiceExecutionContext.PrepareResponseError<GetCurrentLocationRequestDTO, GetCurrentLocationResponseDTO>(request, serviceName);
            }
        }

        #endregion

        #endregion

        #region helpers.

        private void Initialize()
        {
            try
            {
                this.Handler = new GeoServiceHandler();
                this.AddLocationRequestMapper = new AddLocationRequestMapper();
                this.AddLocationResponseMapper = new AddLocationResponseMapper();
                this.GetLocationsRequestMapper = new GetLocationsRequestMapper();
                this.GetLocationsResponseMapper = new GetLocationsResponseMapper();
                this.GetCurrentLocationRequestMapper = new GetCurrentLocationRequestMapper();
                this.GetCurrentLocationResponseMapper = new GetCurrentLocationResponseMapper();
            }
            catch (Exception e)
            {
                // TODO : log
                XLogger.Error("Exception: " + e);
            }

        }

        #endregion
    }
}