using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Attachments.Core.Models.Support
{
    public class AttachmentSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public List<string> Id { get; set; }

        #endregion
    }
}
