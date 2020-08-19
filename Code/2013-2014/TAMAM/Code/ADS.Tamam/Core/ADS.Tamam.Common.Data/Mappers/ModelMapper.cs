using System;
using System.Collections.Generic;

using AutoMapper;

using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.DTO.Services;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Common.Validation;

namespace ADS.Tamam.Common.Data.Mappers
{
    public static class ModelMapper
    {
        #region Maps ...

        private class MapProfile : Profile
        {
            #region custom resolvers

            #endregion

            protected override void Configure()
            {
                #region DetailCode Mapping

                //Mapper.CreateMap<DetailCode, DetailCodeDTO>();
                //Mapper.CreateMap<DetailCodeDTO, DetailCode>();

                #endregion
                #region Person Mapping

                //Mapper.CreateMap<PersonInfo, PersonInfoDTO>();
                //Mapper.CreateMap<PersonInfoDTO, PersonInfo>();

                //Mapper.CreateMap<PersonContactInfo, PersonContactInfoDTO>();
                //Mapper.CreateMap<PersonContactInfoDTO, PersonContactInfo>();

                //Mapper.CreateMap<PersonEmploymentInfo, PersonAccountInfoDTO>();
                //Mapper.CreateMap<PersonAccountInfoDTO, PersonEmploymentInfo>();

                //Mapper.CreateMap<Person, PersonServiceDTO>();
                //Mapper.CreateMap<PersonServiceDTO, Person>();

                #endregion
                #region Department Mapping

                //Mapper.CreateMap<Department, DepartmentServiceDTO>();
                //Mapper.CreateMap<DepartmentServiceDTO, Department>();

                #endregion
                #region PersonSearchResult Mapping

                //Mapper.CreateMap<PersonSearchResult, PersonSearchResultDTO>();
                //Mapper.CreateMap<PersonSearchResultDTO, PersonSearchResult>();

                #endregion
                #region ExecutionResponses

                //Mapper.CreateMap<ExecutionResponse<Person>, ResponseDTO<PersonServiceDTO>>();
                //Mapper.CreateMap<ResponseDTO<PersonServiceDTO>, ExecutionResponse<Person>>();

                //Mapper.CreateMap<ExecutionResponse<Department>, ResponseDTO<DepartmentServiceDTO>>();
                //Mapper.CreateMap<ResponseDTO<DepartmentServiceDTO>, ExecutionResponse<Department>>();

                //Mapper.CreateMap<ExecutionResponse<PersonSearchResult>, ResponseDTO<PersonSearchResultDTO>>();
                //Mapper.CreateMap<ResponseDTO<PersonSearchResultDTO>, ExecutionResponse<PersonSearchResult>>();

                //Mapper.CreateMap<ExecutionResponse<List<Department>>, ResponseDTO<List<DepartmentServiceDTO>>>();
                //Mapper.CreateMap<ResponseDTO<List<DepartmentServiceDTO>>, ExecutionResponse<List<Department>>>();

                #endregion

                //Mapper.AssertConfigurationIsValid ();
            }
        }

        #endregion

        #region cst ...

        static ModelMapper()
        {
            Mapper.Initialize(x => x.AddProfile<MapProfile>());
        }

        #endregion

        #region Auto Map

        #region DetailCode

        //public static bool Map(DetailCode from, out DetailCodeDTO to)
        //{
        //    try
        //    {
        //        to = Mapper.Map<DetailCode, DetailCodeDTO>(from);
        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        to = null;
        //        return false;
        //    }
        //}
        //public static bool Map(DetailCodeDTO from, out DetailCode to)
        //{
        //    try
        //    {
        //        to = Mapper.Map<DetailCodeDTO, DetailCode>(from);
        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        to = null;
        //        return false;
        //    }
        //}

        #endregion
        #region Person

        //public static bool Map(Person from, out PersonServiceDTO to)
        //{
        //    try
        //    {
        //        to = Mapper.Map<Person, PersonServiceDTO>(from);
        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        to = null;
        //        return false;
        //    }
        //}
        //public static bool Map(PersonServiceDTO from, out Person to)
        //{
        //    try
        //    {
        //        to = Mapper.Map<PersonServiceDTO, Person>(from);
        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        to = null;
        //        return false;
        //    }
        //}

        #endregion

        public static bool Map<T, D>(T from, out D to)
            where T : new()
            where D : new()
        {
            try
            {
                to = Mapper.Map<T, D>(from);
                return true;
            }
            catch (Exception x)
            {
                to = default(D);
                return false;
            }
        }
        # region PersonSearchResult
        //public static bool Map(PersonSearchResult from, out PersonSearchResultDTO to)
        //{
        //    try
        //    {
        //        to = Mapper.Map<PersonSearchResult, PersonSearchResultDTO>(from);
        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        to = null;
        //        return false;
        //    }
        //}
        //public static bool Map(PersonSearchResultDTO from, out PersonSearchResult to)
        //{
        //    try
        //    {
        //        to = Mapper.Map<PersonSearchResultDTO, PersonSearchResult>(from);
        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        to = null;
        //        return false;
        //    }
        //}
        #endregion
        #endregion

