using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Localization.Resources;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Validators
{
    public class AdvertisementValidator : AbstractModelValidator<Advertisement>
    {
        #region cst.

        public AdvertisementValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new AdvertisementValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new AdvertisementValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new AdvertisementValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new AdvertisementValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new AdvertisementValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion
        #region nested.

        protected class AdvertisementValidatorContext : AbstractValidator<Advertisement>
        {
            #region props.

            private Lazy<IAdvertisementsHandler> AdvertisementHandler { get; set; }

            #endregion
            #region cst.

            public AdvertisementValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.AdvertisementHandler = new Lazy<IAdvertisementsHandler>(() => services.GetService(typeof(IAdvertisementsHandler)) as IAdvertisementsHandler);

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
                            //HandleCommon();
                            HandleDelete();
                        }
                        break;
                    case ValidationMode.Activate:
                    case ValidationMode.Deactivate:
                        {
                            //HandleCommon();
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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_Advertisement_Id_NotEmpty);

                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUnique).WithMessage(ValidationResources.Advertisement_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckExistsActive).WithMessage(ValidationResources.Advertisement_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckExists).WithMessage(ValidationResources.Advertisement_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckExists).WithMessage(ValidationResources.Advertisement_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Advertisement_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Advertisement_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUnique(Advertisement model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.AdvertisementHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckExists(Advertisement model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.AdvertisementHandler.Value.IsExists(new AdvertisementsSearchCriteria()
                {
                    Id = model.Id,
                    IsActive = null,

                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckExistsActive(Advertisement model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.AdvertisementHandler.Value.IsExists(new AdvertisementsSearchCriteria()
                {
                    Id = model.Id,
                    IsActive = true,
                }, SystemRequestContext.Instance);

                return check.Result;
            }

            #endregion
        }

        #endregion
    }
}
