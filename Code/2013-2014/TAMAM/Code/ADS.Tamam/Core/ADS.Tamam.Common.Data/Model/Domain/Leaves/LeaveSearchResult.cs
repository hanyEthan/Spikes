using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveSearchResult : IXSerializable
    {
        public List<Leave> Leaves { get; set; }
        public long TotalCount { get; set; }
        public LeavesStatistics Statistics { get; set; }

        public LeaveSearchResult()
        {
            Leaves = new List<Leave>();
        }
    }

    [Serializable]
    public class LeavesStatistics : IXSerializable
    {
        public LeavesStatistics()
        {
            Types = new List<StatisticalItem>();
            Statuses = new List<StatisticalItem>();
        }
        public LeavesStatistics( IEnumerable<Leave> leaves , IEnumerable<DetailCode> leaveTypes , IEnumerable<DetailCode> leaveStatuses )
        {
            // Fill Leave Type Statistics
            Types = new List<StatisticalItem>();
            foreach ( var item in leaveTypes )
            {
                var count = leaves.Count( x => x.LeaveTypeId == item.Id );
                Types.Add( new StatisticalItem() { Item = item , Count = count } );
            }

            // Fill Leave Status Statistics
            Statuses = new List<StatisticalItem>();
            foreach ( var item in leaveStatuses )
            {
                var count = leaves.Count( x => x.LeaveStatusId == item.Id );
                Statuses.Add( new StatisticalItem() { Item = item , Count = count } );
            }
        }

        public List<StatisticalItem> Types { get; set; }
        public List<StatisticalItem> Statuses { get; set; }
    }

    [Serializable]
    public class StatisticalItem : IXSerializable
    {
        private const int _ConciseNameLength = 12;

        public StatisticalItem()
        {
        }

        public DetailCode Item { get; set; }
        public int Count { get; set; }
        [XDontSerialize]
        public string Color { get { return Item != null && !string.IsNullOrEmpty( Item.FieldOneValue ) ? Item.FieldOneValue : "Black"; } }
        [XDontSerialize]
        public string ItemName { get { return Item != null && !string.IsNullOrEmpty( Item.Name ) ? Item.Name : string.Empty; } }
        [XDontSerialize]
        public string ItemNameCultureVariant { get { return Item != null && !string.IsNullOrEmpty( Item.NameCultureVariant ) ? Item.NameCultureVariant : string.Empty; } }

        [XDontSerialize]
        public string ItemName_Truncated { get { return TruncateName( ItemName ); } }

        [XDontSerialize]
        public string ItemNameCultureVariant_Truncated { get { return TruncateName( ItemNameCultureVariant ); } }

        private string TruncateName( string name )
        {
            if ( name.Length > _ConciseNameLength )
                return string.Format( "{0}.." , name.Substring( 0 , _ConciseNameLength ) );
            else return name;
        }
    }
}
