using System;
using System.Threading;
using System.Collections.Generic;

using ADS.Common.Contracts;
using ADS.Common.Models;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Domain.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class Person : IXSerializable, IIdentity, IDynamicValuesProvider, IBaseModel
    {
        #region privates ...

        private const int _ConciseNameLength = 25;

        #endregion

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }

        public PersonInfo DetailedInfo { get; set; }
        public PersonContactInfo ContactInfo { get; set; }
        public PersonEmploymentInfo AccountInfo { get; set; }
        public PersonAuthenticationInfo AuthenticationInfo { get; set; }
      
        public Policy.Policy CustomField { get; set; }
      
        [XDontSerialize]
        public IList<EffectiveSchedulePerson> EffectiveSchedulePersonnel { get; set; }

        #region  cst ...

        public Person()
        {
            DetailedInfo = new PersonInfo(this);
            ContactInfo = new PersonContactInfo(this);
            AccountInfo = new PersonEmploymentInfo(this);
            AuthenticationInfo = new PersonAuthenticationInfo(this);          
            EffectiveSchedulePersonnel = new List<EffectiveSchedulePerson>();
        }

        #endregion
        # region internals ...

        #region PersonInfo

        private string _SSN;
        internal string SSN { get { return _SSN; } set { _SSN = value; } }

        private string _FullNameCultureVarient;
        internal string FullNameCultureVarient { get { return _FullNameCultureVarient; } set { _FullNameCultureVarient = value; } }

        internal string FullNameCultureVarientAbstract { get { return _FullNameCultureVarientAbstract; } set { _FullNameCultureVarientAbstract = value; } }
        string _FullNameCultureVarientAbstract;

        private DateTime? _BirthDate;
        internal DateTime? BirthDate { get { return _BirthDate; } set { _BirthDate = value; } }

        private string _BirthDateCultureVarient;
        internal string BirthDateCultureVarient { get { return _BirthDateCultureVarient; } set { _BirthDateCultureVarient = value; } }

        private int? _GenderId;
        internal int? GenderId { get { return _GenderId; } set { _GenderId = value; } }

        private int? _ReligionId;
        internal int? ReligionId { get { return _ReligionId; } set { _ReligionId = value; } }

        private int? _NationalityId;
        internal int? NationalityId { get { return _NationalityId; } set { _NationalityId = value; } }

        private string _PassportNumber;
        internal string PassportNumber { get { return _PassportNumber; } set { _PassportNumber = value; } }

        private int? _MaritalStatusId;
        internal int? MaritalStatusId { get { return _MaritalStatusId; } set { _MaritalStatusId = value; } }

        private DetailCode _Gender;
        internal DetailCode Gender { get { return _Gender; } set { _Gender = value; } }

        private DetailCode _Religion;
        internal DetailCode Religion { get { return _Religion; } set { _Religion = value; } }

        private DetailCode _Nationality;
        internal DetailCode Nationality { get { return _Nationality; } set { _Nationality = value; } }

        private DetailCode _MaritalStatus;
        internal DetailCode MaritalStatus { get { return _MaritalStatus; } set { _MaritalStatus = value; } }

        private DetailCode _Title;
        internal DetailCode Title { get { return _Title; } set { _Title = value; } }

        private DetailCode _EmploymentType;
        internal DetailCode EmploymentType { get { return _EmploymentType; } set { _EmploymentType = value; } }

        private Guid? _CustomFieldId;
        internal Guid? CustomFieldId { get { return _CustomFieldId; } set { _CustomFieldId = value; } }
        #endregion
        #region ContactInfo

        private string _Email;
        internal string Email { get { return _Email; } set { _Email = value; } }

        private string _Phone;
        internal string Phone { get { return _Phone; } set { _Phone = value; } }

        private string _Address;
        internal string Address { get { return _Address; } set { _Address = value; } }

        #endregion
        #region EmploymentInfo

        private string _JoinDateCultureVarient;
        internal string JoinDateCultureVarient { get { return _JoinDateCultureVarient; } set { _JoinDateCultureVarient = value; } }

        private DateTime? _JoinDate;
        internal DateTime? JoinDate { get { return _JoinDate; } set { _JoinDate = value; } }

        private string _SecurityId;
        internal string SecurityId { get { return _SecurityId; } set { _SecurityId = value; } }

        private bool? _ShowAttendance;
        public bool? ShowAttendance { get { return _ShowAttendance; } set { _ShowAttendance = value; } }

        private bool _EnableAttendanceViolations;
        public bool EnableAttendanceViolations { get { return _EnableAttendanceViolations; } set { _EnableAttendanceViolations = value; } }

        private Guid _DepartmentId;
        internal Guid DepartmentId { get { return _DepartmentId; } set { _DepartmentId = value; } }

        private int? _TitleId;
        internal int? TitleId { get { return _TitleId; } set { _TitleId = value; } }

        private int? _EmploymentTypeId;
        internal int? EmploymentTypeId { get { return _EmploymentTypeId; } set { _EmploymentTypeId = value; } }

        private bool _Activated;
        internal bool Activated { get { return _Activated; } set { _Activated = value; } }

        private Guid? _PolicyGroupId;
        internal Guid? PolicyGroupId { get { return _PolicyGroupId; } set { _PolicyGroupId = value; } }

        private Guid? _ReportingToId;
        internal Guid? ReportingToId { get { return _ReportingToId; } set { _ReportingToId = value; } }

        private Department _Department;
        internal Department Department { get { return _Department; } set { _Department = value; } }

        private PolicyGroup _PolicyGroup;
        internal PolicyGroup PolicyGroup { get { return _PolicyGroup; } set { _PolicyGroup = value; } }

        private Person _ReportingTo;
        internal Person ReportingTo { get { return _ReportingTo; } set { _ReportingTo = value; } }

        private string _ManagerName;
        internal string ManagerName { get { return _ManagerName; } set { _ManagerName = value; } }

        private string _ManagerNameCultureVarient;
        internal string ManagerNameCultureVarient { get { return _ManagerNameCultureVarient; } set { _ManagerNameCultureVarient = value; } }

        #endregion
        # region Authentication Info

        private AuthenticationMode _AuthenticationMode = AuthenticationMode.Tamam;
        internal AuthenticationMode AuthenticationMode { get { return _AuthenticationMode; } set { _AuthenticationMode = value; } }

        private string _Username;
        internal string Username { get { return _Username; } set { _Username = value; } }

        private string _Password;
        internal string Password { get { return _Password; } set { _Password = value; } }

        private string _ProviderName;
        internal string ProviderName { get { return _ProviderName; } set { _ProviderName = value; } }

        private bool _IsLocked;
        internal bool IsLocked { get { return _IsLocked; } set { _IsLocked = value; } }

        # endregion

        # endregion
        # region Helpers

        [XDontSerialize]
        public string Name
        {
            get
            {
                if (FullName.Length >= _ConciseNameLength)
                    return string.Format("{0}...", FullName.Substring(0, _ConciseNameLength));
                else return FullName;
            }
        }
        [XDontSerialize]
        public string NameArabic
        {
            get
            {
                if (FullNameCultureVarient.Length >= _ConciseNameLength)
                    return string.Format("{0}...", FullNameCultureVarient.Substring(0, _ConciseNameLength));
                else return FullNameCultureVarient;
            }
        }
        [XDontSerialize]
        public string ManagerFullName
        {
            get
            {
                if (!string.IsNullOrEmpty(AccountInfo.ManagerName) && AccountInfo.ManagerName.Length >= _ConciseNameLength) return string.Format("{0}...", AccountInfo.ManagerName.Substring(0, _ConciseNameLength));
                else return AccountInfo.ManagerName;
            }
        }
        [XDontSerialize]
        public string ManagerFullNameArabic
        {
            get
            {
                if (!string.IsNullOrEmpty(AccountInfo.ManagerNameCultureVarient) && AccountInfo.ManagerNameCultureVarient.Length >= _ConciseNameLength) return string.Format("{0}...", AccountInfo.ManagerNameCultureVarient.Substring(0, _ConciseNameLength));
                else return AccountInfo.ManagerNameCultureVarient;
            }
        }

        public string GetLocalizedFullName()
        {
            return Thread.CurrentThread.CurrentCulture.Name == "ar-EG"
                ? this.DetailedInfo.FullNameCultureVarient
                : this.FullName;
        }

        # endregion

        #region IDynamicValuesProvider

        [XDontSerialize]
        public IList<PolicyFieldValue> Values { get { return CustomField != null ? CustomField.Values : new List<PolicyFieldValue>(); } }

        #endregion
        #region IIdentity

        [XDontSerialize]
        string IIdentity.Username
        {
            get { return AuthenticationInfo.Username; }
            set { AuthenticationInfo.Username = value; }
        }
        [XDontSerialize]
        string IIdentity.Password
        {
            get { return AuthenticationInfo.Password; }
            set { AuthenticationInfo.Password = value; }
        }
        [XDontSerialize]
        string IIdentity.ProviderName
        {
            get { return AuthenticationInfo.AuthenticationMode.ToString(); }
            set
            {
                try
                {
                    AuthenticationMode v;
                    if (Enum.TryParse<AuthenticationMode>(value, out v)) AuthenticationInfo.AuthenticationMode = v;
                }
                catch
                {
                }
            }
        }

        #endregion

        # region IBaseModel

        object IBaseModel.Id
        {
            get
            {
                return Id;
            }
        }
        string IBaseModel.Name
        {
            get
            {
                return FullName;
            }
        }
        [XDontSerialize]
        public string NameCultureVariant
        {
            get
            {
                return DetailedInfo.FullNameCultureVarient;
            }
        }

        # endregion
    }
}
