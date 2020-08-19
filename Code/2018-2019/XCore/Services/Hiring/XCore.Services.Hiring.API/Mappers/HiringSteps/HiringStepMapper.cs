using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.HiringProcesses;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.HiringSteps
{
    public class HiringStepMapper : IModelMapper<HiringStep, HiringStepDTO>
    {
        #region props.
        public static HiringStepMapper Instance { get; } = new HiringStepMapper();

        #endregion

        #region IModelMapper

        public HiringStepDTO Map(HiringStep from, object metadata = null)
        {
            if (from == null) return null;

            var to = new HiringStepDTO
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                 Name = from.Name,
                NameCultured = from.NameCultured,
                HiringProcessId = from.HiringProcessId,

                //HiringProcess = HiringProcessMapper.Instance.Map(from.HiringProcess),

            };

            return to;
        }
        public HiringStep Map(HiringStepDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new HiringStep {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                Name = from.Name,
                NameCultured = from.NameCultured,
                HiringProcessId = from.HiringProcessId,
                //HiringProcess = HiringProcessMapper.Instance.Map(from.HiringProcess),
            };
            return to;
        }

        #endregion
       
    }
}
