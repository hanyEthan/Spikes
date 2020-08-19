using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Personnels;
using XCore.Services.Personnel.Models.DTO.Personnels;
using XCore.Services.Personnel.Models.DTO.Essential.Personnels;
using XCore.Framework.Infrastructure.Context.Services.Models;

namespace XCore.Services.Personnel.API.Mappers.Personnels
{
    public class PersonnelEssentialMapper<TModel, TModelDTO> : IModelMapper<TModel, TModelDTO>
        where TModel : Person, new()
        where TModelDTO : PersonnelEssentialDTO, new()
    {
        #region props.

        public static PersonnelEssentialMapper<TModel, TModelDTO> Instance { get; } = new PersonnelEssentialMapper<TModel, TModelDTO>();

        #endregion
        #region cst.

        static PersonnelEssentialMapper()
        {
        }
        public PersonnelEssentialMapper()
        {
        }

        #endregion

        #region IModelMapper

        public virtual TModelDTO Map(TModel from, object metadata = null)
        {
            if (from == null) return null;

            var to = new TModelDTO();

            to.ManagerId = from.ManagerId;
            to.DepartmentId = from.DepartmentId;
            
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
            to.ManagerId = from.ManagerId;
            to.DepartmentId = from.DepartmentId;

            #region Common

            to.Id = from.Id;
            to.Code = from.Code;
            to.IsActive = from.IsActive ?? true;
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.MetaData = from.MetaData;

            to.AppId = (metadata as IServiceExecutionRequest)?.AppId;
            to.ModuleId = (metadata as IServiceExecutionRequest)?.ModuleId;

            #endregion

            return to;
        }

        #endregion
    }
}
