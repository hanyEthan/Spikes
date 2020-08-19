using System;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Organization;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface ISchedulesHandler : IBaseHandler
    {
        #region schedules

        ExecutionResponse<Guid> CreateSchedule( Schedule schedule , RequestContext requestContext );
        ExecutionResponse<Guid> EditSchedule( Schedule schedule , RequestContext requestContext );
        ExecutionResponse<bool> ChangeScheduleStatus( Guid scheduleId , bool status , RequestContext requestContext );
        ExecutionResponse<bool> SoftDeleteSchedule( Guid scheduleId , RequestContext requestContext );

        ExecutionResponse<Schedule> GetSchedule( Guid id , RequestContext requestContext );
        ExecutionResponse<List<Schedule>> GetSchedules( List<Guid> ids , RequestContext requestContext );
        ExecutionResponse<List<Schedule>> GetSchedules( DateTime startDate , DateTime? endDate , List<Guid> personIds , List<Guid> departmentIds , RequestContext requestContext );

        ExecutionResponse<List<ScheduleDay>> GetScheduleDetails( Schedule schedule , DateTime startDate , DateTime endDate , RequestContext requestContext );
        ExecutionResponse<List<ScheduleDay>> GetScheduleDetails( Schedule schedule , DateTime startDate , DateTime endDate , List<Guid> departmentsIds , List<Guid> personnelIds , RequestContext requestContext );
        ExecutionResponse<ScheduleDaysGrouped> GetScheduleDetails( DateTime startDate , DateTime endDate , List<Guid> departmentsIds , List<Guid> personnelIds , RequestContext requestContext );

        ExecutionResponse<List<ScheduleDay>> GetScheduleDays( DateTime date , RequestContext requestContext );
        ExecutionResponse<ScheduleDay> GetPersonScheduleDay( Guid personId , DateTime date , RequestContext requestContext );
        ExecutionResponse<List<Person>> GetScheduleActivePersons( Guid scheduleId , DateTime date , RequestContext requestContext );

        ExecutionResponse<bool> TransferSchedule( Guid scheduleId , Guid personId , DateTime startDate , DateTime? endDate , RequestContext requestContext );

        #endregion

        ExecutionResponse<Guid> CreateScheduleTemplate ( ScheduleTemplate scheduleTemplate , RequestContext requestContext );
        ExecutionResponse<Guid> CreateShift ( Shift shift , RequestContext requestContext );
        ExecutionResponse<bool> EditShift ( Shift shift , RequestContext requestContext );
        ExecutionResponse<Shift> GetShift ( Guid shiftId , RequestContext requestContext );
        ExecutionResponse<List<Shift>> GetShifts (bool? isActive, RequestContext requestContext );
        ExecutionResponse<bool> DeleteShift ( Guid shiftId , RequestContext requestContext );
        ExecutionResponse<bool> ChangeShiftStatus ( Guid shiftId , bool isActive , RequestContext requestContext );
        ExecutionResponse<bool> SoftDeleteShift(Guid shiftId, RequestContext requestContext);
        ExecutionResponse<List<ScheduleTemplate>> GetScheduleTemplates (bool? isActive, RequestContext requestContext );
        ExecutionResponse<ScheduleTemplate> GetScheduleTemplate ( Guid templateId , RequestContext requestContext );
        ExecutionResponse<bool> EditScheduleTemplate ( ScheduleTemplate scheduleTemplate , RequestContext requestContext );
        ExecutionResponse<bool> ChangeScheduleTemplateStatus ( Guid templateId , bool isActive , RequestContext requestContext );

        ExecutionResponse<bool> ReCalculateEffectiveSchedulesForDepartment(Guid departmentId);
        ExecutionResponse<bool> ReCalculateEffectiveSchedulesForDepartment(Department department);
        ExecutionResponse<bool> ReCalculateEffectiveSchedulesForPerson(Guid personId);
        ExecutionResponse<bool> ReCalculateEffectiveSchedulesForPerson(Person person);

        ExecutionResponse<List<ScheduleDepartment>> GetScheduleDepartments_ScheduleId(Guid scheduleId, RequestContext requestContext);
        ExecutionResponse<List<SchedulePerson>> GetSchedulePersonnel_ScheduleId(Guid scheduleId, RequestContext requestContext);

        ExecutionResponse<List<EffectiveScheduleDepartment>> GetEffectiveScheduleDepartments_ScheduleId(Guid scheduleId, RequestContext requestContext);
        ExecutionResponse<List<EffectiveSchedulePerson>> GetEffectiveSchedulePersonnel_ScheduleId(Guid scheduleId, RequestContext requestContext);
        ExecutionResponse<List<EffectiveSchedulePerson>> GetEffectiveSchedulePersonnel_PersonId(Guid personId, RequestContext requestContext);
    }
}
