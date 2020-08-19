using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Notifications.Core.Contracts;
using XCore.Services.Notifications.Core.Localization.Resources;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.Validators
{
    public class InternalNotificationValidator : AbstractModelValidator<InternalNotification>
    {
        #region cst.

        public InternalNotificationValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }
        #endregion
        #region helpers
        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new InternalNotificationValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new InternalNotificationValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new InternalNotificationValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new InternalNotificationValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new InternalNotificationValidatorContext(services, ValidationMode.Deactivate));
        }
        #endregion
        #region nested.
        protected class InternalNotificationValidatorContext : AbstractValidator<InternalNotification>
        {

            #region props.

            private Lazy<IInternalNotificationHandler> BL { get; set; }

            #endregion
            #region cst.

            public InternalNotificationValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.BL = new Lazy<IInternalNotificationHandler>(() => services.GetService(typeof(IInternalNotificationHandler)) as IInternalNotificationHandler);

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
                            HandleMarkasRead();
                        }
                        break;
                    case ValidationMode.Delete:
                        {
                            HandleCommon();
                            HandleDelete();
                        }
                        break;
                    case ValidationMode.Activate:
                        {
                            HandleMarkasDismissed();
                        }
                        break;
                    case ValidationMode.Deactivate:
                        {
                            HandleMarkasDelete();
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion
            #region Helpers.
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(NotificationsValidationResource.Error_MessageTemplate_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(NotificationsValidationResource.Error_MessageTemplate_Code_Empty);
                });
            }
            private void HandleCreate()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).Equal(0).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Id_NotEmpty);
                    RuleFor(x => x.Content).NotEmpty().WithMessage(NotificationsValidationResource.Error_MessageTemplate_Body_Empty);
                    RuleFor(x => x.ActorId).NotEmpty().WithMessage(NotificationsValidationResource.Error_MessageTemplate_Body_Empty);
                    RuleFor(x => x.ActionId).NotEmpty().WithMessage(NotificationsValidationResource.Error_MessageTemplate_Body_Empty);
                });
            }
            private void HandleMarkasRead()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckEntityExists).WithMessage(NotificationsValidationResource.Error_MessageTemplate_NotExists);
              //   RuleFor(x => x).MustAsync(IsUniqueEntity).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Duplicate);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckEntityExists).WithMessage(NotificationsValidationResource.Error_MessageTemplate_NotExists);
                    //RuleFor(x => x).MustAsync(CheckAppRelationsExists).WithMessage(SecurityValidationResource.Error_App_Relations_Exist);
                });
            }
            private void HandleMarkasDismissed()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckEntityExists).WithMessage(NotificationsValidationResource.Error_MessageTemplate_NotExists);
                    //RuleFor(x => x).MustAsync(IsUniqueEntity).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Duplicate);
                });
            }
            private void HandleMarkasDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckEntityExists).WithMessage(NotificationsValidationResource.Error_MessageTemplate_NotExists);
                   // RuleFor(x => x).MustAsync(IsUniqueEntity).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Duplicate);
                });
            }

            private async Task<bool> IsUniqueEntity(InternalNotification model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip
                var check = await this.BL.Value.IsUnique(model, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckEntityExists(InternalNotification model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.BL.Value.IsExists(new InternalNotificationSearchCriteria()
                {
                    Id = new List<int?>(){ model.Id },
                    Code = new List<string>() { model.Code },
                    IsActive = true,

                }, SystemRequestContext.Instance);
                return check.Result;
            }
            #endregion
        }
        #endregion
    }
}
