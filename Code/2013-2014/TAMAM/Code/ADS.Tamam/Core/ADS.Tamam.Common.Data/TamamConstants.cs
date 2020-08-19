using System;
namespace ADS.Tamam.Common.Data
{
    public static class TamamConstants
    {
        public const string DataHandlersPartialName = "Handlers.Data.";
        public const string PersonnelDataHandlerName = "Handlers.Data.Personnel";
        public const string UsersDataHandlerName = "Handlers.Data.Users";
        public const string AttendanceDataHandlerName = "Handlers.Data.Attendance";
        public const string SchedulesDataHandlerName = "Handlers.Data.Schedules";
        public const string ReportsDataHandlerName = "Handlers.Data.Reporting";
        public const string LeavesDataHandlerName = "Handlers.Data.Leaves";
        public const string OrganizationDataHandlerName = "Handlers.Data.Organization";
        public const string PersonnelHandlerName = "Handlers.Personnel";
        public const string UsersHandlerName = "Handlers.Users";
        public const string AttendanceHandlerName = "Handlers.Attendance";
        public const string SchedulesHandlerName = "Handlers.Schedules";
        public const string LeavesHandlerName = "Handlers.Leaves";
        public const string OrganizationHandlerName = "Handlers.Organization";
        public const string AnalysisHandlerName = "Handlers.Analysis";
        public const string IntegrationHandlerName = "Handlers.Integration";
        public const string ReportingHandlerName = "Handlers.Reporting";

        public static class MasterCodes
        {
            public const string JobTitle = "JobTitle";
            public const string Gender = "Gender";
            public const string EmploymentType = "EmploymentType";
            public const string Religion = "Religion";
            public const string Nationality = "Nationality";
            public const string MaritalStatus = "MaritalStatus";
            public const string LeaveStatus = "LeaveStatus";
            public const string LeaveType = "LeaveType";
            public const string ExcuseStatus = "ExcuseStat";
            public const string ExcuseType = "ExcuseType";
            public const string LeaveModePeriod = "LeaveModePeriod";

            public const string PolicyTypes = "PolicyTypes";
            public const string PolicyFieldsDataTypes = "PolicyFieldsDataTypes";
        }
        public static class AuthorizationConstants
        {
            #region Person
            #region Common
            public const string PersonAuditModuleId = "5";
            #endregion
            #region Create

            public const string CreatePersonActionKey = "CreatePersonAction";

            public const string CreatePersonAuditActionId = "11";
            public const string CreatePersonAuditModuleId = "5";
            public const string CreatePersonAuditMessageSuccessful = "Person Created Successfully (Name: {0})";
            public const string CreatePersonAuditMessageFailure = "Person Creation Failed (Name: {0})";
            #endregion
            #region Edit

            public const string EditPersonActionKey = "EditPersonAction";

            public const string EditPersonAuditActionId = "12";

            public const string EditPersonAuditMessageSuccessful = "Person Edited Successfully (Name: {0})";
            public const string EditPersonAuditMessageFailure = "Person Edit Failed (Name: {0})";
            #endregion
            # region Edit Password

            public const string EditPersonPasswordActionKey = "EditPersonPasswordAction";
            public const string EditPersonPasswordActionAuditActionId = "79";
            public const string EditPersonPasswordAuditMessageSuccessful = "Edit Person Password Successfully (Person Id: {0})";
            public const string EditPersonPasswordAuditMessageFailure = "Edit Person Password Failed (Person Id: {0})";

            # endregion
            #region Edit Security (Roles / Privileges)

            public const string EditPersonSecurityActionKey = "EditPersonSecurityAction";
            public const string EditPersonSecurityActionAuditActionId = "77";
            public const string EditPersonSecurityAuditMessageSuccessful = "Edit Person Security (Roles, Privileges) Successfully (Person Id: {0})";
            public const string EditPersonSecurityAuditMessageFailure = "Edit Person Security (Roles, Privileges) Failed (Person Id: {0})";
            
            #endregion

            #region Edit Leaves PreCredit

            public const string EditPersonPreCreditActionKey = "EditPersonPreCreditAction";

            public const string EditPersonPreCreditAuditActionId = "93";

            public const string EditPersonPreCreditAuditMessageSuccessful = "Person PreCredit Edited Successfully (Id: {0})";
            public const string EditPersonPreCreditAuditMessageFailure = "Person PreCredit Edit Failed (Id: {0})";
            #endregion

            #region ChangeActiveState

            public const string ChangePersonActiveStateActionKey = "ChangePersonActiveState";

            public const string ChangePersonActiveStateAuditActionId = "13";

            public const string ChangePersonActiveStateAuditMessageSuccessful = "Person Active State Changed Successfully (Person : {0})";
            public const string ChangePersonActiveStateAuditMessageFailure = "Person Active State Changed Failed (Person Id: {0})";
            #endregion
            #region GetPerson

