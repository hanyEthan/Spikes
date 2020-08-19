using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.Core.Models.Relations
{
    public class RoleClaim
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
