using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Utilities;
using XCore.Services.Geo.API.Models;
using XCore.Services.Geo.Core.Models.Commands;

namespace XCore.Services.Geo.API.Mappers
{
    public class AddLocationRequestMapper : IModelMapper<AddLocationRequestDomain, AddLocationRequestDTO>
    {
        #region props.

        private LocationEventMapper locationEventMapper { get; set; }

        #endregion
        #region cst.

        public AddLocationRequestMapper()
        {
            this.locationEventMapper = new LocationEventMapper();
        }

        #endregion

        #region IModelMapper

        public AddLocationRequestDomain Map(AddLocationRequestDTO from, object metadata = null)
        {
            try
            {
                if (from == null) return null;

                var to = new AddLocationRequestDomain
                {
                    Location = this.locationEventMapper.Map(@from.Location)
                };


                return to;
            }
            catch (Exception e)
            {
                XLogger.Error("Exception : " + e);
                return null;
            }
        }
        public AddLocationRequestDTO Map(AddLocationRequestDomain from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
