using System;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.WorkFlow.Models
{
    public class ActionInfo

    {
        Application Application { get; set; }
        object ApplicationId { get; set; }
        Action Action { get; set; }
        string ActionLabel { get; set; }
        string UserId { get; set; }
    }
}
