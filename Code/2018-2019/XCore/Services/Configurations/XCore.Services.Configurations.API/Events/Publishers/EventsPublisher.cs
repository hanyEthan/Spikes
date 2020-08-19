using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Services.Configurations.Core.Models.Events.Domain;

namespace XCore.Services.Configurations.API.Events.Publishers
{
    public class EventsPublisher : MediatR.INotificationHandler<ConfigEditedDomainEvent>,
                                   MediatR.IRequestHandler<ConfigEditingDomainEvent, ExecutionResponse<bool>>
    {
        #region props.

        public bool? Initialized { get; protected set; }
        //protected IServiceBus SB { get; set; }

        #endregion
        #region cst.

        public EventsPublisher(/*IServiceBus sB*/)
        {
            //this.SB = sB;

            this.Initialized = this.Initialize();
        }

        #endregion

        #region Event : ConfigEditedDomainEvent.

        public async Task Handle(ConfigEditedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            try
            {
                Check();

                //await SB.Publish<IRoleCreatedIntegrationEvent, RoleCreatedIntegrationEvent>(new RoleCreatedIntegrationEvent()
                //{
                //    AppId = Event.AppId,
                //    RoleId = Event.RoleId,
                //});
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }

        #endregion
        #region Event : ConfigEditingDomainEvent.

        public async Task<ExecutionResponse<bool>> Handle(ConfigEditingDomainEvent request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

            return new ExecutionResponse<bool>()
            {
                Result = true,
                State = ResponseState.Success,
            };
        }

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            //isValid = isValid && (this.SB?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("component is not initialized correctly.");
            }
        }

        #endregion
    }
}
