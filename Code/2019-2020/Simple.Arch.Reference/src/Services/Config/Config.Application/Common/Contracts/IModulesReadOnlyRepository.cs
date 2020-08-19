using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Contracts;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;

namespace Mcs.Invoicing.Services.Config.Application.Common.Contracts
{
    public interface IModulesReadOnlyRepository : IRepositoryRead<Module>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(ModulesSearchCriteria criteria);
        Task<SearchResults<Module>> GetAsync(ModulesSearchCriteria criteria, string includeProperties = null);
    }
}
