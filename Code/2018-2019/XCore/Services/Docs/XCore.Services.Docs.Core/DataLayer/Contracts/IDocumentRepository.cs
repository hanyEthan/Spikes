using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.Core.DataLayer.Contracts
{
    public interface IDocumentRepository : IRepository<Document>
    {
        bool? Initialized { get; }
        Task<SearchResults<Document>> GetAsync(DocumentSearchCriteria criteria);
    }
}
