using System;
using FluentValidation;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Audit.Core.Contracts;
using XCore.Services.Audit.Core.Localization.Resources;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.Core.Validators
{
    public class AuditTrailValidator : AbstractModelValidator<AuditTrail>
    {
        #region cst.

        public AuditTrailValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new AuditTrailValidatorContext(services, ValidationMode.Create));
        }

        #endregion

        #region nested.

        protected class AuditTrailValidatorContext : AbstractValidator<AuditTrail>
        {
            #region props.

            private Lazy<IAuditHandler> AuditHandler { get; set; }

            #endregion
            #region cst.

            public AuditTrailValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.AuditHandler = new Lazy<IAuditHandler>(() => services.GetService(typeof(IAuditHandler)) as IAuditHandler);

                #endregion

                switch (mode)
                {
                    case ValidationMode.Create:
                        {
                            HandleCreate();
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
                RuleFor(x => x).NotNull().WithMessage(ValidationResources.Error_Audit_Null);
            }
            private void HandleCreate()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(ValidateCode).WithMessage(ValidationResources.Error_Audit_Code_Empty);
                });
            }

            private bool ValidateCode(AuditTrail model)
            {
                if (model == null) return true; // skip.
                return !string.IsNullOrWhiteSpace(model.Code);
            }

            #endregion
        }

        #endregion
    }
}
