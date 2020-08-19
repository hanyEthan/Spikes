using ADS.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Common.Models.Domain
{
    [Serializable]
    public class MasterCode : IXSerializable
    {
        
        // basic properties
        public int Id { get; set; }
        public string Code { get; set; }
        public string  Name { get; set; }
        public string NameCultureVariant { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentId { get; set; }

        // extra info properties
        public string FieldOneTitle { get; set; }
        public string FieldOneTitleCultureVariant { get; set; }
        public bool FieldOneIsVisible { get; set; }
        public string FieldTwoTitle { get; set; }
        public string FieldTwoTitleCultureVariant { get; set; }
        public bool FieldTwoIsVisible { get; set; }
        public string FieldThreeTitle { get; set; }
        public string FieldThreeTitleCultureVariant { get; set; }
        public bool FieldThreeIsVisible { get; set; }

        // created and updated properties
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // navigation properties
        public IList<DetailCode> DetailCodes { get; set; }
        public MasterCode ParentMasterCode { get; set; }
        public IList<MasterCode> ChildMasterCodes { get; set; }

        public MasterCode()
        {
            DetailCodes = new List<DetailCode>();
            ChildMasterCodes = new List<MasterCode>();
        }
    }
}