        #region OldMapping

        #region Response

        //public static bool Map<T>(ExecutionResponse<T> from, out ResponseDTO<T> to) where T : new()
        //{
        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ResponseDTO<T>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    to.Result = from.Result;
        //    to.Type = from.Type;

        //    return true;
        //}
        //public static bool Map<T>(ResponseDTO<T> from, out ExecutionResponse<T> to) where T : new()
        //{
        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ExecutionResponse<T>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    to.Result = from.Result;
        //    to.Type = from.Type;

        //    return true;
        //}
        //public static bool Map(ExecutionResponse<Person> from, out ResponseDTO<PersonServiceDTO> to)
        //{

        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ResponseDTO<PersonServiceDTO>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    PersonServiceDTO dto;
        //    Map(from.Result, out dto);
        //    to.Result = dto;
        //    to.Type = from.Type;

        //    return true;
        //}
        //public static bool Map(ResponseDTO<PersonServiceDTO> from, out ExecutionResponse<Person> to)
        //{

        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ExecutionResponse<Person>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    Person model = new Person();
        //    Map(from.Result, out model);
        //    to.Result = model;
        //    to.Type = from.Type;

        //    return true;
        //}

        //public static bool Map(ExecutionResponse<Department> from, out ResponseDTO<DepartmentServiceDTO> to)
        //{

        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ResponseDTO<DepartmentServiceDTO>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    DepartmentServiceDTO dto;
        //    Map(from.Result, out dto);
        //    to.Result = dto;
        //    to.Type = from.Type;

        //    return true;
        //}
        //public static bool Map(ResponseDTO<DepartmentServiceDTO> from, out ExecutionResponse<Department> to)
        //{

        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ExecutionResponse<Department>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    Department model;
        //    Map(from.Result, out model);
        //    to.Result = model;
        //    to.Type = from.Type;

        //    return true;
        //}

        //public static bool Map(ExecutionResponse<PersonSearchResult> from, out ResponseDTO<PersonSearchResultDTO> to)
        //{

        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ResponseDTO<PersonSearchResultDTO>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    PersonSearchResultDTO dto;
        //    Map(from.Result, out dto);
        //    to.Result = dto;
        //    to.Type = from.Type;

        //    return true;
        //}
        //public static bool Map(ResponseDTO<PersonSearchResultDTO> from, out ExecutionResponse<PersonSearchResult> to)
        //{

        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ExecutionResponse<PersonSearchResult>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    PersonSearchResult model = new PersonSearchResult();
        //    Map(from.Result, out model);
        //    to.Result = model;
        //    to.Type = from.Type;

        //    return true;
        //}

        //public static bool Map(ExecutionResponse<Schedule> from, out ResponseDTO<ScheduleServiceDTO> to)
        //{
        //    ScheduleServiceDTO result;

        //    MapPartial<Schedule, ScheduleServiceDTO>(from, out to);
        //    Map(from.Result, out result);

        //    to.Result = result;

        //    return true;
        //}
        //public static bool Map(ResponseDTO<ScheduleServiceDTO> from, out ExecutionResponse<Schedule> to)
        //{
        //    Schedule result;

        //    MapPartial<ScheduleServiceDTO, Schedule>(from, out to);
        //    Map(from.Result, out result);

        //    to.Result = result;

        //    return true;
        //}

        //public static bool Map(ExecutionResponse<ScheduleEvent> from, out ResponseDTO<ScheduleEventServiceDTO> to)
        //{
        //    ScheduleEventServiceDTO result;

        //    MapPartial<ScheduleEvent, ScheduleEventServiceDTO>(from, out to);
        //    Map(from.Result, out result);

        //    to.Result = result;

        //    return true;
        //}
        //public static bool Map(ResponseDTO<ScheduleEventServiceDTO> from, out ExecutionResponse<ScheduleEvent> to)
        //{
        //    ScheduleEvent result;

        //    MapPartial<ScheduleEventServiceDTO, ScheduleEvent>(from, out to);
        //    Map(from.Result, out result);

        //    to.Result = result;

        //    return true;
        //}

        //private static bool MapPartial<T, D>(ExecutionResponse<T> from, out ResponseDTO<D> to)
        //    where T : new()
        //    where D : new()
        //{
        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ResponseDTO<D>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    to.Type = from.Type;

