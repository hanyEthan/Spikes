using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Services.Audit.Core.Contracts;

namespace XCore.Services.Audit.API.Events.Publishers
{
    public class EventsPublisher : IAuditEventsPublisher
    {
        #region props.

        public bool? Initialized { get; protected set; }

        protected IServiceBus SB { get; set; }

        #endregion
        #region cst.

        public EventsPublisher(IServiceBus sB)
        {
            this.SB = sB;

            this.Initialized = this.Initialize();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.SB?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("Events Publisher : not initialized correctly.");
            }
        }

        #endregion
        #region Events Handlers.


        #endregion
    }
}
