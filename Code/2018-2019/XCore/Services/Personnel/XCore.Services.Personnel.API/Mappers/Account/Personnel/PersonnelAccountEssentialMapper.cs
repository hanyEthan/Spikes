using System;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class PersonnelAccountEssentialMapper<TModel, TModelDTO> : IModelMapper<TModel, TModelDTO>
         where TModel : PersonnelAccount, new()
        where TModelDTO : PersonnelAccountEssentialDTO, new()
        
    {
        #region props.

        public static PersonnelAccountEssentialMapper<TModel, TModelDTO> Instance { get; } = new PersonnelAccountEssentialMapper<TModel, TModelDTO>();

        #endregion
        #region cst.

        static PersonnelAccountEssentialMapper()
        {
        }
        public PersonnelAccountEssentialMapper()
        {
        }

        #endregion
        #region IModelMapper

        public virtual TModelDTO Map(TModel from, object metadata = null)
        {
            if (from == null) return null;
            var to = new TModelDTO();
            to.PersonId = from.PersonId;
            #region Common
            to.Id = from.Id;
            to.Code = from.Code;
            to.IsActive = from.IsActive;
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.MetaData = from.MetaData;
            #endregion
            return to;
        }

        public virtual TModel Map(TModelDTO from, object metadata = null)
        {
            if (from == null) return null;
            var to = new TModel();
            to.PersonId = from.PersonId;
            #region Common
            to.Id = from.Id;
            to.Code = from.Code;
            to.IsActive = from.IsActive ?? true;
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.MetaData = from.MetaData;
            #endregion
            return to;
        }

        #endregion
    }
}
