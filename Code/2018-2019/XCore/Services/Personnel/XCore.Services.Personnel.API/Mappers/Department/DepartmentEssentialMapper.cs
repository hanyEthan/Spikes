using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;

namespace XCore.Services.Personnel.API.Mappers.Departments
{
    public class DepartmentEssentialMapper<TModel, TModelDTO> : IModelMapper<TModel, TModelDTO>
         where TModel : Department, new()
        where TModelDTO : DepartmentEssentialDTO, new()
        
    {
        #region props.

        public static DepartmentEssentialMapper<TModel, TModelDTO> Instance { get; } = new DepartmentEssentialMapper<TModel, TModelDTO>();

        #endregion
        #region cst.

        static DepartmentEssentialMapper()
        {
        }
        public DepartmentEssentialMapper()
        {
        }

        #endregion

        #region IModelMapper

        public virtual TModelDTO Map(TModel from, object metadata = null)
        {
            if (from == null) return null;
            var to = new TModelDTO();
            to.DepartmentReferenceId = from.DepartmentReferenceId;
            to.HeadDepartmentId = from.HeadDepartmentId;
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
            to.DepartmentReferenceId = from.DepartmentReferenceId;
            to.HeadDepartmentId = from.HeadDepartmentId;
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
