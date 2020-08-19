using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Contracts
{
    public interface IVenueHandler : IUnityService
    {
        #region Venue
        Task<ExecutionResponse<SearchResults<Venue>>> Get(VenueSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Venue>> Create(Venue Venue, RequestContext requestContext);
        Task<ExecutionResponse<Venue>> Edit(Venue Venue, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Venue Venue, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteVenue(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteVenue(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateVenue(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateVenue(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateVenue(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateVenue(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(VenueSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Venue venue, RequestContext requestContext);

        #endregion
       

    }
}



