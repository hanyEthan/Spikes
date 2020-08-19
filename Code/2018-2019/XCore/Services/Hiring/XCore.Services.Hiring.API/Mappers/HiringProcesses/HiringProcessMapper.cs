using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.HiringSteps;
using XCore.Services.Hiring.API.Mappers.Organizations;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.HiringProcesses
{
    public class HiringProcessMapper : IModelMapper<HiringProcess, HiringProcessDTO>
    {
        #region props.

        public static HiringProcessMapper Instance { get; } = new HiringProcessMapper();

        #endregion

        #region IModelMapper

        public HiringProcessDTO Map(HiringProcess from, object metadata = null)
        {
            if (from == null) return null;

            var to = new HiringProcessDTO
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
                OrganizationId = from.OrganizationId,
                NameCultured = from.NameCultured,
                HiringSteps = Map(from.HiringSteps),
                //Organization = OrganizationMapper.Instance.Map(from.Organization),
                

            };

            return to;
        }
        public HiringProcess Map(HiringProcessDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new HiringProcess
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
                OrganizationId = from.OrganizationId,
                NameCultured = from.NameCultured,
                HiringSteps = Map(from.HiringSteps),
                //Organization = OrganizationMapper.Instance.Map(from.Organization),
            };
            return to;
        }

        #endregion

        #region helpers.
        #region Answer
        private IList<HiringStepDTO> Map(IList<HiringStep> from)
        {
            if (from == null) return null;

            var to = new List<HiringStepDTO>();

            foreach (var fromItem in from)
            {
                var toItem = HiringStepMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private IList<HiringStep> Map(IList<HiringStepDTO> from)
        {
            if (from == null) return null;

            var to = new List<HiringStep>();

            foreach (var fromItem in from)
            {
                var toItem = HiringStepMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }

        #endregion 
        #endregion

    }
}
