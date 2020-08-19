using Mcs.Invoicing.Services.Config.Domain.Support;

namespace Mcs.Invoicing.Services.Config.Application.Services.Modules.Queries.AnyModules
{
    public class AnyModulesQuery : ModulesSearchCriteria, MediatR.IRequest<bool>
    {
    }
}
