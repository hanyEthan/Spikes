using ADS.Common.Context;
using ADS.Common.Validation;
using ADS.Tamam.Common.Data.Context;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADS.Tamam.Common.Data.Validation
{
    public class ScheduleValidator : AbstractModelValidator<Schedule>
    {
        #region cst.

        public ScheduleValidator( Schedule model , TamamConstants.ValidationMode mode )
            : base( model , new ValidationContext( mode ) )
        {
        }

        #endregion

        #region classes

        internal class ValidationContext : AbstractValidator<Schedule>
        {
            public ValidationContext( TamamConstants.ValidationMode mode )
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor( schedule => schedule.Name ).NotEmpty().WithMessage( Resources.Culture.ValidationResources.NameEmpty );
                    RuleFor( schedule => schedule.Name ).Must( IsNameUnique ).WithMessage( Resources.Culture.ValidationResources.NameNotUnique );
                    RuleFor( schedule => schedule.Name ).Length( 0 , 100 ).WithMessage( string.Format( Resources.Culture.ValidationResources.NameLengthInvalid , 100 ) );

                    RuleFor( schedule => schedule.NameCultureVarient ).Must( IsEmptyNameCultureVariant ).WithMessage( Resources.Culture.ValidationResources.NameArabicEmpty );
                    RuleFor( schedule => schedule.NameCultureVarient ).Must( IsNameCultureVariantUnique ).WithMessage( Resources.Culture.ValidationResources.NameCultureVariantNotUnique );
                    RuleFor( schedule => schedule.NameCultureVarient ).Length( 0 , 100 ).WithMessage( string.Format( Resources.Culture.ValidationResources.ArabicNameLengthInvalid , 100 ) );

                    RuleFor( schedule => schedule.ScheduleTemplateId ).NotEmpty().WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateEmpty );

                    RuleFor( schedule => schedule.StartDate ).NotEmpty().WithMessage( Resources.Culture.ValidationResources.StartDateEmpty );

                    RuleFor( schedule => schedule.StartDate.Date )
                        .LessThanOrEqualTo( schedule => schedule.EndDate.Value.Date )
                        .When( schedule => schedule.EndDate.HasValue )
                        .WithMessage( Resources.Culture.ValidationResources.StartDateLessEndDate );

                    // validate after hire date
                    RuleFor( schedule => schedule.SchedulePersonnel ).Must( IsSchedulePersonnelValid_HireDate ).WithMessage( Resources.Culture.ValidationResources.InvalidSchedulePersonnel_HireDate );

                    // check for duplicate records
                    RuleFor( schedule => schedule.ScheduleDepartments ).Must( IsScheduleDepartmentsValid_Duplicates ).WithMessage( Resources.Culture.ValidationResources.InvalidScheduleDepartment_Overlap );
                    RuleFor( schedule => schedule.SchedulePersonnel ).Must( IsSchedulePersonnelValid_Duplicates ).WithMessage( Resources.Culture.ValidationResources.InvalidSchedulePersonnel_Overlap );

                    // check end date > start date
                    RuleFor( schedule => schedule.ScheduleDepartments ).Must( ( s , sds ) => sds.All( sd => sd.EndDate >= sd.StartDate || sd.EndDate == null ) ).WithMessage( Resources.Culture.ValidationResources.InvalidScheduleDepartment_StartDateEndDate );
                    RuleFor( schedule => schedule.SchedulePersonnel ).Must( ( s , sps ) => sps.All( sp => sp.EndDate >= sp.StartDate || sp.EndDate == null ) ).WithMessage( Resources.Culture.ValidationResources.InvalidSchedulePersonnel_StartDateEndDate );

                    // cheack all dates in schedule date range
                    RuleFor( schedule => schedule.ScheduleDepartments ).Must( IsScheduleDepartmentsValid_DateRange ).WithMessage( Resources.Culture.ValidationResources.InvalidScheduleDepartment_ScheduleDateRange );
                    RuleFor( schedule => schedule.SchedulePersonnel ).Must( IsSchedulePersonnelValid_DateRange ).WithMessage( Resources.Culture.ValidationResources.InvalidSchedulePersonnel_ScheduleDateRange );

                    // check overlaps with in the same schedule
                    RuleFor( schedule => schedule.ScheduleDepartments ).Must( IsScheduleDepartmentsValid_Overlap ).WithMessage( Resources.Culture.ValidationResources.InvalidScheduleDepartment_Overlap );
                    RuleFor( schedule => schedule.SchedulePersonnel ).Must( IsSchedulePersonnelValid_Overlap ).WithMessage( Resources.Culture.ValidationResources.InvalidSchedulePersonnel_Overlap );

                    // check overlaps with other schedules
                    RuleFor( schedule => schedule.ScheduleDepartments ).Must( IsScheduleDepartmentsValid_OverlapWithOtherSchedules ).WithMessage( Resources.Culture.ValidationResources.InvalidScheduleDepartment_OverlapWithOtherSchedules );
                    RuleFor( schedule => schedule.SchedulePersonnel ).Must( IsSchedulePersonnelValid_OverlapWithOtherSchedules ).WithMessage( Resources.Culture.ValidationResources.InvalidSchedulePersonnel_OverlapWithOtherSchedules );
                }
                else if ( mode == TamamConstants.ValidationMode.Deactivate )
                {
                    //TO DO,Check for Attendance.
                }
                else if ( mode == TamamConstants.ValidationMode.Activate )
                {
                    // RuleFor(schedule => schedule.Persons).Must(IsPersonsValid).WithMessage(ADS.Tamam.Resources.Culture.ValidationResources.InvalidSchedulePerson);
                }
            }

            private bool IsEmptyNameCultureVariant( Schedule instance , string nameCultureVariant )
            {
                return !string.IsNullOrEmpty( nameCultureVariant );
            }

            private bool IsNameUnique( Schedule instance , string name )
            {
                var handler = new SchedulesDataHandler();
                return handler.IsScheduleNameUnique( instance );
            }

            private bool IsNameCultureVariantUnique( Schedule instance , string nameCultureVariant )
            {
                var handler = new SchedulesDataHandler();
                return handler.IsScheduleNameCultureVariantUnique( instance );
            }

            //private bool IsPersonsValid(Schedule instance, IList<Person> persons)
            //{
            //    var handler = new SchedulesDataHandler();
            //    foreach (var p in persons)
            //    {
            //        var schedules = handler.GetSchedules_DirectRelation(instance.StartDate, instance.EndDate, new List<Guid> { p.Id }, null).Result;
            //        if (schedules.Any(s => s.Id != instance.Id))
            //        {
            //            return false;
            //        }
            //    }
            //    return true;
            //}

            private bool IsScheduleDepartmentsValid_Overlap( Schedule schedule , IList<ScheduleDepartment> scheduleDepartments )
            {
                foreach ( var sdGroup in scheduleDepartments.GroupBy( sd => sd.DepartmentId ) )
                {
                    var sdDepartmentList = sdGroup.ToList();
                    foreach ( var sd1 in sdDepartmentList )
                    {
                        var sdDepartmentList_Except_sd1 = sdDepartmentList.Where( sd => sd.StartDate != sd1.StartDate || sd.EndDate != sd1.EndDate ).ToList();

                        foreach ( var sd2 in sdDepartmentList_Except_sd1 )
                        {
                            if ( IsOverlabed( sd1.StartDate , sd1.EndDate , sd2.StartDate , sd2.EndDate ) )
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            private bool IsSchedulePersonnelValid_Overlap( Schedule schedule , IList<SchedulePerson> schedulePersonnel )
            {
                foreach ( var spGroup in schedulePersonnel.GroupBy( sp => sp.PersonId ) )
                {
                    var spPersonList = spGroup.ToList();
                    foreach ( var sp1 in spPersonList )
                    {
                        var spPersonList_Except_sp1 = spPersonList.Where( sp => sp.StartDate != sp1.StartDate || sp.EndDate != sp1.EndDate );
                        var isOverLaped = spPersonList_Except_sp1.Any( sp2 => sp1.StartDate >= sp2.StartDate && sp1.StartDate <= sp2.EndDate );
                        if ( isOverLaped )
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            private bool IsScheduleDepartmentsValid_OverlapWithOtherSchedules( Schedule schedule , IList<ScheduleDepartment> scheduleDepartments )
            {
                var schedulesDataHandler = new SchedulesDataHandler();
                foreach ( var sdGroup in scheduleDepartments.GroupBy( sd => sd.DepartmentId ) )
                {
                    var sdDepartmentList = sdGroup.ToList();

                    var scheduleDepartmentResponse = schedulesDataHandler.GetScheduleDepartments_DepartmentId( sdGroup.Key );
                    if ( scheduleDepartmentResponse.Type != ResponseState.Success ) throw new ApplicationException();
                    var allOtherScheduleDepartments = scheduleDepartmentResponse.Result.Where( sd => sd.ScheduleId != schedule.Id );

                    foreach ( var sd1 in sdDepartmentList )
                    {
                        if ( allOtherScheduleDepartments.Any( sd2 => IsOverlabed( sd1.StartDate , sd1.EndDate , sd2.StartDate , sd2.EndDate ) ) )
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            private bool IsSchedulePersonnelValid_OverlapWithOtherSchedules( Schedule schedule , IList<SchedulePerson> schedulePersonnel )
            {
                var schedulesDataHandler = new SchedulesDataHandler();
                foreach ( var sdGroup in schedulePersonnel.GroupBy( sd => sd.PersonId ) )
                {
                    var sdPersonnelList = sdGroup.ToList();

                    var schedulePersonnelResponse = schedulesDataHandler.GetSchedulePersonnel_PersonId( sdGroup.Key );
                    if ( schedulePersonnelResponse.Type != ResponseState.Success ) throw new ApplicationException();
                    var allOtherSchedulePersonnel = schedulePersonnelResponse.Result.Where( sd => sd.ScheduleId != schedule.Id );

                    foreach ( var sd1 in sdPersonnelList )
                    {
                        if ( allOtherSchedulePersonnel.Any( sd2 => IsOverlabed( sd1.StartDate , sd1.EndDate , sd2.StartDate , sd2.EndDate ) ) )
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            private bool IsScheduleDepartmentsValid_Duplicates( Schedule schedule , IList<ScheduleDepartment> scheduleDepartments )
            {
                return scheduleDepartments.GroupBy( sd => new { sd.DepartmentId , sd.StartDate , sd.EndDate } ).All( g => g.Count() == 1 );
            }

            private bool IsSchedulePersonnelValid_Duplicates( Schedule schedule , IList<SchedulePerson> schedulePersonnel )
            {
                return schedulePersonnel.GroupBy( sd => new { sd.PersonId , sd.StartDate , sd.EndDate } ).All( g => g.Count() == 1 );
            }

            private bool IsScheduleDepartmentsValid_DateRange( Schedule schedule , IList<ScheduleDepartment> scheduleDepartments )
            {
                var schedulesDataHandler = new SchedulesDataHandler();
                var oldScheduleDepartmentsResponse = schedulesDataHandler.GetScheduleDepartments_ScheduleId( schedule.Id , SystemSecurityContext.Instance );
                if ( oldScheduleDepartmentsResponse.Type != ResponseState.Success ) throw new ApplicationException();

                var oldScheduleDepartments = oldScheduleDepartmentsResponse.Result;
                var newDepartmentIds = scheduleDepartments.Select( sd => sd.DepartmentId ).ToList();
                var oldScheduleDepartmentsOutOfVisability = oldScheduleDepartments.Where( sp => !newDepartmentIds.Contains( sp.DepartmentId ) ).ToList();

                if (
                    schedule.EndDate == null
                    && scheduleDepartments.All( sd => schedule.StartDate <= sd.StartDate )
                    && oldScheduleDepartmentsOutOfVisability.All( sd => schedule.StartDate <= sd.StartDate )
                    ) return true;

                if (
                    scheduleDepartments.All( sd => schedule.StartDate <= sd.StartDate )
                    && scheduleDepartments.All( sd => schedule.EndDate >= sd.EndDate )
                    && oldScheduleDepartmentsOutOfVisability.All( sd => schedule.EndDate >= sd.EndDate )
                    ) return true;

                return false;
            }

            private bool IsSchedulePersonnelValid_DateRange( Schedule schedule , IList<SchedulePerson> schedulePersonnel )
            {
                var schedulesDataHandler = new SchedulesDataHandler();
                var oldSchedulePersonnelResponse = schedulesDataHandler.GetSchedulePersonnel_ScheduleId( schedule.Id , SystemSecurityContext.Instance );
                if ( oldSchedulePersonnelResponse.Type != ResponseState.Success ) throw new ApplicationException();

                var oldSchedulePersonnel = oldSchedulePersonnelResponse.Result;
                var newPersonnelIds = schedulePersonnel.Select( sd => sd.PersonId ).ToList();
                var oldSchedulePersonnelOutOfVisability = oldSchedulePersonnel.Where( sp => !newPersonnelIds.Contains( sp.PersonId ) ).ToList();

                if (
                    schedule.EndDate == null
                    && schedulePersonnel.All( sd => schedule.StartDate <= sd.StartDate )
                    && oldSchedulePersonnelOutOfVisability.All( sd => schedule.StartDate <= sd.StartDate )
                    )
                    return true;

                if (
                    schedulePersonnel.All( sd => schedule.StartDate <= sd.StartDate )
                    && schedulePersonnel.All( sd => schedule.EndDate >= sd.EndDate )
                    && oldSchedulePersonnelOutOfVisability.All( sd => schedule.EndDate >= sd.EndDate )
                    )
                    return true;

                return false;
            }

            private bool IsSchedulePersonnelValid_HireDate( Schedule schedule , IList<SchedulePerson> schedulePersonnel )
            {
                var personnelDataHandler = new PersonnelDataHandler();
                var personnelIds = schedulePersonnel.Select( sp => sp.PersonId ).ToList();
                var personnelResponse = personnelDataHandler.GetPersonnel( new PersonSearchCriteria() { Ids = personnelIds } , SystemSecurityContext.Instance );
                if ( personnelResponse.Type != ResponseState.Success ) throw new ApplicationException();
                var personnel = personnelResponse.Result.Persons;

                foreach ( var sp in schedulePersonnel )
                {
                    var person = personnel.Single( p => p.Id == sp.PersonId );
                    if ( person.AccountInfo.JoinDate.HasValue
                        && person.AccountInfo.JoinDate.Value.Date > sp.StartDate.Date )
                    {
                        return false;
                    }
                }

                return true;
            }

            #region Helpers

            private bool IsOverlabed( DateTime? sd1Start , DateTime? sd1End , DateTime? sd2Start , DateTime? sd2End )
            {
                var isOverLaped = false;

                // sd1   |------------------------------...
                // sd2       |--------------------------...
                isOverLaped = ( sd1End == null && sd2End == null );

                // sd1            |------------------------------...
                // sd2       |--------------------------|
                isOverLaped |= ( sd1End == null && sd2End != null && sd1Start <= sd2End );

                // sd1       |--------------------------|
                // sd2            |------------------------------...
                isOverLaped |= ( sd2End == null && sd1End != null && sd1End >= sd2Start );

                // sd1                |-------------|
                // sd2          |-------------|
                isOverLaped |= sd1Start >= sd2Start && sd1Start <= sd2End;

                // sd1     |-------------|
                // sd2            |-------------|
                isOverLaped |= sd1End >= sd2Start && sd1End <= sd2End;

                return isOverLaped;
            }

            private string GetLocalizedFullName( Person p )
            {
                return p == null ? string.Empty : p.GetLocalizedFullName();
            }

            #endregion
        }

        #endregion
    }
}
