using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Attachments.Core.Contracts;
using XCore.Services.Attachments.Core.Localization.Resources;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Support;
using System.Linq;
using System.Collections.Generic;

namespace XCore.Services.Attachments.Core.Validators
{
    public class AttachmentValidator : AbstractModelValidator<Models.Attachment>
    {
        #region cst.

        public AttachmentValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new AttachmentTrailValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new AttachmentTrailValidatorContext(services, ValidationMode.Delete));
        }

        #endregion

        #region nested.

        protected class AttachmentTrailValidatorContext : AbstractValidator<Models.Attachment>
        {
            #region props.

            private Lazy<IAttachmentsHandler> AttachmentsHandler { get; set; }

            #endregion
            #region cst.

            public AttachmentTrailValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.AttachmentsHandler = new Lazy<IAttachmentsHandler>(() => services.GetService(typeof(IAttachmentsHandler)) as IAttachmentsHandler);

                #endregion

                switch (mode)
                {
                    case ValidationMode.Create:
                        {
                            HandleCreate();
                            HandleCommon();
                            break;
                        }
                    case ValidationMode.Delete:
                        {
                            HandleAttachmentExists();
                            HandleCommon();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            #endregion
            #region Helpers.

            private void HandleCommon()
            {
                RuleFor(x => x).NotNull().WithMessage(ValidationResources.Error_Attachment_Null);
            }
            private void HandleCreate()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationResources.Error_Attachment_Id_NotEmpty);
                    RuleFor(x => x.Body).NotEmpty().WithMessage(ValidationResources.Error_Attachment_Body_Empty);
                    RuleFor(x => x).MustAsync(IsUniqueAttachment).WithMessage(ValidationResources.Attachment_Error_Duplicate);

                });
            }
            private void HandleAttachmentExists()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAttachmentExists).WithMessage(ValidationResources.Attachment_Error_NotExists);
                });
            }
           
            private async Task<bool> CheckAttachmentExists(Attachment model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip
               
                var check = await this.AttachmentsHandler.Value.IsExists(
                    new AttachmentSearchCriteria()
                {
                    Id = new List<string>() { model.Id },
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> IsUniqueAttachment(Attachment model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.AttachmentsHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private bool ValidateCode(Models.Attachment model)
            {
                if (model == null) return true; // skip.
                return !string.IsNullOrWhiteSpace(model.Code);
            }

            #endregion
        }

        #endregion
    }
}
