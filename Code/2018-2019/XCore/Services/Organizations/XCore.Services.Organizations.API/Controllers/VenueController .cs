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
using XCore.Services.Organizations.API.Models.Venue;
using XCore.Services.Organizations.API.Mappers;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;
using VenueDTOXCore.Services.Organizations.API.Models.Venue;
using XCore.Services.Organizations.API.Models;
namespace XCore.Services.Venues.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class VenueController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IVenueHandler venueHandler;

        #endregion
        #region cst.

        public VenueController(IVenueHandler venueHandler)
        {
            this.venueHandler = venueHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.
        #region Venue : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<VenueDTO>> VenueCreate(ServiceExecutionRequestDTO<VenueDTO> request)
        {
            string serviceName = "xcore.Venues.Venue.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<VenueDTO> requestDTO = request;
                ServiceExecutionRequest<Venue> requestDMN;
                ServiceExecutionResponse<Venue> responseDMN;
                ServiceExecutionResponseDTO<VenueDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<VenueDTO, Venue, VenueDTO, Venue>(requestDTO, VenueMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.venueHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<VenueDTO, Venue, VenueDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, VenueMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<VenueDTO, VenueDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Venue : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<VenueDTO>> VenueEdit(ServiceExecutionRequestDTO<VenueDTO> request)
        {
            string serviceName = "xcore.Venues.Venue.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<VenueDTO> requestDTO = request;
                ServiceExecutionRequest<Venue> requestDMN;
                ServiceExecutionResponse<Venue> responseDMN;
                ServiceExecutionResponseDTO<VenueDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<VenueDTO, Venue, VenueDTO, Venue>(requestDTO, VenueMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.venueHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<VenueDTO, Venue, VenueDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, VenueMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<VenueDTO, VenueDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Venue : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> VenueDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.Venues.Venue.delete";

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

                var domainResponse = await  this.venueHandler.DeleteVenue(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Venue : Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<VenueDTO>>> VenueGet(ServiceExecutionRequestDTO<VenueSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.Venues.venue.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<VenueSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<VenueSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Venue>> responseDMN = new ServiceExecutionResponse<SearchResults<Venue>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<VenueDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<VenueSearchCriteriaDTO, VenueSearchCriteria, SearchResultsDTO<VenueDTO>, SearchResults<Venue>>(requestDTO, VenueSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await  this.venueHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<VenueSearchCriteriaDTO, SearchResults<Venue>, SearchResultsDTO<VenueDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, VenueSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<VenueSearchCriteriaDTO, SearchResultsDTO<VenueDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Venue : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateVenue(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.Venues.Venue.activate";

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

                var domainResponse = await  this.venueHandler.ActivateVenue(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Venue : DeActivate

        [HttpPost]
        [Route("Deactivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateVenue(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.Venues.Venue.deactivate";

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

                var domainResponse = await  this.venueHandler.DeactivateVenue(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (this.venueHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}