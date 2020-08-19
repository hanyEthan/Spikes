using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Configurations.Core.Models.Events.Domain
{
    public class ConfigEditingDomainEvent : DomainEventBase, MediatR.IRequest<ExecutionResponse<bool>>
    {
        #region props.

        public string ConfigCode { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }

        #endregion
        #region cst.

        public ConfigEditingDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Config";
            base.Action = "Editing";
            base.User = null;
        }

        #endregion
    }
}
