using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Localization.Resources;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Validators
{
    public class EventValidator : AbstractModelValidator<Event>
    {
        #region cst.

        public EventValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new EventValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new EventValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new EventValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new EventValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new EventValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion
        #region nested.

        protected class EventValidatorContext : AbstractValidator<Event>
        {
            #region props.

            private Lazy<IEventHandler> EventHandler { get; set; }

            #endregion
            #region cst.

            public EventValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.EventHandler = new Lazy<IEventHandler>(() => services.GetService(typeof(IEventHandler)) as IEventHandler);

                #endregion

                switch (mode)
                {
                    case ValidationMode.Create:
                        {
                            HandleCommon();
                            HandleCreate();
                        }
                        break;
                    case ValidationMode.Edit:
                        {
                            HandleCommon();
                            HandleEdit();
                        }
                        break;
                    case ValidationMode.Delete:
                        {
                            HandleCommon();
                            HandleDelete();
                        }
                        break;
                    case ValidationMode.Activate:
                    case ValidationMode.Deactivate:
                        {
                            HandleCommon();
                            HandleActivation();
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion
            #region Helpers.

            private void HandleCreate()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Event_Id_NotEmpty);

                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueEvent).WithMessage(ValidationResources.Event_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(ValidationResources.Event_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppEvent).WithMessage(ValidationResources.Event_Error_NotExists );
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppEvent).WithMessage(ValidationResources.Event_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Event_Code_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Event_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueEvent(Event model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.EventHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAppEvent(Event model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.EventHandler.Value.IsExists(new EventSearchCriteria ()
                {
                    Ids = Map(model.Id),
                    //Code = model.Code,
                    //IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(Event model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip
                var check = await this.EventHandler.Value.IsExists(new EventSearchCriteria()
                {
                    Ids = Map(model.Id),
                    Code = model.Code,
                    IsActive = true,
                }, SystemRequestContext.Instance);

                return check.Result;
            }

            #endregion
        }

        #endregion
        #region Helper
     internal static List<int?> Map(int? id)
        {
            if (id == null) return null;
            List<int?> Ret = new List<int?>();
            Ret.Add(id);
            return Ret;
        }
        #endregion


    }
}