            public const string GetPersonActionKey = "GetPerson";

            public const string GetPersonAuditActionId = "14";

            public const string GetPersonAuditMessageSuccessful = "Get Person Successfully (Person Id: {0})";
            public const string GetPersonAuditMessageFailure = "Get Person Failed (Person Id: {0})";
            #endregion
            #region SearchPersons

            public const string SearchPersonnelActionKey = "SearchPersonnel";

            public const string SearchPersonnelAuditActionId = "16";

            public const string SearchPersonnelAuditMessageSuccessful = "Search Personnel Preformed Successfully";
            public const string SearchPersonnelAuditMessageFailure = "Search Personnel Failed";
            public const string SearchPersonnelAuditMessageAccessDenied = "Search Personnel Access Denied";
            #endregion
            #region Create Delegate

            public const string CreateDelegateActionKey = "CreateDelegateAction";
                                      
            public const string CreateDelegateAuditActionId = "86";
            public const string CreateDelegateAuditModuleId = "5";
            public const string CreateDelegateAuditMessageSuccessful = "Delegate Created Successfully";
            public const string CreateDelegateAuditMessageFailure = "Delegate Creation Failed";
            #endregion
            #region Edit Delegate

            public const string EditDelegateActionKey = "EditDelegateAction";
                                
            public const string EditDelegateAuditActionId = "87";
            public const string EditDelegateAuditModuleId = "5";
            public const string EditDelegateAuditMessageSuccessful = "Delegate Edited Successfully";
            public const string EditDelegateAuditMessageFailure = "Delegate Edited Failed";
            #endregion
            #region Search Delegates
            public const string SearchDelegatesActionKey = "SearchDelegatesAction";
                                      
            public const string SearchDelegatesAuditActionId = "81";

            public const string SearchDelegatesAuditMessageSuccessful = "Search Delegates Preformed Successfully";
            public const string SearchDelegatesAuditMessageFailure = "Search Delegates Failed";
            public const string SearchDelegatesAuditMessageAccessDenied = "Search Delegates Access Denied";
            #endregion
            #endregion
            #region Organization

            #region Common
            public const string OrganizationAuditModuleId = "6";
            #endregion
            #region Create

            public const string CreateOrganizationActionKey = "CreateDepartmentAction";

            public const string CreateOrganizationAuditActionId = "15";
            public const string CreateOrganizationAuditMessageSuccessful = "Department Created Successfully (Name: {0})";
            public const string CreateOrganizationAuditMessageFailure = "Department Creation Failed (Name: {0})";
            #endregion
            #region Edit

            public const string EditOrganizationActionKey = "EditDepartmentAction";

            public const string EditOrganizationAuditActionId = "17";
            public const string EditOrganizationAuditMessageSuccessful = "Department Edited Successfully (Name: {0})";
            public const string EditOrganizationAuditMessageFailure = "Department Edit Failed (Name: {0})";
            #endregion
            #region Delete

            public const string DeleteOrganizationActionKey = "DeleteDepartmentAction";

            public const string DeleteOrganizationAuditActionId = "18";
            public const string DeleteOrganizationAuditMessageSuccessful = "Department Deleted Successfully (Name: {0})";
            public const string DeleteOrganizationAuditMessageFailure = "Department Delete Failed (Name: {0})";
            #endregion
            # region View All

            public const string ViewAllOrganizationsActionKey = "ViewAllDepartmentsAction";

            # endregion

            #region GetDepartments

            public const string GetOrganizationActionKey = "GetDepartmentAction";

            public const string GetOrganizationAuditActionId = "19";
            public const string GetOrganizationAuditMessageSuccessful = "Department Get Successfully";
            public const string GetOrganizationAuditMessageFailure = "Department Get Failed";
            #endregion
            # region GetAttendanceCodes

            public const string GetAttendanceCodesActionKey = "GetAttendanceCodesAction";

            public const string GetAttendanceCodesAuditActionId = "61";
            public const string GetAttendanceCodesAuditMessageSuccessful = "Attendance Codes Get Successfully";
            public const string GetAttendanceCodesAuditMessageFailure = "Attendance Codes Get Failed";

            #endregion
            # region EditAttendanceCode
            
            public const string EditAttendanceCodesActionKey = "EditAttendanceCodesAction";

            public const string EditAttendanceCodesAuditActionId = "62";
            public const string EditAttendanceCodesAuditMessageSuccessful = "Attendance Codes updated Successfully";
            public const string EditAttendanceCodesAuditMessageFailure = "Attendance Codes updated Failed";

            #endregion
            # region GetDashboardWebParts

            public const string GetDashboardWebPartsActionKey = "GetDashboardWebPartsAction";

