using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Personnel.Core.Contracts.Accounts;
using XCore.Services.Personnel.Core.Localization.Resources;
using XCore.Services.Personnel.Models.Accounts;

namespace XCore.Services.Personnel.Core.Validators
{
    public class OrganizationAccountValidator : AbstractModelValidator<OrganizationAccount>
    {
        #region cst.

        public OrganizationAccountValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new OrganizationAccountValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new OrganizationAccountValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new OrganizationAccountValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new OrganizationAccountValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new OrganizationAccountValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class OrganizationAccountValidatorContext : AbstractValidator<OrganizationAccount>
        {
            #region props.

            private Lazy<IOrganizationAccountHandler> OrganizationAccountHandler { get; set; }

            #endregion
            #region cst.

            public OrganizationAccountValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.OrganizationAccountHandler = new Lazy<IOrganizationAccountHandler>(() => services.GetService(typeof(IOrganizationAccountHandler)) as IOrganizationAccountHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Account_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueAccount).WithMessage(ValidationResources.Account_Error_Duplicate);
                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueAccount).WithMessage(ValidationResources.Account_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAccountExistsActive).WithMessage(ValidationResources.Account_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAccountExists).WithMessage(ValidationResources.Account_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAccountExists).WithMessage(ValidationResources.Account_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Account_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Account_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueAccount(OrganizationAccount model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.OrganizationAccountHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAccountExists(OrganizationAccount model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.OrganizationAccountHandler.Value.IsExists(new OrganizationAccountSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAccountExistsActive(OrganizationAccount model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.OrganizationAccountHandler.Value.IsExists(new OrganizationAccountSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = true,
                }, SystemRequestContext.Instance);

                return check.Result;
            }

            #endregion
        }

        #endregion
    }
}
