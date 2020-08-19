using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.Core.Models.Relations
{
    public class ActorClaim
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
