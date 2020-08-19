using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.HiringProcesses;
using XCore.Services.Hiring.API.Mappers.Organizations;
using XCore.Services.Hiring.API.Mappers.Positions;
using XCore.Services.Hiring.API.Mappers.Questions;
using XCore.Services.Hiring.API.Mappers.Roles;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Advertisements
{
    public class AdvertisementMapper : IModelMapper<Advertisement, AdvertisementDTO>
    {
        #region props.

        public static AdvertisementMapper Instance { get; } = new AdvertisementMapper();

        #endregion

        #region IModelMapper

        public AdvertisementDTO Map(Advertisement from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AdvertisementDTO
            {
                AppId = from.AppId,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                HiringProccesId = from.HiringProccesId,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                ModuleId = from.ModuleId,
                Title = from.Title,
                Name = from.Name,
                NameCultured = from.NameCultured,
                RoleId = from.RoleId,
                OrganizationId = from.OrganizationId,

                Positions = Map(from.Positions),
                Questions = Map(from.Questions),
                Role = RoleMapper.Instance.Map(from.Role),
                HiringProcces = HiringProcessMapper.Instance.Map(from.HiringProcces),
                Organization = OrganizationMapper.Instance.Map(from.Organization),
                //Skills = Map(from.Skills),
            };

            return to;
        }       

        public Advertisement Map(AdvertisementDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Advertisement
            {
                AppId = from.AppId,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                HiringProccesId = from.HiringProccesId,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                ModuleId = from.ModuleId,
                Title = from.Title,
                Name = from.Name,
                NameCultured = from.NameCultured,
                RoleId = from.RoleId,
                OrganizationId = from.OrganizationId,

                Positions = Map(from.Positions),
                Questions = Map(from.Questions),
                Role = RoleMapper.Instance.Map(from.Role),
                HiringProcces = HiringProcessMapper.Instance.Map(from.HiringProcces),
                Organization = OrganizationMapper.Instance.Map(from.Organization),
                //Skills = Map(from.Skills),
            };
            return to;
        }

        #endregion

        #region helpers.

        #region Position
        private IList<PositionDTO> Map(IList<Position> from)
        {
            if (from == null) return null;

            var to = new List<PositionDTO>();

            foreach (var fromItem in from)
            {
                var toItem = PositionMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private IList<Position> Map(IList<PositionDTO> from)
        {
            if (from == null) return null;

            var to = new List<Position>();

            foreach (var fromItem in from)
            {
                var toItem = PositionMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }

        #endregion

        #region Question
        private IList<QuestionDTO> Map(IList<Question> from)
        {
            if (from == null) return null;

            var to = new List<QuestionDTO>();

            foreach (var fromItem in from)
            {
                var toItem = QuestionMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private IList<Question> Map(IList<QuestionDTO> from)
        {
            if (from == null) return null;

            var to = new List<Question>();

            foreach (var fromItem in from)
            {
                var toItem = QuestionMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }

        #endregion



        #endregion

    }
}