        //    // MessageDetailed ...
        //    if (from.MessageDetailed != null)
        //    {
        //        to.MessageDetailed = new List<ResponseDTO<D>.ModelMetaPair>();
        //        for (int i = 0; i < from.MessageDetailed.Count; i++)
        //        {
        //            ResponseDTO<D>.ModelMetaPair mmp;
        //            MapPartial(from.MessageDetailed[i], out mmp);

        //            to.MessageDetailed[i] = mmp;
        //        }
        //    }

        //    return true;
        //}
        //private static bool MapPartial<T, D>(ResponseDTO<T> from, out ExecutionResponse<D> to)
        //    where T : new()
        //    where D : new()
        //{
        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ExecutionResponse<D>();

        //    to.Code = from.Code;
        //    to.Exception = from.Exception;
        //    //to.Message = from.Message;
        //    to.Type = from.Type;

        //    // MessageDetailed ...
        //    if (from.MessageDetailed != null)
        //    {
        //        to.MessageDetailed = new List<ModelMetaPair>();
        //        for (int i = 0; i < from.MessageDetailed.Count; i++)
        //        {
        //            ModelMetaPair mmp;
        //            MapPartial(from.MessageDetailed[i], out mmp);

        //            to.MessageDetailed[i] = mmp;
        //        }
        //    }

        //    return true;
        //}

        //private static bool MapPartial<D>(ModelMetaPair from, out ResponseDTO<D>.ModelMetaPair to) where D : new()
        //{
        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ResponseDTO<D>.ModelMetaPair();

        //    to.PropertyName = from.PropertyName;
        //    to.Meta = from.Meta;

        //    return true;
        //}
        //private static bool MapPartial<T>(ResponseDTO<T>.ModelMetaPair from, out ModelMetaPair to) where T : new()
        //{
        //    if (from == null)
        //    {
        //        to = null;
        //        return true;
        //    }

        //    to = new ModelMetaPair();

        //    to.PropertyName = from.PropertyName;
        //    to.Meta = from.Meta;

        //    return true;
        //}

        #endregion
        #region oldPerson

        //public static bool Map ( PersonServiceDTO from , out Person to )
        //{
        //    to = new Person ();

        //    to.Id = from.Id;
        //    to.FullName = from.FullName;
        //    to.Code = from.Code;
        //    to.DetailedInfo.SSN = from.SSN;
        //    to.DetailedInfo.FullNameCultureVarient = from.FullNameCultureVarient;
        //    to.DetailedInfo.FullNameDisplayName = from.FullNameDisplayName;
        //    to.DetailedInfo.BirthDate = from.BirthDate;
        //    to.DetailedInfo.BirthDateCultureVarient = from.BirthDateCultureVarient;
        //    to.DetailedInfo.GenderId = from.GenderId;
        //    to.DetailedInfo.ReligionId = from.ReligionId;
        //    to.DetailedInfo.NationalityId = from.NationalityId;
        //    to.DetailedInfo.PassportNumber = from.PassportNumber;
        //    to.DetailedInfo.MaritalStatusId = from.MaritalStatusId;
        //    to.ContactInfo.Email = from.Email;
        //    to.ContactInfo.Phone = from.Phone;
        //    to.ContactInfo.Address = from.Address;
        //    to.AccountInfo.EmploymentTypeId = from.EmploymentTypeId;
        //    to.AccountInfo.Activated = from.Activated;
        //    to.AccountInfo.JoinDate = from.JoinDate;
        //    to.AccountInfo.JoinDateCultureVarient = from.JoinDateCultureVarient;
        //    to.AccountInfo.SecurityId = from.SecurityId;
        //    to.AccountInfo.CheckinRequired = from.CheckinRequired;
        //    to.AccountInfo.DepartmentId = from.DepartmentId;
        //    to.AccountInfo.TitleId = from.TitleId;

        //    to.Gender = Map ( from.Gender );
        //    to.Religion = Map ( from.Religion );
        //    to.Nationality = Map ( from.Nationality );
        //    to.MaritalStatus = Map ( from.MaritalStatus );
        //    to.Title = Map ( from.Title );
        //    to.EmploymentType = Map ( from.EmploymentType );
        //    Department result;
        //    Map ( from.Department , out result );
        //    to.Department = result;

        //    return true;
        //}
        //public static bool Map ( Person from , out PersonServiceDTO to )
        //{
        //    to = new PersonServiceDTO ();

