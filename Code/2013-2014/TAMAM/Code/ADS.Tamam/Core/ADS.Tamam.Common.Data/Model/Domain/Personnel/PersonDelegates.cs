using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class PersonDelegate : IXSerializable
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        public Guid DelegateId { get; set; }
        public Person Delegate { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Code { get; set; }
    }
}
