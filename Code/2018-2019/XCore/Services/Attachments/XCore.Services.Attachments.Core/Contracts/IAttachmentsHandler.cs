using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Support;

namespace XCore.Services.Attachments.Core.Contracts
{
    public interface IAttachmentsHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Attachment>>> Get(AttachmentSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Attachment>> Create(Attachment Attachment, RequestContext requestContext);
        Task<ExecutionResponse<List<Attachment>>> CreateConfirm(List<Attachment> Attachment, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteListConfirm(List<string> id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteSoft(string id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(AttachmentSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Attachment attachment, RequestContext requestContext);

    }
}
