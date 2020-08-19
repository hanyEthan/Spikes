using ADS.Common.Contracts;
using System;

namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    [Serializable]
    public class ReportDefinition : IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVariant { get; set; }
        public string FiltersUserControlPath { get; set; }
        public string ReportDataFullName { get; set; }
        public string ReportServiceCriteriaFullName { get; set; }
        public string ReportPath { get; set; }
        public string ReportPathCultureVariant { get; set; }
        public string Privilege { get; set; }
        public bool HasFilters { get; set; }
    }
}
