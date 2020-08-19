using System;
using System.Collections.Generic;
using ADS.Common.Contracts;
using ADS.Common.Contracts.Security;

namespace ADS.Common.Models.Domain.Authorization
{
    [Serializable]
    public class Action : IAuthorizationTarget , IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual IList<Privilege> Privileges { get; set; }

        #region cst ...

        public Action()
        {
            Privileges = new List<Privilege>();
        }

        #endregion
    }
}