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
    public class SettingsValidator: AbstractModelValidator<Settings>
    {
        #region cst.

        public SettingsValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new SettingsValidatorContext(services,ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new SettingsValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new SettingsValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new SettingsValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new SettingsValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion
        #region nested.

        protected class SettingsValidatorContext : AbstractValidator<Settings>
        {
            #region props.

            private Lazy<ISettingsHandler> SettingsHandler { get; set; }

            #endregion
            #region cst.

            public SettingsValidatorContext(IServiceProvider services,ValidationMode mode)
            {
                #region init.

                this.SettingsHandler = new Lazy<ISettingsHandler>(() => services.GetService(typeof(ISettingsHandler)) as ISettingsHandler);

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
                    RuleFor(x => x.Code).NotEqual("").WithMessage(ValidationResources.Error_Settings_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueSettings).WithMessage(ValidationResources.Settings_Error_Duplicate);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueSettings).WithMessage(ValidationResources.Settings_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckSettingsExistsActive).WithMessage(ValidationResources.Settings_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckSettingsExists).WithMessage(ValidationResources.Settings_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckSettingsExists).WithMessage(ValidationResources.Settings_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Settings_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Settings_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueSettings(Settings model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

               var Check=await this.SettingsHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !Check.Result;

            }
            private async Task<bool> CheckSettingsExists(Settings model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var res=await this.SettingsHandler.Value.IsExists(new Models.Support.SettingsSearchCriteria()
                {
                    Ids = Map(model.Id),
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);
                return res.Result;

            }
            private async Task<bool> CheckSettingsExistsActive(Settings model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var res=await this.SettingsHandler.Value.IsExists(new Models.Support.SettingsSearchCriteria()
                {
                    Ids =Map( model.Id),
                    Code = model.Code,
                    IsActive = true,
                }, SystemRequestContext.Instance);
                return res.Result;

            }
            #region Helper
            List<int> Map(int id)
            {
                if (id == null) return null;
                List<int> Ret = new List<int>();
                Ret.Add(id);
                return Ret;
            }
            #endregion

            #endregion
        }

        #endregion
    }
}
