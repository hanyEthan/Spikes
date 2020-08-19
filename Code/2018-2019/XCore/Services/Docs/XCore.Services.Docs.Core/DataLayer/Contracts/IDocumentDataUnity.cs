using System.Threading.Tasks;

namespace XCore.Services.Docs.Core.DataLayer.Contracts
{
    public interface IDocumentDataUnity
    {
        bool? Initialized { get; }
        IDocumentRepository Document { get; }

        Task SaveAsync();
    }
}