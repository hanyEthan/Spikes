using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Attachments.API.Models;
using XCore.Services.Attachments.Core.Models.Support;

namespace XCore.Services.Attachments.API.Mappers
{
    public class AttachmentGetRequestMapper : IModelMapper<AttachmentSearchCriteria, AttachmentSearchCriteriaDTO>
    {
        #region props.

        public static AttachmentGetRequestMapper Instance { get; } = new AttachmentGetRequestMapper();

        #endregion
        #region cst.

        static AttachmentGetRequestMapper()
        {
        }
        public AttachmentGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public AttachmentSearchCriteria Map(AttachmentSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AttachmentSearchCriteria();

            to.Id = from.Id;

            return to;
        }
        public AttachmentSearchCriteriaDTO Map(AttachmentSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
