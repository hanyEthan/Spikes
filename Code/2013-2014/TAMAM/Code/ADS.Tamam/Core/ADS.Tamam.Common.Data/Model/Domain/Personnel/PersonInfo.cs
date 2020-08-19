using System;
using ADS.Common.Models.Domain;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    [Serializable]
    public class PersonInfo : IXSerializable
    {
        public string SSN
        {
            get { return _person.SSN; }
            set { _person.SSN = value; }
        }
        public string FullNameCultureVarient
        {
            get { return _person.FullNameCultureVarient; }
            set { _person.FullNameCultureVarient = value; }
        }
        public string FullNameCultureVarientAbstract
        {
            get { return _person.FullNameCultureVarientAbstract; }
            set { _person.FullNameCultureVarientAbstract = value; }
        }
        public DateTime? BirthDate
        {
            get { return _person.BirthDate; }
            set { _person.BirthDate = value; }
        }
        public string BirthDateCultureVarient
        {
            get { return _person.BirthDateCultureVarient; }
            set { _person.BirthDateCultureVarient = value; }
        }
        public int? GenderId
        {
            get { return _person.GenderId; }
            set { _person.GenderId = value; }
        }
        public int? ReligionId
        {
            get { return _person.ReligionId; }
            set { _person.ReligionId = value; }
        }
        public int? NationalityId
        {
            get { return _person.NationalityId; }
            set { _person.NationalityId = value; }
        }
        public string PassportNumber 
        {
            get { return _person.PassportNumber; }
            set { _person.PassportNumber = value; }
        }
        public int? MaritalStatusId
        {
            get { return _person.MaritalStatusId; }
            set { _person.MaritalStatusId = value; }
        }
        public Guid? CustomFieldId { get { return _person.CustomFieldId; } set { _person.CustomFieldId = value; } }

        public DetailCode Gender
        {
            get { return _person.Gender; }
            set { _person.Gender = value; }
        }
        public DetailCode Religion
        {
            get { return _person.Religion; }
            set { _person.Religion = value; }
        }
        public DetailCode Nationality
        {
            get { return _person.Nationality; }
            set { _person.Nationality = value; }
        }
        public DetailCode MaritalStatus
        {
            get { return _person.MaritalStatus; }
            set { _person.MaritalStatus = value; }
        }

        #region UI Helpers
        [XDontSerialize]
        public string BirthDateStr
        {
            get
            {
                if ( BirthDate.HasValue )
                {
                    return BirthDate.Value.ToString ( "yyyy-MM-ddTHH:mm:ss" );
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

        public PersonInfo( Person p )
        {
            _person = p;
        }
        
        #endregion
    }
}
