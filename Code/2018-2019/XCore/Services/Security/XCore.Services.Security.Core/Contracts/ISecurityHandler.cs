using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Events.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Contracts
{
    public interface ISecurityHandler : IUnityService
    {
        #region App
        Task<ExecutionResponse<SearchResults<App>>> Get(AppSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic);
        Task<ExecutionResponse<App>> Register(App app, RequestContext requestContext);
        Task<ExecutionResponse<App>> Edit(App app, RequestContext requestContext);
        Task<ExecutionResponse<bool>> UnregisterApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> UnregisterApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(App App, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(AppSearchCriteria criteria, RequestContext requestContext);

        #endregion
        #region Privilege
        Task<ExecutionResponse<SearchResults<Privilege>>> Get(PrivilegeSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic);
        Task<ExecutionResponse<bool>> IsExists(PrivilegeSearchCriteria criteria, RequestContext requestContext);

        #endregion
        #region Target
        Task<ExecutionResponse<SearchResults<Target>>> Get(TargetSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic);
        Task<ExecutionResponse<bool>> IsExists(TargetSearchCriteria criteria, RequestContext requestContext);

        #endregion
        #region Role
        Task<ExecutionResponse<SearchResults<Role>>> Get(RoleSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic);
        Task<ExecutionResponse<Role>> Create(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<Role>> Edit(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteRole(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateRole(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateRole(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteRole(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateRole(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateRole(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(RoleSearchCriteria criteria, RequestContext requestContext);
        #endregion
        #region Claim
        Task<ExecutionResponse<SearchResults<Claim>>> Get(ClaimSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic);
        Task<ExecutionResponse<Claim>> Create(Claim Claim, RequestContext requestContext);
        Task<ExecutionResponse<Claim>> Edit(Claim Claim, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteClaim(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteClaim(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Claim Claim, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(ClaimSearchCriteria criteria, RequestContext requestContext);
        #endregion
        #region Actor
        Task<ExecutionResponse<SearchResults<Actor>>> Get(ActorSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic);
        Task<ExecutionResponse<Actor>> Create(Actor Actor, RequestContext requestContext);
        Task<ExecutionResponse<Actor>> Edit(Actor Actor, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteActor(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateActor(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateActor(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteActor(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateActor(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateActor(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Actor Actor, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(ActorSearchCriteria criteria, RequestContext requestContext);
        #endregion
    }
}
