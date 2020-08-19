using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Config.Contracts;

namespace XCore.Services.Organizations.SDK.Models.Support
{
  public  class OrganizationClientConfig : IConfigData
    {
        public string Endpoint { get; set; }
    }
}
