using System;
using ADS.Tamam.Common.Data.Handlers;
using FluentValidation;

using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Common.Utilities;

namespace ADS.Tamam.Common.Data.Validation
{
    public class PersonValidator : AbstractModelValidator<Person>
    {
        #region classes
        #region cst.

        public PersonValidator(Person model, TamamConstants.ValidationMode mode)
            : base(model, new ValidationContext(mode))
        {
        }

        #endregion

        internal class ValidationContext : AbstractValidator<Person>
        {
            #region privates

            private bool _skipPersonId { get; set; }

            #endregion

            public ValidationContext(TamamConstants.ValidationMode mode)
            {
                if (mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit)
                {
                    RuleFor(person => person.Code).NotEmpty().WithMessage(ValidationResources.PersonCodeEmpty);                    
                    RuleFor(person => person.Code).Length(1, 50).WithMessage(ValidationResources.PersonCodeLength);

                    RuleFor(person => person.FullName).NotEmpty().WithMessage(ValidationResources.PersonFullNameEmpty);
                    RuleFor(person => person.FullName).Length(4, 100).WithMessage(ValidationResources.PersonFullNameLength);

                    RuleFor(person => person.FullNameCultureVarient).NotEmpty().WithMessage(ValidationResources.PersonFullNameCultureVariantEmpty);
                    RuleFor(person => person.FullNameCultureVarient).Length(4, 100).WithMessage(ValidationResources.PersonFullNameCultureVariantLength);

                    RuleFor(person => person.BirthDate).NotEmpty().WithMessage(ValidationResources.PersonBirthDateEmpty);
                    RuleFor(person => person.BirthDate).Must(IsValidBirthDate).WithMessage(ValidationResources.PersonBirthDateInvalid);

                    RuleFor(person => person.SSN).Must(IsValidSSN).WithMessage(ValidationResources.PersonSSNLength);

                    //RuleFor(person => person.Email).EmailAddress().When(Email => !string.IsNullOrEmpty(Email.Email)).WithMessage(ValidationResources.InvalidEmail);
                    RuleFor(person => person.Email).Length(0, 50).WithMessage(ValidationResources.PersonEmailLength);
                   

                    RuleFor(person => person.Phone).Must(IsValidPhone).WithMessage(ValidationResources.InvalidPhone);
                    RuleFor(person => person.PassportNumber).Length(0, 40).WithMessage(ValidationResources.PersonPassportNumberLength);
                    RuleFor(person => person.Address).Length(0, 200).WithMessage(ValidationResources.PersonAddressLength);

                    RuleFor(person => person.AccountInfo.JoinDate).NotEmpty().WithMessage(ValidationResources.PersonJoinDateEmpty);
                    RuleFor(person => person.AccountInfo.JoinDate).Must(IsValidJoinDate).WithMessage(ValidationResources.PersonJoinDateGreaterThanBirthDate);

                    RuleFor(person => person.AccountInfo).Must(IsValidDepartmentId).WithMessage(ValidationResources.PersonDepartmentEmpty);
                    RuleFor(person => person.AccountInfo.SecurityId).Length(0, 100).WithMessage(ValidationResources.SecurityIdLength);

                    //Identifier Uniqueness
                    //RuleFor(person => person.Code).NotEqual(person => person.Username).WithMessage(ValidationResources.PersonCodeUsernameInvalid);
                    //RuleFor(person => person.Code).NotEqual(person => person.Email).WithMessage(ValidationResources.PersonCodeEmailInvalid);
                    //RuleFor(person => person.Username).NotEqual(person => person.Email).WithMessage(ValidationResources.PersonUsernameEmailInvalid);

                    RuleFor(person => person.Code).Must(IsCodeUnique).WithMessage(ValidationResources.PersonCodeUnique);
                    RuleFor(person => person.Email).Must(IsEmailUnique).WithMessage(ValidationResources.PersonEmailUnique);
                    RuleFor(person => person.Username).Must(IsUsernameUnique).WithMessage(ValidationResources.PersonUsernameUniqueEditMode);
                 
                    // valid detail code 
                    RuleFor(person => person.TitleId).GreaterThan(0).WithMessage(ValidationResources.JobTitleEmpty);
                    RuleFor(person => person.EmploymentTypeId).GreaterThan(0).WithMessage(ValidationResources.EmploymentTypeEmpty);
                    RuleFor(person => person.GenderId).GreaterThan(0).WithMessage(ValidationResources.InvalidGenderId);

                    if (mode == TamamConstants.ValidationMode.Edit)
                    {
                        RuleFor(person => person.AccountInfo.ReportingToId).NotEqual(person => person.Id).WithMessage(Personnel.PersonSelfReportingTo);
                        RuleFor( person => person.AccountInfo.ReportingToId ).Must( IsREportingToValid ).When( p => p.ReportingToId.HasValue && p.ReportingToId.Value != Guid.Empty && p.ReportingToId.Value != p.Id ).WithMessage( Personnel.PersonReportingToCyclicdependency );
                        RuleFor(person => person.Id).Must(IsValidId);


                        //// validate not repeated Username when edit person
                        //RuleFor(person => person.Username).Must(IsUsernameEditUnique).WithMessage(ValidationResources.PersonUsernameUnique);
                    }
                }
                if (mode == TamamConstants.ValidationMode.EditIdentity)
                {
                    //Identifier Uniqueness
                    //RuleFor(person => person.Code).NotEqual(person => person.Username).WithMessage(ValidationResources.PersonCodeUsernameInvalid);
                    //RuleFor(person => person.Code).NotEqual(person => person.Email).WithMessage(ValidationResources.PersonCodeEmailInvalid);
                    //RuleFor(person => person.Username).NotEqual(person => person.Email).WithMessage(ValidationResources.PersonUsernameEmailInvalid);
                    RuleFor(person => person.Username).Must(IsUsernameUnique).WithMessage(ValidationResources.PersonUsernameUnique);
                    RuleFor(person => person.Email).Must(IsEmailUnique).WithMessage(ValidationResources.PersonEmailUnique);
                    RuleFor(person => person.Code).Must(IsCodeUnique).WithMessage(ValidationResources.PersonEmailUnique);
                 
                }
                if (mode == TamamConstants.ValidationMode.Deactivate)
                {
                    RuleFor(person => person.Id).Must(ValidateReportingTo).WithMessage(ValidationResources.PersonIsManager);
                }
            }

