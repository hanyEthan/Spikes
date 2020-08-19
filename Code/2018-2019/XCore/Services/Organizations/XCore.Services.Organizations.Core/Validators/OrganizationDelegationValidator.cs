using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Localization.Resources;
using XCore.Services.Organizations.Core.Models.Domain;


namespace XCore.Services.Organizations.Core.Validators
{
    public class OrganizationDelegationValidator : AbstractModelValidator<OrganizationDelegation>
    {
        #region cst.

        public OrganizationDelegationValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new OrganizationDelegationValidatorContext(services,ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new OrganizationDelegationValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new OrganizationDelegationValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new OrganizationDelegationValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new OrganizationDelegationValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class OrganizationDelegationValidatorContext : AbstractValidator<OrganizationDelegation>
        {

            #region props.

            private Lazy<IOrganizationDelegationHandler> OrganizationDelegationHandler { get; set; }

            #endregion
            #region cst.

            public OrganizationDelegationValidatorContext(IServiceProvider services,ValidationMode mode)
            {
                #region init.

                this.OrganizationDelegationHandler = new Lazy<IOrganizationDelegationHandler>(() => services.GetService(typeof(IOrganizationDelegationHandler)) as IOrganizationDelegationHandler);

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
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(ValidationResources.Error_ContactInfo_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueOrganizationDelegation).WithMessage(ValidationResources.ContactInfo_Error_Duplicate);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueOrganizationDelegation).WithMessage(ValidationResources.ContactInfo_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckOrganizationDelegationExistsActive).WithMessage(ValidationResources.ContactInfo_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckOrganizationDelegationExists).WithMessage(ValidationResources.ContactInfo_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckOrganizationDelegationExists).WithMessage(ValidationResources.ContactInfo_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.ContactInfo_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.ContactInfo_Code_Error_Empty);
                });
            }

            private async Task< bool> IsUniqueOrganizationDelegation(OrganizationDelegation model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check=await this.OrganizationDelegationHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task< bool> CheckOrganizationDelegationExists(OrganizationDelegation model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var res=await this.OrganizationDelegationHandler.Value.IsExists(new Models.Support.OrganizationDelegationSearchCriteria()
                {
                    DelegateId = model.DelegateId,
                    DelegatorId=model.DelegatorId,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);
                return res.Result;

            }
            private async Task< bool> CheckOrganizationDelegationExistsActive(OrganizationDelegation model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var res=await this.OrganizationDelegationHandler.Value.IsExists(new Models.Support.OrganizationDelegationSearchCriteria()
                {
                    DelegateId = model.DelegateId,
                    DelegatorId=model.DelegatorId,
                    Code = model.Code,
                    IsActive = true,
                }, SystemRequestContext.Instance);
                return res.Result;
            }

            #endregion
        }

        #endregion
    }
}
