using System;
using System.Linq;
using System.Collections.Generic;

using FluentValidation;

using ADS.Common.Validation;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Common.Utilities;

namespace ADS.Tamam.Common.Data.Validation
{
    public class ScheduleTemplateValidator : AbstractModelValidator<ScheduleTemplate>
    {
        #region cst.

        public ScheduleTemplateValidator( ScheduleTemplate model , TamamConstants.ValidationMode mode ) : base( model , new ValidationContext( mode ) )
        {
        }

        #endregion
        #region classes

        internal class ValidationContext : AbstractValidator<ScheduleTemplate>
        {
            public ValidationContext(TamamConstants.ValidationMode mode)
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    RuleFor( Template => Template.Name ).NotEmpty().WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateNameNotValid );
                    RuleFor( Template => Template.NameCultureVarient ).NotEmpty().WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateNameNotValid );
                    RuleFor( Template => Template.Name ).Length( 1 , 100 ).WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateNameNotValid );
                    RuleFor( Template => Template.NameCultureVarient ).Length( 1 , 100 ).WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateNameNotValid );
                    RuleFor( Template => Template.Description ).Length( 0 , 200 );
                    RuleFor( Template => Template.Repeat ).NotEmpty().WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateRepeatEmpty );

                    //Week type has limit to 6 weeks.
                    RuleFor( Template => Template.Repeat ).LessThanOrEqualTo( 6 ).When( Template => Template.RepeatType == 2 ).WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateWeekRepeat );

                    //Monthly type has limit to only 1 month.
                    RuleFor( Template => Template.Repeat ).Equal( 1 ).When( Template => Template.RepeatType == 3 ).WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateMonthRepeat );

                    RuleFor( Template => Template.RepeatType ).NotEmpty().WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateRepeatTypeEmpty );

                    RuleFor( Template => Template.Name ).Must( IsNameUnique ).WithMessage( Resources.Culture.ValidationResources.NameNotUnique );
                    RuleFor( Template => Template.NameCultureVarient ).Must( IsNameCultureVariantUnique ).WithMessage( Resources.Culture.ValidationResources.NameCultureVariantNotUnique );
                    RuleFor( Template => Template.TemplateDetails ).Must( IsValidScheduleTemplateDetails ).WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateNoDetails );
                    RuleFor( Template => Template.TemplateDetails ).Must( IsValidFlexibleShift ).WithMessage( Resources.Culture.ValidationResources.ScheduleTemplateMoreThanOneFlexible );
                    RuleFor( Template => Template.TemplateDetails ).Must( IsValidNightShift ).WithMessage( Resources.Culture.ValidationResources.ShiftOverlap );
                    RuleFor( Template => Template.TemplateDetails ).SetCollectionValidator( new ScheduleTemplateDetailsValidator() );
                }
                else if ( mode == TamamConstants.ValidationMode.Deactivate || mode == TamamConstants.ValidationMode.Delete )
                {
                    RuleFor( Template => Template.Id ).Must( CanDeleteTemplate ).WithMessage( Resources.Culture.ValidationResources.TemplateHaveSchedule );
                }
            }

            #region Helpers

            private bool IsNameUnique(ScheduleTemplate instance, string Name)
            {
                return new SchedulesDataHandler().IsScheduleTemplateNameUnique(instance);
            }
            private bool IsNameCultureVariantUnique(ScheduleTemplate instance, string NameCultureVariant)
            {
                return new SchedulesDataHandler().IsScheduleTemplateNameCultureVariantUnique(instance);
            }
            private bool IsValidScheduleTemplateDetails(ScheduleTemplate instance, IList<ScheduleTemplateDays> details)
            {
                var shifts = details.Where(x => x.IsDayOff == false && x.DayShifts != null).ToList();
                if (instance.Repeat == 0) return true;
                return shifts.Count > 0;
            }
            private bool IsValidFlexibleShift(ScheduleTemplate instance, IList<ScheduleTemplateDays> scheduleTemplateDays)
            {
                var schedulesDataHandler = new SchedulesDataHandler();
                var shiftsResponse = schedulesDataHandler.GetShifts(true);
                if (shiftsResponse.Type != ADS.Common.Context.ResponseState.Success) return false;
                var flexibleShiftsIds = shiftsResponse.Result.Where(s => s.IsFlexible).Select(s => s.Id);
                
                foreach (var scheduleTemplateDay in scheduleTemplateDays)
                {
                    var hasFlexibleShift = scheduleTemplateDay.DayShifts.Any(x => flexibleShiftsIds.Contains(x.ShiftId));
                    var manyShifts = scheduleTemplateDay.DayShifts.Count() > 1;
                    if (hasFlexibleShift && manyShifts) return false;
                }

                return true;
            }
            private bool IsValidNightShift( ScheduleTemplate instance , IList<ScheduleTemplateDays> scheduleTemplateDays )
            {
                var schedulesDataHandler = new SchedulesDataHandler();
                var shiftsResponse = schedulesDataHandler.GetShifts( true );
                if ( shiftsResponse.Type != ADS.Common.Context.ResponseState.Success ) return false;
                var shifts = shiftsResponse.Result;

                foreach ( var templateDay in scheduleTemplateDays )
                {
                    var nightShift = shifts.Where( x => x.IsNightShift && templateDay.DayShifts.Any( y => y.ShiftId == x.Id ) ).FirstOrDefault();   // has a night shift ? get it ...
                    if ( nightShift == null ) continue;
                    
                    var nextDaysNonNightShifts = shifts.Where( x => !x.IsNightShift && scheduleTemplateDays.Any( y => y.Sequence == templateDay.Sequence + 1 && y.DayShifts.Any( z => z.ShiftId == x.Id ) ) ).ToList();
                    if ( nextDaysNonNightShifts.Count == 0 ) continue;

                    foreach ( var nextDaysNonNightShift in nextDaysNonNightShifts )
                    {
                        if ( XIntervals.IsOverlapped( new XIntervals.TimeRange( new TimeSpan( 0 , 0 , 0 ) , nightShift.EndTime.Value.TimeOfDay ) , new XIntervals.TimeRange( nextDaysNonNightShift.StartTime.Value.TimeOfDay , nextDaysNonNightShift.EndTime.Value.TimeOfDay ) ) ) return false;
                    }
                }

                return true;
            }

            private bool CanDeleteTemplate(ScheduleTemplate instance, Guid Id)
            {
                var handler = new SchedulesDataHandler();
                var template = handler.GetScheduleTemplate(Id).Result;
                return template.Schedules == null || template.Schedules.Count == 0;
            }

            #endregion
        }

        #endregion
    }
    internal class ScheduleTemplateDetailsValidator : AbstractValidator<ScheduleTemplateDays>
    {
        #region cst ...

        public ScheduleTemplateDetailsValidator()
        {
            RuleFor( detail => detail.Template ).NotEmpty();
            //RuleFor ( detail => detail.DayShifts ).NotEmpty ().When ( detail => !detail.IsDayOff );
            RuleFor( detail => detail.DayShifts ).Must( CheckOverlap ).WithMessage( ADS.Tamam.Resources.Culture.ValidationResources.ShiftOverlap );
            // RuleFor ( detail => detail.Sequence ).Must ( IsSequenceUnique );
        }
        
        #endregion
        #region Helpers
        
        private bool IsSequenceUnique(ScheduleTemplateDays instance, int Sequence)
        {
            return new SchedulesDataHandler().IsTemplateDetailUniquePerTemplate(instance);
        }
        private bool CheckOverlap( IList<ScheduleTemplateDayShifts> days )
        {
            var dataHandler = new SchedulesDataHandler();
            var shifts = days.Select( s => dataHandler.GetShift( s.ShiftId ).Result ).ToList();
            return Shift.CheckOverlap( shifts );
        }

        #endregion
    }
}
