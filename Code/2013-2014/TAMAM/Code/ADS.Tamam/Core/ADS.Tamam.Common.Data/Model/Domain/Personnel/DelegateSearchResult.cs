using System;
using System.Collections.Generic;

using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class PersonnelDelegatesSearchResult : IXSerializable
    {
        public List<PersonDelegate> PersonnelDelegates { get; set; }
        public long TotalCount { get; set; }

        public PersonnelDelegatesSearchResult()
        {
            PersonnelDelegates = new List<PersonDelegate>();
        }
    }
}
