using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Configurations.Core.Models.Events.Domain
{
    public class ConfigEditedDomainEvent : DomainEventBase, MediatR.INotification
    {
        #region props.

        public string ConfigCode { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }

        #endregion
        #region cst.

        public ConfigEditedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Config";
            base.Action = "Edited";
            base.User = null;
        }

        #endregion
    }
}
