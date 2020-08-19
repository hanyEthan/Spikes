using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Security.Models.Contracts
{
    public interface ISecurityMessage
    {
          int PersonId { get; set; }
          string AppId { get; set; }
      

    }
}
