using System;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class ExcuseReview : IXSerializable
    {
        public Guid ExcuseId { get; set; }
        public ReviewExcuseStatus Status { get; set; }
        public Guid ReviewerId { get; set; }
        public string Comment { get; set; }
    }
}
