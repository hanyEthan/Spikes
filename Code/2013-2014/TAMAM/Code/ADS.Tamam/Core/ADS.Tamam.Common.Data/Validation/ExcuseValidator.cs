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
    public class ExcuseValidator : AbstractModelValidator<Excuse>
    {
        #region cst.

        public ExcuseValidator( Excuse model , ExcuseValidationMode mode ) : base( model , new ValidationContext( mode ) )
        {
        }

        # endregion
        #region classes

        internal class ValidationContext : AbstractValidator<Excuse>
        {
            public ValidationContext( ExcuseValidationMode mode )
            {
                RuleFor( excuse => excuse.ExcuseDate ).NotEmpty().NotNull().WithMessage( ValidationResources.ExcuseDateEmpty , l => GetLocalizedFullName( l.Person ) );
                RuleFor( excuse => excuse.ExcuseStatusId ).NotEmpty().NotNull().WithMessage( ValidationResources.ExcuseStatusEmpty , l => GetLocalizedFullName( l.Person ) );
                RuleFor( excuse => excuse.ExcuseTypeId ).GreaterThan( 0 ).WithMessage( ValidationResources.ExcuseTypeEmpty );
                RuleFor( excuse => excuse.PersonId ).Must( IsEmptyPersonId ).WithMessage( ValidationResources.ExcusePersonEmpty );
                RuleFor( excuse => excuse.StartTime ).NotEmpty().NotNull().WithMessage( ValidationResources.ExcuseStartTimeEmpty , l => GetLocalizedFullName( l.Person ) );
                RuleFor( excuse => excuse.EndTime ).NotEmpty().NotNull().WithMessage( ValidationResources.ExcuseEndTimeEmpty , l => GetLocalizedFullName( l.Person ) );
                //RuleFor( excuse => excuse.EndTime ).Must( IsValidTimes ).WithMessage( ValidationResources.ExcuseInvalidEndTime , x => x.IsAwayExcuse() ? Resources.Culture.Common.Away : Resources.Culture.Common.Excuse , l => GetLocalizedFullName( l.Person ) );
                RuleFor( excuse => excuse.EndTime ).Must( IsExcuseUnique ).WithMessage( ValidationResources.ExcuseUnique, x => x.IsAwayExcuse() ? Resources.Culture.Common.Away : Resources.Culture.Common.Excuse, l => GetLocalizedFullName( l.Person ) );

                switch ( mode )
                {
                    case ExcuseValidationMode.Create:
                        {
                            // check for date (must be in previous or current or next month)
                            RuleFor( excuse => excuse.ExcuseDate ).Must( IsValidExcuseCreationDate ).WithMessage( ValidationResources.InvalidExcuseCreationDate , x => x.IsAwayExcuse() ? Resources.Culture.Common.Away : Resources.Culture.Common.Excuse , l => GetLocalizedFullName( l.Person ) );
                            RuleFor( excuse => excuse.IsNative ).Must( IsNativeTamamExcuse ).WithMessage( ValidationResources.IsNative );
                        }
                        break;

                    case ExcuseValidationMode.Request:
                        {
                            RuleFor( excuse => excuse.ExcuseDate ).Must( IsValidExcuseRequestDate ).WithMessage( ValidationResources.InvalidExcuseRequestDate , x => x.IsAwayExcuse() ? Resources.Culture.Common.Away : Resources.Culture.Common.Excuse );
                            RuleFor( excuse => excuse.IsNative ).Must( IsNativeTamamExcuse ).WithMessage( ValidationResources.IsNative );
                        }
                        break;
                    case ExcuseValidationMode.Edit:
                        {
                            RuleFor( excuse => excuse.Id ).Must( CanEditTakenExcuse ).WithMessage( ValidationResources.EditTakenExcuse , l => GetLocalizedFullName( l.Person ) );
                            RuleFor( excuse => excuse.IsNative ).Must( IsNativeTamamExcuse ).WithMessage( ValidationResources.IsNative );
                        }
                        break;
                    case ExcuseValidationMode.Cancel:
                        {
                            RuleFor( excuse => excuse.ExcuseStatusId ).Must( CanCancelExcuse ).WithMessage( ValidationResources.ExcuseCanCancelled , l => GetLocalizedFullName( l.Person ) );
                            RuleFor( excuse => excuse.IsNative ).Must( IsNativeTamamExcuse ).WithMessage( ValidationResources.IsNative );
                        }
                        break;
                    case ExcuseValidationMode.Review:
                        {
                            RuleFor( excuse => excuse.ExcuseStatusId ).Must( CanReviewExcuse ).WithMessage( ValidationResources.ExcuseCanReviewed , l => GetLocalizedFullName( l.Person ) );
                            RuleFor( excuse => excuse.IsNative ).Must( IsNativeTamamExcuse ).WithMessage( ValidationResources.IsNative );
                        }
                        break;
                    case ExcuseValidationMode.SystemCreate:
                        {
                            RuleFor( excuse => excuse.ExcuseStatusId ).Must( CanCancelExcuse ).WithMessage( ValidationResources.InvalidExcuseStatusIntegrationMode );
                            RuleFor( excuse => excuse.IsNative ).Must( IsIntegratedExcuse ).WithMessage( ValidationResources.IsNative );
                        }
                        break;
                    case ExcuseValidationMode.SystemEdit:
                        {
                            RuleFor( excuse => excuse.ExcuseStatusId ).Must( CanCancelExcuse ).WithMessage( ValidationResources.InvalidExcuseStatusIntegrationMode );
                            RuleFor( excuse => excuse.IsNative ).Must( IsIntegratedExcuse ).WithMessage( ValidationResources.IsNative );
                        }
                        break;
                    default: break;
                }
            }

            #region Helpers

            private bool IsEmptyPersonId( Excuse instance , Guid personId )
            {
                return !( personId == Guid.Empty );
            }
            private bool CanCancelExcuse( Excuse instance , int statusId )
            {
                return statusId != ( int ) ExcuseStatus.Denied && statusId != ( int ) ExcuseStatus.Cancelled;
            }
            private bool CanReviewExcuse( Excuse instance , int statusId )
            {
                return statusId != ( int ) ExcuseStatus.Cancelled && statusId != ( int ) ExcuseStatus.Taken;
            }
            private bool IsExcuseUnique( Excuse instance , DateTime endDate )
            {
                //var handler = new LeavesDataHandler();
                //return handler.IsExcuseUnique( instance );

                return true;
            }
            private bool IsValidTimes( Excuse instance , DateTime endDate )
            {
                var ordered = instance.EndTime > instance.StartTime;

                return ordered;
            }
            private bool IsValidExcuseRequestDate( Excuse instance , DateTime date )
            {
                var start = DateTime.Today.Date;
                var end = new DateTime( start.Year , start.Month , DateTime.DaysInMonth( start.Year , start.Month ) ).AddMonths( 1 );

                return date.Date >= start.Date && date.Date <= end.Date;
            }
            private bool IsValidExcuseCreationDate( Excuse instance , DateTime date )
            {
                var start = new DateTime( DateTime.Today.Year , DateTime.Today.Month , 1 ).AddMonths( -1 );
                var end = new DateTime( DateTime.Today.Year , DateTime.Today.Month , DateTime.DaysInMonth( DateTime.Today.Year , DateTime.Today.Month ) ).AddMonths( 1 );

                return date.Date >= start.Date && date.Date <= end.Date;
            }
            private bool CanEditTakenExcuse( Excuse instance , Guid excuseId )
            {
                //var handler = new LeavesDataHandler();
                //var response = handler.GetExcuse( excuseId , SystemSecurityContext.Instance );
                //var orgExcuse = response.Result;
                //if ( orgExcuse == null ) return false;

                //if ( orgExcuse.ExcuseStatusId == ( int ) ExcuseStatus.Taken )
                //{
                //    var differences = XModel.GetDifferences( orgExcuse , instance , "Notes" );
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
            private bool IsValidStatusForSystemCreateAndEdit( Excuse instance , int statusId )
            {
                return statusId == ( int ) ExcuseStatus.Approved;
            }
            private bool IsNativeTamamExcuse( Excuse excuse , bool isNative )
            {
                return isNative;
            }
            private bool IsIntegratedExcuse( Excuse excuse , bool isNative )
            {
                return !isNative;
            }

            #endregion
        }
        public enum ExcuseValidationMode
        {
            Create ,
            Request ,
            Edit ,
            Cancel ,
            Review ,
            SystemCreate ,
            SystemEdit ,
        }

        #endregion
    }
}
