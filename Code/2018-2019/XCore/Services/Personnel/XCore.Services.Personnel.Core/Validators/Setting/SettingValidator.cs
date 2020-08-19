using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Personnel.Core.Contracts.Settings;
using XCore.Services.Personnel.Core.Localization.Resources;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.Core.Validators
{
    public class SettingValidator : AbstractModelValidator<Setting>
    {
        #region cst.

        public SettingValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new SettingValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new SettingValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new SettingValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new SettingValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new SettingValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class SettingValidatorContext : AbstractValidator<Setting>
        {
            #region props.

            private Lazy<ISettingHandler> SettingHandler { get; set; }

            #endregion
            #region cst.

            public SettingValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.SettingHandler = new Lazy<ISettingHandler>(() => services.GetService(typeof(ISettingHandler)) as ISettingHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Setting_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Setting_Error_Duplicate);
                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Setting_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(ValidationResources.Setting_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Setting_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Setting_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Setting_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Setting_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueApp(Setting model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.SettingHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAppExists(Setting model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.SettingHandler.Value.IsExists(new SettingSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(Setting model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.SettingHandler.Value.IsExists(new SettingSearchCriteria()
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
