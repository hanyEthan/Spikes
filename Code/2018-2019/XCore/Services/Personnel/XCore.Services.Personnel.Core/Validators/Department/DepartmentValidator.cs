using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Personnel.Core.Contracts.Departments;
using XCore.Services.Personnel.Core.Localization.Resources;
using XCore.Services.Personnel.Models.Departments;


namespace XCore.Services.Personnel.Core.Validators
{
    public class DepartmentValidator : AbstractModelValidator<Department>
    {
        #region cst.

        public DepartmentValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new DepartmentValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new DepartmentValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new DepartmentValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new DepartmentValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new DepartmentValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class DepartmentValidatorContext : AbstractValidator<Department>
        {
            #region props.

            private Lazy<IDepartmentHandler> DepartmentHandler { get; set; }

            #endregion
            #region cst.

            public DepartmentValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.DepartmentHandler = new Lazy<IDepartmentHandler>(() => services.GetService(typeof(IDepartmentHandler)) as IDepartmentHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Department_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Department_Error_Duplicate);
                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Department_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(ValidationResources.Department_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Department_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Department_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Department_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Department_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueApp(Department model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.DepartmentHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAppExists(Department model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.DepartmentHandler.Value.IsExists(new DepartmentSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(Department model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.DepartmentHandler.Value.IsExists(new DepartmentSearchCriteria()
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
