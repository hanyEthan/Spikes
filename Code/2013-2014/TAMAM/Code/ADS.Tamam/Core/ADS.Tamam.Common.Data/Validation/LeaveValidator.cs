using System;
using System.Linq;

using FluentValidation;

using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Context;
//using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;

namespace ADS.Tamam.Common.Data.Validation
{
    public class LeaveValidator : AbstractModelValidator<Leave>
    {
        #region cst.

        public LeaveValidator( Leave model , ValidationMode validationMode ) : base( model , new ValidationContext( validationMode ) )
        {
        }

        # endregion
        #region classes

        public enum ValidationMode { Create , Edit , Cancel , SystemCreate , SystemEdit }

        internal class ValidationContext : AbstractValidator<Leave>
        {
            #region cst ...

            public ValidationContext( ValidationMode validationMode )
            {
                if (validationMode == ValidationMode.Create || validationMode == ValidationMode.Edit || validationMode == ValidationMode.SystemCreate || validationMode == ValidationMode.SystemEdit )
                {
                    RuleFor( leave => leave.StartDate ).Must( isValidDate ).WithMessage( ValidationResources.LeaveStartDateEmpty , l => GetLocalizedFullName( l.Person ) );
                    RuleFor( leave => leave.EndDate ).Must( isValidDate ).WithMessage( ValidationResources.LeaveEndDateEmpty , l => GetLocalizedFullName( l.Person ) );
                    RuleFor( leave => leave.EndDate ).Must( isValidDateRange ).WithMessage( ValidationResources.EndDateShouldGreaterThanStartDate , l => GetLocalizedFullName( l.Person ) );
                    RuleFor( leave => leave.LeaveStatusId ).Must( IsValidInt ).WithMessage( ValidationResources.LeaveStatusIdEmpty , l => GetLocalizedFullName( l.Person ) );
                    RuleFor( leave => leave.LeaveTypeId ).Must( IsValidInt ).WithMessage( ValidationResources.LeaveTypeIdEmpty , l => GetLocalizedFullName( l.Person ) );
                    //RuleFor( leave => leave.EndDate ).GreaterThanOrEqualTo( leave => leave.StartDate ).WithMessage( ValidationResources.LeaveInvalidStartAndEndDate , l => GetLocalizedFullName( l.Person ) );

                    RuleFor( leave => leave.PersonId ).Must( isValidGuid ).WithMessage( ValidationResources.PersonIdEmpty );
                    RuleFor( leave => leave.EndDate ).Must( IsLeaveUnique ).WithMessage( ValidationResources.LeaveUnique , l => GetLocalizedFullName( l.Person ) );
                    
                    //RuleFor(leave => leave.EffectiveDaysCount).NotEqual(0).WithMessage(ValidationResources.LeaveEffectiveDaysEqualZero);

                    // for both (Create Leave & Edit Leave)

                    //MG,9-11-2014.Check that start date is after the Effective Year Start for the leave credit.
                    RuleFor( leave => leave.StartDate ).Must( IsValidStartDate ).WithMessage( ValidationResources.LeaveStartDateInvalid , l => GetLocalizedFullName( l.Person ) );
                }

                if (validationMode == ValidationMode.Create || validationMode == ValidationMode.Edit )
                {
                    RuleFor( leave => leave.PersonId ).Must( IsPersonActive ).WithMessage( ValidationResources.LeavePersonNotActive , l => GetLocalizedFullName( l.Person ) );
                    RuleFor(leave => leave.LeaveStatusId).Must(IsLeaveStatusValid).WithMessage(ValidationResources.InvalidCreateLeaveStatus, l => GetLocalizedFullName(l.Person));
                    RuleFor(leave => leave.LeaveMode).Must(CheckHalfDayCondition).WithMessage(ValidationResources.LeaveHalfDayInvalid, l => GetLocalizedFullName(l.Person));
                    RuleFor(leave => leave.Id).Must(CanEditLeave).WithMessage(ValidationResources.LeaveCanEdit, l => GetLocalizedFullName(l.Person));
                    RuleFor(leave => leave.IsNative).Must(IsNativeTamamLeave).WithMessage(ValidationResources.IsNative); 
                }
                if ( validationMode == ValidationMode.SystemEdit )
                {
                    RuleFor( leave => leave.Id ).Must( CanEditSystemLeave ).WithMessage( ValidationResources.LeaveCanEdit , l => GetLocalizedFullName( l.Person ) );
                }
                if (validationMode == ValidationMode.SystemCreate || validationMode == ValidationMode.SystemEdit)
                {
                    RuleFor(leave => leave.LeaveStatusId).Must(IsLeaveStatusValidForSystemMode).WithMessage(ValidationResources.InvalidSystemCreateLeaveStatus);
                    RuleFor(leave => leave.IsNative).Must(IsIntegratedLeave).WithMessage(ValidationResources.IsNative); 
                }

                if (validationMode == ValidationMode.Edit || validationMode == ValidationMode.SystemEdit)
                {
                    RuleFor( leave => leave.Id ).Must( IsValidId );
                }

                if (validationMode == ValidationMode.Edit )
                {
                    RuleFor(leave => leave.Id).Must(CanEditTakenLeave).WithMessage(ValidationResources.EditCanceledLeaveWithUnexpectedValues, l => GetLocalizedFullName(l.Person));
                }

                if ( validationMode == ValidationMode.Cancel )
                {
                    RuleFor( leave => leave.LeaveStatusId ).Must( CanCancelLeave ).WithMessage( ValidationResources.LeaveCanCancelled , l => GetLocalizedFullName( l.Person ) );
                    RuleFor(leave => leave.IsNative).Must(IsNativeTamamLeave).WithMessage(ValidationResources.IsNative); 
                }
            }
            
