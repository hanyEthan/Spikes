using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Notifications.Core.Contracts;
using XCore.Services.Notifications.Core.Localization.Resources;
using XCore.Services.Notifications.Core.Models.Domain;

namespace XCore.Services.Notifications.Core.Validators
{
    public class MessageTemplateValidator : AbstractModelValidator<MessageTemplate>
    {
        #region cst.

        public MessageTemplateValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }
        #endregion
        #region helpers
        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new NotificationsValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new NotificationsValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new NotificationsValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new NotificationsValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new NotificationsValidatorContext(services, ValidationMode.Deactivate));
        }
        #endregion
        #region nested.

        protected class NotificationsValidatorContext : AbstractValidator<MessageTemplate>
        {
            #region props.

            private Lazy<IMessageTemplatesHandler> BL { get; set; }

            #endregion

            #region cst.

            public NotificationsValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.BL = new Lazy<IMessageTemplatesHandler>(() => services.GetService(typeof(IMessageTemplatesHandler)) as IMessageTemplatesHandler);

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
                        {
                            HandleActivation();
                        }
                        break;
                    case ValidationMode.Deactivate:
                        {
                            HandleDeactivation();
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
                    RuleFor(x => x.Body).NotEmpty().WithMessage(NotificationsValidationResource.Error_MessageTemplate_Body_Empty);
                    RuleFor(x => x).MustAsync(IsUniqueEntity).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Duplicate);
                    RuleFor(x => x).Must(CheckValidKeys).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Key_Invalid);
                    RuleFor(x => x).Must(CheckValidAttachments).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Attachment_Invalid);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueEntity).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Duplicate);
                    RuleFor(x => x.Body).NotEmpty().WithMessage(NotificationsValidationResource.Error_MessageTemplate_Body_Empty);
                    RuleFor(x => x).MustAsync(CheckEntityExistsActive).WithMessage(NotificationsValidationResource.Error_MessageTemplate_NotExists);
                    RuleFor(x => x).Must(CheckValidKeys).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Key_Invalid);
                    RuleFor(x => x).Must(CheckValidAttachments).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Attachment_Invalid);
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
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckEntityExists).WithMessage(NotificationsValidationResource.Error_MessageTemplate_NotExists);
                    RuleFor(x => x).MustAsync(IsUniqueEntity).WithMessage(NotificationsValidationResource.Error_MessageTemplate_Duplicate);
                });
            }
            private void HandleDeactivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckEntityExists).WithMessage(NotificationsValidationResource.Error_MessageTemplate_NotExists);
                });
            }

            private async Task<bool> IsUniqueEntity(MessageTemplate model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip
                var check = await this.BL.Value.IsUnique(model, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckEntityExistsActive(MessageTemplate model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.BL.Value.IsExists(new Models.Support.MessageTemplateSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = true,

                }, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckEntityExists(MessageTemplate model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.BL.Value.IsExists(new Models.Support.MessageTemplateSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,

                }, SystemRequestContext.Instance);
                return check.Result;
            }
            private bool CheckValidKeys(MessageTemplate model)
            {
                if (model == null) return true;  // skip
                if (model.Keys == null) return true;  // skip
                if (model.Keys.Count == 0) return true;  // skip

                var invalidKeyIncluded = model.Keys.Any(KeyItem => string.IsNullOrWhiteSpace(KeyItem.Key));

                return !invalidKeyIncluded;
            }
            private bool CheckValidAttachments(MessageTemplate model)
            {
                if (model == null) return true;  // skip
                if (model.Attachments == null) return true;  // skip
                if (model.Attachments.Count == 0) return true;  // skip

                var invalidattachmentIncluded = model.Attachments.Any(att => string.IsNullOrWhiteSpace(att.AttachmentReferenceId));

                return !invalidattachmentIncluded;
            }

            #endregion
        }

        #endregion
    }
}
