using System;
using System.Linq;
using System.Collections.Generic;

using FluentValidation;

using ADS.Common.Context;
using ADS.Common.Validation;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Reports;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;

namespace ADS.Tamam.Common.Data.Validation
{
    public class OrganizationValidator : AbstractModelValidator<Department>
    {
        internal class ValidationContext : AbstractValidator<Department>
        {
            #region props.

            internal List<string> DbValidationErros;
            private OrganizationDataHandler DataHandler;

            #endregion

            public ValidationContext()
            {
                this.DataHandler = new Handlers.OrganizationDataHandler();
            }
            public ValidationContext( Department model , TamamConstants.ValidationMode mode ) : this()
            {
                if ( mode == TamamConstants.ValidationMode.Create || mode == TamamConstants.ValidationMode.Edit )
                {
                    DbValidation( model );
                    RuleFor( department => department.Name ).NotEmpty().WithMessage( ValidationResources.DepartmentNameEmpty );
                    RuleFor( department => department.NameCultureVarient ).NotEmpty().WithMessage( ValidationResources.DepartmentNameCultureVariantEmpty );
                    RuleFor( department => department.Code ).NotEmpty().WithMessage( ValidationResources.OrganizationDetailCodeEmpty );
                    RuleFor( department => department.ParentDepartmentId ).NotEqual( department => department.Id ).WithMessage( ValidationResources.DepartmentParentEqSelf );
                    RuleFor( department => department.Name ).Must( IsNameUnique ).WithMessage( ValidationResources.DepartmentNameUnique );
                    RuleFor( department => department.Code ).Must( IsCodeUnique ).WithMessage( ValidationResources.DepartmentCodeUnique );
                    RuleFor( department => department.SupervisorId ).Must( IsSupervisorValid ).WithMessage( ValidationResources.SupervisorNotVaild );
                    RuleFor( department => department.ParentDepartmentId ).Must( IsParentValid ).WithMessage( ValidationResources.DepartmentParentInvalid );

                    RuleFor( department => department.Name ).Length( 0 , 200 ).WithMessage( ValidationResources.NameLength );
                    RuleFor( department => department.NameCultureVarient ).Length( 0 , 200 ).WithMessage( ValidationResources.ArabicNameLength );
                    RuleFor( department => department.NameCultureVarientAbstract ).Length( 0 , 200 ).WithMessage( ValidationResources.ArabicNameLength );
                    RuleFor( department => department.Description ).Length( 0 , 200 ).WithMessage( ValidationResources.DepartmentDescriptionLength );

                    if ( mode == TamamConstants.ValidationMode.Edit )
                    {
                        RuleFor( department => department.ParentDepartmentId ).Must( IsParentRecursive ).WithMessage( ValidationResources.DepartmentParentRecursive );
                    }
                }
                else if ( mode == TamamConstants.ValidationMode.Delete )
                {
                    RuleFor( department => department.Id ).Must( DepartmentUsedInHRPolicy ).WithMessage( ValidationResources.DepartmentUsedInPolicy );
                    RuleFor( department => department.Id ).Must( ValidateDeleteDepend ).WithMessage( ValidationResources.DepartmentDeleteFailed );
                    RuleFor( department => department.Id ).Must( IsEmptyFromSchedules ).WithMessage( ValidationResources.DepartmentDeleteFailed_RelatedSchedules );
                    RuleFor( department => department.Id ).Must( IsEmptyFromScheduledReports ).WithMessage( ValidationResources.DepartmentDeleteFailed_RelatedScheduledReports );
                }
            }

            #region Helpers

            private bool IsSupervisorValid( Department instance , Guid? supervisorId )
            {
                if ( !supervisorId.HasValue || supervisorId == Guid.Empty ) return true;
                if ( DbValidationErros.Any( v => v == "SupervisorId" ) ) return false;

                return true;
            }
            private bool IsNameUnique( Department instance , string name )
            {
                if ( DbValidationErros.Any( v => v == "Name" ) ) return false;

                return true;
            }
            private bool IsCodeUnique( Department instance , string code )
            {
                if ( DbValidationErros.Any( v => v == "Code" ) ) return false;

                return true;
            }
            private void DbValidation( Department instance )
            {
                DbValidationErros = this.DataHandler.ValidateOrganization( instance );
            }
            private bool IsParentRecursive( Department instance , Guid? parentId )
            {
                if ( !parentId.HasValue || parentId == default( Guid ) ) return true;

                var tempParent = parentId;
                while ( tempParent.HasValue && tempParent.Value != default( Guid ) )
                {
                    var depart = this.DataHandler.GetDepartment( tempParent.Value , SystemSecurityContext.Instance );
                    if ( depart.Result.ParentDepartmentId == instance.Id )
                    {
                        return false;
                    }
                    else
                    {
                        tempParent = depart.Result.ParentDepartmentId;
                    }
                }

                return true;
            }
            private bool IsParentValid( Guid? arg )
            {
                if ( DbValidationErros.Any( v => v == "ParentId" ) )
                    return false;
                else
                    return true;
            }
            private bool DepartmentUsedInHRPolicy( Guid arg )
            {
                try
                {
                    var policies = this.DataHandler.GetPolicies( Guid.Parse( PolicyTypes.HRPolicyType ) ).Result.Select( p => new HRPolicy( p ) );
                    return !policies.Any( hr => hr.HRDepartment.Id == arg );
                }
                catch
                {
                    return false;
                }
            }
            private bool ValidateDeleteDepend( Guid arg )
            {
                return this.DataHandler.CheckDepartmentDependancy( arg );
            }

            private bool IsEmptyFromSchedules( Guid departmentId )
            {
                var hasActiveScheduleResponse = new SchedulesDataHandler().GetScheduleDepartments_DepartmentId( departmentId );
                if ( hasActiveScheduleResponse.Type != ResponseState.Success ) return false;

                return !hasActiveScheduleResponse.Result.Any();
            }
            private bool IsEmptyFromScheduledReports( Guid departmentId )
            {
                var criteria = new ScheduledReportEventsSearchCriteria() { Departments = new List<Guid> { departmentId } };
                var scheduledReportsResponse = new ReportsDataHandler().GetScheduledReportEvents( criteria , SystemSecurityContext.Instance );
                if ( scheduledReportsResponse.Type != ResponseState.Success ) return false;

                return !scheduledReportsResponse.Result.Any();
            }

            #endregion
        }
        #region cst.

        public OrganizationValidator( Department model , TamamConstants.ValidationMode mode = TamamConstants.ValidationMode.Create ) : base( model , new ValidationContext( model , mode ) )
        {
        }

        #endregion
    }
}
