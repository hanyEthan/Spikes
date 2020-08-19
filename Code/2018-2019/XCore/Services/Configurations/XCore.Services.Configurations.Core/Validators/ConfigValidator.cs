using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Configurations.Core.Contracts;
using XCore.Services.Configurations.Core.Localization.Resources;
using XCore.Services.Configurations.Core.Models.Domain;

namespace XCore.Services.Configurations.Core.Validators
{
    public class ConfigValidator: AbstractModelValidator<ConfigItem>
    {
        #region cst.

        public ConfigValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new ConfigValidatorsContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new ConfigValidatorsContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new ConfigValidatorsContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new ConfigValidatorsContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new ConfigValidatorsContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class ConfigValidatorsContext : AbstractValidator<ConfigItem>
        {
            #region props.

            private Lazy<IConfigHandler> ConfigHandler { get; set; }

            #endregion
            #region cst.

            public ConfigValidatorsContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.ConfigHandler = new Lazy<IConfigHandler>(() => services.GetService(typeof(IConfigHandler)) as IConfigHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Config_Id_NotEmpty);
                    RuleFor(x => x).Must(IsValidKey).WithMessage(ValidationResources.Config_Key_NotValid);
                    RuleFor(x => x).MustAsync(IsUniqueConfig).WithMessage(ValidationResources.Config_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                    RuleFor(x => x).MustAsync(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(ValidationResources.Error_ConfigEdit_Id_Empty);
                    RuleFor(x => x).Must(IsValidKey).WithMessage(ValidationResources.Config_Key_NotValid);
                    RuleFor(x => x).MustAsync(IsUniqueConfig).WithMessage(ValidationResources.Config_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckConfigExistsActive).WithMessage(ValidationResources.Config_Error_NotExists);
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                    RuleFor(x => x).MustAsync(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckConfigExists).WithMessage(ValidationResources.Config_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckConfigExists).WithMessage(ValidationResources.Config_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Config_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Config_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueConfig(ConfigItem model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckConfigExists(ConfigItem model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsExists(new Models.Support.ConfigSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);
                
                return check.Result;
            }
            private async Task<bool> CheckAppExists(ConfigItem model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsExists(new Models.Support.AppSearchCriteria() { Id = model.AppId }, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckModuleExists(ConfigItem model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsExists(new Models.Support.ModuleSearchCriteria() { Id = model.ModuleId }, SystemRequestContext.Instance);
                return check.Result;
            }
            private bool IsValidKey(ConfigItem model)
            {
                if (model == null) return true;  // skip
                return !string.IsNullOrWhiteSpace(model.Key);
            }
            private async Task<bool> CheckConfigExistsActive(ConfigItem model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsExists(new Models.Support.ConfigSearchCriteria()
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
