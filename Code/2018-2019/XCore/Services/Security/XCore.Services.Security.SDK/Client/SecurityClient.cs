using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Security.SDK.Contracts;
using XCore.Services.Security.SDK.Models.DTOs;
using XCore.Services.Security.SDK.Models.Support;

namespace XCore.Services.Security.SDK.Client
{
    public class SecurityClient : ISecurityClient
    {
        #region props.

        public bool Initialized { get; private set; }

        protected virtual IRestHandler<SecurityClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<SecurityClientConfig> ConfigProvider { get; set; }
        public IServiceBus ServiceBus { get; set; }

        #endregion
        #region cst.

        public SecurityClient(IRestHandler<SecurityClientConfig> restHandler) : this(restHandler, null, null)
        {
        }
        public SecurityClient(IRestHandler<SecurityClientConfig> restHandler, IConfigProvider<SecurityClientConfig> configProvider) : this(restHandler, null, configProvider)
        {
        }
        public SecurityClient(IRestHandler<SecurityClientConfig> restHandler, IServiceBus serviceBus, IConfigProvider<SecurityClientConfig> configProvider)
        {
            this.ServiceBus = serviceBus;
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;

            this.Initialized = Initialize();
        }

        #endregion
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateActor(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Actor/ActivateActor");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateApp(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/App/ActivateApp");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateRole(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Role/ActivateRole");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> Create(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<RoleDTO>, ServiceExecutionResponseDTO<RoleDTO>>(HttpMethod.POST, request, "/api/v0.1/Role/Create");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<ActorDTO>>> Create(ServiceExecutionRequestDTO<ActorDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ActorDTO>, ServiceExecutionResponseDTO<ActorDTO>>(HttpMethod.POST, request, "/api/v0.1/Actor/Create");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateActor(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Actor/DeActivateActor");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateApp(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/App/DeActivateApp");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateRole(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Role/DeActivateRole");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteRole(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Role/Delete");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteActor(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Actor/Delete");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Edit(ServiceExecutionRequestDTO<AppDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AppDTO>, ServiceExecutionResponseDTO<AppDTO>>(HttpMethod.POST, request, "/api/v0.1/App/Edit");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> Edit(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<RoleDTO>, ServiceExecutionResponseDTO<RoleDTO>>(HttpMethod.POST, request, "/api/v0.1/Role/Edit");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<ActorDTO>>> Edit(ServiceExecutionRequestDTO<ActorDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ActorDTO>, ServiceExecutionResponseDTO<ActorDTO>>(HttpMethod.POST, request, "/api/v0.1/Actor/Edit");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>>> Get(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AppSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>>(HttpMethod.POST, request, "/api/v0.1/App/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<PrivilegeDTO>>>> Get(ServiceExecutionRequestDTO<PrivilegeSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PrivilegeSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<PrivilegeDTO>>>(HttpMethod.POST, request, "/api/v0.1/Privilege/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<TargetDTO>>>> Get(ServiceExecutionRequestDTO<TargetSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<TargetSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<TargetDTO>>>(HttpMethod.POST, request, "/api/v0.1/Target/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>>> Get(ServiceExecutionRequestDTO<RoleSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<RoleSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>>(HttpMethod.POST, request, "/api/v0.1/Role/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ActorDTO>>>> Get(ServiceExecutionRequestDTO<ActorSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ActorSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<ActorDTO>>>(HttpMethod.POST, request, "/api/v0.1/Actor/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Register(ServiceExecutionRequestDTO<AppDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AppDTO>, ServiceExecutionResponseDTO<AppDTO>>(HttpMethod.POST, request, "/api/v0.1/App/Register");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> UnRegisterApp(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/App/Unregister");
        }
        #region Claim
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ClaimDTO>>>> Get(ServiceExecutionRequestDTO<ClaimSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ClaimSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<ClaimDTO>>>(HttpMethod.POST, request, "/api/v0.1/Claim/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<ClaimDTO>>> Create(ServiceExecutionRequestDTO<ClaimDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ClaimDTO>, ServiceExecutionResponseDTO<ClaimDTO>>(HttpMethod.POST, request, "/api/v0.1/Claim/Create");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<ClaimDTO>>> Edit(ServiceExecutionRequestDTO<ClaimDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ClaimDTO>, ServiceExecutionResponseDTO<ClaimDTO>>(HttpMethod.POST, request, "/api/v0.1/Claim/Edit");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteClaim(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Claim/Delete");
        }
        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.ServiceBus != null;
            isValid = isValid && this.ServiceBus.Initialized;

            isValid = isValid && this.ConfigProvider != null;
            isValid = isValid && this.ConfigProvider.Initialized;

            isValid = isValid && this.RestHandler != null;
            isValid = isValid && this.RestHandler.Initialized;

            return isValid;
        }

        
        //private AuditTrailDTO Map(ServiceExecutionRequestDTO<IAuditMessage> from)
        //{
        //    if (from == null || from.Content == null) return null;

        //    var to = new AuditTrailDTO()
        //    {
        //        Action = from.Content.Action,
        //        App = from.Content.App,
        //        ConnectionMethod = from.Content.ConnectionMethod,
        //        CreatedBy = from.Content.CreatedBy,
        //        DestinationAddress = from.Content.DestinationAddress,
        //        DestinationIP = from.Content.DestinationIP,
        //        DestinationPort = from.Content.DestinationPort,
        //        Entity = from.Content.Entity,
        //        Level = from.Content.Level,
        //        MetaData = from.Content.MetaData,
        //        ModifiedBy = from.Content.ModifiedBy,
        //        Module = from.Content.Module,
        //        SourceClient = from.Content.SourceClient,
        //        SourceIP = from.Content.SourceIP,
        //        SourceOS = from.Content.SourceOS,
        //        SourcePort = from.Content.SourcePort,
        //        SyncStatus = from.Content.SyncStatus,
        //        Text = from.Content.Text,
        //        UserId = from.Content.UserId,
        //        UserName = from.Content.UserName,
        //    };

        //    return to;

        //}

        #endregion
    }
}