        //    to.Id = from.Id;
        //    to.FullName = from.FullName;
        //    to.Code = from.Code;
        //    to.SSN = from.DetailedInfo.SSN;
        //    to.FullNameCultureVarient = from.DetailedInfo.FullNameCultureVarient;
        //    to.FullNameDisplayName = from.DetailedInfo.FullNameDisplayName;
        //    to.BirthDate = from.DetailedInfo.BirthDate;
        //    to.BirthDateCultureVarient = from.DetailedInfo.BirthDateCultureVarient;
        //    to.GenderId = from.DetailedInfo.GenderId;
        //    to.ReligionId = from.DetailedInfo.ReligionId;
        //    to.NationalityId = from.DetailedInfo.NationalityId;
        //    to.PassportNumber = from.DetailedInfo.PassportNumber;
        //    to.MaritalStatusId = from.DetailedInfo.MaritalStatusId;
        //    to.Email = from.ContactInfo.Email;
        //    to.Phone = from.ContactInfo.Phone;
        //    to.Address = from.ContactInfo.Address;
        //    to.EmploymentTypeId = from.AccountInfo.EmploymentTypeId;
        //    to.Activated = from.AccountInfo.Activated;
        //    to.JoinDate = from.AccountInfo.JoinDate;
        //    to.JoinDateCultureVarient = from.AccountInfo.JoinDateCultureVarient;
        //    to.SecurityId = from.AccountInfo.SecurityId;
        //    to.CheckinRequired = from.AccountInfo.CheckinRequired;
        //    to.DepartmentId = from.AccountInfo.DepartmentId;
        //    to.TitleId = from.AccountInfo.TitleId;

        //    to.Gender = Map ( from.Gender );
        //    to.Religion = Map ( from.Religion );
        //    to.Nationality = Map ( from.Nationality );
        //    to.MaritalStatus = Map ( from.MaritalStatus );
        //    to.Title = Map ( from.Title );
        //    to.EmploymentType = Map ( from.EmploymentType );
        //    DepartmentServiceDTO result;
        //    Map ( from.Department , out result );
        //    to.Department = result;
        //    return true;
        //}

        #endregion
        #region oldDetailCode
        //public static DetailCodeDTO Map ( DetailCode from )
        //{
        //    if ( from == null )
        //        return null;
        //    DetailCodeDTO to = new DetailCodeDTO
        //    {
        //        Id = from.Id ,
        //        Code = from.Code ,
        //        Name = from.Name ,
        //        NameCultureVariant = from.NameCultureVariant ,
        //        IsActive = from.IsActive ,
        //        IsDeleted = from.IsDeleted ,
        //        ParentId = from.ParentId ,
        //        MasterCodeId = from.MasterCodeId ,
        //        FieldOneValue = from.FieldOneValue ,
        //        FieldTwoValue = from.FieldTwoValue ,
        //        FieldThreeValue = from.FieldThreeValue ,
        //        CreatedBy = from.CreatedBy ,
        //        CreatedOn = from.CreatedOn ,
        //        UpdatedBy = from.UpdatedBy ,
        //        UpdatedOn = from.UpdatedOn
        //    };
        //    return to;
        //}

        //public static DetailCode Map ( DetailCodeDTO from )
        //{

        //    if ( from == null )
        //        return null;
        //    DetailCode to = new DetailCode
        //    {
        //        Id = from.Id ,
        //        Code = from.Code ,
        //        Name = from.Name ,
        //        NameCultureVariant = from.NameCultureVariant ,
        //        IsActive = from.IsActive ,
        //        IsDeleted = from.IsDeleted ,
        //        ParentId = from.ParentId ,
        //        MasterCodeId = from.MasterCodeId ,
        //        FieldOneValue = from.FieldOneValue ,
        //        FieldTwoValue = from.FieldTwoValue ,
        //        FieldThreeValue = from.FieldThreeValue ,
        //        CreatedBy = from.CreatedBy ,
        //        CreatedOn = from.CreatedOn ,
        //        UpdatedBy = from.UpdatedBy ,
        //        UpdatedOn = from.UpdatedOn
        //    };
        //    return to;
        //} 
        #endregion
        #region Unused
        //public static bool Map ( DepartmentServiceDTO from,out Department to)
        //{
        //    to = new Department ();
        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.ParentDepartmentId = from.ParentDepartmentId;
        //    to.SupervisorId = from.SupervisorId;
        //    if ( from.ParentDepartment != null )
        //    {
        //        Department result;
        //        Map ( from.ParentDepartment,out result );
        //        to.ParentDepartment = result;
        //    }
        //    if ( from.Supervisor != null )
        //    {
        //        Person person;
        //        Map ( from.Supervisor , out person );
        //        to.Supervisor = person;
        //    }
        //    return true;
        //}
        //public static bool Map ( Department from, out DepartmentServiceDTO to)
        //{
        //    to = new DepartmentServiceDTO ();
        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.ParentDepartmentId = from.ParentDepartmentId;
        //    to.SupervisorId = from.SupervisorId;
        //    if ( from.ParentDepartment != null )
        //    {
        //        DepartmentServiceDTO result;
        //        Map ( from.ParentDepartment,out result );
        //        to.ParentDepartment = result;
        //    }
        //    if ( from.Supervisor != null )
        //    {
        //        PersonServiceDTO person;
        //        Map ( from.Supervisor,out person );
        //        to.Supervisor = person;
        //    }
        //    return true;
        //}

