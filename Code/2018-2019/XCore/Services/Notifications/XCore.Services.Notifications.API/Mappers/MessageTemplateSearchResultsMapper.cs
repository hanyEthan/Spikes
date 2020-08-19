using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Domain;

namespace XCore.Services.Notifications.API.Mappers
{
    public class MessageTemplateSearchResultsMapper : IModelMapper<SearchResults<MessageTemplate>, SearchResultsDTO<MessageTemplateDTO>>
    {
        #region props.

        public static MessageTemplateSearchResultsMapper Instance { get; } = new MessageTemplateSearchResultsMapper();

        #endregion
        #region IModelMapper
        public SearchResultsDTO<MessageTemplateDTO> Map(SearchResults<MessageTemplate> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<MessageTemplateDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }
        public SearchResults<MessageTemplate> Map(SearchResultsDTO<MessageTemplateDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region helpers.

        public List<MessageTemplateDTO> Map(List<MessageTemplate> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<MessageTemplateDTO>();

            foreach (var item in from)
            {
                var toItem = MessageTemplateMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