            public const string GetDashboardWebPartsAuditActionId = "63";
            public const string GetDashboardWebPartsAuditMessageSuccessful = "Dashboard WebParts Get Successfully";
            public const string GetDashboardWebPartsAuditMessageFailure = "Dashboard WebParts Get Failed";

            #endregion
            # region Notification Privilege

            public const string GetNotificationActionKey = "Dashboard_NotificationsWebPart";

            # endregion
            # region GetReports

            public const string GetReportsActionKey = "GetReportsAction";

            public const string GetReportsAuditActionId = "64";
            public const string GetReportsAuditMessageSuccessful = "Reports Get Successfully";
            public const string GetReportsAuditMessageFailure = "Reports Get Failed";

            #endregion
           
            # region Holidays

            public const string HolidaysActionKey = "HolidaysAction";
            public const string HolidaysAuditActionId = "80";
            public const string HolidaysAuditMessageSuccessful = "Holidays Successfully";
            public const string HolidaysAuditMessageFailure = "Holidays Failed";

            #endregion

            #endregion
            # region Organization Detail

            #region Common

            public const string OrganizationDetailAuditModuleId = "8";

            #endregion
            # region Create

            public const string CreateOrganizationDetailActionKey = "CreateOrganizationDetailAction";
            public const string CreateOrganizationDetailAuditActionId = "24";
            public const string CreateOrganizationDetailAuditMessageSuccessful = "Organization Detail Created Successfully (Name: {0})";
            public const string CreateOrganizationDetailAuditMessageFailure = "Organization Detail Creation Failed (Name: {0})";

            # endregion
            #region Edit

            public const string EditOrganizationDetailActionKey = "EditOrganizationDetailAction";

            public const string EditOrganizationDetailAuditActionId = "25";
            public const string EditOrganizationDetailAuditMessageSuccessful = "Organization Detail Edited Successfully (Name: {0})";
            public const string EditOrganizationDetailAuditMessageFailure = "Organization Detail Edit Failed (Name: {0})";

            #endregion
            #region Delete

            public const string DeleteOrganizationDetailActionKey = "DeleteOrganizationDetailAction";

            public const string DeleteOrganizationDetailAuditActionId = "26";
            public const string DeleteOrganizationDetailAuditMessageSuccessful = "Organization Detail Deleted Successfully (Name: {0})";
            public const string DeleteOrganizationDetailAuditMessageFailure = "Organization Detail Delete Failed (Name: {0})";

            #endregion
            #region GetDepartments

            public const string GetOrganizationDetailActionKey = "GetOrganizationDetailAction";

            public const string GetOrganizationDetailAuditActionId = "27";
            public const string GetOrganizationDetailAuditMessageSuccessful = "Organization Detail Get Successfully";
            public const string GetOrganizationDetailAuditMessageFailure = "Organization Detail Get Failed";

            #endregion
            
            # endregion
            #region Policy

            public const string PoliciesAuditModuleId = "14";

            public const string PolicyGetActionKey = "GetPolicyAction";
            public const string PolicyGetAuditActionId = "60";
            public const string PolicyGetAuditMessageSuccessful = "Policy Retrieved Successfully ";
            public const string PolicyGetAuditMessageFailure = "Policy Retrieval Failed";

            public const string PolicyCreateActionKey = "CreatePolicyAction";
            public const string PolicyCreateAuditActionId = "59";
            public const string PolicyCreateAuditMessageSuccessful = "Policy Created Successfully ( {0} )";
            public const string PolicyCreateAuditMessageFailure = "Policy Creation Failed";

            public const string PolicyEditActionKey = "EditPolicyAction";
            public const string PolicyEditAuditActionId = "58";
            public const string PolicyEditAuditMessageSuccessful = "Policy Edited Successfully ( {0}:{1} )";
            public const string PolicyEditAuditMessageFailure = "Policy Edit Failed ( {0}:{1} )";

            public const string PolicyDeleteActionKey = "DeletePolicyAction";
            public const string PolicyDeleteAuditActionId = "57";
            public const string PolicyDeleteAuditMessageSuccessful = "Policy Deleted Successfully";
            public const string PolicyDeleteAuditMessageFailure = "Policy Delete Failed";

            #endregion
            #region Dynamic Fields

            public const string DynamicFieldsAuditModuleId = "16";

            public const string DynamicFieldsGetActionKey = "GetDynamicFieldsAction";
            public const string DynamicFieldsGetAuditActionId = "70";
            public const string DynamicFieldsGetAuditMessageSuccessful = "Dynamic Fields Retrieved Successfully ";
            public const string DynamicFieldsGetAuditMessageFailure = "Dynamic Fields Retrieval Failed";

