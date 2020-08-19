using System;
using System.Collections.Generic;
using System.Globalization;

using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using System.Threading;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class Leave : IWorkFlowTarget , IWorkflowTarget , IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        public Guid RequestedById { get; set; }
        public Person RequestedBy { get; set; }

        public int LeaveStatusId { get; set; }
        public int LeaveTypeId { get; set; }
        public string Notes { get; set; }
        public DateTime RequestTime { get; set; }

        public Guid? ReviewerId { get; set; }
        public string ReviewComment { get; set; }
        public bool IsReviewed { get; set; }

        public double EffectiveDaysCount { get; set; }

        public bool IsNative { get; set; }

        public DateTime? EffectiveYearStart { get; set; }

        public Person Reviewer { get; set; }
        public DetailCode LeaveStatus { get; set; }
        public DetailCode LeaveType { get; set; }
        public LeaveMode LeaveMode { get; set; }
        public LeaveModePeriod LeaveModePeriod { get; set; }

        public IList<LeaveAttachment> Attachments { get; set; }

        #region IWorkFlowTarget

        [XDontSerialize]
        public int SubTypeId
        {
            get { return LeaveTypeId; }
        }
        [XDontSerialize]
        public WorkFlowTargetType TargetType
        {
            get { return WorkFlowTargetType.Leave; }
        }
        public bool CheckApprovalCondition(int condition)
        {
            return EffectiveDaysCount >= condition;
        }
        
        #endregion
        #region IWorkflowTarget

        [XDontSerialize]
        public double EffectiveAmount
        {
            get { return EffectiveDaysCount; }
        }

        #endregion
        #region cst ...

        public Leave() {
            Attachments = new List<LeaveAttachment>();
        }
        public Leave(Guid id , Guid personId , string dateFrom , string DateTo , int status , int type , string comment) : this()
        {
            Id = id;
            PersonId = personId;

            StartDate = DateTime.ParseExact( dateFrom , "dd/MM/yyyy" , CultureInfo.InvariantCulture );
            EndDate = DateTime.ParseExact( DateTo , "dd/MM/yyyy" , CultureInfo.InvariantCulture );

            LeaveStatusId = status;
            LeaveTypeId = type;
            ReviewComment = comment;
            Attachments = new List<LeaveAttachment>();
        }
        
        #endregion

        # region ToString

        public override string ToString()
        {
            var isLocalized = Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains( "ar" );

            string meta_command = isLocalized ? "اجازة" : "Leave";
            string meta_person = Person != null ? isLocalized ? Person.FullNameCultureVarient : Person.FullName : PersonId.ToString();
            string meta_dateStart = StartDate.ToString( "dd/MM/yyyy" );
            string meta_dateEnd = EndDate.ToString( "dd/MM/yyyy" );
            string meta_days = EffectiveDaysCount.ToString();
            string meta_format = isLocalized ? "{0} للموظف ({1}) من ({2}) الى ({3}), بصافي أيام ({4}) بالنسبة لمواعيد عمله"
                                             : "{0} for ({1}), from ({2}) to ({3}), with effective number of days of ({4}).";

            return string.Format( meta_format , meta_command , meta_person , meta_dateStart , meta_dateEnd , meta_days );
        }

        # endregion
    }
}