using MassTransit;
using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.IntegrationModels.Person;
using XCore.Services.Personnel.Models.DTO.Personnels;
using XCore.Services.Personnel.SDK.Contracts;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.API.Events.Consumers
{
    public class SecurityMessageConsumer : IConsumer<IPersonCreatedIntegrationEvent>, 
                                           IConsumer<IPersonUpdatedIntegrationEvent>,
                                           IConsumer<IPersonDeletedIntegrationEvent>, 
                                           IConsumer<IPersonDeActivatedIntegrationEvent>,
                                           IConsumer<IPersonActivatedIntegrationEvent>
    {
        #region props.

        private readonly IActorHandler ActorHandler;
        private readonly IPersonnelClient PersonnelService;
        public bool? Initialized { get; protected set; }



        #endregion
        #region cst.

        public SecurityMessageConsumer(IActorHandler actorHandler, IPersonnelClient personnelService)
        {
            this.ActorHandler = actorHandler;
            this.PersonnelService = personnelService;
            this.Initialized = Initialize();
        }

        #endregion
        #region MassTransit.IConsumer
        public async Task Consume(ConsumeContext<IPersonActivatedIntegrationEvent> context)
        {

            #region Bl.
            if (context?.Message != null && !string.IsNullOrEmpty(context.Message.Code))
            {
                var domainResponse = await this.ActorHandler.ActivateActor(context.Message.Code, new Framework.Infrastructure.Context.Execution.Support.RequestContext());
                if (domainResponse.State != Framework.Infrastructure.Context.Execution.Models.ResponseState.Success)
                {
                    throw new Exception($"Event handling for person {context.Message.App}:{context.Message.Module}:{context.Message.Code}");
                }
            }
            else
            {
                throw new Exception($" {context.Message} is null or {context.Message.Code} is null or empty");

            }


            #endregion
        }
        public async Task Consume(ConsumeContext<IPersonDeActivatedIntegrationEvent> context)
        {

            #region Bl.
            if (context?.Message != null && !string.IsNullOrEmpty(context.Message.Code))
            {
                var domainResponse = await this.ActorHandler.DeactivateActor(context.Message.Code, new Framework.Infrastructure.Context.Execution.Support.RequestContext());
                if (domainResponse.State != Framework.Infrastructure.Context.Execution.Models.ResponseState.Success)
                {
                    throw new Exception($"Event handling for person {context.Message.App}:{context.Message.Module}:{context.Message.Code}");
                }
            }
            else
            {
                throw new Exception($" {context.Message} is null or {context.Message.Code} is null or empty");

            }


            #endregion
        }
        public async Task Consume(ConsumeContext<IPersonDeletedIntegrationEvent> context)
        {

            #region Bl.
            if (context.Message != null && !string.IsNullOrEmpty(context.Message.Code))
            {
                var domainResponse = await this.ActorHandler.DeleteActor(context.Message.Code, new Framework.Infrastructure.Context.Execution.Support.RequestContext());
                if (domainResponse?.State != Framework.Infrastructure.Context.Execution.Models.ResponseState.Success)
                {
                    throw new Exception($"Event handling for person {context.Message.App}:{context.Message.Module}:{context.Message.Code}");
                }
            }
            else
            {
                throw new Exception($" {context.Message} is null or {context.Message.Code} is null or empty");

            }


            #endregion
        }
        public async Task Consume(ConsumeContext<IPersonUpdatedIntegrationEvent> context)
        {
            #region personnel service.
            var personnelSearchCriteria = new PersonnelSearchCriteriaDTO() { Id = context.Message.PersonId };
            var response = await this.PersonnelService.Get(new ServiceExecutionRequestDTO<PersonnelSearchCriteriaDTO>()
            {
                RequestClientToken = "634a5d14-81c1-4c9b-9484-0b3c21bb2299",
                RequestUserCode = null,
                RequestSessionCode = null,
                RequestCorrelationCode = "4557d16f-3dd0-4bfc-a913-70db20e43e08",
                RequestMetadata = null,
                RequestTime = DateTime.Now.ToString(XCoreConstants.DateTimeFormat),
                RequestCulture = "en-US",
                RequestAppId = context.Message.App,
                RequestModuleId = context.Message.Module,
                Content = personnelSearchCriteria
            });


            var actor = response?.Response?.Content?.Results?.Count > 0 ? Map(response?.Response?.Content?.Results[0]) : null;

            #endregion
            #region request.

            //bool isValidRequest;
            //ServiceExecutionRequestDTO<Actor> requestDTO = Map(context);
            //ServiceExecutionRequest<Actor> requestDMN;
            //ServiceExecutionResponse<Actor> responseDMN;
            //ServiceExecutionResponseDTO<bool> responseDTO;

            // ...
            //responseDTO = ServiceExecutionContext.HandleRequestDTO<ActorDTO, Actor, bool, Actor>(requestDTO, ActorMapper.Instance, out requestDMN, out isValidRequest, httpRequest: null);

            #endregion
            #region Bl.

            var domainResponse = await this.ActorHandler.Edit(actor, new Framework.Infrastructure.Context.Execution.Support.RequestContext());
            if (domainResponse.State != Framework.Infrastructure.Context.Execution.Models.ResponseState.Success)
            {
                throw new Exception($"Event handling for person {context.Message.App}:{context.Message.Module}:{context.Message.PersonId}");
            }

            #endregion
        }
        public async Task Consume(ConsumeContext<IPersonCreatedIntegrationEvent> context)
        {
            #region personnel service.
            var personnelSearchCriteria = new PersonnelSearchCriteriaDTO() { Id = context.Message.PersonId };
            var response = await this.PersonnelService.Get(new ServiceExecutionRequestDTO<PersonnelSearchCriteriaDTO>()
            {
                RequestClientToken = "634a5d14-81c1-4c9b-9484-0b3c21bb2299",
                RequestUserCode = null,
                RequestSessionCode = null,
                RequestCorrelationCode = "4557d16f-3dd0-4bfc-a913-70db20e43e08",
                RequestMetadata = null,
                RequestTime = DateTime.Now.ToString(),
                RequestCulture = "en-US",
                RequestAppId = context.Message.App,
                RequestModuleId = context.Message.Module,
                Content = personnelSearchCriteria
            });


            var actor = response?.Response?.Content?.Results?.Count > 0 ? Map(response?.Response?.Content?.Results[0]) : null;


            // ...

            #endregion
            #region request.

            //bool isValidRequest;
            //ServiceExecutionRequestDTO<Actor> requestDTO = Map(context);
            //ServiceExecutionRequest<Actor> requestDMN;
            //ServiceExecutionResponse<Actor> responseDMN;
            //ServiceExecutionResponseDTO<bool> responseDTO;

            // ...
            //responseDTO = ServiceExecutionContext.HandleRequestDTO<ActorDTO, Actor, bool, Actor>(requestDTO, ActorMapper.Instance, out requestDMN, out isValidRequest, httpRequest: null);

            #endregion
            #region Bl.

            var domainResponse = await this.ActorHandler.Create(actor, new Framework.Infrastructure.Context.Execution.Support.RequestContext());
            if (domainResponse.State != Framework.Infrastructure.Context.Execution.Models.ResponseState.Success)
            {
                throw new Exception($"Event handling for person {context.Message.App}:{context.Message.Module}:{context.Message.PersonId}");
            }

            #endregion
        }

        #endregion
        #region helpers.
        private Actor Map(PersonnelDTO from)
        {

            var actor = new Actor()
            {
                AppId = Convert.ToInt32(from.AppId),
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat),
                IsActive = from.IsActive.HasValue ? from.IsActive.Value : false,
                MetaData = from.MetaData,
                Name = from.Name,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat),
                NameCultured = from.NameCultured,
                ModifiedBy = from.ModifiedBy,
            };
          

            return actor;


        }
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.ActorHandler?.Initialized ?? false) &&
                (this.PersonnelService?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}
