using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Personnel.Core.Contracts.Organizations;
using XCore.Services.Personnel.Core.Localization.Resources;
using XCore.Services.Personnel.Models.Organizations;

namespace XCore.Services.Personnel.Core.Validators
{
    public class OrganizationValidator : AbstractModelValidator<Organization>
    {
        #region cst.

        public OrganizationValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new OrganizationValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new OrganizationValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new OrganizationValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new OrganizationValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new OrganizationValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class OrganizationValidatorContext : AbstractValidator<Organization>
        {
            #region props.

            private Lazy<IOrganizationHandler> OrganizationHandler { get; set; }

            #endregion
            #region cst.

            public OrganizationValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.OrganizationHandler = new Lazy<IOrganizationHandler>(() => services.GetService(typeof(IOrganizationHandler)) as IOrganizationHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Organization_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Organization_Error_Duplicate);
                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Organization_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(ValidationResources.Organization_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Organization_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Organization_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Organization_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Organization_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueApp(Organization model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.OrganizationHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAppExists(Organization model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.OrganizationHandler.Value.IsExists(new OrganizationSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(Organization model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.OrganizationHandler.Value.IsExists(new OrganizationSearchCriteria()
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
