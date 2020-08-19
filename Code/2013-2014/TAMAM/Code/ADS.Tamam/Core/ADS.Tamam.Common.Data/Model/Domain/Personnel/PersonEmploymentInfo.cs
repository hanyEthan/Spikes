using System;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class PersonEmploymentInfo : IXSerializable
    {
        public string JoinDateCultureVarient
        {
            get { return _person.JoinDateCultureVarient; }
            set { _person.JoinDateCultureVarient = value; }
        }
        public DateTime? JoinDate
        {
            get { return _person.JoinDate; }
            set { _person.JoinDate = value; }
        }
        public string SecurityId
        {
            get { return _person.SecurityId; }
            set { _person.SecurityId = value; }
        }
        public bool? ShowAttendance
        {
            get { return _person.ShowAttendance; }
            set { _person.ShowAttendance = value; }
        }
        public bool EnableAttendanceViolations
        {
            get { return _person.EnableAttendanceViolations; }
            set { _person.EnableAttendanceViolations = value; }
        }
        public Guid DepartmentId
        {
            get { return _person.DepartmentId; }
            set { _person.DepartmentId = value; }
        }
        public int? TitleId
        {
            get { return _person.TitleId; }
            set { _person.TitleId = value; }
        }
        public int? EmploymentTypeId
        {
            get { return _person.EmploymentTypeId; }
            set { _person.EmploymentTypeId = value; }
        }
        public bool Activated
        {
            get { return _person.Activated; }
            set { _person.Activated = value; }
        }
        public Guid? PolicyGroupId
        {
            get { return _person.PolicyGroupId; }
            set { _person.PolicyGroupId = value; }
        }
        public DetailCode EmploymentType
        {
            get { return _person.EmploymentType; }
            set { _person.EmploymentType = value; }
        }
        public DetailCode Title
        {
            get { return _person.Title; }
            set { _person.Title = value; }
        }
        public Department Department
        {
            get { return _person.Department; }
            set { _person.Department = value; }
        }
        public PolicyGroup PolicyGroup
        {
            get { return _person.PolicyGroup; }
            set { _person.PolicyGroup = value; }
        }
        public Guid? ReportingToId
        {
            get { return _person.ReportingToId; }
            set { _person.ReportingToId = value; }
        }
        public Person ReportingTo
        {
            get { return _person.ReportingTo; }
            set { _person.ReportingTo = value; }
        }

        public string ManagerName
        {
            get { return _person.ManagerName; }
            set { _person.ManagerName = value; }
        }
        public string ManagerNameCultureVarient
        {
            get { return _person.ManagerNameCultureVarient; }
            set { _person.ManagerNameCultureVarient = value; }
        }

        #region UI Helpers
        [XDontSerialize]
        public string JoinDateStr
        {
            get
            {
                if ( JoinDate.HasValue )
                {
                    return JoinDate.Value.ToString( "yyyy-MM-ddTHH:mm:ss" );
                }
                else
                    return string.Empty;
            }
        }
        #endregion
        #region privates

        private Person _person;

        #endregion
        #region cst ...

        public PersonEmploymentInfo( Person p )
        {
            _person = p;
        }

        #endregion
    }
}
