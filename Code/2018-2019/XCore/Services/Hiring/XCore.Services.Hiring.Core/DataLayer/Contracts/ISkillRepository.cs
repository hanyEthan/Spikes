using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.DataLayer.Contracts
{
    public interface ISkillRepository : IRepository<Skill>
    {
        bool? Initialized { get; }
        Task<SearchResults<Skill>> Get(SkillsSearchCriteria criteria, string includeProperties = null);
        Task<bool> AnyAsync(SkillsSearchCriteria criteria);
    }
}