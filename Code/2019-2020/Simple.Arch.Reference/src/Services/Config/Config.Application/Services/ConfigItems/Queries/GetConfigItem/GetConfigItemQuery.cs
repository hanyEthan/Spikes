using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Domain.Entities;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.GetConfigItem
{
    public class GetConfigItemQuery : BaseRequestContext, MediatR.IRequest<BaseResponseContext<ConfigItem>>
    {
        public int ModuleId { get; set; }
        public string Key { get; set; }
    }
}
