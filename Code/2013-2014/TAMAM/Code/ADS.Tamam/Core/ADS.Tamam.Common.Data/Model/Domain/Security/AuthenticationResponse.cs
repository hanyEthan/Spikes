using System;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;

namespace ADS.Tamam.Common.Data.Model.Domain.Security
{
    [Serializable]
    public class AuthenticationResponse : IXSerializable
    {
        #region props.

        public Person Person { get; set; }
        public AuthenticationStatus Status { get; set; }

        #endregion
        #region helpers.

        public AuthenticationResponse Set( AuthenticationStatus status )
        {
            this.Status = status;
            return this;
        }
        public AuthenticationResponse Set( Person person )
        {
            this.Person = person;
            return this;
        }
        public AuthenticationResponse Set( AuthenticationStatus status , Person person )
        {
            this.Status = status;
            this.Person = person;
            return this;
        }

        #endregion
    }
}
