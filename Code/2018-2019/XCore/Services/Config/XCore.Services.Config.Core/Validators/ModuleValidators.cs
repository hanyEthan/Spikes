using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Config.Core.Localization.Resources;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Unity;

namespace XCore.Services.Config.Core.Validators
{
    public class ModuleValidators : AbstractModelValidator<Module>
    {
        #region cst.

        internal ModuleValidators() : base()
        {
            Initialize();
        }

        #endregion
        #region helpers

        private void Initialize()
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new ModuleValidatorsContext(ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new ModuleValidatorsContext(ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new ModuleValidatorsContext(ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new ModuleValidatorsContext(ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new ModuleValidatorsContext(ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class ModuleValidatorsContext : AbstractValidator<Module>
        {
            #region cst.

            public ModuleValidatorsContext(ValidationMode mode)
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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Module_Id_Error_IsNotEqualZero);
                    RuleFor(x => x).Must(IsUniqueModule).WithMessage(ValidationResources.Module_Error_Duplicate);
                    RuleFor(x => x).Must(CheckAppExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(ValidationResources.Error_ModuleEdit_Id_Empty);
                    RuleFor(x => x).Must(IsUniqueModule).WithMessage(ValidationResources.Module_Error_Duplicate);
                    RuleFor(x => x).Must(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                    RuleFor(x => x).Must(CheckModuleExistsActive).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Module_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Module_Code_Error_Empty);
                });
            }

            private bool IsUniqueModule(Module model)
            {
                if (model == null) return true;  // skip

                return !ConfigUnity.Configs.IsUnique(model, SystemRequestContext.Instance).Result;
            }
            private bool CheckModuleExists(Module model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.ModuleSearchCriteria() {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance).Result;
            }
            private bool CheckAppExists(Module model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.AppSearchCriteria() { Id = model.AppId }, SystemRequestContext.Instance).Result;
            }
            private bool CheckModuleExistsActive(Module model)
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
