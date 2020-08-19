using ADS.Common.Contracts;
using System;

namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    [Serializable]
    public class DashboardWebPart : IXSerializable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TitleCultureVarient { get; set; }
        public string TitleUrl { get; set; }
        public string Path { get; set; }
        public string Privilege { get; set; }
        public int PartSize { get; set; }
        public int Sequence { get; set; }
        public bool IsSelfService { get; set; }
        public string ReferenceData { get; set; }
        public int PartType { get; set; }
    }

    public enum DashboardWebPartType
    {
        Managerial = 0,
        Personal = 1,
        Map = 2,
    }
}
