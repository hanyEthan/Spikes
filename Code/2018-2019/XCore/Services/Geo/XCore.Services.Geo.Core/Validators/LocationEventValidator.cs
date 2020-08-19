using FluentValidation;
using XCore.Framework.Framework.Geo.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Handlers;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Geo.Core.Localization.Resources;
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.Core.Validators
{
    public class LocationEventValidator : AbstractModelValidator<LocationEvent>
    {
        #region cst.

        internal LocationEventValidator() : base()
        {
            Initialize();
        }

        #endregion
        #region helpers

        private void Initialize()
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new LocationEventValidatorContext(ValidationMode.Create));
        }

        #endregion

        #region nested.

        protected class LocationEventValidatorContext : AbstractValidator<LocationEvent>
        {
            #region cst.

            public LocationEventValidatorContext(ValidationMode mode)
            {
                switch (mode)
                {
                    case ValidationMode.Create:
                        {
                            HandleCommon();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            #endregion
            #region Helpers.

            private void HandleCommon()
            {
                #region Mandatory Validations

                RuleFor(x => x.EntityCode).NotEmpty().WithMessage(ValidationResources.Error_LocationEvent_EntityCode_Empty);
                RuleFor(x => x.EntityType).NotEmpty().WithMessage(ValidationResources.Error_LocationEvent_EntityType_Empty);
                RuleFor(x => x.EventCode).NotEmpty().WithMessage(ValidationResources.Error_LocationEvent_EventCode_Empty);

                RuleFor(x => x.Id).Must(IsValidGeo).WithMessage(ValidationResources.Error_LocationEvent_location_NotValid);

                #endregion
            }

            private bool IsValidGeo(LocationEvent model, int arg)
            {
                return ((new GeoLocation(model?.Longitude, model?.Latitude))?.IsValid).GetValueOrDefault();
            }

            #endregion
        }

        #endregion
    }
}
