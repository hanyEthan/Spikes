using ADS.Common.Contracts;
using System;

namespace ADS.Tamam.Common.Data.Model.DTO.Composite
{
    [Serializable]
    public class ScheduleEventCurrentlyInDTO : IXSerializable
    {
        public Guid SEId { get; set; }
        public Guid DepId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime ActualIn { get; set; }
        public DateTime EventDate { get; set; }
        public string PersonName { get; set; }
        public string PersonNameCultureVariant { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameCultureVariant { get; set; }
        public string ShiftName { get; set; }
        public string ShiftNameCultureVariant { get; set; }

        [XDontSerialize]
        public string EventDate_Formatted
        {
            get { return EventDate.ToString("dd/MM/yyyy"); }
        }
         [XDontSerialize]
        public string ActualIn_Formatted
        {
            get
            {
                return  ActualIn.ToString("hh:mm tt");
            }
        }      
    }         
}
