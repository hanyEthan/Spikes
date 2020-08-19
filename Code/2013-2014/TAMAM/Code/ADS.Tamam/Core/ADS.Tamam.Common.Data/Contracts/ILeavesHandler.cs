using System;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.DTO.Composite;
using ADS.Tamam.Common.Data.Model.DTO.Services;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface ILeavesHandler : IBaseHandler
    {
        ExecutionResponse<bool> CreateLeaves(List<Leave> leave, RequestContext reqContext);
        ExecutionResponse<bool> RequestLeave(Leave leave, RequestContext requestContext);
        ExecutionResponse<bool> EditLeave(Leave leave, RequestContext reqContext);
        ExecutionResponse<Leave> GetLeave(Guid leaveId, RequestContext reqContext);
        ExecutionResponse<LeaveSearchResult> SearchLeaves ( LeaveSearchCriteria criteria , bool activePersonsOnly , RequestContext reqContext );
        ExecutionResponse<List<LeaveDetails>> SearchDetailsLeaves(LeaveSearchCriteria criteria, bool activePersonsOnly, RequestContext requestContext);

        ExecutionResponse<bool> CancelLeave(Guid leaveId, RequestContext reqContext);
        ExecutionResponse<bool> ReviewLeave(LeaveReview review, RequestContext reqContext);
        ExecutionResponse<LeaveAttachment> GetLeaveAttachment(Guid attachmentId, RequestContext reqContext);

        ExecutionResponse<List<DetailedLeaveCreditDTO>> GetDetailedLeaveCredit( List<Guid> departments, List<Guid> personnel, List<int> leaveTypes, RequestContext requestContext );
        ExecutionResponse<DetailedLeaveCreditDTO> GetDetailedLeaveCredit( Guid personId, RequestContext requestContext );

        ExecutionResponse<LeaveCredit> GetLeaveCredit(Guid personId, RequestContext requestContext);
        ExecutionResponse<LeaveCredit> GetLeaveCreditForLeaveDates(Guid personId, DateTime leaveStart, DateTime leaveEnd,RequestContext requestContext);
        ExecutionResponse<LeaveCredit> GetCurrentLeaveCredit(Guid personId,RequestContext requestContext);
        ExecutionResponse<LeaveCredit> GetNextLeaveCredit(Guid personId, RequestContext requestContext);
        //ExecutionResponse<LeavePreCredit> GetPersonLeavePreCredit(Guid personId);

        ExecutionResponse<bool> UpdateLeaveCreditsMetaData(Guid personId, DateTime originalEffectiveDate,
            DateTime updatedEffectiveDate, RequestContext requestContext);
        ExecutionResponse<double> CheckLeaveTypeCredit( Guid personId , LeaveTypes leaveType , Leave leave , RequestContext reqContext );
        ExecutionResponse<bool> UpdateLeaveCredit ( Leave updatedLeave , LeaveStatus originalLeaveStatus , RequestContext requestContext );
        ExecutionResponse<bool> RecalculateLeaveCredit(Guid personId, RequestContext reqContext);
        ExecutionResponse<bool> TransferCredits(RequestContext requestContext);
        //ExecutionResponse<bool> ResetCurrentLeaveCredit(Guid personId, RequestContext requestContext);
        //ExecutionResponse<bool> BuildPersonPreCredits(Guid personId, int amount, LeaveTypes leaveType, RequestContext requestContext);
        ExecutionResponse<bool> ResetPreCredits(RequestContext requestContext);

        ExecutionResponse<bool> CreateExcuses(List<Excuse> excuse, RequestContext reqContext);
        ExecutionResponse<bool> RequestExcuse(Excuse excuse, RequestContext requestContext);
        ExecutionResponse<bool> EditExcuse(Excuse excuse, RequestContext reqContext);
        ExecutionResponse<Excuse> GetExcuse(Guid excuseId, RequestContext reqContext);
        ExecutionResponse<ExcuseSearchResult> SearchExcuses(ExcuseSearchCriteria criteria, RequestContext reqContext);
        ExecutionResponse<bool> CancelExcuse(Guid excuseId, RequestContext reqContext);
        ExecutionResponse<bool> ReviewExcuse(ExcuseReview review, RequestContext reqContext);
        ExecutionResponse<ExcuseAttachment> GetExcuseAttachment(Guid attachmentId, RequestContext reqContext);

        ExecutionResponse<bool> RecalculateExcuseDuration(Guid personId, RequestContext requestContext);
        ExecutionResponse<bool> CheckExcusesCredit(Guid personId, ExcuseTypes excuseType , DateTime date, double duration, RequestContext requestContext);
        ExecutionResponse<bool> ChangeLeaveStatus( Guid leaveId , LeaveStatus status );
        ExecutionResponse<ExcuseCredit> GetExcusesCredit(Guid personId,int year, int monthNo, RequestContext requestContext);
        
        ExecutionResponse<List<WorkflowCheckPoint>> ApprovalStepsGet( Guid targetId , RequestContext requestContext );
        ExecutionResponse<bool> ApprovalIntegrityMaintainByOwner( Guid personId , RequestContext requestContext );
        ExecutionResponse<bool> ApprovalIntegrityMaintainByReviewer( Guid reviewerId , RequestContext requestContext );
        ExecutionResponse<bool> ApprovalInstancesCancel( Guid personId , RequestContext requestContext );

        ExecutionResponse<bool> CanPersonCancelLeave( Guid targetId , RequestContext requestContext );
        ExecutionResponse<bool> CanPersonCancelExcuse( Guid targetId , RequestContext requestContext );
        ExecutionResponse<bool> CanPersonReviewTarget( Guid targetId , RequestContext requestContext );


        // Pre-Credits..
        ExecutionResponse<LeavePreCredit> GetPersonPreCredits( Guid personId, RequestContext requestContext );
        ExecutionResponse<bool> BuildPreCredits( List<LeavePreCredit> preCredits , RequestContext requestContext );
    }
}