        //public static bool Map( ScheduleServiceDTO from , out Schedule to )
        //{
        //    to = new Schedule();

        //    to.Id = from.Id;
        //    to.StartDate = from.StartDate;
        //    to.EndDate = from.EndDate;
        //    to.Active = from.Active;
        //    to.ScheduleTemplateId = from.ScheduleTemplateId;
        //    to.PersonId = from.PersonId;
        //    to.LocationId = from.LocationId;

        //    // ScheduleTemplate
        //    ScheduleTemplate scheduleTemplate;
        //    Map( from.ScheduleTemplate , out scheduleTemplate );
        //    to.ScheduleTemplate = scheduleTemplate;

        //    // Person
        //    Person person;
        //    Map( from.Person , out person );
        //    to.Person = person;

        //    // Location
        //    Location location;
        //    Map( from.Location , out location );
        //    to.Location = location;

        //    // ScheduleEvents
        //    if ( from.ScheduleEvents != null )
        //    {
        //        to.ScheduleEvents = new List<ScheduleEvent>();

        //        for ( int i = 0 ; i < from.ScheduleEvents.Count ; i++ )
        //        {
        //            ScheduleEvent scheduleEvent;
        //            Map( from.ScheduleEvents[i] , out scheduleEvent );
        //        }
        //    }

        //    return true;
        //}
        //public static bool Map( Schedule from , out ScheduleServiceDTO to )
        //{
        //    to = new ScheduleServiceDTO();

        //    to.Id = from.Id;
        //    to.StartDate = from.StartDate;
        //    to.EndDate = from.EndDate;
        //    to.Active = from.Active;
        //    to.ScheduleTemplateId = from.ScheduleTemplateId;
        //    to.PersonId = from.PersonId;
        //    to.LocationId = from.LocationId;

        //    // ScheduleTemplate
        //    ScheduleTemplateServiceDTO scheduleTemplate;
        //    Map( from.ScheduleTemplate , out scheduleTemplate );
        //    to.ScheduleTemplate = scheduleTemplate;

        //    // Person
        //    PersonServiceDTO person;
        //    Map( from.Person , out person );
        //    to.Person = person;

        //    // Location
        //    LocationServiceDTO location;
        //    Map( from.Location , out location );
        //    to.Location = location;

        //    // ScheduleEvents
        //    if ( from.ScheduleEvents != null )
        //    {
        //        to.ScheduleEvents = new List<ScheduleEventServiceDTO>();

        //        for ( int i = 0 ; i < from.ScheduleEvents.Count ; i++ )
        //        {
        //            ScheduleEventServiceDTO scheduleEvent;
        //            Map( from.ScheduleEvents[i] , out scheduleEvent );
        //        }
        //    }

        //    return true;
        //}

        //public static bool Map( ScheduleTemplateServiceDTO from , out ScheduleTemplate to )
        //{
        //    to = new ScheduleTemplate();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.ShiftId = from.ShiftId;
        //    to.OccurEvery = from.OccurEvery;
        //    to.Occurrences = from.Occurrences;
        //    to.NeverEnd = from.NeverEnd;
        //    to.RecurrenceType = from.RecurrenceType;

        //    // Shift
        //    Shift shift;
        //    Map( from.Shift , out shift );
        //    to.Shift = shift;

        //    // Schedules
        //    if ( from.Schedules != null )
        //    {
        //        to.Schedules = new List<Schedule>();

        //        for ( int i = 0 ; i < from.Schedules.Count ; i++ )
        //        {
        //            Schedule schedule;
        //            Map( from.Schedules[i] , out schedule );
        //        }
        //    }

        //    // WeeklyPattern
        //    if ( from.WeeklyPattern != null )
        //    {
        //        to.WeeklyPattern.Sunday = from.WeeklyPattern.Sunday;
        //        to.WeeklyPattern.Monday = from.WeeklyPattern.Monday;
        //        to.WeeklyPattern.Tuesday = from.WeeklyPattern.Tuesday;
        //        to.WeeklyPattern.Wednesday = from.WeeklyPattern.Wednesday;
        //        to.WeeklyPattern.Thursday = from.WeeklyPattern.Thursday;
        //        to.WeeklyPattern.Friday = from.WeeklyPattern.Friday;
        //        to.WeeklyPattern.Saturday = from.WeeklyPattern.Saturday;
        //    }

