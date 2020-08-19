using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    [Serializable]
    public class OrganizationDetail : IXSerializable
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string Address { get; set; }
        public string AddressCultureVarient { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public byte[] Logo { get; set; }
    }
}
