using System;
using ADS.Common.Contracts;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class PersonSearchResult : IXSerializable
    {
        public List<Person> Persons { get; set; }
        public long ResultTotalCount { get; set; }

        #region cst ...

        public PersonSearchResult()
        {
            Persons = new List<Person>();
        }
        
        #endregion
    }
}
