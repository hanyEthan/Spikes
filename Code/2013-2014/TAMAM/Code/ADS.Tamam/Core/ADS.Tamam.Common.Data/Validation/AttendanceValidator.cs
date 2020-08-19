using System;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using FluentValidation;

using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;

namespace ADS.Tamam.Common.Data.Validation
{
    public class AttendanceValidator : AbstractModelValidator<AttendanceEventMetadata>
    {
        #region classes

        protected class ValidationContext : AbstractValidator<AttendanceEventMetadata>
        {
            #region privates

            private bool _initialized;
            private ValidationMode _validationMode;
            //private AttendanceDataHandler _dataHandler;
            private PersonnelDataHandler _dataHandler_Personnel;

            #endregion

            public ValidationContext( ValidationMode mode )
            {
                _validationMode = mode;
                _initialized = InitializeDataLayer();

                if ( mode == ValidationMode.Edit )
                {
                    RuleFor( x => x.ScheduleId ).Must( IsScheduleEventValid ).WithMessage( ValidationResources.ScheduleEventMissing );

                    // make sure that no multiple manual edits (Pending) in the same time..
                    RuleFor( x => x.AttendanceId ).Must( IsAttendanceEventValidForEdit ).WithMessage( ValidationResources.InvalidEditAttendanceEvent );

                    RuleFor( x => x.Date ).NotEqual( DateTime.MinValue ).WithMessage( ValidationResources.AttendanceDateMissing );
                    //RuleFor( x => x.Comment ).NotNull().WithMessage( ValidationResources.CommentMissing );
                    RuleFor( x => x.Comment ).NotEmpty().WithMessage( ValidationResources.CommentMissing );
                    RuleFor( x => x.Comment ).Length( 0 , 100 ).WithMessage( ValidationResources.CommentIncorrect );
                }
                else if ( mode == ValidationMode.Create )
                {
                    RuleFor( x => x.PersonId ).Must( IsPersonValid ).WithMessage( ValidationResources.PersonMissing );

                    RuleFor( x => x.Date ).NotEqual( DateTime.MinValue ).WithMessage( ValidationResources.AttendanceDateMissing );
                    //RuleFor( x => x.Comment ).NotNull().WithMessage( ValidationResources.CommentMissing );
                    RuleFor( x => x.Comment ).NotEmpty().WithMessage( ValidationResources.CommentMissing );
                    RuleFor(x => x.Comment).Length(0, 100).WithMessage(ValidationResources.CommentIncorrect);
                }
                else if ( mode == ValidationMode.Delete )
                {
                    RuleFor( x => x.PersonId ).Must( IsPersonValid ).WithMessage( ValidationResources.PersonMissing );
                }
            }

            #region Helpers

            private bool IsAttendanceEventValidForEdit( AttendanceEventMetadata instance , Guid? attendanceEventId )
            {
                //if ( !attendanceEventId.HasValue ) return true;
                //var AE = _dataHandler.GetAttendanceEvent( attendanceEventId.Value ).Result;
                //return AE != null && AE.ManualAttendanceStatus != ManualAttendanceStatus.Pending;
                return true;
            }
            private bool IsScheduleEventValid( AttendanceEventMetadata instance , Guid? scheduleEventId )
            {
                //if ( !_initialized || !scheduleEventId.HasValue ) return false;
                //return _dataHandler.CheckScheduleEvent( scheduleEventId.Value ).Result;
                return true;
            }
            private bool IsPersonValid( AttendanceEventMetadata instance , Guid? personId )
            {
                if ( !_initialized || !personId.HasValue ) return false;
                return _dataHandler_Personnel.CheckPersonExistence(personId.Value).Result;
            }

            private bool InitializeDataLayer()
            {
                //_dataHandler = new AttendanceDataHandler();
                //_dataHandler_Personnel = new PersonnelDataHandler();
                //return _dataHandler.Initialized && _dataHandler_Personnel.Initialized;
                return true;
            }

            #endregion
        }
        public enum ValidationMode { Create , Edit , Delete , }

        #endregion
        #region cst.

        public AttendanceValidator( AttendanceEventMetadata model , ValidationMode mode ) : base( model , new ValidationContext( mode ) )
        {
        }

        #endregion
    }
}
