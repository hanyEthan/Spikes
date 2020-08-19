using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Organizations;
using XCore.Services.Personnel.Models.DTO.Organizations;
using XCore.Services.Personnel.Models.DTO.Essential.Organizations;

namespace XCore.Services.Personnel.API.Mappers.Organizations
{
    public class OrganizationEssentialMapper<TModel, TModelDTO> : IModelMapper<TModel, TModelDTO>
         where TModel : Organization, new()
        where TModelDTO : OrganizationEssentialDTO, new()
        
    {
        #region props.

        public static OrganizationEssentialMapper<TModel, TModelDTO> Instance { get; } = new OrganizationEssentialMapper<TModel, TModelDTO>();

        #endregion
        #region cst.

        static OrganizationEssentialMapper()
        {
        }
        public OrganizationEssentialMapper()
        {
        }

        #endregion

        #region IModelMapper

        public virtual TModelDTO Map(TModel from, object metadata = null)
        {
            if (from == null) return null;

            var to = new TModelDTO();
            to.OrganizationReferenceId = from.OrganizationReferenceId;
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
            to.OrganizationReferenceId = from.OrganizationReferenceId;
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