            public const string DynamicFieldsCreateActionKey = "CreateDynamicFieldsAction";
            public const string DynamicFieldsCreateAuditActionId = "71";
            public const string DynamicFieldsCreateAuditMessageSuccessful = "Dynamic Fields Created Successfully ";
            public const string DynamicFieldsCreateAuditMessageFailure = "Dynamic Fields Creation Failed";

            public const string DynamicFieldsEditActionKey = "EditDynamicFieldsAction";
            public const string DynamicFieldsEditAuditActionId = "72";
            public const string DynamicFieldsEditAuditMessageSuccessful = "Dynamic Fields Edited Successfully ";
            public const string DynamicFieldsEditAuditMessageFailure = "Dynamic Fields Edit Failed";

            public const string DynamicFieldsDeleteActionKey = "DeleteDynamicFieldsAction";
            public const string DynamicFieldsDeleteAuditActionId = "73";
            public const string DynamicFieldsDeleteAuditMessageSuccessful = "Dynamic Fields Deleted Successfully ";
            public const string DynamicFieldsDeleteAuditMessageFailure = "Dynamic Fields Delete Failed";

            #endregion
            #region PolicyGroup
            #region Common
            public const string PolicyGroupAuditModuleId = "7";
            #endregion
            #region Create

            public const string CreatePolicyGroupActionKey = "CreatePolicyGroupAction";

            public const string CreatePolicyGroupAuditActionId = "20";
            public const string CreatePolicyGroupAuditMessageSuccessful = "Policy Group Created Successfully (Name: {0})";
            public const string CreatePolicyGroupAuditMessageFailure = "Policy Group  Creation Failed (Name: {0})";
            #endregion
            #region Edit

            public const string EditPolicyGroupActionKey = "EditPolicyGroupAction";

            public const string EditPolicyGroupAuditActionId = "21";
            public const string EditPolicyGroupAuditMessageSuccessful = "Policy Group Edited Successfully (Name: {0})";
            public const string EditPolicyGroupAuditMessageFailure = "Policy Group  Edit Failed (Name: {0})";
            #endregion
            #region Delete

            public const string DeletePolicyGroupActionKey = "DeletePolicyGroupAction";

            public const string DeletePolicyGroupAuditActionId = "22";
            public const string DeletePolicyGroupAuditMessageSuccessful = "Policy Group Deleted Successfully ";
            public const string DeletePolicyGroupAuditMessageFailure = "Policy Group  Delete Failed ";
            #endregion
            #region Get

            public const string GetPolicyGroupActionKey = "GetPolicyGroupAction";

            public const string GetPolicyGroupAuditActionId = "23";
            public const string GetPolicyGroupAuditMessageSuccessful = "Policy Group Get Successfully ";
            public const string GetPolicyGroupAuditMessageFailure = "Policy Group  Get Failed ";
            #endregion
            #region ChangeActiveState

            public const string ChangePolicyGroupActiveStateActionKey = "ChangePolicyGroupActiveStateAction";

            public const string ChangePolicyGroupActiveStateAuditActionId = "43";
            public const string ChangePolicyGroupActiveStateAuditMessageSuccessful = "Policy Group Change Active State Successfully (PolicyGroup : {0})";
            public const string ChangePolicyGroupActiveStateAuditMessageFailure = "Policy Group Change Active State Failed (PolicyGroup: {0})";

            #endregion
            #endregion
            #region Shift
            #region Common
            public const string ShiftAuditModuleId = "9";
            #endregion
            #region Create

            public const string CreateShiftActionKey = "CreateShiftAction";

            public const string CreateShiftAuditActionId = "28";
            public const string CreateShiftAuditMessageSuccessful = "Shift Created Successfully (Name: {0})";
            public const string CreateShiftAuditMessageFailure = "Shift  Creation Failed (Name: {0})";
            #endregion
            #region Edit

            public const string EditShiftActionKey = "EditShiftAction";
            public const string EditShiftAuditActionId = "29";
            public const string EditShiftAuditMessageSuccessful = "Shift Edited Successfully (Name: {0})";
            public const string EditShiftAuditMessageFailure = "Shift  Edit Failed (Name: {0})";

            public const string EditShiftStatusAuditMessageSuccessful = "Shift Status Changed Successfully ( ID: {0} / Status: {1} )";
            public const string EditShiftStatusAuditMessageFailure = "Shift Status Change Failed ( {0} )";

            #endregion
            #region Delete

            public const string DeleteShiftActionKey = "DeleteShiftAction";

            public const string DeleteShiftAuditActionId = "30";
            public const string DeleteShiftAuditMessageSuccessful = "Shift Deleted Successfully ({0})";
            public const string DeleteShiftAuditMessageFailure = "Shift  Delete Failed ({0})";
            #endregion
            #region Get

            public const string GetShiftActionKey = "GetShiftAction";

