using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.API.Models;

namespace XCore.Services.Attachments.API.Mappers
{
    public class AttachmentListMapper : IModelMapper<List<Attachment>, List<AttachmentDTO>>,
                                        IModelMapper<List<AttachmentDTO>, List<Attachment>>
    {
        #region props.

        public static AttachmentListMapper Instance { get; } = new AttachmentListMapper();

        #endregion
        #region cst.

       

        #endregion

        #region IModelMapper

        public List<AttachmentDTO> Map(List<Attachment> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<AttachmentDTO>();

            foreach (var fromItem in from)
            {
                var toItem = AttachmentMapper.Instance.Map(fromItem);
                if (toItem == null) return null;
                to.Add(toItem);
            }

            return to;
        }
        public List<Attachment> Map(List<AttachmentDTO> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<Attachment>();

            foreach (var fromItem in from)
            {
                var toItem = AttachmentMapper.Instance.Map(fromItem);
                if (toItem == null) return null;
                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
