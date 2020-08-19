using System.Collections.Generic;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Docs.API.Models;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.API.Mappers
{
    public class DocumentListMapper : IModelMapper<List<Document>, List<DocumentDTO>>,
                                      IModelMapper<List<DocumentDTO>, List<Document>>
    {
        #region props.

        public static DocumentListMapper Instance { get; } = new DocumentListMapper();

        #endregion
        #region cst.

       

        #endregion

        #region IModelMapper

        public List<DocumentDTO> Map(List<Document> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<DocumentDTO>();

            foreach (var fromItem in from)
            {
                var toItem = DocumentMapper.Instance.Map(fromItem);
                if (toItem == null) return null;
                to.Add(toItem);
            }

            return to;
        }
        public List<Document> Map(List<DocumentDTO> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<Document>();

            foreach (var fromItem in from)
            {
                var toItem = DocumentMapper.Instance.Map(fromItem);
                if (toItem == null) return null;
                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