            #endregion
            #region Helpers

            //MG,9-11-2014.Check that start date is after the Effective Year Start for the leave credit.
            private bool IsValidStartDate( Leave instance , DateTime arg )
            {
                //var handler = new LeavesDataHandler();
                //var creditHistory = handler.GetLeaveCreditHistory( instance.PersonId , SystemSecurityContext.Instance );
                //if ( creditHistory.Type == ADS.Common.Context.ResponseState.Success )
                //{
                //    var credit = creditHistory.Result.OrderByDescending( o => o.EffectiveYearStart ).FirstOrDefault();
                //    if ( credit == null )
                //        return true;
                //    return instance.StartDate >= credit.EffectiveYearStart;
                //}
                return true;
            }

            // Check for empty values
            private bool isValidDate( Leave instance , DateTime date )
            {
                return date != default( DateTime );
            }
            private bool isValidDateRange( Leave instance , DateTime date )
            {
                return instance.StartDate <= instance.EndDate;
            }
            private bool isValidGuid( Leave instance , Guid personId )
            {
                return personId != Guid.Empty;
            }
            private bool IsValidInt( Leave instance , int id )
            {
                return id != default( int );
            }

            private bool IsPersonActive( Leave instance , Guid personId )
            {
                // To display only the error validation related to IsEmpty
                if ( personId == Guid.Empty ) return true;

                var handler = new PersonnelDataHandler();
                var person = handler.GetPerson( personId , SystemSecurityContext.Instance );
                return person.Result != null && person.Result.Activated;
            }
            private bool IsLeaveUnique( Leave instance , DateTime endDate )
            {
                //var handler = new LeavesDataHandler();
                //return handler.IsLeaveUnique( instance );
                return true;
            }
            private bool CanEditLeave( Leave instance , Guid leaveId )
            {
                //var handler = new LeavesDataHandler();
                //var obj = handler.GetLeave( leaveId , SystemSecurityContext.Instance );
                //if ( obj.Result == null )
                //    return true;
                //if ( obj.Result.LeaveStatusId == ( int ) LeaveStatus.Cancelled ||
                //    obj.Result.LeaveStatusId == ( int ) LeaveStatus.Approved ||
                //    obj.Result.LeaveStatusId == ( int ) LeaveStatus.Denied )
                //    return false;
                return true;
            }
            private bool CanEditSystemLeave(Leave instance, Guid leaveId)
            {
                //var handler = new LeavesDataHandler();
                //var obj = handler.GetLeave(leaveId, SystemSecurityContext.Instance);
                //if (obj.Result == null) return true;
                //if (!(obj.Result.LeaveStatusId == (int)LeaveStatus.Approved || obj.Result.LeaveStatusId == (int) LeaveStatus.Taken)) return false;
                return true;
            }
            private bool IsLeaveStatusValid(Leave instance, int status)
            {
                return status == ( int ) LeaveStatus.Planned || status == ( int ) LeaveStatus.Pending || status == ( int ) LeaveStatus.Taken;
            }
            private bool IsLeaveStatusValidForSystemMode(Leave instance, int status)
            {
                return status == (int)LeaveStatus.Approved || status ==(int)LeaveStatus.Cancelled;
            }

            private bool IsValidId( Guid Id )
            {
                return Id != Guid.Empty;
            }

            private bool CanCancelLeave( Leave instance , int statusId )
            {
                return statusId != ( int ) LeaveStatus.Denied && statusId != ( int ) LeaveStatus.Cancelled;
            }

            private bool CanEditTakenLeave( Leave instance , Guid leaveId )
            {
                //var handler = new LeavesDataHandler();
                //var response = handler.GetLeave( leaveId , SystemSecurityContext.Instance );
                //var orgLeave = response.Result;
                //if ( orgLeave == null ) return false;

                //if ( orgLeave.LeaveStatusId == ( int ) LeaveStatus.Taken )
                //{
                //    var differences = XModel.GetDifferences( orgLeave , instance , "Notes" );
                //    if ( differences.Any() )
                //    {
                //        return false;
                //    }
                //}

                return true;
            }

            private string GetLocalizedFullName( Person p )
            {
                return p == null ? string.Empty : p.GetLocalizedFullName();
            }

            private bool CheckHalfDayCondition( Leave instance , LeaveMode leaveMode )
            {
                if ( instance.LeaveMode != LeaveMode.HalfDay ) return true;
                if ( instance.StartDate != instance.EndDate ) return false;

                return true;
            }

            private bool IsNativeTamamLeave(Leave leave, bool isNative)
            {
                return isNative;
            }

            private bool IsIntegratedLeave(Leave leave, bool isNative)
            {
                return !isNative;
            }

            #endregion
        }

        #endregion
    }
}