        //    // MonthlyPattern
        //    if ( from.MonthlyPattern != null )
        //    {
        //        to.MonthlyPattern.DayNumber = from.MonthlyPattern.DayNumber;
        //        to.MonthlyPattern.DayOrder = from.MonthlyPattern.DayOrder;
        //        to.MonthlyPattern.WeekDayName = from.MonthlyPattern.WeekDayName;
        //        to.MonthlyPattern.MonthlyPatternType = from.MonthlyPattern.MonthlyPatternType;
        //    }

        //    // YearlyTemplatePattern
        //    if ( from.YearlyPattern != null )
        //    {
        //        to.YearlyPattern.DayNumber = from.YearlyPattern.DayNumber;
        //        to.YearlyPattern.DayOrder = from.YearlyPattern.DayOrder;
        //        to.YearlyPattern.WeekDayName = from.YearlyPattern.WeekDayName;
        //        to.YearlyPattern.Month = from.YearlyPattern.Month;
        //        to.YearlyPattern.YearlyPatternType = from.YearlyPattern.YearlyPatternType;
        //    }

        //    return true;
        //}
        //public static bool Map( ScheduleTemplate from , out ScheduleTemplateServiceDTO to )
        //{
        //    to = new ScheduleTemplateServiceDTO();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.ShiftId = from.ShiftId;
        //    to.OccurEvery = from.OccurEvery;
        //    to.Occurrences = from.Occurrences;
        //    to.NeverEnd = from.NeverEnd;
        //    to.RecurrenceType = from.RecurrenceType;

        //    // Shift
        //    ShiftServiceDTO shift;
        //    Map( from.Shift , out shift );
        //    to.Shift = shift;

        //    // Schedules
        //    if ( from.Schedules != null )
        //    {
        //        to.Schedules = new List<ScheduleServiceDTO>();

        //        for ( int i = 0 ; i < from.Schedules.Count ; i++ )
        //        {
        //            ScheduleServiceDTO schedule;
        //            Map( from.Schedules[i] , out schedule );
        //        }
        //    }

        //    // WeeklyPattern
        //    if ( from.WeeklyPattern != null )
        //    {
        //        to.WeeklyPattern.Sunday = from.WeeklyPattern.Sunday;
        //        to.WeeklyPattern.Monday = from.WeeklyPattern.Monday;
        //        to.WeeklyPattern.Tuesday = from.WeeklyPattern.Tuesday;
        //        to.WeeklyPattern.Wednesday = from.WeeklyPattern.Wednesday;
        //        to.WeeklyPattern.Thursday = from.WeeklyPattern.Thursday;
        //        to.WeeklyPattern.Friday = from.WeeklyPattern.Friday;
        //        to.WeeklyPattern.Saturday = from.WeeklyPattern.Saturday;
        //    }

        //    // MonthlyPattern
        //    if ( from.MonthlyPattern != null )
        //    {
        //        to.MonthlyPattern.DayNumber = from.MonthlyPattern.DayNumber;
        //        to.MonthlyPattern.DayOrder = from.MonthlyPattern.DayOrder;
        //        to.MonthlyPattern.WeekDayName = from.MonthlyPattern.WeekDayName;
        //        to.MonthlyPattern.MonthlyPatternType = from.MonthlyPattern.MonthlyPatternType;
        //    }

        //    // YearlyTemplatePattern
        //    if ( from.YearlyPattern != null )
        //    {
        //        to.YearlyPattern.DayNumber = from.YearlyPattern.DayNumber;
        //        to.YearlyPattern.DayOrder = from.YearlyPattern.DayOrder;
        //        to.YearlyPattern.WeekDayName = from.YearlyPattern.WeekDayName;
        //        to.YearlyPattern.Month = from.YearlyPattern.Month;
        //        to.YearlyPattern.YearlyPatternType = from.YearlyPattern.YearlyPatternType;
        //    }

        //    return true;
        //}
        //public static bool Map( ScheduleTemplate from , out XDate.Pattern to )
        //{
        //    to = new XDate.Pattern();

        //    to.RecurrenceType = from.RecurrenceType;
        //    to.OccurEvery = from.OccurEvery;
        //    to.Occurrences = from.Occurrences;
        //    to.NeverEnd = from.NeverEnd;

