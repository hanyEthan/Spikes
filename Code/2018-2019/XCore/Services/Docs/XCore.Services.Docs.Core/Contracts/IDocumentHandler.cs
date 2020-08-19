using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.Core.Contracts
{
    public interface IDocumentHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Document>>> Get(DocumentSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Document>> Create(Document Document, RequestContext requestContext);
        Task<ExecutionResponse<List<Document>>> Create(List<Document> documents, RequestContext requestContext);
        Task<ExecutionResponse<Document>> Edit(Document Document, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int Id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(List<int> Id, RequestContext requestContext);
    }
}
