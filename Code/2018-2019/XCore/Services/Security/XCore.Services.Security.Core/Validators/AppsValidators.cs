using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Localization.Resources;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.Core.Validators
{
    public class AppsValidators : AbstractModelValidator<App>
    {
        #region cst.

        public AppsValidators(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new AppsValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new AppsValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new AppsValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new AppsValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new AppsValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class AppsValidatorContext : AbstractValidator<App>
        {
            #region props.

            private Lazy<IAppHandler> AppHandler { get; set; }

            #endregion
            #region cst.

            public AppsValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.AppHandler = new Lazy<IAppHandler>(() => services.GetService(typeof(IAppHandler)) as IAppHandler);

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
                        {
                            HandleActivation();
                        }
                        break;
                    case ValidationMode.Deactivate:
                        {
                            HandleDeactivation();
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
                    RuleFor(x => x.Id).Equal(0).WithMessage(SecurityValidationResource.Error_App_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(SecurityValidationResource.Error_App_Duplicate);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(SecurityValidationResource.Error_App_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(SecurityValidationResource.Error_App_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(SecurityValidationResource.Error_App_NotExists);
                    //RuleFor(x => x).MustAsync(CheckAppRelationsExists).WithMessage(SecurityValidationResource.Error_App_Relations_Exist);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(SecurityValidationResource.Error_App_NotExists);
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(SecurityValidationResource.Error_App_Duplicate);
                });
            }
            private void HandleDeactivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(SecurityValidationResource.Error_App_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(SecurityValidationResource.Error_App_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(SecurityValidationResource.Error_App_Code_Empty);
                });
            }

            private async Task<bool> IsUniqueApp(App model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip
                var check = await this.AppHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result ;
            }
            private async Task<bool> CheckAppExistsActive(App model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.AppHandler.Value.IsExists(new Models.Support.AppSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = true,

                }, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckAppExists(App model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.AppHandler.Value.IsExists(new Models.Support.AppSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,

                }, SystemRequestContext.Instance);
                return check.Result;
            }
            //private async Task<bool> CheckAppRelationsExists(App model, CancellationToken cancellationToken)
            //{
            //    if (model == null) return true;  // skip
            //    var actorCheck = await this.AppHandler.Value.IsExists(new Models.Support.ActorSearchCriteria() { AppId = model.Id, IsActive = true, }, SystemRequestContext.Instance);
            //    var roleCheck = await this.AppHandler.Value.IsExists(new Models.Support.RoleSearchCriteria() { AppId = model.Id, IsActive = true, }, SystemRequestContext.Instance);
            //    var targetCheck = await this.AppHandler.Value.IsExists(new Models.Support.TargetSearchCriteria() { AppId = model.Id, IsActive = true, }, SystemRequestContext.Instance);
            //    var privilegeCheck = await this.AppHandler.Value.IsExists(new Models.Support.PrivilegeSearchCriteria() { AppId = model.Id, IsActive = true, }, SystemRequestContext.Instance);
            //    var ClaimCheck = await this.AppHandler.Value.IsExists(new Models.Support.ClaimSearchCriteria() { AppId = model.Id, IsActive = true, }, SystemRequestContext.Instance);

            //    return !actorCheck.Result && !roleCheck.Result && !targetCheck.Result && !privilegeCheck.Result;

                   
            //}

            #endregion
        }

        #endregion
    }
}
