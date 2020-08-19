using System;
using ADS.Common.Context;
using ADS.Tamam.Common.Data.Model.Enums;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Context
{
    [Serializable]
    public class SecurityContext : RequestContextBase
    {
        #region props ...

        public Guid? PersonId { get; set; }
        public string Username { get; set; }

        private AuthorizationVisibilityMode _VisibilityMode = AuthorizationVisibilityMode.None;
        public AuthorizationVisibilityMode VisibilityMode
        {
            get
            {
                return _VisibilityMode;
                //switch ( _VisibilityMode )
                //{
                //    case AuthorizationVisibilityMode.Personnel:
                //        return !string.IsNullOrEmpty( PersonnelRange ) ? AuthorizationVisibilityMode.Personnel : AuthorizationVisibilityMode.None;
                //        break;
                //    case AuthorizationVisibilityMode.Departments:
                //        return !string.IsNullOrEmpty( DepartmentsRange ) ? AuthorizationVisibilityMode.Departments : AuthorizationVisibilityMode.None;
                //        break;
                //    default:
                //        return _VisibilityMode;
                //        break;
                //}
            }
            set
            {
                _VisibilityMode = value;
            }
        }

        public string DepartmentsRange { get; set; }
        public List<string> PersonnelRange { get; set; }

        #endregion
        #region cst ...

        public SecurityContext()
        {

        }
        public SecurityContext( Guid? personId ) : this()
        {
            PersonId = personId;
        }
        public SecurityContext( Guid? personId , string username ) : this( personId )
        {
            Username = username;
        }

        #endregion
        #region helpers ...

        public override string ToString()
        {
            return string.Format("{0}", PersonId);
        }

        #endregion
    }
}