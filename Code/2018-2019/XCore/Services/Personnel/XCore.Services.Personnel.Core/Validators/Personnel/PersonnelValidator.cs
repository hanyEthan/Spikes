using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Personnel.Core.Contracts.Personnels;
using XCore.Services.Personnel.Core.Localization.Resources;
using XCore.Services.Personnel.Models.Personnels;

namespace XCore.Services.Personnel.Core.Validators
{
    public class PersonnelValidator : AbstractModelValidator<Person>
    {
        #region cst.

        public PersonnelValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new PersonnelValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new PersonnelValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new PersonnelValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new PersonnelValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new PersonnelValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class PersonnelValidatorContext : AbstractValidator<Person>
        {
            #region props.

            private Lazy<IPersonnelHandler> PersonnelHandler { get; set; }

            #endregion
            #region cst.

            public PersonnelValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.PersonnelHandler = new Lazy<IPersonnelHandler>(() => services.GetService(typeof(IPersonnelHandler)) as IPersonnelHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Personnel_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Personnel_Error_Duplicate);
                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Personnel_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(ValidationResources.Personnel_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckPersonnelExists).WithMessage(ValidationResources.Personnel_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckPersonnelExists).WithMessage(ValidationResources.Personnel_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Personnel_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Personnel_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueApp(Person model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.PersonnelHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckPersonnelExists(Person model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.PersonnelHandler.Value.IsExists(new PersonSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(Person model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.PersonnelHandler.Value.IsExists(new PersonSearchCriteria()
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
