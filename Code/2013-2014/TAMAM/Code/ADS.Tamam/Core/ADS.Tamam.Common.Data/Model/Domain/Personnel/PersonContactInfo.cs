using ADS.Common.Contracts;
using System;
using System.Runtime.Serialization;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class PersonContactInfo : IXSerializable
    {
        public string Email
        {
            get { return _person.Email; }
            set { _person.Email = value; }
        }
        public string Phone
        {
            get { return _person.Phone; }
            set { _person.Phone = value; }
        }
        public string Address
        {
            get { return _person.Address; }
            set { _person.Address = value; }
        }

        #region privates

        private Person _person;
        
        #endregion
        #region cst ...

        public PersonContactInfo( Person p )
        {
            _person = p;
        }
        
        #endregion
    }
}
