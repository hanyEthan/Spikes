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
    public class ActorsValidators : AbstractModelValidator<Actor>
    {
        #region cst.

        public ActorsValidators(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new ActorsValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new ActorsValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new ActorsValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new ActorsValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new ActorsValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion

        #region nested.

        protected class ActorsValidatorContext : AbstractValidator<Actor>
        {
            #region props.

            private Lazy<IActorHandler> ActorHandler { get; set; }

            #endregion
            #region cst.

            public ActorsValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.ActorHandler = new Lazy<IActorHandler>(() => services.GetService(typeof(IActorHandler)) as IActorHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(SecurityValidationResource.Error_Actor_Id_NotEmpty);
                    RuleFor(x => x).MustAsync(IsUniqueActor).WithMessage(SecurityValidationResource.Error_Actor_Duplicate);
                    //RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(SecurityValidationResource.Error_App_NotExists);

                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUniqueActor).WithMessage(SecurityValidationResource.Error_Actor_Duplicate);
                    RuleFor(x => x).MustAsync(CheckActorExistsActive).WithMessage(SecurityValidationResource.Error_Actor_NotExists);
                    //RuleFor(x => x).MustAsync(CheckAppExists).WithMessage(SecurityValidationResource.Error_App_NotExists);

                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckActorExists).WithMessage(SecurityValidationResource.Error_Actor_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckActorExists).WithMessage(SecurityValidationResource.Error_Actor_NotExists);
                    RuleFor(x => x).MustAsync(IsUniqueActor).WithMessage(SecurityValidationResource.Error_Actor_Duplicate);

                });
            }
            private void HandleDeactivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckActorExists).WithMessage(SecurityValidationResource.Error_Actor_NotExists);

                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(SecurityValidationResource.Error_Actor_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(SecurityValidationResource.Error_Actor_Code_Empty);
                });
            }

            private async Task<bool>  IsUniqueActor(Actor model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ActorHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckActorExistsActive(Actor model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ActorHandler.Value.IsExists(new Models.Support.ActorSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = true
                }, SystemRequestContext.Instance);
                return check.Result;
            }
            private async Task<bool> CheckActorExists(Actor model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.ActorHandler.Value.IsExists(new Models.Support.ActorSearchCriteria()
                {
                    Id = model.Id,
                    Code = model.Code,
                    IsActive = null
                }, SystemRequestContext.Instance);
                return check.Result;
            }
            //private async Task<bool> CheckAppExists(Actor model, CancellationToken cancellationToken)
            //{
            //    if (model == null) return true;  // skip

            //    var check = await this.ActorHandler.Value.IsExists(new Models.Support.AppSearchCriteria()
            //    { Id = model.AppId }, SystemRequestContext.Instance);
            //    return check.Result;
            //}

            #endregion
        }

        #endregion
    }
}
