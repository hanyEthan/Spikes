using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Personnel.Core.Contracts.Personnels;
using XCore.Services.Personnel.Models.Personnels;
using XCore.Services.Personnel.Models.DTO.Personnels;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.API.Mappers.Personnels;
using XCore.Services.Personnel.Models.DTO.Essential.Personnels;
using XCore.Services.Personnel.Models.DTO.Support.Enum;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class PersonnelController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IPersonnelHandler PersonnelHandler;

        #endregion
        #region cst.

        public PersonnelController(IPersonnelHandler personnelHandler)
        {
            this.PersonnelHandler = personnelHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Personnel : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<PersonnelDTO>> PersonnelCreate(ServiceExecutionRequestDTO<PersonnelEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.Personnel.create";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<PersonnelEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Person> requestDMN;
                ServiceExecutionResponse<Person> responseDMN;
                ServiceExecutionResponseDTO<PersonnelDTO> responseDTO;

                // ...
                 responseDTO = ServiceExecutionContext.HandleRequestDTO<PersonnelEssentialDTO, Person, PersonnelDTO, Person>(requestDTO, PersonnelEssentialMapper<Person,PersonnelEssentialDTO>.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.
                
                //requestDMN = Map(requestDMN);//Map to Get APPId,ModuleId from ContextRequest

                var domainResponse = await PersonnelHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<PersonnelEssentialDTO, Person, PersonnelDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, PersonnelMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<PersonnelEssentialDTO, PersonnelDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Personnel : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<PersonnelDTO>> PersonnelEdit(ServiceExecutionRequestDTO<PersonnelEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.Personnel.edit";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<PersonnelEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Person> requestDMN;
                ServiceExecutionResponse<Person> responseDMN;
                ServiceExecutionResponseDTO<PersonnelDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<PersonnelEssentialDTO, Person, PersonnelDTO, Person>(requestDTO, PersonnelEssentialMapper<Person, PersonnelEssentialDTO>.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                //requestDMN = Map(requestDMN);//Map to Get APPId,ModuleId from ContextRequest
                var domainResponse = await PersonnelHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<PersonnelEssentialDTO, Person, PersonnelDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, PersonnelMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<PersonnelEssentialDTO, PersonnelDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Personnel : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> PersonnelDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.personnel.Personnel.delete";

            try
            {
                #region check.
                Check();
                #endregion
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

                var domainResponse = await PersonnelHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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

        #endregion
        #region Personnel : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.apps.activate";

            try
            {
                #region check.
                Check();
                #endregion
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

                var domainResponse = await PersonnelHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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

        #endregion
        #region Personnel : DeActivate

        [HttpPost]
        [Route("DeActivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivate(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.apps.deactivate";

            try
            {
                #region check.
                Check();
                #endregion
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

                var domainResponse = await PersonnelHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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

        #endregion
        #region Personnel : Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelDTO>>> PersonnelGet(ServiceExecutionRequestDTO<PersonnelSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.personnel.Personnel.get";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<PersonnelSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<PersonSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Person>> responseDMN = new ServiceExecutionResponse<SearchResults<Person>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<PersonnelSearchCriteriaDTO, PersonSearchCriteria, SearchResultsDTO<PersonnelDTO>, SearchResults<Person>>(requestDTO, PersonnelGetRequestMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await PersonnelHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<PersonnelSearchCriteriaDTO, SearchResults<Person>, SearchResultsDTO<PersonnelDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, PersonnelGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<PersonnelSearchCriteriaDTO, SearchResultsDTO<PersonnelDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (PersonnelHandler?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }


        private ServiceExecutionRequest<Person> Map(ServiceExecutionRequest<Person> from)
        {
            ServiceExecutionRequest<Person> to = new ServiceExecutionRequest<Person>();
            to.Content = from.Content;
            to.Content.AppId = from.ToRequestContext().AppId;
            to.Content.ModuleId = from.ToRequestContext().ModuleId;
            return to;
        }

        #endregion
    }
}