using System;
using System.Collections.Generic;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Attachments.API.Models;
using XCore.Services.Attachments.Core.Models;

namespace XCore.Services.Attachments.API.Mappers
{
    public class AttachmentGetResponseMapper : IModelMapper<SearchResults<Attachment>, SearchResultsDTO<AttachmentDTO>>
    {
        #region props.

        private AttachmentMapper AttachmentMapper { get; set; } = new AttachmentMapper();

        public static AttachmentGetResponseMapper Instance { get; } = new AttachmentGetResponseMapper();

        #endregion
        #region cst.

        static AttachmentGetResponseMapper()
        {
        }
        public AttachmentGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<Attachment> Map(SearchResultsDTO<AttachmentDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<AttachmentDTO> Map(SearchResults<Attachment> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<AttachmentDTO>();

            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<AttachmentDTO> Map(List<Attachment> from)
        {
            if (from == null) return null;

            var to = new List<AttachmentDTO>();

            foreach (var fromItem in from)
            {
                var toItem = this.AttachmentMapper.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
