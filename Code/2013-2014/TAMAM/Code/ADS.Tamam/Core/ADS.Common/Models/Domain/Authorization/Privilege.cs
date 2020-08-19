using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Common.Models.Domain.Authorization
{
    [Serializable]
    public class Privilege : IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual IList<Action> Actions { get; set; }
        public virtual IList<Role> Roles { get; set; }

        #region cst ...

        public Privilege()
        {
            Actions = new List<Action>();
            Roles = new List<Role>();
        }

        #endregion
    }
}