            public const string GetShiftAuditActionId = "31";
            public const string GetShiftAuditMessageSuccessful = "Shift Get Successfully ";
            public const string GetShiftAuditMessageFailure = "Shift  Get Failed ";
            #endregion
            #endregion
            #region Leave
            #region Common
            public const string LeaveAuditModuleId = "10";
            #endregion
            #region Create

            public const string CreateLeaveActionKey = "CreateLeaveAction";

            public const string CreateLeaveAuditActionId = "32";
            public const string CreateLeaveAuditMessageSuccessful = "Leave Created Successfully (ID: {0})";
            public const string CreateLeaveAuditMessageFailure = "Leave Creation Failed (ID: {0})";
            #endregion
            #region Request

            public const string RequestLeaveActionKey = "RequestLeaveAction";

            public const string RequestLeaveAuditActionId = "65";
            public const string RequestLeaveAuditMessageSuccessful = "Leave requested Successfully (ID: {0})";
            public const string RequestLeaveAuditMessageFailure = "Leave requested Failed (ID: {0})";
            #endregion
            #region Edit

            public const string EditLeaveActionKey = "EditLeaveAction";

            public const string EditLeaveActionId = "33";
            public const string EditLeaveAuditMessageSuccessful = "Leave Edited Successfully (Id: {0})";
            public const string EditLeaveAuditMessageFailure = "Leave Edit Failed (Id: {0})";
            #endregion
            #region Get

            public const string GetLeaveActionKey = "GetLeaveAction";

            public const string GetLeaveAuditActionId = "34";
            public const string GetLeaveAuditMessageSuccessful = "Leave Get Successfully (Id: {0})";
            public const string GetLeaveAuditMessageFailure = "Leave Get Failed (Id: {0})";
            #endregion
            #region Delete
            // temp not used [REDA]
            public const string DeleteLeaveActionKey = "DeleteLeaveAction";

            public const string DeleteLeaveAuditActionId = "35";
            public const string DeleteLeaveAuditMessageSuccessful = "Leave Deleted Successfully";
            public const string DeleteLeaveAuditMessageFailure = "Leave Delete Failed";

            #endregion
            # region SearchLeaves

            public const string SearchLeavesActionKey = "SearchLeavesAction";

            public const string SearchLeavesAuditActionId = "39";

            public const string SearchLeavesAuditMessageSuccessful = "Search for Leaves Preformed Successfully";
            public const string SearchLeavesAuditMessageFailure = "Search for Leaves Failed";
            public const string SearchLeavesAuditMessageAccessDenied = "Search for Leaves Access Denied";
            
            # endregion
            #region Cancel

            public const string CancelLeaveActionKey = "CancelLeaveAction";

            public const string CancelLeaveAuditActionId = "40";

            public const string CancelLeaveAuditMessageSuccessful = "Leave cancelled Successfully (Leave Id : {0})";
            public const string CancelLeaveAuditMessageFailure = "Leave cancellation Failed (Leave Id: {0})";
            #endregion
            # region Review
            public const string ReviewLeaveActionKey = "ReviewLeaveAction";

            public const string ReviewLeaveAuditActionId = "41";

            public const string ReviewLeaveAuditMessageSuccessful = "Leave reviewed Successfully (Leave Id : {0})";
            public const string ReviewLeaveAuditMessageFailure = "Leave review Failed (Leave Id: {0})";
            # endregion
            # region Get Leave Attachment
            public const string GetLeaveAttachmentActionKey = "GetLeaveAttachmentAction";

            public const string GetLeaveAttachmentAuditActionId = "42";

            public const string GetLeaveAttachmentAuditMessageSuccessful = "Leave Attachment Get Successfully (Attachment Id : {0})";
            public const string GetLeaveAttachmentAuditMessageFailure = "Leave Attachment Get Failed (Attachment Id: {0})";
            #endregion
            #endregion
            # region Leave Credits
            
            # region Check Leave Credit

            public const string CheckLeaveCreditActionKey = "CheckLeaveCreditAction";

            public const string CheckLeaveCreditAuditActionId = "53";
            public const string CheckLeaveCreditAuditMessageSuccessful = "Leave Credit Checked Successfully (Person ID: {0})";
            public const string CheckLeaveCreditAuditMessageFailure = "Leave Credit Check Failed (Person ID: {0})";

            # endregion
            # region Get Working Days in Period

            public const string GetLeaveWorkingDaysActionKey = "GetLeaveWorkingDaysAction";

            public const string GetLeaveWorkingDaysAuditActionId = "54";
            public const string GetLeaveWorkingDaysAuditMessageSuccessful = "Get Leave Working Days Successfully";
            public const string GetLeaveWorkingDaysAuditMessageFailure = "Get Leave Working Days Failed";

            # endregion
            #region Update Leave Credit

            public const string UpdateLeaveCreditActionKey = "UpdateLeaveCreditAction";

