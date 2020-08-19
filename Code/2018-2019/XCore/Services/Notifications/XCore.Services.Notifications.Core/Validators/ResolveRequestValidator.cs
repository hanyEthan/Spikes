using FluentValidation;
using FluentValidation.Validators;
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
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.Validators
{
    public class ResolveRequestValidator : AbstractModelValidator<ResolveRequest>
    {
        #region cst.

        public ResolveRequestValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }
        #endregion
        #region helpers
        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new ResolveRequestValidatorContext(services, ValidationMode.Create));
        }
        #endregion
        #region nested.

        protected class ResolveRequestValidatorContext : AbstractValidator<ResolveRequest>
        {
            #region props.

            private Lazy<IMessageTemplatesHandler> BL { get; set; }

            #endregion
            #region cst.

            public ResolveRequestValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.BL = new Lazy<IMessageTemplatesHandler>(() => services.GetService(typeof(IMessageTemplatesHandler)) as IMessageTemplatesHandler);

                #endregion

                switch (mode)
                {
                    case ValidationMode.Create:
                        {
                            RuleFor(x => x).NotEmpty().WithMessage(NotificationsValidationResource.Error_ResolveRequest_Empty);
                            When(x => x != null, () =>
                            {
                                RuleFor(x => x.RequestId).NotEmpty().WithMessage(NotificationsValidationResource.Error_ResolveRequest_RequestId_Empty);
                                RuleFor(x => x.MessageTemplateId).NotEmpty().WithMessage(NotificationsValidationResource.Error_ResolveRequest_MessageTemplateId_Empty);
                                
                                RuleFor(x => x.RequestId).MustAsync(CheckMessageTemplateRequest).WithMessage("{ValidationMessage}");
                                RuleFor(x => x).Must(CheckValidKeys).WithMessage(NotificationsValidationResource.Error_ResolveRequest_Key_Invalid);
                            });
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion

            #region Helpers.

            private async Task<bool> CheckMessageTemplateRequest(ResolveRequest model, string requestId, PropertyValidatorContext context, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var messageTemplate = await GetMessageTemplate(model.MessageTemplateId);

                var isValid = true;

                isValid = CheckMessageTemplateId(messageTemplate, context) && isValid;
                isValid = CheckMissingKeys(model, messageTemplate, context) && isValid;
                isValid = CheckExtraKeys(model, messageTemplate, context) && isValid;

                return isValid;
            }
            private bool CheckValidKeys(ResolveRequest model)
            {
                if (model == null) return true;  // skip

                var check = model.Values?.Any(x => string.IsNullOrWhiteSpace(x.Value));

                return !check.GetValueOrDefault();
            }

            #endregion
            #region helpers.

            private async Task<MessageTemplate> GetMessageTemplate(int id)
            {
                var result = await this.BL.Value.Get(new MessageTemplateSearchCriteria()
                {
                    Id = id,
                    IsActive = true,
                }, SystemRequestContext.Instance);

                return result?.Result?.Results?.FirstOrDefault();
            }
            private bool CheckMessageTemplateId(MessageTemplate messageTemplate, PropertyValidatorContext context)
            {
                var isValid = messageTemplate != null;
                if (!isValid)
                {
                    context.MessageFormatter.AppendArgument("ValidationMessage", NotificationsValidationResource.Error_MessageTemplate_NotFound);
                    
                }

                return isValid;
            }
            private bool CheckMissingKeys(ResolveRequest resolveModel, MessageTemplate messageTemplate, PropertyValidatorContext context)
            {
                if (resolveModel == null) return false;
                if (messageTemplate == null) return false;

                var matched = messageTemplate.Keys?.All(x => (resolveModel.Values?.Any(y => y.Key?.ToLower() == x.Key?.ToLower())).GetValueOrDefault()) ?? true;
                if (!matched)
                {
                    context.MessageFormatter.AppendArgument("ValidationMessage", NotificationsValidationResource.Error_ResolveRequest_MissingKeys);
                }

                return matched;
            }
            private bool CheckExtraKeys(ResolveRequest resolveModel, MessageTemplate messageTemplate, PropertyValidatorContext context)
            {
                if (resolveModel == null) return false;
                if (messageTemplate == null) return false;

                var matched = resolveModel.Values?.All(x => (messageTemplate.Keys?.Any(y => y.Key?.ToLower() == x.Key?.ToLower())).GetValueOrDefault()) ?? true;
                if (!matched)
                {
                    context.MessageFormatter.AppendArgument("ValidationMessage", NotificationsValidationResource.Error_ResolveRequest_ExtraKeys);
                }

                return matched;
            }

            #endregion
        }

        #endregion
    }
}
