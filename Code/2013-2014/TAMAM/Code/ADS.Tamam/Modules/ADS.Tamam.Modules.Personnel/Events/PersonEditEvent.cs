using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Handlers;

namespace ADS.Tamam.Modules.Personnel.Events
{
    [Serializable]
    public class PersonEditEvent : EventCell, IXIncludable
    {
        #region props

        public Guid PersonId { get; set; }
        public Person PersonPreEdit { get; set; }

        private PersonnelDataHandler dataHandler;
        
        #endregion
        #region cst ...

        public PersonEditEvent() : base()
        {

        }
        public PersonEditEvent( Guid personId , Person personPreEdit ) : this()
        {
            this.PersonId = personId;
            this.PersonPreEdit = personPreEdit;
        }
        
        #endregion
        #region EventCell

        [XDontSerialize] public override string ContentType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }
        [XDontSerialize] public override string TargetId
        {
            get { return ""; }
        }
        [XDontSerialize] public override string TargetType
        {
            get { return typeof( Person ).ToString(); }
        }

        #endregion

        public override bool Process()
        {
            try
            {
                #region Prep

                if ( !ValidateData() ) return false;
                PrepareDataLayer();
                var person = GetPerson();
                if ( person == null ) return false;

                #endregion

                #region Effective Schedules : Recalculate

                var response_recalculateEffectiveSchedule = TamamServiceBroker.SchedulesHandler.ReCalculateEffectiveSchedulesForPerson( person.Id );
                if ( response_recalculateEffectiveSchedule.Type != ResponseState.Success ) return false;

                #endregion
                #region Leave Credits : Edit

                //if personal join date changed and the person work in a hire date accrual policy, so credit effective date must be updated to the new date ...
                if ( PersonPreEdit.AccountInfo.JoinDate.Value != person.AccountInfo.JoinDate.Value )
                {
                    var response_AccrualPolicies = TamamServiceBroker.OrganizationHandler.GetPolicies( person.Id , new PolicyFilters( new Guid( PolicyTypes.AccrualPolicyType ) , true ) , SystemRequestContext.Instance );
                    if ( response_AccrualPolicies.Type == ResponseState.Success && response_AccrualPolicies.Result.Count > 0 )
                    {
                        var accrualPolicy = new AccrualPolicy( response_AccrualPolicies.Result[0] );
                        var oldEffectivDate = accrualPolicy.GetAccrualPolicyStartDate( PersonPreEdit.AccountInfo.JoinDate.Value );
                        var newEffectiveDate = accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value );

                        //May be the person work in Yearly accrual policy, or in Hire Date-but the change done in the year- so the effective start date will not be change
                        if ( oldEffectivDate != newEffectiveDate )
                        {
                            var responseUpdate = TamamServiceBroker.LeavesHandler.UpdateLeaveCreditsMetaData( person.Id , oldEffectivDate , newEffectiveDate , SystemRequestContext.Instance );
                            if ( responseUpdate.Type != ResponseState.Success )
                            {
                                // TODO: Cannot Update Leaves Credit
                            }
                        }
                    }
                }

                #endregion
                #region Leave Credits : Recalculate

                // Update Leaves Credit
                var responseRecalculate = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( person.Id , SystemRequestContext.Instance );
                if ( responseRecalculate.Type != ResponseState.Success )
                {
                    // TODO: Cannot Recalculate Leaves Credit
                }

                #endregion
                #region Excuses : duration

                // Update Excuses Duration
                var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( person.Id , SystemRequestContext.Instance );
                if ( responseExcuses.Type != ResponseState.Success )
                {
                    // TODO: Cannot Recalculate Excuses
                }

                #endregion
                #region Leaves : Validate approval Workflows

                // Validate Leaves approval Workflows
                TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( person.Id , SystemRequestContext.Instance );

                var subordinates = dataHandler.GetPersonnelByDirectManager( person.Id ).Result;
                foreach ( var subPerson in subordinates )
                {
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( subPerson.Id , SystemRequestContext.Instance );
                }

                TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByReviewer( person.Id , SystemRequestContext.Instance );

                // Validate attendance approval Workflows
                SystemBroker.AttendanceHandler.ApprovalIntegrityMaintainByOwner( person.Id );

                #endregion
                #region Attendance : handle in Holidays

                //Handle Attendance in Holiday
                var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( person.Id , SystemRequestContext.Instance );
                if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                {
                    var personHolidays = personPoliciesResult.Result.Where( h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                    foreach ( var holiday in personHolidays )
                    {
                        TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( person.Id , holiday , SystemRequestContext.Instance );
                    }
                }

                #endregion

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #region Helpers

        private bool ValidateData()
        {
            return ( PersonId != Guid.Empty && PersonPreEdit != null );
        }
        private void PrepareDataLayer()
        {
            // TODO : should be done through the data broker ...
            dataHandler = new PersonnelDataHandler();
        }
        private Person GetPerson()
        {
            var reponse_GetPerson = dataHandler.GetPerson( PersonId );
            if ( reponse_GetPerson.Type != ResponseState.Success || reponse_GetPerson.Result == null ) return null;

            return reponse_GetPerson.Result;
        }

        #endregion
    }
}