        //    if ( from.MonthlyPattern != null )
        //    {
        //        to.Monthly = new XDate.Pattern.MonthlyPattern();
        //        to.Monthly.DayNumber = from.MonthlyPattern.DayNumber;
        //        to.Monthly.DayOrder = from.MonthlyPattern.DayOrder;
        //        to.Monthly.MonthlyPatternType = from.MonthlyPattern.MonthlyPatternType;
        //        to.Monthly.WeekDayName = from.MonthlyPattern.WeekDayName;
        //    }
        //    if ( from.WeeklyPattern != null )
        //    {
        //        to.Weekly = new XDate.Pattern.WeeklyPattern();
        //        to.Weekly.Sunday = from.WeeklyPattern.Sunday;
        //        to.Weekly.Monday = from.WeeklyPattern.Monday;
        //        to.Weekly.Tuesday = from.WeeklyPattern.Tuesday;
        //        to.Weekly.Wednesday = from.WeeklyPattern.Wednesday;
        //        to.Weekly.Thursday = from.WeeklyPattern.Thursday;
        //        to.Weekly.Friday = from.WeeklyPattern.Friday;
        //        to.Weekly.Saturday = from.WeeklyPattern.Saturday;
        //    }
        //    if ( from.YearlyPattern != null )
        //    {
        //        to.Yearly = new XDate.Pattern.YearlyPattern();
        //        to.Yearly.DayNumber = from.YearlyPattern.DayNumber;
        //        to.Yearly.DayOrder = from.YearlyPattern.DayOrder;
        //        to.Yearly.Month = from.YearlyPattern.Month;
        //        to.Yearly.WeekDayName = from.YearlyPattern.WeekDayName;
        //        to.Yearly.YearlyPatternType = from.YearlyPattern.YearlyPatternType;
        //    }

        //    return true;
        //}

        //public static bool Map( ScheduleEventServiceDTO from , out ScheduleEvent to )
        //{
        //    to = new ScheduleEvent();

        //    to.Id = from.Id;
        //    to.Date = from.Date;
        //    to.ExpectedIn = from.ExpectedIn;
        //    to.ActualIn = from.ActualIn;
        //    to.ExpectedOut = from.ExpectedOut;
        //    to.ActualOut = from.ActualOut;
        //    to.PersonId = from.PersonId;
        //    to.ScheduleId = from.ScheduleId;
        //    to.InStatusId = from.InStatusId;
        //    to.OutStatusId = from.OutStatusId;
        //    to.TotalStatusId = from.TotalStatusId;

        //    // Person
        //    Person person;
        //    Map( from.Person , out person );
        //    to.Person = person;

        //    // Schedule
        //    Schedule schedule;
        //    Map( from.Schedule , out schedule );
        //    to.Schedule = schedule;

        //    // InStatus
        //    AttendanceCode attendanceCode;
        //    Map( from.InStatus , out attendanceCode );
        //    to.InStatus = attendanceCode;

        //    // OutStatus
        //    Map( from.InStatus , out attendanceCode );
        //    to.OutStatus = attendanceCode;

        //    // TotalStatus
        //    Map( from.InStatus , out attendanceCode );
        //    to.TotalStatus = attendanceCode;

        //    return true;
        //}
        //public static bool Map( ScheduleEvent from , out ScheduleEventServiceDTO to )
        //{
        //    to = new ScheduleEventServiceDTO();

        //    to.Id = from.Id;
        //    to.Date = from.Date;
        //    to.ExpectedIn = from.ExpectedIn;
        //    to.ActualIn = from.ActualIn;
        //    to.ExpectedOut = from.ExpectedOut;
        //    to.ActualOut = from.ActualOut;
        //    to.PersonId = from.PersonId;
        //    to.ScheduleId = from.ScheduleId;
        //    to.InStatusId = from.InStatusId;
        //    to.OutStatusId = from.OutStatusId;
        //    to.TotalStatusId = from.TotalStatusId;

        //    // Person
        //    PersonServiceDTO person;
        //    Map( from.Person , out person );
        //    to.Person = person;

        //    // Schedule
        //    ScheduleServiceDTO schedule;
        //    Map( from.Schedule , out schedule );
        //    to.Schedule = schedule;

        //    // InStatus
        //    AttendanceCodeServiceDTO attendanceCode;
        //    Map( from.InStatus , out attendanceCode );
        //    to.InStatus = attendanceCode;

        //    // OutStatus
        //    Map( from.InStatus , out attendanceCode );
        //    to.OutStatus = attendanceCode;

        //    // TotalStatus
        //    Map( from.InStatus , out attendanceCode );
        //    to.TotalStatus = attendanceCode;

        //    return true;
        //}

        //public static bool Map( ShiftPolicyServiceDTO from , out ShiftPolicy to )
        //{
        //    to = new ShiftPolicy();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.InGraceTime = from.InGraceTime;
        //    to.OutGraceTime = from.OutGraceTime;
        //    to.MinimumHours = from.MinimumHours;
        //    to.MaximumHours = from.MaximumHours;

        //    // Shifts
        //    if ( from.Shifts != null )
        //    {
        //        to.Shifts = new List<Shift>();

        //        for ( int i = 0 ; i < from.Shifts.Count ; i++ )
        //        {
        //            Shift shift;
        //            Map( from.Shifts[i] , out shift );
        //        }
        //    }

