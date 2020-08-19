using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class Location : IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<Schedule> Schedules { get; set; }

        #region cst

        public Location()
        {
            Schedules = new List<Schedule>();
        }

        #endregion
    }
}