            public const string UpdateLeaveCreditAuditActionId = "52";
            public const string UpdateLeaveCreditAuditMessageSuccessful = "Leave Credit updated Successfully  (Leave Id : {0})";
            public const string UpdateLeaveCreditAuditMessageFailure = "Leave Credit Update Failed (Leave Id : {0})";
            #endregion
            
            #region Recalculate Leave Credit

            public const string RecalculateLeaveCreditActionKey = "RecalculateLeaveCreditAction";

            public const string RecalculateLeaveCreditAuditActionId = "55";
            public const string RecalculateLeaveCreditAuditMessageSuccessful = "Leave Credit Recalculated Successfully (Person ID: {0})";
            public const string RecalculateLeaveCreditAuditMessageFailure = "Leave Credit Recalculation Failed (Person ID: {0})";
            #endregion
            # region Recalculate Excuses Duration

            public const string RecalculateExcuseDurationAuditActionId = "76";
            public const string RecalculateExcuseDurationAuditMessageSuccessful = "Excuses Duration Recalculated Successfully (Person ID: {0})";
            public const string RecalculateExcuseDurationAuditMessageFailure = "Excuses Duration Recalculation Failed (Person ID: {0})";

            # endregion

            #region Transfer Leave Credit

            public const string TransferLeaveCreditActionKey = "TransferLeaveCreditAction";

            public const string TransferLeaveCreditAuditActionId = "56";
            public const string TransferLeaveCreditAuditMessageSuccessful = "Leaves Credit Transferred Successfully";
            public const string TransferLeaveCreditAuditMessageFailure = "Leaves Credit Transfer Failed";
            #endregion

            #endregion
            # region Excuse
            #region Common
            public const string ExcuseAuditModuleId = "13";
            #endregion
            #region Create

            public const string CreateExcuseActionKey = "CreateExcuseAction";
            public const string CreateAwayActionKey = "CreateAwayAction";

            public const string CreateExcuseAuditActionId = "44";
            public const string CreateExcuseAuditMessageSuccessful = "Excuse Created Successfully (ID: {0})";
            public const string CreateExcuseAuditMessageFailure = "Excuse Creation Failed (ID: {0})";
            #endregion
            #region Request

            public const string RequestExcuseActionKey = "RequestExcuseAction";
            public const string RequestAwayActionKey = "RequestAwayAction";

            public const string RequestExcuseAuditActionId = "66";
            public const string RequestExcuseAuditMessageSuccessful = "Excuse requested Successfully (ID: {0})";
            public const string RequestExcuseAuditMessageFailure = "Excuse requested Failed (ID: {0})";
            #endregion
            #region Edit

            public const string EditExcuseActionKey = "EditExcuseAction";
            public const string EditAwayActionKey = "EditAwayAction";

            public const string EditExcuseActionId = "49";
            public const string EditExcuseAuditMessageSuccessful = "Excuse Edited Successfully (Id: {0})";
            public const string EditExcuseAuditMessageFailure = "Excuse Edit Failed (Id: {0})";
            #endregion
            #region Get

            public const string GetExcuseActionKey = "GetExcuseAction";
            public const string GetAwayActionKey = "GetAwayAction";

            public const string GetExcuseAuditActionId = "47";
            public const string GetExcuseAuditMessageSuccessful = "Excuse Get Successfully (Id: {0})";
            public const string GetExcuseAuditMessageFailure = "Excuse Get Failed (Id: {0})";
            #endregion
            # region SearchExcuses

            public const string SearchExcusesActionKey = "SearchExcusesAction";
            public const string SearchAwayActionKey = "SearchAwayAction";

            public const string SearchExcusesAuditActionId = "45";

            public const string SearchExcusesAuditMessageSuccessful = "Search for Excuses Preformed Successfully";
            public const string SearchExcusesAuditMessageFailure = "Search for Excuses Failed";
            public const string SearchExcusesAuditMessageAccessDenied = "Search for Excuses Access Denied";

            # endregion
            #region Cancel

            public const string CancelExcuseActionKey = "CancelExcuseAction";
            public const string CancelAwayActionKey = "CancelAwayAction";

            public const string CancelExcuseAuditActionId = "46";

            public const string CancelExcuseAuditMessageSuccessful = "Excuse cancelled Successfully (Excuse Id : {0})";
            public const string CancelExcuseAuditMessageFailure = "Excuse cancellation Failed (Excuse Id: {0})";
            #endregion
            # region Review
            public const string ReviewExcuseActionKey = "ReviewExcuseAction";
            public const string ReviewAwayActionKey = "ReviewAwayAction";

            public const string ReviewExcuseAuditActionId = "48";

