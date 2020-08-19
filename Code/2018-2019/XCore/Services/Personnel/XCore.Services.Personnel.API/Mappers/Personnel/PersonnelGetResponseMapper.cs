using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Personnels;
using XCore.Services.Personnel.Models.DTO.Personnels;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.API.Mappers.Personnels
{
    public class PersonnelGetResponseMapper : IModelMapper<SearchResults<Person>, SearchResultsDTO<PersonnelDTO>>
    {
        #region props.

        private PersonnelMapper PersonnelMapper { get; set; } = new PersonnelMapper();

        public static PersonnelGetResponseMapper Instance { get; } = new PersonnelGetResponseMapper();

        #endregion
        #region cst.

        static PersonnelGetResponseMapper()
        {
        }
        public PersonnelGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<Person> Map(SearchResultsDTO<PersonnelDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<PersonnelDTO> Map(SearchResults<Person> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<PersonnelDTO>();

            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<PersonnelDTO> Map(List<Person> from)
        {
            if (from == null) return null;

            var to = new List<PersonnelDTO>();

            foreach (var fromItem in from)
            {
                var toItem = this.PersonnelMapper.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
