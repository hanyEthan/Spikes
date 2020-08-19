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
    public class CityValidator : AbstractModelValidator<City>
    {
        #region cst.

        public CityValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new CityValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new CityValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new CityValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new CityValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new CityValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion
        #region nested.

        protected class CityValidatorContext : AbstractValidator<City>
        {
            #region props.

            private Lazy<ICityHandler> CityHandler { get; set; }

            #endregion
            #region cst.

            public CityValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.CityHandler = new Lazy<ICityHandler>(() => services.GetService(typeof(ICityHandler)) as ICityHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_City_Id_NotEmpty);

                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.City_Error_Duplicate1);
                    RuleFor(x => x).MustAsync(CheckAppExistsActive).WithMessage(ValidationResources.City_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.City_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.City_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.City_Code_Error_Empty1);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.City_Code_Error_Empty1);
                });
            }

            private async Task<bool> IsUniqueApp(City model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.CityHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAppExists(City model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.CityHandler.Value.IsExists(new CitySearchCriteria()
                {
                    Id = model.Id,
                    //Code = model.Code,
                    //IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(City model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.CityHandler.Value.IsExists(new CitySearchCriteria()
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
