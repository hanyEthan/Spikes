using System;
using System.Collections.Generic;
using System.Text;

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface IFullAudited : IAudited, IDeletionAudited
    {
        
    }
}
