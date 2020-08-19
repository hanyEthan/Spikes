using ADS.Common.Contracts;
using System;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    [Serializable]
    public class ReportCategory : IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVariant { get; set; }
        public bool IsQuick { get; set; }

        public IList<ReportDefinition> Reports { get; set; } 

        public ReportCategory()
        {
            Reports = new List<ReportDefinition>();
        }
    }
}
