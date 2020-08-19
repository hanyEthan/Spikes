using System;
using System.Runtime.Serialization;

using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleTemplateDayShifts : ISerializable , IXSerializable
    {
        public Guid Id { get; set; }
        public Guid ShiftId { get; set; }
        public Guid ScheduleTemplateDayId { get; set; }

        public Shift Shift { get; set; }
        [XDontSerialize]
        public ScheduleTemplateDays TemplateDay { get; set; }

        #region cst ...

        public ScheduleTemplateDayShifts() { }
        public ScheduleTemplateDayShifts( SerializationInfo info , StreamingContext ctxt )
        {
            this.Shift = ( Shift ) info.GetValue( "Shift" , typeof( Shift ) );
            this.TemplateDay = ( ScheduleTemplateDays ) info.GetValue( "TemplateDay" , typeof( ScheduleTemplateDays ) );

            this.Id = ( Guid ) info.GetValue( "Id" , typeof( Guid ) );
            this.ShiftId = ( Guid ) info.GetValue( "ShiftId" , typeof( Guid ) );
            this.ScheduleTemplateDayId = ( Guid ) info.GetValue( "ScheduleTemplateDayId" , typeof( Guid ) );
        }

        #endregion
        #region ISerializable

        public void GetObjectData( SerializationInfo info , StreamingContext context )
        {
            info.AddValue( "Id" , this.Id );
            info.AddValue( "ShiftId" , this.ShiftId );
            info.AddValue( "ScheduleTemplateDayId" , this.ScheduleTemplateDayId );
            info.AddValue( "Shift" , this.Shift );
            info.AddValue( "TemplateDay" , this.TemplateDay );
        }

        #endregion
    }
}
