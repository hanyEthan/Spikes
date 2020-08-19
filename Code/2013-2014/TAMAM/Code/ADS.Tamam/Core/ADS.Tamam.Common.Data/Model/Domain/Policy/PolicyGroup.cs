using System;
using System.Collections.Generic;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy
{
    [Serializable]
    public class PolicyGroup : IXSerializable
    {
        #region props

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string NameCultureVarientAbstract { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }

        public IList<Policy> Policies { get; set; }
        public IList<Department> Departments { get; set; }

        #endregion
        #region cst ...

        public PolicyGroup()
        {
            Policies = new List<Policy>();
            Departments = new List<Department>();
        }

        public PolicyGroup(string name, string nameCultureVariant, string description, bool isActive) : this()
        {
            Name = name;
            NameCultureVarient = nameCultureVariant;
            NameCultureVarientAbstract = nameCultureVariant;
            Description = description;
            IsActive = isActive;
            IsSystem = false;
        }

        public PolicyGroup(Guid id, string name, string nameCultureVariant, string description, bool isActive) : this(name, nameCultureVariant, description, isActive)
        {
            Id = id;
        }

        #endregion
    }
}