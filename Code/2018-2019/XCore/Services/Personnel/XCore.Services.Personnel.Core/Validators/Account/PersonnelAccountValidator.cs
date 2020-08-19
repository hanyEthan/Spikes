using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Personnel.Core.Contracts.Accounts;
using XCore.Services.Personnel.Core.Handlers.Accounts;
using XCore.Services.Personnel.Core.Localization.Resources;
using XCore.Services.Personnel.Models.Accounts;

namespace XCore.Services.Personnel.Core.Validators
{
    public class PersonnelAccountValidator : AbstractModelValidator<PersonnelAccount>
    {
        #region cst.

        public PersonnelAccountValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new PersonnelAccountValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new PersonnelAccountValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new PersonnelAccountValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new PersonnelAccountValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new PersonnelAccountValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class PersonnelAccountValidatorContext : AbstractValidator<PersonnelAccount>
        {
            #region props.

            private Lazy<IPersonnelAccountHandler> PersonnelAccountHandler { get; set; }

            #endregion
            #region cst.

            public PersonnelAccountValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.PersonnelAccountHandler = new Lazy<IPersonnelAccountHandler>(() => services.GetService(typeof(IPersonnelAccountHandler)) as IPersonnelAccountHandler);

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

            private async Task<bool> IsUniqueAccount(PersonnelAccount model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.PersonnelAccountHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAccountExists(PersonnelAccount model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.PersonnelAccountHandler.Value.IsExists(new PersonnelAccountSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAccountExistsActive(PersonnelAccount model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.PersonnelAccountHandler.Value.IsExists(new PersonnelAccountSearchCriteria()
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
