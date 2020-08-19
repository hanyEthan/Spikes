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
    public class HiringProcessValidator : AbstractModelValidator<HiringProcess>
    {
        #region cst.

        public HiringProcessValidator(IServiceProvider services) : base()
        {
            Initialize(services);
        }

        #endregion
        #region helpers

        private void Initialize(IServiceProvider services)
        {
            base.ValidationContexts.Add(ValidationMode.Create.ToString(), new HiringProcessValidatorContext(services, ValidationMode.Create));
            base.ValidationContexts.Add(ValidationMode.Edit.ToString(), new HiringProcessValidatorContext(services, ValidationMode.Edit));
            base.ValidationContexts.Add(ValidationMode.Delete.ToString(), new HiringProcessValidatorContext(services, ValidationMode.Delete));
            base.ValidationContexts.Add(ValidationMode.Activate.ToString(), new HiringProcessValidatorContext(services, ValidationMode.Activate));
            base.ValidationContexts.Add(ValidationMode.Deactivate.ToString(), new HiringProcessValidatorContext(services, ValidationMode.Deactivate));
        }

        #endregion
        #region nested.

        protected class HiringProcessValidatorContext : AbstractValidator<HiringProcess>
        {
            #region props.

            private Lazy<IHiringProcessesHandler> HiringProcessHandler { get; set; }

            #endregion
            #region cst.

            public HiringProcessValidatorContext(IServiceProvider services, ValidationMode mode)
            {
                #region init.

                this.HiringProcessHandler = new Lazy<IHiringProcessesHandler>(() => services.GetService(typeof(IHiringProcessesHandler)) as IHiringProcessesHandler);

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
                    RuleFor(x => x.Id).Equal(0).WithMessage(ValidationResources.Error_HiringProcess_Id_NotEmpty);

                });
            }
            private void HandleEdit()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(IsUnique).WithMessage(ValidationResources.HiringProcess_Error_Duplicate);
                    RuleFor(x => x).MustAsync(CheckExistsActive).WithMessage(ValidationResources.HiringProcess_Error_NotExists);
                });
            }
            private void HandleDelete()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckExists).WithMessage(ValidationResources.HiringProcess_Error_NotExists);
                });
            }
            private void HandleActivation()
            {
                When(x => x != null, () =>
                {
                    RuleFor(x => x).MustAsync(CheckExists).WithMessage(ValidationResources.HiringProcess_Error_NotExists);
                });
            }
            private void HandleCommon()
            {
                RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.HiringProcess_Error_Empty);
                When(x => x != null, () =>
                {
                    RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationResources.HiringProcess_Code_Error_Empty);
                });
            }

            private async Task<bool> IsUnique(HiringProcess model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.HiringProcessHandler.Value.IsUnique(model, SystemRequestContext.Instance);
                return !check.Result;
            }
            private async Task<bool> CheckExists(HiringProcess model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.HiringProcessHandler.Value.IsExists(new HiringProcessesSearchCriteria()
                {
                    Id = model.Id,
                    IsActive = null,

                }, SystemRequestContext.Instance);

                return check.Result;
            }
            private async Task<bool> CheckExistsActive(HiringProcess model, CancellationToken cancellationToken)
            {
                if (model == null) return true;  // skip

                var check = await this.HiringProcessHandler.Value.IsExists(new HiringProcessesSearchCriteria()
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
