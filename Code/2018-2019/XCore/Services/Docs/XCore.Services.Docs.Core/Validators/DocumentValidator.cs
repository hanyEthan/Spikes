using System;
using FluentValidation;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Docs.Core.Localization.Resources;
using XCore.Services.Docs.Core.Contracts;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.Core.Validators
{
    public class DocumentValidator : AbstractModelValidator<Document>
    {
        #region cst.

        public DocumentValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new DocumentValidatorContext(services,ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new DocumentValidatorContext(services,ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new DocumentValidatorContext(services,ValidationMode.Delete));

        }

        #endregion

        #region nested.

        protected class DocumentValidatorContext : AbstractValidator<Models.Document>
        {
            #region props.

            private Lazy<IDocumentHandler> DocumentHandler { get; set; }

            #endregion
            #region cst.

            public DocumentValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.DocumentHandler = new Lazy<IDocumentHandler>(() => services.GetService(typeof(IDocumentHandler)) as IDocumentHandler);

                #endregion
                switch (mode)
                {
                    case ValidationMode.Create:
                        {
                            HandleCreate();
                            HandleCommon();
                            break;
                        }
                    case ValidationMode.Edit:
                        {
                            HandleEdit();
                            break;
                        }
                    case ValidationMode.Delete:
                        {
                            HandleDelete();
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
                RuleFor(x => x).NotNull().WithMessage(ValidationResources.Error_Document_Null);
            }
            private void HandleCreate()
            {
                When(x => x != null, () =>
                {
                    //RuleFor(x => x).Must(ValidateCode).WithMessage(ValidationResources.Error_Document_Code_Empty);
                });
            }
            private void HandleEdit()
            {
            }
            private void HandleDelete()
            {
               
            }
            private bool ValidateCode(Models.Document model)
            {
                if (model == null) return true; // skip.
                return !string.IsNullOrWhiteSpace(model.Code);
            }
         
            #endregion
        }

        #endregion
    }
}
