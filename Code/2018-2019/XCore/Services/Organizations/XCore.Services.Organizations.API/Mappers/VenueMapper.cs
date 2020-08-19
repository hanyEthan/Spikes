using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.City;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.Venue;
using XCore.Services.Organizations.Core.Models.Domain;


namespace XCore.Services.Organizations.API.Mappers
{
    public class VenueMapper : IModelMapper<VenueDTO, Venue>,
                               IModelMapper<Venue, VenueDTO>
    {
        #region props.

        public static VenueMapper Instance { get; } = new VenueMapper(); 
        public static CityMapper CityMapper { get; } = new CityMapper();
        public static DepartmentMapper DepartmentMapper { get; } = new DepartmentMapper();
      
       



        #endregion
        #region IModelMapper

        public VenueDTO Map(Venue from, object metadata = null)
        {
            if (from == null) return null;

            var to = new VenueDTO()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                SubVenuesDTO = Map(from.SubVenues),
                CitiesDTO = Map(from.Cities),
                DepartmentsDTO = Map(from.Departments)


            };

            return to;
        }
     
        public Venue Map(VenueDTO from, object metadata = null)
         {
            if (from == null) return null;

            var to = new Venue() { };

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



        #endregion
        #region Helpers.
        public List<CityDTO> Map(IList<VenueCity> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<CityDTO>();
            foreach (var item in from)
            {
                var City = CityMapper.Map(item.City);
                to.Add(City);
            }




            return to;
        }
        public List<DepartmentDTO> Map(IList<VenueDepartment> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<DepartmentDTO>();
            foreach (var item in from)
            {
                var department = DepartmentMapper.Map(item.Department);
                to.Add(department);
            }




            return to;
        }
        private List<VenueDTO> Map(IList<Venue> from)
        {
            if (from == null) return null;
            var to = new List<VenueDTO>();
            foreach (var item in from)
            {
                var toItem = Instance.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }
            return to;

        }











        #endregion

    }
}
