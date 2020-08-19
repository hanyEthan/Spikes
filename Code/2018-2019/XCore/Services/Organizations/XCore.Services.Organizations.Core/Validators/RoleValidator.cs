using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Localization.Resources;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Validators
{
    public class RoleValidator : AbstractModelValidator<Role>
    {
        #region cst.

        public RoleValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new RoleValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new RoleValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new RoleValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new RoleValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new RoleValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion
        #region nested.

        protected class RoleValidatorContext : AbstractValidator<Role>
        {
            #region props.

            private Lazy<IRoleHandler> RoleHandler { get; set; }

            #endregion
            #region cst.

            public RoleValidatorContext(IServiceProvider services, ValidationMode mode)
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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Role_Id_NotEmpty);

                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Role_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(ValidationResources.Role_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Role_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Role_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Role_Code_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Role_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueApp(Role model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.RoleHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAppExists(Role model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.RoleHandler.Value.IsExists(new RoleSearchCriteria ()
                {
                    Id = model.Id,
                    //Code = model.Code,
                    //IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(Role model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.RoleHandler.Value.IsExists(new RoleSearchCriteria ()
                {
                    Id = model.Id,
                    //Code = model.Code,
                    //IsActive = true,
                }, SystemRequestContext.Instance);

                return check.Result;
            }

            #endregion
        }

        #endregion


    }
}
