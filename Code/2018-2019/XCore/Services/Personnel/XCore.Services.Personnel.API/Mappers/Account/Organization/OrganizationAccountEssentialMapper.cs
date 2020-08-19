using System;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class OrganizationAccountEssentialMapper<TModel, TModelDTO> : IModelMapper<TModel, TModelDTO>
         where TModel : OrganizationAccount, new()
        where TModelDTO : OrganizationAccountEssentialDTO, new()
    {
        #region props.

        public static OrganizationAccountEssentialMapper<TModel, TModelDTO> Instance { get; } = new OrganizationAccountEssentialMapper<TModel, TModelDTO>();

        #endregion
        #region cst.

        static OrganizationAccountEssentialMapper()
        {
        }
        public OrganizationAccountEssentialMapper()
        {
        }

        #endregion

        #region IModelMapper

        public virtual TModelDTO Map(TModel from, object metadata = null)
        {
            if (from == null) return null;

            var to = new TModelDTO();
            to.OrganizationId = from.OrganizationId;
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
            to.OrganizationId = from.OrganizationId;
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
