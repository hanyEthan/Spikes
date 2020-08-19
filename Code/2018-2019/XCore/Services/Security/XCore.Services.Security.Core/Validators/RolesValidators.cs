using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Localization.Resources;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Unity;

namespace XCore.Services.Security.Core.Validators
{
    public class RolesValidators : AbstractModelValidator<Role>
    {
        #region cst.

        public RolesValidators(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new RolesValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new RolesValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new RolesValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new RolesValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new RolesValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class RolesValidatorContext : AbstractValidator<Role>
        {
            #region props.

            private Lazy<IRoleHandler> RoleHandler { get; set; }

            #endregion
            #region cst.

            public RolesValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.RoleHandler = new Lazy<IRoleHandler>(() => services.GetService(typeof(IRoleHandler)) as IRoleHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(SecurityValidationResource.Error_Role_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueRole).WithMessage(SecurityValidationResource.Error_Role_Duplicate);
                    //RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(SecurityValidationResource.Error_App_NotExists);
                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    //RuleFor(x => x.Id).NotEqual(0).WithMessage(SecurityValidationResource.Error_Role_Id_Empty);
                    RuleFor(x => x).MustAsync(IsUniqueRole).WithMessage(SecurityValidationResource.Error_Role_Duplicate);
                    RuleFor(x => x).MustAsync(CheckRoleExistsActive).WithMessage(SecurityValidationResource.Error_Role_NotExists);
                    //RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(SecurityValidationResource.Error_App_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(SecurityValidationResource.Error_Role_Id_Empty);
                    RuleFor(x => x).MustAsync(CheckRoleExists).WithMessage(SecurityValidationResource.Error_Role_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(SecurityValidationResource.Error_Role_Id_Empty);
                    RuleFor(x => x).MustAsync(CheckRoleExists).WithMessage(SecurityValidationResource.Error_Role_NotExists);
                    RuleFor(x => x).MustAsync(IsUniqueRole).WithMessage(SecurityValidationResource.Error_Role_Duplicate);

                });
            }
            private void HandleDeactivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage(SecurityValidationResource.Error_Role_Id_Empty);
                    RuleFor(x => x).MustAsync(CheckRoleExists).WithMessage(SecurityValidationResource.Error_Role_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(SecurityValidationResource.Error_Role_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(SecurityValidationResource.Error_Role_Code_Empty);
                });
            }

            private async Task<bool> IsUniqueRole(Role model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.RoleHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckRoleExistsActive(Role model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.RoleHandler.Value.IsExists(new Models.Support.RoleSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = true
                }, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckRoleExists(Role model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.RoleHandler.Value.IsExists(new Models.Support.RoleSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null
                }, SystemRequestContext.Instance);
                return check.Result;
            }
            //private async Task<bool> CheckAppExists(Role model, CancellationToken cancellationToken)
            //{
            //    if (model == null) return true;  // skip

            //    var check = await this.RoleHandler.Value.IsExists(new Models.Support.AppSearchCriteria()
            //    { Id = model.AppId}, SystemRequestContext.Instance);
            //    return check.Result;
            //}

            #endregion
        }

        #endregion
    }
}
