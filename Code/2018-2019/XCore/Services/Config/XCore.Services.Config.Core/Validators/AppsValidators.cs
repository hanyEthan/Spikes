using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Config.Core.Localization.Resources;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Unity;

namespace XCore.Services.Config.Core.Validators
{
    public class AppsValidators : AbstractModelValidator<App>
    {
        #region cst.

        internal AppsValidators() : base()
        {
            Initialize();
        }

        #endregion
        #region helpers

        private void Initialize()
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new AppsValidatorContext(ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new AppsValidatorContext(ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new AppsValidatorContext(ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new AppsValidatorContext(ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new AppsValidatorContext(ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class AppsValidatorContext : AbstractValidator<App>
        {
            #region cst.

            public AppsValidatorContext(ValidationMode mode)
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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_App_Id_NotEmpty);
                    RuleFor(x => x).Must(IsUniqueApp).WithMessage(ValidationResources.App_Error_Duplicate);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(IsUniqueApp).WithMessage(ValidationResources.App_Error_Duplicate);
                    RuleFor(x => x).Must(CheckAppExistsActive).WithMessage(ValidationResources.App_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).Must(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.App_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.App_Code_Error_Empty);
                });
            }

            private bool IsUniqueApp(App model)
            {
                if (model == null) return true;  // skip

                return !ConfigUnity.Configs.IsUnique(model, SystemRequestContext.Instance).Result;
            }
            private bool CheckAppExists(App model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.AppSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance).Result;
            }
            private bool CheckAppExistsActive(App model)
            {
                if (model == null) return true;  // skip

                return ConfigUnity.Configs.IsExists(new Models.Support.AppSearchCriteria()
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
