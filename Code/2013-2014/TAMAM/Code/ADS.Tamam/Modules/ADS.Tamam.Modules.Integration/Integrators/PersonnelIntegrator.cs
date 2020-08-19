using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;

using ADS.Common.Context;
using ADS.Common.Models.Domain;

using ADS.Tamam.Modules.Integration.DataHandlers;
using ADS.Tamam.Modules.Integration.Repositories;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Modules.Integration.Helpers;

using ADS.Common.Utilities;
using ADS.Common.Models.Domain.Authorization;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using RAWPerson = ADS.Tamam.Modules.Integration.Models.Person;
using Person = ADS.Tamam.Common.Data.Model.Domain.Personnel.Person;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class PersonnelIntegrator
    {
        #region Props

        private PersonnelDataHandler RAWPersonDataHandler { get; set; }
        private DetailCodeRepository JopTitleRepository { get; set; }
        private DetailCodeRepository GenderRepository { get; set; }
        private DetailCodeRepository ReligionRepository { get; set; }
        private DetailCodeRepository NationalityRepository { get; set; }
        private DetailCodeRepository MaritalStatusRepository { get; set; }

        private List<RAWPerson> FailedPersonnel { get; set; }
        private List<Person> Managers { get; set; }
        private List<Role> Roles { get; set; }

        #endregion
        #region Ctor

        public PersonnelIntegrator()
        {
            RAWPersonDataHandler = new PersonnelDataHandler();
            JopTitleRepository = new DetailCodeRepository( TamamConstants.MasterCodes.JobTitle );
            GenderRepository = new DetailCodeRepository( TamamConstants.MasterCodes.Gender );
            ReligionRepository = new DetailCodeRepository( TamamConstants.MasterCodes.Religion );
            NationalityRepository = new DetailCodeRepository( TamamConstants.MasterCodes.Nationality );
            MaritalStatusRepository = new DetailCodeRepository( TamamConstants.MasterCodes.MaritalStatus );

            FailedPersonnel = new List<RAWPerson>();
            Roles = GetRoles();
        }

        #endregion
        #region Integrate

        public void Integrate()
        {
            // check ..
            //DepartmentsRepository.Reload();

            var round = 0;
            XLogger.Info( LogHelper.BuildMessage( "Round {0}", round++ ) );
            IntegrateAllPersonnel();

            for ( ; round <= IntegrationConstants.Retries; round++ )
            {
                if ( !FailedPersonnel.Any() ) break;
                XLogger.Info( LogHelper.BuildMessage( "Round {0}", round ) );

                // check ..
                //DepartmentsRepository.Reload();

                var integrationPersonnel = new List<RAWPerson>( FailedPersonnel );
                FailedPersonnel.Clear();
                Integrate( integrationPersonnel );
            }
        }
        private void IntegrateAllPersonnel()
        {
            var skip = 0;
            while ( true )
            {
                var RAWPersonnel = GetRAWPersonnel( IntegrationConstants.BatchSize, skip );
                if ( RAWPersonnel.Count == 0 ) { break; } else { skip += IntegrationConstants.BatchSize; }
                Integrate( RAWPersonnel );

                if ( IntegrationConstants.BatchSize == -1 ) break;
            }
        }
        private void Integrate( List<RAWPerson> RAWPersonnel )
        {
            var personnelCodes = RAWPersonnel.Select( p => p.Code ).ToList();
            var tamamPersonnel = GetTamamPersonnel( personnelCodes );
            var managersCodes = RAWPersonnel.Select( p => p.ReportingToCode ).Distinct().ToList();

            foreach ( var RAWPerson in RAWPersonnel )
            {
                this.Managers = GetTamamPersonnel( managersCodes );

                var tamamPerson = tamamPersonnel.SingleOrDefault( p => p.Code == RAWPerson.Code );
                if ( tamamPerson == null )
                {
                    Create( RAWPerson );
                }
                else
                {
                    var isChanged = IsPersonChanged( tamamPerson, RAWPerson );
                    var isActivatedChanged = IsPersonActivationStatusChanged( tamamPerson, RAWPerson );

                    if ( !isChanged && !isActivatedChanged )
                    {
                        var message = LogHelper.BuildSkippedMessage( RAWPerson );
                        XLogger.Info( message );
                    }

                    bool state = true;

                    if ( isChanged ) state = state && Edit( tamamPerson, RAWPerson );
                    if ( isActivatedChanged ) state = state && EditActivationStatus( tamamPerson, RAWPerson );

                    UpdateBaseOnResponse( RAWPerson, state );
                }
            }
        }

        private bool IsPersonChanged( Person Person, RAWPerson RAWPerson )
        {
            if ( RAWPerson.Name != Person.FullName ) return true;
            if ( RAWPerson.NameVariant != Person.DetailedInfo.FullNameCultureVarient ) return true;
            if ( RAWPerson.Birthdate != Person.DetailedInfo.BirthDate ) return true;
            if ( RAWPerson.SSN != Person.DetailedInfo.SSN ) return true;
            if ( RAWPerson.PassportNumber != Person.DetailedInfo.PassportNumber ) return true;

            if ( Person.DetailedInfo.Gender == null && !string.IsNullOrWhiteSpace( RAWPerson.GenderCode ) ) return true;
            if ( Person.DetailedInfo.Gender != null && RAWPerson.GenderCode != Person.DetailedInfo.Gender.Code ) return true;

            if ( Person.DetailedInfo.Religion == null && !string.IsNullOrWhiteSpace( RAWPerson.ReligionCode ) ) return true;
            if ( Person.DetailedInfo.Religion != null && RAWPerson.ReligionCode != Person.DetailedInfo.Religion.Code ) return true;

            if ( Person.DetailedInfo.Nationality == null && !string.IsNullOrWhiteSpace( RAWPerson.NationalityCode ) ) return true;
            if ( Person.DetailedInfo.Nationality != null && RAWPerson.NationalityCode != Person.DetailedInfo.Nationality.Code ) return true;

            if ( Person.DetailedInfo.MaritalStatus == null && !string.IsNullOrWhiteSpace( RAWPerson.MaritalStatusCode ) ) return true;
            if ( Person.DetailedInfo.MaritalStatus != null && RAWPerson.NationalityCode != Person.DetailedInfo.MaritalStatus.Code ) return true;

            if ( RAWPerson.Phone != Person.ContactInfo.Phone ) return true;
            if ( RAWPerson.Email != Person.ContactInfo.Email ) return true;
            if ( RAWPerson.Address != Person.ContactInfo.Address ) return true;

            if ( Person.AccountInfo.Title == null && !string.IsNullOrEmpty( RAWPerson.JobTitleCode ) ) return true;
            if ( Person.AccountInfo.Title != null && RAWPerson.JobTitleCode != Person.AccountInfo.Title.Code ) return true;

            if ( RAWPerson.JoinDate != Person.AccountInfo.JoinDate ) return true;

            if ( Person.AccountInfo.Department == null && !string.IsNullOrEmpty( RAWPerson.DepartmentCode ) ) return true;
            if ( Person.AccountInfo.Department != null && RAWPerson.DepartmentCode != Person.AccountInfo.Department.Code ) return true;

            if ( Person.AccountInfo.ReportingTo == null && !string.IsNullOrEmpty( RAWPerson.ReportingToCode ) ) return true;
            if ( Person.AccountInfo.ReportingTo != null && RAWPerson.ReportingToCode != Person.AccountInfo.ReportingTo.Code ) return true;

            return false;
        }
        private bool IsPersonActivationStatusChanged( Person Person, RAWPerson RAWPerson )
        {
            if ( RAWPerson.Activated != Person.AccountInfo.Activated ) return true;

            return false;
        }

        #endregion
        # region Actions

        private void Create( RAWPerson RAWPerson )
        {
            try
            {
                var newPerson = Map( RAWPerson );
                newPerson.AccountInfo.Activated = RAWPerson.Activated;
                newPerson.AuthenticationInfo.Username = RAWPerson.Username;

                if ( newPerson == null )
                {
                    ResponseHelper.HandleResponse( null, RAWPerson, Actions.CreatePerson );
                    FailedPersonnel.Add( RAWPerson );
                }
                else
                {
                    // 1. create person
                    var CreateResponse = TamamServiceBroker.PersonnelHandler.CreatePerson( newPerson, null, SystemRequestContext.Instance );
                    var state = CreateResponse.Type == ResponseState.Success;
                    ResponseHelper.HandleResponse( CreateResponse, RAWPerson, Actions.CreatePerson );
                    if ( !state ) return;

                    // 2. edit person identity
                    if ( IsValidIdentity( RAWPerson.Username, RAWPerson.UsernameType ) )
                    {
                        var detachedPerson = TamamServiceBroker.PersonnelHandler.GetPerson( newPerson.Id, SystemRequestContext.Instance ).Result;
                        var mode = GetAuthenticationMode( RAWPerson.UsernameType );
                        detachedPerson.AuthenticationInfo.AuthenticationMode = mode;
                        detachedPerson.AuthenticationInfo.Password = mode == AuthenticationMode.Tamam ? RAWPerson.Password : null;
                        var EditPasswordResponse = TamamServiceBroker.PersonnelHandler.EditPersonPassword( detachedPerson, SystemRequestContext.Instance );
                        ResponseHelper.HandleResponse( EditPasswordResponse, RAWPerson, Actions.CreatePersonIdentity );
                    }

                    // 3. edit person Roles..
                    var actor = Broker.AuthorizationHandler.GetActor( newPerson.Id );
                    var systemRole = Roles.FirstOrDefault( R => R.Id == new Guid( IntegrationConstants.DefaultSysSecurityRole ) );
                    if ( systemRole != null )
                    {
                        actor.Roles = new List<Role>();
                        actor.Roles.Add( systemRole );
                        var success = Broker.AuthorizationHandler.UpdateActor( actor );

                        if ( !success )
                        {
                            XLogger.Error( "Error at integrating Role [{0}] for person [{0}] , Roles should be associated manually.", systemRole.Name, newPerson.Name );
                            return;
                        }
                    }

                    UpdateBaseOnResponse( RAWPerson, state );
                }
            }
            catch ( ApplicationException e )
            {
                XLogger.Error( LogHelper.BuildSkippedMessage( RAWPerson, e.Message ) );
            }
        }
        private bool Edit( Person Person, RAWPerson RAWPerson )
        {
            try
            {
                var modifiedPerson = Map( Person, RAWPerson );
                var response = TamamServiceBroker.PersonnelHandler.EditPerson( modifiedPerson, null, SystemRequestContext.Instance );
                ResponseHelper.HandleResponse( response, RAWPerson, Actions.EditPerson );

                return response.Type == ResponseState.Success;
            }
            catch ( ApplicationException e )
            {
                XLogger.Error( LogHelper.BuildSkippedMessage( RAWPerson, e.Message ) );
                return false;
            }
        }
        private bool EditActivationStatus( Person Person, RAWPerson RAWPerson )
        {
            try
            {
                var response = TamamServiceBroker.PersonnelHandler.EditPersonStatus( Person.Id, RAWPerson.Activated, SystemRequestContext.Instance );
                ResponseHelper.HandleResponse( response, RAWPerson, Actions.EditActivationStatus );

                return response.Type == ResponseState.Success;
            }
            catch ( ApplicationException e )
            {
                XLogger.Error( LogHelper.BuildSkippedMessage( RAWPerson, e.Message ) );
                return false;
            }
        }

        # endregion
        # region Mappers

        private Person Map( RAWPerson RAWPerson )
        {
            return Map( new Person(), RAWPerson );
        }
        private Person Map( Person tamamPerson, RAWPerson RAWPerson )
        {
            // Basic Info
            tamamPerson.Code = RAWPerson.Code;
            tamamPerson.FullName = RAWPerson.Name;

            // Detailed Info
            tamamPerson.DetailedInfo.FullNameCultureVarient = RAWPerson.NameVariant;
            tamamPerson.DetailedInfo.FullNameCultureVarientAbstract = RAWPerson.NameVariant;
            tamamPerson.DetailedInfo.BirthDate = RAWPerson.Birthdate;
            tamamPerson.DetailedInfo.SSN = RAWPerson.SSN;
            tamamPerson.DetailedInfo.PassportNumber = RAWPerson.PassportNumber;
            tamamPerson.DetailedInfo.GenderId = GenderRepository.Translate( RAWPerson.GenderCode );
            tamamPerson.DetailedInfo.ReligionId = ReligionRepository.Translate( RAWPerson.ReligionCode );
            tamamPerson.DetailedInfo.NationalityId = NationalityRepository.Translate( RAWPerson.NationalityCode );
            tamamPerson.DetailedInfo.MaritalStatusId = MaritalStatusRepository.Translate( RAWPerson.MaritalStatusCode );

            // Contact Info
            tamamPerson.ContactInfo.Phone = RAWPerson.Phone;
            tamamPerson.ContactInfo.Email = RAWPerson.Email;
            tamamPerson.ContactInfo.Address = RAWPerson.Address;

            // Account Info
            tamamPerson.AccountInfo.TitleId = JopTitleRepository.Translate( RAWPerson.JobTitleCode );
            tamamPerson.AccountInfo.JoinDate = RAWPerson.JoinDate;
            tamamPerson.AccountInfo.Department = null;
            tamamPerson.AccountInfo.DepartmentId = GetDepartmentId( RAWPerson.DepartmentCode );
            tamamPerson.AccountInfo.ReportingToId = TranslateManager( RAWPerson.ReportingToCode );
            tamamPerson.AccountInfo.EnableAttendanceViolations = true;
            tamamPerson.AccountInfo.ShowAttendance = true;

            // in case we couldn't get the manager, while being provided, then we will skip that person for now, and will insert it in the next time ...
            if ( !IsValidReportingTo( tamamPerson, RAWPerson ) ) return null;

            return tamamPerson;
        }

        # endregion
        #region Helpers

        private List<Role> GetRoles()
        {
            return Broker.AuthorizationHandler.GetRoles();
        }
        private List<Person> GetTamamPersonnel( List<string> codes )
        {
            var searchCriteria = new PersonSearchCriteria() { Codes = codes };
            var response = TamamServiceBroker.PersonnelHandler.GetPersonnel( searchCriteria, SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success ) return null;
            return response.Result.Persons;
        }
        private List<RAWPerson> GetRAWPersonnel( int take, int skip )
        {
            var RAWPersonnel = RAWPersonDataHandler.GetIntegrationPersonnel( take, skip );
            return RAWPersonnel;
        }

        private Guid TranslateManager( string code )
        {
            var manager = Managers.SingleOrDefault( m => m.Code == code );
            if ( manager == null ) return new Guid();
            return manager.Id;
        }
        private void UpdateBaseOnResponse( RAWPerson RAWPerson, bool state )
        {
            if ( state )
            {
                RAWPersonDataHandler.UpdateAsSynced( RAWPerson );
            }
            else
            {
                FailedPersonnel.Add( RAWPerson );
            }
        }
        private void UpdateBaseOnResponse( RAWPerson RAWPerson, ResponseState state )
        {
            if ( state == ResponseState.Success )
            {
                RAWPersonDataHandler.UpdateAsSynced( RAWPerson );
            }
            else
            {
                FailedPersonnel.Add( RAWPerson );
            }
        }

        private Guid GetDepartmentId( string departmentCode )
        {
            var departmentId = DepartmentsRepository.Translate( departmentCode );
            if ( !departmentId.HasValue ) throw new ApplicationException( string.Format( "[DepartmentCode [{0}] is not valid]", departmentCode ) );
            return departmentId.Value;
        }
        private bool IsValidReportingTo( Person P, RAWPerson RAWP )
        {
            if ( ( !P.AccountInfo.ReportingToId.HasValue || P.AccountInfo.ReportingToId.Value == Guid.Empty ) && !string.IsNullOrWhiteSpace( RAWP.ReportingToCode ) ) return false;
            return true;
        }
        private bool IsValidIdentity( string username, string usernameType )
        {
            if ( !string.IsNullOrWhiteSpace( username ) && !string.IsNullOrWhiteSpace( usernameType ) ) return true;
            return false;
        }
        private AuthenticationMode GetAuthenticationMode( string usernameType )
        {
            var errorMsg = string.Format( "[UsernameType [{0}] is not supported]", usernameType );

            if ( string.IsNullOrWhiteSpace( usernameType ) ) throw new ApplicationException( errorMsg );

            AuthenticationMode mode;
            var isParsed = Enum.TryParse<AuthenticationMode>( usernameType, true, out mode );
            var isDefiend = Enum.IsDefined( typeof( AuthenticationMode ), mode );
            if ( !isParsed || !isDefiend ) throw new ApplicationException( errorMsg );

            return mode;
        }

        #endregion
        # region inner

        static class Actions
        {
            public const string CreatePerson = "Create Person Action";
            public const string CreatePersonIdentity = "Create Person Identity Action";

            public const string EditPerson = "Edit Person Action";
            public const string EditActivationStatus = "Edit Person Activation Status Action";
        }

        # endregion
    }
}