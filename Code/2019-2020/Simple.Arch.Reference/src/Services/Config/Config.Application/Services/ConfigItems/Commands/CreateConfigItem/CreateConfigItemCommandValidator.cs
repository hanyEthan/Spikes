using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Mcs.Invoicing.Services.Config.Application.Common.Localization;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.AnyConfigItems;
using Mcs.Invoicing.Services.Config.Application.Services.Modules.Queries.AnyModules;
using MediatR;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem
{
    public class CreateConfigItemCommandValidator : AbstractValidator<CreateConfigItemCommand>
    {
        #region props.

        private readonly IMediator _mediator;

        #endregion
        #region cst.

        public CreateConfigItemCommandValidator(IMediator mediator)
        {
            #region init.

            this._mediator = mediator;

            #endregion
            #region rules.

            RuleFor(x => x).NotEmpty().WithMessage(ValidationResources.CreateConfigItemCommand_Error_Empty);
            When(x => x != null, () =>
            {
                RuleFor(x => x.Key).NotEmpty().WithMessage(ValidationResources.CreateConfigItemCommand_Error_Key);
                RuleFor(x => x.Value).NotEmpty().WithMessage(ValidationResources.CreateConfigItemCommand_Error_Value);
                RuleFor(x => x.ModuleId).NotEmpty().WithMessage(ValidationResources.CreateConfigItemCommand_Error_Module);

                RuleFor(x => x).MustAsync(IsUniqueConfig).WithMessage(ValidationResources.CreateConfigItemCommand_Error_Duplicate);
                RuleFor(x => x).MustAsync(CheckModuleExists).WithMessage(ValidationResources.CreateConfigItemCommand_Error_Module);
            });

            #endregion
        }

        #endregion
        #region custom.

        private async Task<bool> IsUniqueConfig(CreateConfigItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null) return true;  // skip

            Map(command, out AnyConfigItemsQuery query);
            var exists = await this._mediator.Send(query);

            return !exists;
        }
        private async Task<bool> CheckModuleExists(CreateConfigItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null) return true;  // skip

            Map(command, out AnyModulesQuery query);
            var exists = await this._mediator.Send(query);

            return exists;
        }

        #endregion
        #region helpers.

        private void Map(CreateConfigItemCommand from, out AnyConfigItemsQuery to)
        {
            to = from == null
               ? null
               : new AnyConfigItemsQuery()
               {
                   Key = from.Key,
                   ModuleId = from.ModuleId,
               };
        }
        private void Map(CreateConfigItemCommand from, out AnyModulesQuery to)
        {
            to = from == null
               ? null
               : new AnyModulesQuery()
               {
                   Id = from.ModuleId,
               };
        }

        #endregion
    }
}
