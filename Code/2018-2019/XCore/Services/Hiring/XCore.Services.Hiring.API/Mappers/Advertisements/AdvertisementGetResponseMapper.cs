using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Mappers.Advertisements;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Advertisements.API.Mappers.Advertisements
{
    public class AdvertisementGetResponseMapper : IModelMapper<SearchResults<Advertisement>, SearchResultsDTO<AdvertisementDTO>>
    {
        #region props.

        public static AdvertisementGetResponseMapper Instance { get; } = new AdvertisementGetResponseMapper();

        #endregion      

        #region IModelMapper

        public SearchResults<Advertisement> Map(SearchResultsDTO<AdvertisementDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<AdvertisementDTO> Map(SearchResults<Advertisement> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<AdvertisementDTO>
            {
                Results = Map(from.Results),
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
            };

            return to;
        }

        #endregion
        #region helpers.

        private List<AdvertisementDTO> Map(List<Advertisement> from)
        {
            if (from == null) return null;

            var to = new List<AdvertisementDTO>();

            foreach (var fromItem in from)
            {
                var toItem = AdvertisementMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
