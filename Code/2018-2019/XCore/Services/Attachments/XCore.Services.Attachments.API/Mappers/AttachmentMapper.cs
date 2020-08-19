using System;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Attachments.API.Models;
using XCore.Services.Attachments.Core.Models;

namespace XCore.Services.Attachments.API.Mappers
{
    public class AttachmentMapper : IModelMapper<Attachment, AttachmentDTO>,
                                    IModelMapper<AttachmentDTO, Attachment>
    {
        #region props.

        public static AttachmentMapper Instance { get; } = new AttachmentMapper();

        #endregion
        #region cst.

        static AttachmentMapper()
        {
        }
        public AttachmentMapper()
        {
        }

        #endregion

        #region IModelMapper

        public AttachmentDTO Map(Attachment from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AttachmentDTO();
            to.Id = from.Id;
            
           
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);          
            to.MetaData = from.MetaData;
            to.MimeType = from.MimeType;
            to.Body = from.Body;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Name = from.Name;
            to.Extension = from.Extension;
            to.Status = from.Status;
            return to;
        }
        public Attachment Map(AttachmentDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Attachment();
            to.Id = from.Id;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.MetaData = from.MetaData;
            to.MimeType = from.MimeType;
            to.Body = from.Body;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Name = from.Name;
            to.Extension = from.Extension;
            to.Status = from.Status;
            return to;
        }

        #endregion
    }
}
