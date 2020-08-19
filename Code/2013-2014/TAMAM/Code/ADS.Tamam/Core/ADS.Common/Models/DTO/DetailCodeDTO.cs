using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Common.Models.DTO
{
    public class DetailCodeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVariant { get; set; }
        public int MasterCodeId { get; set; }
    }
}
