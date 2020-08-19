using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.City;
using XCore.Services.Organizations.API.Models.Venue;
using XCore.Services.Organizations.Core.Models.Domain;


namespace XCore.Services.Organizations.API.Mappers
{
    public class CityMapper : IModelMapper<CityDTO, City>,
                             IModelMapper<City, CityDTO>
    {
        #region props.

        public static CityMapper Instance { get; } = new CityMapper();
        public static VenueMapper VenueMapper { get; } = new VenueMapper();
      
       



        #endregion
        #region IModelMapper

        public CityDTO Map(City from, object metadata = null)
        {
            if (from == null) return null;

            var to = new CityDTO()
            {   
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat) ,
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                VenuesDTO=Map(from.Venues)
                
               
               
              
            };

            return to;
        }
        public City Map(CityDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new City() { };

            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.IsActive = from.IsActive;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.Id = from.Id;




            return to;
        }

        #region Helpers.




        public List<VenueDTO> Map(IList<VenueCity> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<VenueDTO>();
            foreach (var item in from)
            {
                var Venue = VenueMapper.Map(item.Venue);
                to.Add(Venue);
            }




            return to;
        }








        #endregion
        #endregion

    }
}
