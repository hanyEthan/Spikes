using System;
using ADS.Tamam.Modules.Integration.Helpers;

namespace ADS.Tamam.Modules.Integration.Models
{
    public class Person : ILoggable
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameVariant { get; set; }
        public string JobTitleCode { get; set; }
        public string DepartmentCode { get; set; }
        public string ReportingToCode { get; set; }
        public string GenderCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime Birthdate { get; set; }
        public string Username { get; set; }
        public string UsernameType { get; set; }
        public string Password { get; set; }

        public string ReligionCode { get; set; }
        public string NationalityCode { get; set; }
        public string SSN { get; set; }
        public string PassportNumber { get; set; }
        public string MaritalStatusCode { get; set; }
        public string Address { get; set; }

        public bool Activated { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool isSynced { get; set; }

        public string GetLoggingData()
        {
            return string.Format(
                "Code [{0}] Name [{1}] NameVariant [{2}] JobTitleCode [{3}] DepartmentCode [{4}] ReportingToCode [{5}] GenderCode [{6}] Phone [{7}] Email [{8}] JoinDate [{9}] Birthdate [{10}] Username [{11}] UsernameType [{12}] Password [{13}] ReligionCode [{14}]	NationalityCode [{15}] SSN [{16}] PassportNumber [{17}] MaritalStatusCode [{18}] Address [{19}] Activated [{20}] DateCreated [{21}] DateUpdated [{22}]",
                this.Code,
                this.Name,
                this.NameVariant,
                this.JobTitleCode,
                this.DepartmentCode,
                this.ReportingToCode,
                this.GenderCode,
                this.Phone,
                this.Email,
                this.JoinDate.ToString( IntegrationConstants.DateFormat ),
                this.Birthdate.ToString( IntegrationConstants.DateFormat ),
                this.Username,
                this.UsernameType,
                "###",
                this.ReligionCode,
                this.NationalityCode,
                this.SSN,
                this.PassportNumber,
                this.MaritalStatusCode,
                this.Address,
                this.Activated,
                this.DateCreated.ToString( IntegrationConstants.DateFormat ),
                this.DateUpdated.ToString( IntegrationConstants.DateFormat ) );
        }

        public string Reference
        {
            get
            {
                return this.Code.ToString();
            }
        }
    }
}