            public const string ReviewExcuseAuditMessageSuccessful = "Excuse reviewed Successfully (Excuse Id : {0})";
            public const string ReviewExcuseAuditMessageFailure = "Excuse review Failed (Excuse Id: {0})";
            # endregion
            # region Get Excuse Attachment
          
            public const string GetExcuseAttachmentAuditActionId = "50";

            public const string GetExcuseAttachmentAuditMessageSuccessful = "Excuse Attachment Get Successfully (Attachment Id : {0})";
            public const string GetExcuseAttachmentAuditMessageFailure = "Excuse Attachment Get Failed (Attachment Id: {0})";
            #endregion
            # endregion
            #region ScheduleTemplates
            #region Common
            public const string ScheduleTemplatesAuditModuleId = "11";
            #endregion
            #region Create

            public const string CreateScheduleTemplatesActionKey = "CreateScheduleTemplatesAction";

            public const string CreateScheduleTemplatesAuditActionId = "37";
            public const string CreateScheduleTemplatesAuditMessageSuccessful = "ScheduleTemplates Created Successfully (Name: {0})";
            public const string CreateScheduleTemplatesAuditMessageFailure = "ScheduleTemplates  Creation Failed (Name: {0})";
            #endregion
            #region Edit

            public const string EditScheduleTemplatesActionKey = "EditScheduleTemplatesAction";

            public const string EditScheduleTemplatesAuditActionId = "38";
            public const string EditScheduleTemplatesAuditMessageSuccessful = "ScheduleTemplates Edited Successfully (Name: {0})";
            public const string EditScheduleTemplatesAuditMessageFailure = "ScheduleTemplates  Edit Failed (Name: {0})";
            #endregion
            #region Delete

            public const string DeleteScheduleTemplatesActionKey = "DeleteScheduleTemplatesAction";

            public const string DeleteScheduleTemplatesAuditActionId = "30";
            public const string DeleteScheduleTemplatesAuditMessageSuccessful = "ScheduleTemplates Deleted Successfully ";
            public const string DeleteScheduleTemplatesAuditMessageFailure = "ScheduleTemplates  Delete Failed ";
            #endregion
            #region Get

            public const string GetScheduleTemplatesActionKey = "GetScheduleTemplatesAction";

            public const string GetScheduleTemplatesAuditActionId = "36";
            public const string GetScheduleTemplatesAuditMessageSuccessful = "ScheduleTemplates Get Successfully ";
            public const string GetScheduleTemplatesAuditMessageFailure = "ScheduleTemplates  Get Failed ";
            #endregion
            #endregion
            #region Schedules
            #region Common
            public const string ScheduleAuditModuleId = "12";
            #endregion
            #region Create

            public const string CreateScheduleActionKey = "CreateScheduleAction";

            public const string CreateScheduleAuditActionId = "51";
            public const string CreateScheduleAuditMessageSuccessful = "Schedule Created Successfully ( {0} )";
            public const string CreateScheduleAuditMessageFailure = "Schedule  Creation Failed ( {0} )";
            #endregion
            # region Transfer

            public const string TransferScheduleActionKey = "TransferPersonScheduleAction";
            public const string TransferScheduleAuditActionId = "78";
            public const string TransferScheduleAuditMessageSuccessful = "Transfer Person Schedule performed Successfully ( {0} )";
            public const string TransferScheduleAuditMessageFailure = "Transfer Person Schedule Failed ( {0} )";

            # endregion
            #region Edit

            public const string EditScheduleActionKey = "EditScheduleAction";

            public const string EditScheduleAuditActionId = "38";
            public const string EditScheduleAuditMessageSuccessful = "Schedule Edited Successfully ( {0} )";
            public const string EditScheduleAuditMessageFailure = "Schedule Edit Failed ( {0} )";

            public const string EditScheduleStatusAuditMessageSuccessful = "Schedule Status Changed (Id: {0} / State: {1})";
            public const string EditScheduleStatusAuditMessageFailure = "Schedule Status Edit Failed (Id: {0})";
            
            #endregion
            #region Delete

            public const string DeleteScheduleActionKey = "DeleteScheduleAction";

            public const string DeleteScheduleAuditActionId = "30";
            public const string DeleteScheduleAuditMessageSuccessful = "Schedule Deleted Successfully ( {0} ) ";
            public const string DeleteScheduleAuditMessageFailure = "Schedule  Delete Failed ( {0} )";
            #endregion
            #region Get

            public const string GetScheduleActionKey = "GetScheduleAction";

            public const string GetScheduleAuditActionId = "36";
            public const string GetScheduleAuditMessageSuccessful = "Schedule Get Successfully ";
            public const string GetScheduleAuditMessageFailure = "Schedule  Get Failed ";
            #endregion
            #endregion
            # region Administration Area

            public const string ShowAdministration = "ShowAdministration";
            public const string ViewArchive = "ViewArchiveAction";
            public const string ViewAdminManual = "ViewAdminManualAction";