            private bool IsValidBirthDate(Person person, DateTime? birthDate)
            {
                if (!birthDate.HasValue && !person.AccountInfo.JoinDate.HasValue) return true;

                //return ( birthDate.Value < person.AccountInfo.JoinDate.Value );

                var minAcceptableAge = 15; //15 Year
                var acceptedAge = CalculateAge(birthDate.Value, person.AccountInfo.JoinDate.Value) >= minAcceptableAge;
                return acceptedAge;
            }

            private bool IsREportingToValid(Person instance, Guid? arg)
            {
                PersonnelDataHandler handler = new PersonnelDataHandler();
                return handler.IsReportToValid(instance);
            }
            private bool ValidateReportingTo(Person instance, Guid Id)
            {
                PersonnelDataHandler handler = new PersonnelDataHandler();
                return !handler.CheckReportingTo(instance);
            }

            #region Helpers

            private bool IsCodeUnique(Person instance, string code)
            {
                var dataHandler = new PersonnelDataHandler();
                return dataHandler.IsCodeUnique(instance);
            }
            private bool IsValidPhone(string Phone)
            {
                if (string.IsNullOrWhiteSpace(Phone))
                    return true;
                else
                {
                    return XString.MatchPattern(Phone, @"^([0-9\(\)\/\+ \-]*)$") && Phone.Length >= 8;
                }
            }
            private bool IsValidSSN(Person instance, string SSN)
            {
                if (string.IsNullOrWhiteSpace(SSN)) return true;
                string _ssn = SSN.Trim();
                if (_ssn.Length >= 6 && _ssn.Length <= 40) return true;
                return false;
            }
            private bool IsValidJoinDate(Person instance, DateTime? joinDate)
            {
                if (joinDate.HasValue && instance.BirthDate.HasValue && joinDate.Value > instance.BirthDate.Value && joinDate.Value.Date <= DateTime.Now.Date) return true;
                return false;
            }        
            private bool IsUsernameUnique(Person instance, string username)
            {
                if (string.IsNullOrWhiteSpace(username)) return true;
                PersonnelDataHandler handler = new PersonnelDataHandler();
                return handler.IsUsernameUnique(instance);
            }
            private bool IsEmailUnique(Person instance, string email)
            {
                if (string.IsNullOrWhiteSpace(email)) return true;
                PersonnelDataHandler handler = new PersonnelDataHandler();
                return handler.IsEmailUnique(instance);
            }
            //private bool IsIdentifierUnique(Person person, Guid id)
            //{
            //    if (person.Email == person.Username) return false;
            //    if (person.Email == person.Code) return false;
            //    if (person.Code == person.Username) return false;

            //    PersonnelDataHandler handler = new PersonnelDataHandler();
            //    return handler.IsIdentifierUnique(person);
            //}
            private bool IsValidDepartmentId(PersonEmploymentInfo accountInfo)
            {
                return accountInfo.DepartmentId != Guid.Empty;
            }
            private bool IsValidId(Guid Id)
            {
                return _skipPersonId || Id != Guid.Empty ? true : false;
            }
            public int CalculateAge(DateTime birthDate, DateTime joinDate)
            {
                int age = joinDate.Year - birthDate.Year;
                if (joinDate.Month < birthDate.Month || (joinDate.Month == birthDate.Month && joinDate.Day < birthDate.Day)) age--;
                return age;
            }

            #endregion
        }

        #endregion
    }
}
