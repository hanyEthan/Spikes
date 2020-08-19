using System;
using System.Collections.Generic;
using ADS.Common.Models;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    [Serializable]
    public class Department : IXSerializable, IBaseModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string NameCultureVarientAbstract { get; set; }
        public string Description { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public Guid? SupervisorId { get; set; }
        public Guid? PolicyGroupId { get; set; }

        public Department ParentDepartment { get; set; }
       
    
        public PolicyGroup PolicyGroup { get; set; }

        public string Hashcode { get; set; }

      
        public Department()
        {
          
        }

        # region IBaseModel
        
        object IBaseModel.Id
        {
            get
            {
                return Id;
            }
        }
        [XDontSerialize]
        public string NameCultureVariant
        {
            get { return NameCultureVarient; }
        }

        # endregion
    }
}