        //    return true;
        //}
        //public static bool Map( ShiftPolicy from , out ShiftPolicyServiceDTO to )
        //{
        //    to = new ShiftPolicyServiceDTO();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.InGraceTime = from.InGraceTime;
        //    to.OutGraceTime = from.OutGraceTime;
        //    to.MinimumHours = from.MinimumHours;
        //    to.MaximumHours = from.MaximumHours;

        //    // Shifts
        //    if ( from.Shifts != null )
        //    {
        //        to.Shifts = new List<ShiftServiceDTO>();

        //        for ( int i = 0 ; i < from.Shifts.Count ; i++ )
        //        {
        //            ShiftServiceDTO shift;
        //            Map( from.Shifts[i] , out shift );
        //        }
        //    }

        //    return true;
        //}

        //public static bool Map( ShiftServiceDTO from , out Shift to )
        //{
        //    to = new Shift();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.StartTime = from.StartTime;
        //    to.EndTime = from.EndTime;
        //    to.MinimumParties = from.MinimumParties;
        //    to.ShiftPolicyId = from.ShiftPolicyId;

        //    // ShiftPolicy ...
        //    ShiftPolicy policy;
        //    Map( from.ShiftPolicy , out policy );
        //    to.ShiftPolicy = policy;

        //    // SchedulePatterns
        //    if ( from.SchedulePatterns != null )
        //    {
        //        to.SchedulePatterns = new List<ScheduleTemplate>();

        //        for ( int i = 0 ; i < from.SchedulePatterns.Count ; i++ )
        //        {
        //            ScheduleTemplate schedulePattern;
        //            Map( from.SchedulePatterns[i] , out schedulePattern );
        //        }
        //    }

        //    return true;
        //}
        //public static bool Map( Shift from , out ShiftServiceDTO to )
        //{
        //    to = new ShiftServiceDTO();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.StartTime = from.StartTime;
        //    to.EndTime = from.EndTime;
        //    to.MinimumParties = from.MinimumParties;
        //    to.ShiftPolicyId = from.ShiftPolicyId;

        //    // ShiftPolicy ...
        //    ShiftPolicyServiceDTO policy;
        //    Map( from.ShiftPolicy , out policy );
        //    to.ShiftPolicy = policy;

        //    // SchedulePatterns
        //    if ( from.SchedulePatterns != null )
        //    {
        //        to.SchedulePatterns = new List<ScheduleTemplateServiceDTO>();

        //        for ( int i = 0 ; i < from.SchedulePatterns.Count ; i++ )
        //        {
        //            ScheduleTemplateServiceDTO schedulePattern;
        //            Map( from.SchedulePatterns[i] , out schedulePattern );
        //        }
        //    }

        //    return true;
        //}

        //public static bool Map( LocationServiceDTO from , out Location to )
        //{
        //    to = new Location();

        //    to.Id = from.Id;
        //    to.Name = from.Name;

        //    return true;
        //}
        //public static bool Map( Location from , out LocationServiceDTO to )
        //{
        //    to = new LocationServiceDTO();

        //    to.Id = from.Id;
        //    to.Name = from.Name;

        //    return true;
        //}

        //public static bool Map( AttendanceCodeServiceDTO from , out AttendanceCode to )
        //{
        //    to = new AttendanceCode();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.Code = from.Code;
        //    to.Grace = from.Grace;
        //    to.Points = from.Points;
        //    to.IsActive = from.IsActive;

        //    return true;
        //}
        //public static bool Map( AttendanceCode from , out AttendanceCodeServiceDTO to )
        //{
        //    to = new AttendanceCodeServiceDTO();

        //    to.Id = from.Id;
        //    to.Name = from.Name;
        //    to.Code = from.Code;
        //    to.Grace = from.Grace;
        //    to.Points = from.Points;
        //    to.IsActive = from.IsActive;

        //    return true;
        //}
        //public static bool Map ( PersonSearchResult from , out PersonSearchResultDTO to )
        //{
        //    to = new PersonSearchResultDTO ();
        //    to.ResultTotalCount = from.ResultTotalCount;
        //    to.Persons = new List<PersonServiceDTO> ();

        //    foreach ( var person in from.Persons )
        //    {
        //        PersonServiceDTO dTO;
        //        Map ( person , out dTO );
        //        to.Persons.Add ( dTO );
        //    }

        //    return true;
        //}
        //public static bool Map ( PersonSearchResultDTO from , out PersonSearchResult to )
        //{
        //    to = new PersonSearchResult ();
        //    to.ResultTotalCount = from.ResultTotalCount;
        //    to.Persons = new List<Person> ();

        //    foreach ( var personServiceDto in from.Persons )
        //    {
        //        Person p;
        //        Map ( personServiceDto , out p );
        //        to.Persons.Add ( p );
        //    }

        //    return true;
        //}
        #endregion

        #endregion
    }
}