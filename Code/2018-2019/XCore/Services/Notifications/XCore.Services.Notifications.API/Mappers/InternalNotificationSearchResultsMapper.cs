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
    public class InternalNotificationSearchResultsMapper : IModelMapper<SearchResults<InternalNotification>, SearchResultsDTO<InternalNotificationDTO>>
    {
        #region props.
        public static InternalNotificationSearchResultsMapper Instance { get; } = new InternalNotificationSearchResultsMapper();
        #endregion
        #region IModelMapper
        public SearchResultsDTO<InternalNotificationDTO> Map(SearchResults<InternalNotification> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<InternalNotificationDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }
        public SearchResults<InternalNotification> Map(SearchResultsDTO<InternalNotificationDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region helpers

        public List<InternalNotificationDTO> Map(List<InternalNotification> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<InternalNotificationDTO>();

            foreach (var item in from)
            {
                var toItem = InternalNotificationMapper.Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }
        #endregion
    }
}
