using System.Threading.Tasks;

namespace XCore.Services.Attachments.Core.DataLayer.Contracts
{
    public interface IAttachmentDataUnity
    {
        bool? Initialized { get; }
        IAttachmentRepository Attachments { get; }
        Task SaveAsync();
    }
}