using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem
{
    public class CreateConfigItemCommand : BaseRequestContext, MediatR.IRequest<BaseResponseContext<int>>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }
    }
}
