using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Support;

namespace XCore.Services.Attachments.Core.DataLayer.Contracts
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        bool? Initialized { get; }
        
        void CreateConfirm(List<Attachment> attachment);
        void DeleteSoft(Attachment attachment);
        Task<SearchResults<Attachment>> GetAsync(AttachmentSearchCriteria criteria);
        //void ConfirmStatus(Attachment attachment);
        //void ConfirmStatus(List<Attachment> attachment);
        Task<bool> AnyAsync(AttachmentSearchCriteria criteria);

    }
}
