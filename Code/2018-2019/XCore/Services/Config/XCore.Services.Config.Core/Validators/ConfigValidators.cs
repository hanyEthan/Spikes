using System;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Config.Core.Localization.Resources;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Unity;

namespace XCore.Services.Config.Core.Validators
{
    public class ConfigValidators: AbstractModelValidator<ConfigItem>
    {
        #region cst.

        internal ConfigValidators() : base()
        {
            Initialize();
        }

        #endregion
        #region helpers

        private void Initialize()
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new ConfigValidatorsContext(ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new ConfigValidatorsContext(ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new ConfigValidatorsContext(ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new ConfigValidatorsContext(ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new ConfigValidatorsContext(ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class ConfigValidatorsContext : AbstractValidator<ConfigItem>
        {
            #region cst.

            public ConfigValidatorsContext(ValidationMode mode)
            {
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
                    RuleFor(x => x).Must(IsUniqueConfig).WithMessage(ValidationResources.Config_Error_Duplicate);
                    RuleFor(x => x).Must(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                    RuleFor(x => x).Must(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(ValidationResources.Error_ConfigEdit_Id_Empty);
                    RuleFor(x => x).Must(IsValidKey).WithMessage(ValidationResources.Config_Key_NotValid);
                    RuleFor(x => x).Must(IsUniqueConfig).WithMessage(ValidationResources.Config_Error_Duplicate);
                    RuleFor(x => x).Must(CheckConfigExistsActive).WithMessage(ValidationResources.Config_Error_NotExists);
                    RuleFor(x => x).Must(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                    RuleFor(x => x).Must(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(CheckConfigExists).WithMessage(ValidationResources.Config_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(CheckConfigExists).WithMessage(ValidationResources.Config_Error_NotExists);
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

            private bool IsUniqueConfig(ConfigItem model)
            {
                if (model == null) return true;  // skip

                return !ConfigUnity.Configs.IsUnique(model, SystemRequestContext.Instance).Result;
            }
            private bool CheckConfigExists(ConfigItem model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.ConfigSearchCriteria() {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance).Result;
            }
            private bool CheckAppExists(ConfigItem model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.AppSearchCriteria() { Id = model.AppId }, SystemRequestContext.Instance).Result;
            }
            private bool CheckModuleExists(ConfigItem model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.ModuleSearchCriteria() { Id = model.ModuleId }, SystemRequestContext.Instance).Result;
            }
            private bool IsValidKey(ConfigItem model)
            {
                if (model == null) return true;  // skip
                return !string.IsNullOrWhiteSpace(model.Key);
            }
            private bool CheckConfigExistsActive(ConfigItem model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.ModuleSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = true,
                }, SystemRequestContext.Instance).Result;
            }

            #endregion
        }

        #endregion
    }
}
