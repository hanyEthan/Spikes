using ADS.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Common.Models.Domain
{
    [Serializable]
    public class DetailCode : IXSerializable
    {
        // basic properties
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVariant { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentId { get; set; }
        public int MasterCodeId { get; set; }

        // extra info properties
        public string FieldOneValue { get; set; }
        public string FieldTwoValue { get; set; }
        public string FieldThreeValue { get; set; }

        // created and updated properties
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // navigation properties
        public MasterCode MasterCode { get; set; }
        public DetailCode ParentDetailCode { get; set; }
        public IList<DetailCode> ChildDetailCodes { get; set; }

        public DetailCode()
        {
            ChildDetailCodes = new List<DetailCode>();
        }
    }
}
