using System;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class PersonAuthenticationInfo : IXSerializable
    {
        public AuthenticationMode AuthenticationMode
        {
            get { return _person.AuthenticationMode; }
            set { _person.AuthenticationMode = value; }
        }
        
        public string Username
        {
            get { return _person.Username; }
            set { _person.Username = value; }
        }
        public string Password
        {
            get { return _person.Password; }
            set { _person.Password = value; }
        }
        public string ProviderName
        {
            get { return _person.ProviderName; }
            set { _person.ProviderName = value; }
        }
        public bool IsLocked
        {
            get { return _person.IsLocked; }
            set { _person.IsLocked = value; }
        }

        #region privates

        private Person _person;

        #endregion
        #region cst ...

        public PersonAuthenticationInfo(Person p)
        {
            _person = p;
        }

        #endregion
    }
}