            # endregion
            #region Attendance

            public const string AttendanceAuditModuleId = "15";

            public const string AttendanceGetActionKey = "GetAttendanceAction";
            public const string AttendanceGetAuditActionId = "67";
            public const string AttendanceGetAuditMessageSuccessful = "Attendance Events Retrieved";
            public const string AttendanceGetAuditMessageFailure = "Attendance Retrieval Failed";

            public const string AttendanceStatsGetActionKey = "GetAttendanceStatsAction";
            public const string AttendanceStatsGetAuditActionId = "69";
            public const string AttendanceStatsGetAuditMessageSuccessful = "Attendance Stats Retrieved";
            public const string AttendanceStatsGetAuditMessageFailure = "Attendance Stats Retrieval Failed";

            public const string AttendancePunchActionKey = "PunchAttendanceAction";
            public const string AttendancePunchAuditActionId = "68";
            public const string AttendancePunchAuditMessageSuccessful = "Attendance Punched";
            public const string AttendancePunchAuditMessageFailure = "Attendance Punching Failed";

            public const string AttendanceCreateActionKey = "CreateAttendanceAction";
            public const string AttendanceCreateAuditActionId = "74";
            public const string AttendanceCreateAuditMessageSuccessful = "Attendance Manually Created";
            public const string AttendanceCreateAuditMessageFailure = "Attendance Manual Creation Failed";

            public const string AttendanceEditActionKey = "EditAttendanceAction";
            public const string AttendanceEditAuditActionId = "75";
            public const string AttendanceEditAuditMessageSuccessful = "Attendance Manually Edited ( Person:{0} / Date:{1} )";
            public const string AttendanceEditAuditMessageFailure = "Attendance Manual Editing Failed ( Person:{0} / Date:{1} )";


            public const string AttendanceReprocessActionKey = "ReprocessAttendanceAction";
            public const string AttendanceReprocessAuditActionId = "82";
            public const string AttendanceReprocessAuditMessageSuccessful = "Attendance reprocess  done successfully";
            public const string AttendanceReprocessAuditMessageFailure = "Attendance reprocess  failed";

            
            public const string ReviewAttendanceViolationActionKey = "ReviewAttendanceViolationAction";
            public const string ReviewAttendanceViolationAuditActionId = "83";
            public const string ReviewAttendanceViolationAuditMessageSuccessful = "Review Attendance Violation done successfully";
            public const string ReviewAttendanceViolationAuditMessageFailure = "Review Attendance Violation failed";

            public const string ReviewAttendanceManualEditActionKey = "ReviewAttendanceManualEditAction";
            public const string ReviewAttendanceManualEditAuditActionId = "92";
            public const string ReviewAttendanceManualEditAuditMessageSuccessful = "Review Attendance Manual Edit done successfully ( History Id : {0} )";
            public const string ReviewAttendanceManualEditAuditMessageFailure = "Review Attendance Manual Edit failed ( History Id : {0} )";


            #endregion
            
            # region ScheduledReports

            // public const string GetScheduledReportEventsActionKey = "GetScheduledReportEventsAction";
            public const string GetScheduledReportEventsAuditActionId = "88";
            public const string GetScheduledReportEventsAuditMessageSuccessful = "Get Scheduled Report Events Successfully";
            public const string GetScheduledReportEventsAuditMessageFailure = "Get Scheduled Report Events Failed";

            // public const string EditScheduledReportEventsActionKey = "EditScheduledReportEventsAction";
            public const string EditScheduledReportEventsAuditActionId = "89";
            public const string EditScheduledReportEventsAuditMessageSuccessful = "Edit Scheduled Report Events Successfully";
            public const string EditScheduledReportEventsAuditMessageFailure = "Edit Scheduled Report Events Failed";

            // public const string CreateScheduledReportEventsActionKey = "CreateScheduledReportEventsAction";
            public const string CreateScheduledReportEventsAuditActionId = "90";
            public const string CreateScheduledReportEventsAuditMessageSuccessful = "Create Scheduled Report Events Successfully";
            public const string CreateScheduledReportEventsAuditMessageFailure = "Create Scheduled Report Events Failed";
         
            // public const string DeleteScheduledReportEventsActionKey = "DeleteScheduledReportEventsAction";
            public const string DeleteScheduledReportEventsAuditActionId = "91";
            public const string DeleteScheduledReportEventsAuditMessageSuccessful = "Delete Scheduled Report Events Successfully";
            public const string DeleteScheduledReportEventsAuditMessageFailure = "Delete Scheduled Report Events Failed";
         
            #endregion
        }
        public enum ValidationMode
        {
            Create ,
            Edit ,
            EditIdentity,
            Activate ,
            Deactivate ,
            Delete
        }
    }
}
