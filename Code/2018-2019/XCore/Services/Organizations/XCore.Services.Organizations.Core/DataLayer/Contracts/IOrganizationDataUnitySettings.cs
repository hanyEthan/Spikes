using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Organizations.Core.DataLayer.Contracts
{
    public interface IOrganizationDataUnitySettings
    {
        string DBConnectionName { get; }
    }
}
