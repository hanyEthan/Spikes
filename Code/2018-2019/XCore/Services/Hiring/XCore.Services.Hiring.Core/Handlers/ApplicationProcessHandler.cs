using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.WorkFlow.Handlers;
using XCore.Services.Hiring.Core.WorkFlow.Models;
using XCore.Services.Hiring.Core.WorkFlow.Models.Enums;
using Action = XCore.Services.Hiring.Core.WorkFlow.Models.Enums.Action;

namespace XCore.Services.Hiring.Core.Handlers
{
    class ApplicationProcessHandler : WorkFlowHandler<Status, Action, ActionInfo, Application, TResponse>
    {
    }
}
