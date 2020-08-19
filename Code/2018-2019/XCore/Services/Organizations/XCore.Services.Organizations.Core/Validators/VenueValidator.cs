using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;
using XCore.Services.Organizations.Core.Localization.Resources;
using XCore.Services.Organizations.Core.Contracts;

namespace XCore.Services.Organizations.Core.Validators
{
    public class VenueValidator : AbstractModelValidator<Venue>
    {
        #region cst.

        public VenueValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new VenueValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new VenueValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new VenueValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new VenueValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new VenueValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion
        #region nested.

        protected class VenueValidatorContext : AbstractValidator<Venue>
        {
            #region props.

            private Lazy<IVenueHandler> VenueHandler { get; set; }

            #endregion
            #region cst.

            public VenueValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.VenueHandler = new Lazy<IVenueHandler>(() => services.GetService(typeof(IVenueHandler)) as IVenueHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Venue_Code_Error_Empty);

                });
            }

            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueApp).WithMessage(ValidationResources.Venue_Error_Duplicate);
                    
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    //RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.App_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(ValidationResources.Venue_Error_Empty);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.Venue_Code_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.Venue_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUniqueApp(Venue model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.VenueHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckAppExists(Venue model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.VenueHandler.Value.IsExists(new VenueSearchCriteria()
                {
                    Id = model.Id,
                    //Code = model.Code,
                    //IsActive = null,
                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckAppExistsActive(Venue model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.VenueHandler.Value.IsExists(new VenueSearchCriteria()
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
