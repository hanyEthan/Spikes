using System;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Attachments.API.Models;
using XCore.Services.Attachments.Core.Models;

namespace XCore.Services.Attachments.API.Mappers
{
    public class AttachmentCreateResponseMapper : IModelMapper<Attachment, bool>
    {
        #region props.

        public static AttachmentCreateResponseMapper Instance { get; } = new AttachmentCreateResponseMapper();

        #endregion
        #region cst.

        static AttachmentCreateResponseMapper()
        {
        }
        public AttachmentCreateResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public bool Map(Attachment from, object metadata = null)
        {
            return from != null;
        }
        public Attachment Map(bool from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
