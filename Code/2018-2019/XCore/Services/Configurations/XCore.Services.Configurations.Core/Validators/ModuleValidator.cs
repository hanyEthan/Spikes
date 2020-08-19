using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Configurations.Core.Contracts;
using XCore.Services.Configurations.Core.Localization.Resources;
using XCore.Services.Configurations.Core.Models.Domain;

namespace XCore.Services.Configurations.Core.Validators
{
    public class ModuleValidator : AbstractModelValidator<Module>, IModelValidator<Module>
    {
        #region cst.

        public ModuleValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new ModuleValidatorsContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new ModuleValidatorsContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new ModuleValidatorsContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new ModuleValidatorsContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new ModuleValidatorsContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class ModuleValidatorsContext : AbstractValidator<Module>
        {
            #region props.

            private Lazy<IConfigHandler> ConfigHandler { get; set; }

            #endregion
            #region cst.

            public ModuleValidatorsContext(IServiceProvider services, ValidationMode mode)
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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Module_Id_Error_IsNotEqualZero);
                    RuleFor(x => x).MustAsync(IsUniqueModule).WithMessage(ValidationResources.Module_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(ValidationResources.Error_ModuleEdit_Id_Empty);
                    RuleFor(x => x).MustAsync(IsUniqueModule).WithMessage(ValidationResources.Module_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                    RuleFor(x => x).MustAsync(CheckModuleExistsActive).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckModuleExists).WithMessage(ValidationResources.Module_Error_NotExists);
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

            private async Task<bool> IsUniqueModule(Module model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckModuleExists(Module model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsExists(new Models.Support.ModuleSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExists(Module model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsExists(new Models.Support.AppSearchCriteria() { Id = model.AppId }, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckModuleExistsActive(Module model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ConfigHandler.Value.IsExists(new Models.Support.ModuleSearchCriteria()
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
