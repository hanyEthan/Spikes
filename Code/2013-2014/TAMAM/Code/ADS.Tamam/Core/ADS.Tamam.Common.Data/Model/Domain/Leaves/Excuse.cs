using System;
using System.Collections.Generic;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using System.Threading;
using ADS.Common.Utilities;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class Excuse : IWorkFlowTarget, IWorkflowTarget, IXSerializable, XIntervals.ITimeRange
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime ExcuseDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public Guid PersonId { get; set; }
        public int ExcuseStatusId { get; set; }
        public int ExcuseTypeId { get; set; }
        public string Notes { get; set; }
        public DateTime RequestTime { get; set; }

        public Guid? ReviewerId { get; set; }
        public string ReviewComment { get; set; }
        public bool IsReviewed { get; set; }

        public bool IsNative { get; set; }

        public Person Person { get; set; }
        public Person Reviewer { get; set; }
        public DetailCode ExcuseStatus { get; set; }
        public DetailCode ExcuseType { get; set; }

        public IList<ExcuseAttachment> Attachments { get; set; }

        public Excuse()
        {
            Attachments = new List<ExcuseAttachment>();
        }

        #region IWorkFlowTarget

        [XDontSerialize]
        public int SubTypeId
        {
            get
            {
                // throw new NotImplementedException ();
                return -1;
            }
        }
        [XDontSerialize]
        public WorkFlowTargetType TargetType
        {
            get
            {
                return IsAwayExcuse() ? WorkFlowTargetType.Away : WorkFlowTargetType.Excuse;
            }
        }
        [XDontSerialize]
        public double EffectiveDaysCount
        {
            //get { return (int) Math.Floor(Duration); }
            get { return Duration; }
        }
        public bool CheckApprovalCondition( int condition )
        {
            return EffectiveDaysCount >= condition;
        }

        #endregion
        #region IWorkflowTarget

        [XDontSerialize]
        public double EffectiveAmount
        {
            //get { return ( int ) Math.Floor( Duration ); }
            get { return Duration; }
        }

        #endregion

        # region internals

        public bool IsAwayExcuse()
        {
            return this.ExcuseTypeId == ( int )ExcuseTypes.AwayExcuse;
        }

        # endregion
        # region ToString

        public override string ToString()
        {
            var isLocalized = Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains( "ar" );

            string meta_command = IsAwayExcuse() ? ( isLocalized ? "مهام العمل" : "Away" ) : ( isLocalized ? "إذن" : "Excuse" );
            string meta_person = Person != null ? isLocalized ? Person.FullNameCultureVarient : Person.FullName : PersonId.ToString();
            string meta_date = ExcuseDate.ToString( "dd/MM/yyyy" );
            string meta_timeStart = StartTime.ToString( "hh:mm tt" );
            string meta_timeEnd = EndTime.ToString( "hh:mm tt" );
            string meta_format = isLocalized ? "{0} للموظف ({1}) في تاريخ ({2}) من ({3}) الى ({4}) بالنسبة لأوقات عمله"
                                             : "{0} for ({1}), in ({2}) from ({3}) to ({4})";

            return string.Format( meta_format, meta_command, meta_person, meta_date, meta_timeStart, meta_timeEnd );
        }

        # endregion
        #region ITimeRange

        [XDontSerialize]
        DateTime? XIntervals.ITimeRange.StartTime
        {
            get { return this.StartTime; }
            set { this.StartTime = value ?? new DateTime(); }
        }
        [XDontSerialize]
        DateTime? XIntervals.ITimeRange.EndTime
        {
            get { return this.EndTime; }
            set { this.EndTime = value ?? new DateTime(); }
        }
        [XDontSerialize]
        public bool SpansMultipleDays
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
