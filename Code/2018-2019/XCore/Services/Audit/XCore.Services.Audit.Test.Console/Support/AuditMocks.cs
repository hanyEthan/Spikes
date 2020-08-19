using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Audit.SDK.Models;

namespace XCore.Services.Audit.Test.Console.Support
{
    public static class AuditMocks
    {
        #region AuditSearchRequest

        public static readonly ServiceExecutionRequestDTO<AuditSearchCriteriaDTO> AuditSearchRequest = new ServiceExecutionRequestDTO<AuditSearchCriteriaDTO>()
        {
            Content = new AuditSearchCriteriaDTO()
            {
                
            },
        };

        #endregion
    }
}
