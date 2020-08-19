using ADS.Tamam.Modules.Integration.Models;
using System.Collections.Generic;
using Telerik.OpenAccess.Metadata.Fluent;

namespace ADS.Tamam.Modules.Integration.ORM
{
    public partial class DomainMetadataSource : FluentMetadataSource
    {
        #region FluentMetadataSource

        protected override IList<MappingConfiguration> PrepareMapping()
        {
            var configurations = new List<MappingConfiguration>();

            configurations.Add( GetPersonnelMapping() );
            configurations.Add( GetDelegatesMapping() );
            configurations.Add( GetDepartmentsMapping() );
            configurations.Add( GetLeavesMapping() );
            configurations.Add( GetExcusesMapping() );
            configurations.Add( GetHolidaysMapping() );
            configurations.Add( GetJobTitleMapping() );
            configurations.Add( GetLeaveTypeMapping() );
            configurations.Add( GetMaritalStatusMapping() );
            configurations.Add( GetNationalityMapping() );
            configurations.Add( GetGenderMapping() );
            configurations.Add( GetReligionMapping() );
            configurations.Add( GetLeavePoliciesMapping() );

            return configurations;
        }

        #endregion

        # region Mappings

        protected MappingConfiguration GetPersonnelMapping()
        {
            MappingConfiguration<Person> configuration = new MappingConfiguration<Person>();
            configuration.MapType( p => new
            {
                Birthdate = p.Birthdate,
                Code = p.Code,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                DepartmentCode = p.DepartmentCode,
                Email = p.Email,
                GenderCode = p.GenderCode,
                ReligionCode = p.ReligionCode,
                JobTitleCode = p.JobTitleCode,
                NationalityCode = p.NationalityCode,
                MaritalStatusCode = p.MaritalStatusCode,
                JoinDate = p.JoinDate,
                Name = p.Name,
                NameVariant = p.NameVariant,
                Password = p.Password,
                Phone = p.Phone,
                ReportingToCode = p.ReportingToCode,
                Username = p.Username,
                UsernameType = p.UsernameType,
                Activated = p.Activated,
                isSynced = p.isSynced,
            } ).ToTable( "Personnel" );

            configuration.HasProperty( p => p.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( p => p.Name ).IsUnicode();
            configuration.HasProperty( p => p.NameVariant ).IsUnicode();
            configuration.HasProperty( p => p.DepartmentCode ).IsUnicode();
            configuration.HasProperty( p => p.Email ).IsUnicode();
            configuration.HasProperty( p => p.GenderCode ).IsUnicode();
            configuration.HasProperty( p => p.ReligionCode ).IsUnicode();
            configuration.HasProperty( p => p.NationalityCode ).IsUnicode();
            configuration.HasProperty( p => p.MaritalStatusCode ).IsUnicode();
            configuration.HasProperty( p => p.JobTitleCode ).IsUnicode();
            configuration.HasProperty( p => p.Password ).IsUnicode();
            configuration.HasProperty( p => p.Phone ).IsUnicode();
            configuration.HasProperty( p => p.ReportingToCode ).IsUnicode();
            configuration.HasProperty( p => p.Username ).IsUnicode();
            configuration.HasProperty( p => p.UsernameType ).IsUnicode();

            return configuration;
        }

        protected MappingConfiguration GetDelegatesMapping()
        {
            MappingConfiguration<Delegate> configuration = new MappingConfiguration<Delegate>();
            configuration.MapType( pd => new
            {
                Code = pd.Code,
                DelegateCode = pd.DelegateCode,
                PersonCode = pd.PersonCode,
                StartDate = pd.StartDate,
                EndDate = pd.EndDate,
                DateCreated = pd.DateCreated,
                DateUpdated = pd.DateUpdated,
                isSynced = pd.isSynced,
            } ).ToTable( "Delegates" );
            configuration.HasProperty( p => p.Code ).IsIdentity().IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetHolidaysMapping()
        {
            MappingConfiguration<Holiday> configuration = new MappingConfiguration<Holiday>();
            configuration.MapType( h => new
            {
                Code = h.Code,
                DateCreated = h.DateCreated,
                DateUpdated = h.DateUpdated,
                Name = h.Name,
                EndDate = h.EndDate,
                NameVariant = h.NameVariant,
                StartDate = h.StartDate,
                isSynced = h.isSynced,
            } ).ToTable( "Holidays" );
            configuration.HasProperty( h => h.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( h => h.Name ).IsUnicode();
            configuration.HasProperty( h => h.NameVariant ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetExcusesMapping()
        {
            MappingConfiguration<Excuse> configuration = new MappingConfiguration<Excuse>();
            configuration.MapType( e => new
            {
                Code = e.Code,
                DateCreated = e.DateCreated,
                DateUpdated = e.DateUpdated,
                EndTime = e.EndTime,
                Notes = e.Notes,
                StartTime = e.StartTime,
                StatusCode = e.StatusCode,
                TypeCode = e.TypeCode,
                PersonCode = e.PersonCode,
                isSynced = e.isSynced,
            } ).ToTable( "Excuses" );
            configuration.HasProperty( e => e.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( e => e.Notes ).IsUnicode();
            configuration.HasProperty( e => e.StatusCode ).IsUnicode();
            configuration.HasProperty( e => e.TypeCode ).IsUnicode();
            configuration.HasProperty( e => e.PersonCode ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetLeavesMapping()
        {
            MappingConfiguration<Leave> configuration = new MappingConfiguration<Leave>();
            configuration.MapType( l => new
            {
                Code = l.Code,
                DateCreated = l.DateCreated,
                DateUpdated = l.DateUpdated,
                EndDate = l.EndDate,
                Notes = l.Notes,
                StartDate = l.StartDate,
                StatusCode = l.StatusCode,
                TypeCode = l.TypeCode,
                PersonCode = l.PersonCode,
                isSynced = l.isSynced,
            } ).ToTable( "Leaves" );
            configuration.HasProperty( l => l.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( l => l.Notes ).IsUnicode();
            configuration.HasProperty( l => l.StatusCode ).IsUnicode();
            configuration.HasProperty( l => l.TypeCode ).IsUnicode();
            configuration.HasProperty( l => l.PersonCode ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetDepartmentsMapping()
        {
            MappingConfiguration<Department> configuration = new MappingConfiguration<Department>();
            configuration.MapType( p => new
            {
                Code = p.Code,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                Name = p.Name,
                NameVariant = p.NameVariant,
                ParentCode = p.ParentCode,
                isSynced = p.isSynced,
            } ).ToTable( "Departments" );
            configuration.HasProperty( d => d.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( d => d.Name ).IsUnicode();
            configuration.HasProperty( d => d.NameVariant ).IsUnicode();
            configuration.HasProperty( d => d.ParentCode ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetJobTitleMapping()
        {
            MappingConfiguration<JobTitle> configuration = new MappingConfiguration<JobTitle>();
            configuration.MapType( p => new
            {
                Code = p.Code,
                Name = p.Name,
                NameVariant = p.NameVariant,
                Activated = p.Activated,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                isSynced = p.isSynced,
            } ).ToTable( "JobTitles" );
            configuration.HasProperty( d => d.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( d => d.Name ).IsUnicode();
            configuration.HasProperty( d => d.NameVariant ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetLeaveTypeMapping()
        {
            MappingConfiguration<LeaveType> configuration = new MappingConfiguration<LeaveType>();
            configuration.MapType( p => new
            {
                Code = p.Code,
                Name = p.Name,
                NameVariant = p.NameVariant,
                Activated = p.Activated,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                isSynced = p.isSynced,
            } ).ToTable( "LeaveTypes" );
            configuration.HasProperty( d => d.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( d => d.Name ).IsUnicode();
            configuration.HasProperty( d => d.NameVariant ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetMaritalStatusMapping()
        {
            MappingConfiguration<MaritalStatus> configuration = new MappingConfiguration<MaritalStatus>();
            configuration.MapType( p => new
            {
                Code = p.Code,
                Name = p.Name,
                NameVariant = p.NameVariant,
                Activated = p.Activated,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                isSynced = p.isSynced,
            } ).ToTable( "MaritalStatuses" );
            configuration.HasProperty( d => d.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( d => d.Name ).IsUnicode();
            configuration.HasProperty( d => d.NameVariant ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetNationalityMapping()
        {
            MappingConfiguration<Nationality> configuration = new MappingConfiguration<Nationality>();
            configuration.MapType( p => new
            {
                Code = p.Code,
                Name = p.Name,
                NameVariant = p.NameVariant,
                Activated = p.Activated,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                isSynced = p.isSynced,
            } ).ToTable( "Nationalities" );

            configuration.HasProperty( d => d.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( d => d.Name ).IsUnicode();
            configuration.HasProperty( d => d.NameVariant ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetGenderMapping()
        {
            MappingConfiguration<Gender> configuration = new MappingConfiguration<Gender>();
            configuration.MapType( p => new
            {
                Code = p.Code,
                Name = p.Name,
                NameVariant = p.NameVariant,
                Activated = p.Activated,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                isSynced = p.isSynced,
            } ).ToTable( "Genders" );

            configuration.HasProperty( d => d.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( d => d.Name ).IsUnicode();
            configuration.HasProperty( d => d.NameVariant ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetReligionMapping()
        {
            MappingConfiguration<Religion> configuration = new MappingConfiguration<Religion>();
            configuration.MapType( p => new
            {
                Code = p.Code,
                Name = p.Name,
                NameVariant = p.NameVariant,
                Activated = p.Activated,
                DateCreated = p.DateCreated,
                DateUpdated = p.DateUpdated,
                isSynced = p.isSynced,
            } ).ToTable( "Religions" );

            configuration.HasProperty( d => d.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( d => d.Name ).IsUnicode();
            configuration.HasProperty( d => d.NameVariant ).IsUnicode();

            return configuration;
        }

        private MappingConfiguration GetLeavePoliciesMapping()
        {
            MappingConfiguration<LeavePolicy> configuration = new MappingConfiguration<LeavePolicy>();
            configuration.MapType( l => new
            {
                Code = l.Code,
                Name = l.Name,
                NameCultureVarient = l.NameCultureVarient,
                LeaveTypeCode = l.LeaveTypeCode,
                AllowedAmount = l.AllowedAmount,
                AllowRequests = l.AllowRequests,
                DaysBeforeRequest = l.DaysBeforeRequest,
                CarryOver = l.CarryOver,
                MaxCarryOverDays = l.MaxCarryOverDays,
                AllowHalfDays = l.AllowHalfDays,
                RequireAttachements = l.RequireAttachements,
                IncludeWeekEndsandHolidays = l.IncludeWeekEndsandHolidays,
                DaysLimitForOldLeaves = l.DaysLimitForOldLeaves,
                MaxDaysPerRequest = l.MaxDaysPerRequest,
                EssentialCredit = l.EssentialCredit,
                DisablePlannedLeaves = l.DisablePlannedLeaves,
                UnlimitedCredit = l.UnlimitedCredit,
                ExceedsProgressiveCredit = l.ExceedsProgressiveCredit,
                isSynced = l.isSynced,
            } ).ToTable( "LeavePolicies" );
            configuration.HasProperty( l => l.Code ).IsIdentity().IsUnicode();
            configuration.HasProperty( l => l.Name ).IsUnicode();
            configuration.HasProperty( l => l.NameCultureVarient ).IsUnicode();

            return configuration;
        }

        # endregion

        #region Helpers

        #endregion
    }
}
