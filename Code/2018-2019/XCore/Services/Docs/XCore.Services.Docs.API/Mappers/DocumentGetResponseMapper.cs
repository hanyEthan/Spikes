using System;
using System.Collections.Generic;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Docs.API.Models;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.API.Mappers
{
    public class DocumentGetResponseMapper : IModelMapper<SearchResults<Document>, SearchResultsDTO<DocumentDTO>>
    {
        #region props.
        private DocumentMapper DocumentMapper { get; set; } = new DocumentMapper();
        public static DocumentGetResponseMapper Instance { get; } = new DocumentGetResponseMapper();

        #endregion
        #region cst.

        static DocumentGetResponseMapper()
        {
        }
        public DocumentGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<Document> Map(SearchResultsDTO<DocumentDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<DocumentDTO> Map(SearchResults<Document> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<DocumentDTO>();

            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<DocumentDTO> Map(List<Document> from)
        {
            if (from == null) return null;

            var to = new List<DocumentDTO>();

            foreach (var fromItem in from)
            {
                
                var toItem = this.DocumentMapper.Map(fromItem);
                if (toItem != null) to.Add(toItem);

            }

            return to;
        }

        #endregion
    }
}
