using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class ExcuseSearchResult : IXSerializable
    {
        public List<Excuse> Excuses { get; set; }
        public long TotalCount { get; set; }
        public ExcuseStatistics Statistics { get; set; }

        public ExcuseSearchResult()
        {
            Excuses = new List<Excuse>();
        }
    }

    [Serializable]
    public class ExcuseStatistics:IXSerializable
    {
        public ExcuseStatistics()
        {
            Statuses = new List<StatisticalItem>();
        }
        public ExcuseStatistics(IEnumerable<Excuse> excuses, IEnumerable<DetailCode> excuseStatuses)
        {
            // Fill Excuse Status Statistics
            Statuses = new List<StatisticalItem>();
            foreach ( var item in excuseStatuses )
            {
                var count = excuses.Count( x => x.ExcuseStatusId == item.Id );
                Statuses.Add( new StatisticalItem() { Item = item , Count = count } );
            }
        }

        public List<StatisticalItem> Statuses { get; set; }
    }
}
