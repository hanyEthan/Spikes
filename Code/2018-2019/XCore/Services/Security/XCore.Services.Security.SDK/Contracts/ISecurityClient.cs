using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Security.SDK.Models.DTOs;
using XCore.Services.Security.SDK.Models.Support;

namespace XCore.Services.Security.SDK.Contracts
{
   public interface ISecurityClient
    {
        bool Initialized { get; }

        #region AppDTO

        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>>> Get(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Register(ServiceExecutionRequestDTO<AppDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Edit(ServiceExecutionRequestDTO<AppDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> UnRegisterApp(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateApp(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateApp(ServiceExecutionRequestDTO<int> request);

        #endregion
        #region PrivilegeDTO
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<PrivilegeDTO>>>> Get(ServiceExecutionRequestDTO<PrivilegeSearchCriteriaDTO> request);

        #endregion
        #region TargetDTO
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<TargetDTO>>>> Get(ServiceExecutionRequestDTO<TargetSearchCriteriaDTO> request);
        #endregion
        #region RoleDTO

        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>>> Get(ServiceExecutionRequestDTO<RoleSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> Create(ServiceExecutionRequestDTO<RoleDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> Edit(ServiceExecutionRequestDTO<RoleDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteRole(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateRole(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateRole(ServiceExecutionRequestDTO<int> request);

        #endregion
        #region ActorDTO

        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ActorDTO>>>> Get(ServiceExecutionRequestDTO<ActorSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ActorDTO>>> Create(ServiceExecutionRequestDTO<ActorDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ActorDTO>>> Edit(ServiceExecutionRequestDTO<ActorDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteActor(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateActor(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateActor(ServiceExecutionRequestDTO<int> request);

        #endregion
        #region Claim
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ClaimDTO>>>> Get(ServiceExecutionRequestDTO<ClaimSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ClaimDTO>>> Create(ServiceExecutionRequestDTO<ClaimDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ClaimDTO>>> Edit(ServiceExecutionRequestDTO<ClaimDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteClaim(ServiceExecutionRequestDTO<int> request);
        #endregion




    }
}
