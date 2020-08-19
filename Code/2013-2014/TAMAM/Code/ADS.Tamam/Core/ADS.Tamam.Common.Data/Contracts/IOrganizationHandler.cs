using System;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IOrganizationHandler : IBaseHandler
    {
        // Department
        ExecutionResponse<bool> CreateDepartment( Department department , RequestContext requestContext );
        ExecutionResponse<bool> EditDepartment( Department department , RequestContext requestContext );
        ExecutionResponse<bool> DeleteDepartment( Department department , RequestContext requestContext );

        ExecutionResponse<Department> GetDepartment ( Guid id , RequestContext requestContext );
        ExecutionResponse<List<Department>> GetDepartments( RequestContext requestContext );
       // ExecutionResponse<List<Department>> GetChildDepartments_WithSchedules(Guid? parentDepartmentId, RequestContext requestContext);
        // ExecutionResponse<object> GetDepartmentsByRoot( Guid rootId , RequestContext requestContext );
        ExecutionResponse<object> GetDepartmentsByPerson( Guid personId , RequestContext requestContext );

        ExecutionResponse<List<Department>> GetDepartmentsByParentId(Guid id, RequestContext requestContext);

        // OrganizationDetail
        ExecutionResponse<OrganizationDetail> GetOrganizationDetail(RequestContext reqContext);
        ExecutionResponse<bool> CreateOrganizationDetail(OrganizationDetail organizationDetail, RequestContext requestContext);
        ExecutionResponse<bool> EditOrganizationDetail(OrganizationDetail organizationDetail, RequestContext requestContext);
        ExecutionResponse<bool> DeleteOrganizationDetail(OrganizationDetail organizationDetail, RequestContext requestContext);

        // Policies
        ExecutionResponse<PolicyType> GetPolicyType( Guid id , RequestContext requestContext );
        
        ExecutionResponse<List<Policy>> GetPolicies ( Person person ,PolicyFilters filters, RequestContext requestContext );
        ExecutionResponse<List<Policy>> GetPolicies ( Person person , RequestContext requestContext );
        ExecutionResponse<List<Policy>> GetPolicies ( Guid personId , RequestContext requestContext );
        ExecutionResponse<List<Policy>> GetPolicies( PolicyFilters filters , RequestContext requestContext );
        ExecutionResponse<List<Policy>> GetPolicies(List<string> PolicyCodes, RequestContext requestContext);
        ExecutionResponse<Policy> GetPolicy( Guid id , RequestContext requestContext );
        ExecutionResponse<Guid> CreatePolicy( Policy policy , RequestContext requestContext );
        ExecutionResponse<bool> EditPolicy( Policy policy , RequestContext requestContext );
        ExecutionResponse<bool> DeletePolicy( Guid id , RequestContext requestContext );

        ExecutionResponse<List<Policy>> GetPolicies( Guid personId , PolicyFilters filter , RequestContext requestContext );
        ExecutionResponse<List<PolicyField>> GetPolicyFields( Guid policyTypeId , RequestContext requestContext );
        ExecutionResponse<PolicyField> GetPolicyField( Guid policyFieldId , RequestContext requestContext );
        ExecutionResponse<bool> AddPolicyField( PolicyField policyField , RequestContext requestContext );
        ExecutionResponse<bool> EditPolicyField( PolicyField policyField , RequestContext requestContext );
        ExecutionResponse<bool> DeletePolicyField( Guid policyFieldId , RequestContext requestContext );
        ExecutionResponse<bool> SwapPolicyFieldSequence( PolicyField policyFieldOne , PolicyField policyFieldTwo , RequestContext requestContext );
        
        // Attendance Codes
        ExecutionResponse<List<AttendanceCode>> GetAttendanceCodes(RequestContext requestContext);
        ExecutionResponse<bool> EditAttendanceCode(AttendanceCode attendanceCode, RequestContext requestContext);

        // Dashboard Web Parts
        ExecutionResponse<List<DashboardWebPart>> GetDashboardWebParts(RequestContext requestContext);

        // Reports
        ExecutionResponse<List<ReportCategory>> GetReports(RequestContext requestContext);
        ExecutionResponse<List<ReportCategory>> GetReports(ReportCategoryFilters filters, RequestContext requestContext);
        ExecutionResponse<ReportDefinition> GetReportReportDefinition(Guid id, RequestContext requestContext);
        //ExecutionResponse<List<ReportDefinition>> GetReportsDefinitions(RequestContext requestContext);

        ExecutionResponse<PolicyGroup> GetPolicyGroup( Guid policygroupId , RequestContext requestContext );
        ExecutionResponse<List<PolicyGroup>> GetPolicyGroups( RequestContext requestContext );
        ExecutionResponse<Guid> CreatePolicyGroup( PolicyGroup group , RequestContext requestContext );

        ExecutionResponse<bool> EditPolicyGroup( PolicyGroup group , List<Guid> updatedDepartments , List<Guid> updatedPersonnel , List<Guid> updatedPolicies , RequestContext requestContext );
        ExecutionResponse<bool> EditPolicyGroup( PolicyGroup group , RequestContext requestContext );
        ExecutionResponse<bool> DeletePolicyGroup( Guid policyGroupId , RequestContext requestContext );

        ExecutionResponse<List<Person>> GetPolicyGroupPersons( Guid policyGroupId , RequestContext requestContext );
        ExecutionResponse<bool> AssociateGroupToPerson( List<Guid> personIds , Guid policyGroupId , RequestContext requestContext );
        ExecutionResponse<bool> RemoveGroupAssociationFromPerson( Guid personId , RequestContext requestContext );
        ExecutionResponse<bool> RemoveGroupAssociationFromPerson( List<Guid> personId , RequestContext requestContext );

        ExecutionResponse<List<Department>> GetPolicyGroupDepartments( Guid policyGroupId , RequestContext requestContext );
        ExecutionResponse<bool> AssociateGroupToDepartment( List<Guid> departmentIds , Guid policyGroupId , RequestContext requestContext );
        ExecutionResponse<bool> RemoveGroupAssociationFromDepartment( List<Guid> departmentIds , RequestContext requestContext );

        ExecutionResponse<List<Policy>> GetPolicyGroupPolicies( Guid policyGroupId ,PolicyFilters filter, RequestContext requestContext );
        ExecutionResponse<bool> AssociateGroupToPolicies( List<Guid> policyIds , Guid policyGroupId , RequestContext requestContext );
        ExecutionResponse<bool> RemoveGroupAssociationFromPolicies( List<Guid> policyIds , Guid policyGroupId , RequestContext requestContext );
        ExecutionResponse<bool> ChangePolicyGroupActiveState( Guid policyGroupId , bool activeStatus , RequestContext requestContext );

        // Holidays..
        ExecutionResponse<List<Holiday>> GetHolidays(RequestContext requestContext);
        ExecutionResponse<List<Holiday>> GetHolidays(Guid personId,RequestContext requestContext);
        ExecutionResponse<List<Holiday>> GetHolidays(Guid personId, DateTime from, DateTime to, RequestContext requestContext);

        ExecutionResponse<List<Holiday>> GetNativeHolidays(RequestContext requestContext);
        ExecutionResponse<Holiday> GetNativeHoliday(Guid id, RequestContext requestContext);
        ExecutionResponse<Guid> CreateNativeHoliday(Holiday holiday, RequestContext requestContext);
        ExecutionResponse<bool> EditNativeHoliday(Holiday holiday, RequestContext requestContext);
        ExecutionResponse<bool> DeleteNativeHoliday(Guid id, RequestContext requestContext);
    }
}
