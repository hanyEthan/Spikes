using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Contracts
{
    public interface ICityHandler : IUnityService
    {
        
        #region City
        Task<ExecutionResponse<SearchResults<City>>> Get(CitySearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<City>> Create(City City, RequestContext requestContext);
        Task<ExecutionResponse<City>> Edit(City City, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(City City, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(CitySearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(City City, RequestContext requestContext);
        #endregion
       

    }
}



