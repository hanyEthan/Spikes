using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Contracts
{
   public interface IActorHandler : IUnityService
    {
        #region Actor
        Task<ExecutionResponse<SearchResults<Actor>>> Get(ActorSearchCriteria criteria, RequestContext requestContext);
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
