using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Localization.Resources;

namespace XCore.Services.Security.Core.Validators
{
   public class ClaimsValidators : AbstractModelValidator<Claim>
    {
        #region cst.

        public ClaimsValidators(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new ClaimsValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new ClaimsValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new ClaimsValidatorContext(services, ValidationMode.Delete));
        }

        #endregion

        #region nested.

        protected class ClaimsValidatorContext : AbstractValidator<Claim>
        {
            #region props.

            private Lazy<IClaimHandler> ClaimHandler { get; set; }

            #endregion
            #region cst.

            public ClaimsValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.ClaimHandler = new Lazy<IClaimHandler>(() => services.GetService(typeof(IClaimHandler)) as IClaimHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(SecurityValidationResource.Error_Claim_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueClaim).WithMessage(SecurityValidationResource.Error_Claim_Duplicate);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueClaim).WithMessage(SecurityValidationResource.Error_Claim_Duplicate);
                    RuleFor(x => x).MustAsync(CheckClaimExistsActive).WithMessage(SecurityValidationResource.Error_Claim_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckClaimExists).WithMessage(SecurityValidationResource.Error_Claim_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(SecurityValidationResource.Error_Claim_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(SecurityValidationResource.Error_Claim_Code_Empty);
                });
            }

            private async Task<bool> IsUniqueClaim(Claim model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip
                var check = await this.ClaimHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckClaimExistsActive(Claim model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ClaimHandler.Value.IsExists(new Models.Support.ClaimSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = true,

                }, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckClaimExists(Claim model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ClaimHandler.Value.IsExists(new Models.Support.ClaimSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,

                }, SystemRequestContext.Instance);
                return check.Result;
            }
     

            #endregion
        }

        #endregion
    }
}
