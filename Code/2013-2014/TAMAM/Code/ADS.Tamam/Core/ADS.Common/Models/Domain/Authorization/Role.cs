using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Common.Models.Domain.Authorization
{
    [Serializable]
    public class Role : IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SystemRole { get; set; }

        public virtual IList<Privilege> Privileges { get; set; }

        #region cst ...

        public Role()
        {
            //Privileges = new List<Privilege>();
        }

        #endregion
    }
}
