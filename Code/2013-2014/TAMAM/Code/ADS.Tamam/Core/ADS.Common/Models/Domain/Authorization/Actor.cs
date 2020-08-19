using System;
using System.Collections.Generic;
using ADS.Common.Contracts;
using ADS.Common.Contracts.Security;

namespace ADS.Common.Models.Domain.Authorization
{
    [Serializable]
    public class Actor : IAuthorizationActor , IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public virtual IList<Privilege> Privileges { get; set; }
        public virtual IList<Role> Roles { get; set; }

        #region MyRegion

        public Actor()
        {
            Privileges = new List<Privilege>();
            Roles = new List<Role>();
        }

        #endregion
    }
}
