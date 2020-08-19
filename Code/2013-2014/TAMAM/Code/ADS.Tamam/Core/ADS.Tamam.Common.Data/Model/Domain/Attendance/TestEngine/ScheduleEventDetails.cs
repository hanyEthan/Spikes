using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.Domain.Attendance.TestEngine
{
    public class ScheduleEventDetails
    {
        public string ExpectedIn { get; set; }
        public string ExpectedOut { get; set; }
        public string ShiftName { get; set; }
        public string ActualIn { get; set; }
        public string InOffset { get; set; }
        public string InOffsetCalculated { get; set; }
        public string InStatus { get; set; }
        public string ActualOut { get; set; }
        public string OutOffset { get; set; }
        public string OutOffsetCalculated { get; set; }
        public string OutStatus { get; set; }
        public string TotalStatus { get; set; }
        public string PayCode { get; set; }
        public string Hours { get; set; }
        public string CalculatedHours { get; set; }
        public string Overtime { get; set; }
        public string CalculatedOvertime { get; set; }

        public override string ToString()
        {
            return string.Format ( "<td>{0}</td> <td>{1}</td> <td>{2}</td> <td>{3}</td> <td>{4}</td> <td>{5}</td> <td>{6}</td> <td>{7}</td>"+
                                   "<td>{8}</td> <td>{9}</td> <td>{10}</td> <td>{11}</td> <td>{12}</td> <td>{13}</td> <td>{14}</td> <td>{15}</td> <td>{16}</td>" ,
                                   ExpectedIn , ExpectedOut , ShiftName , ActualIn , InOffset , InOffsetCalculated , InStatus , ActualOut , OutOffset
                                   , OutOffsetCalculated , OutStatus , TotalStatus , PayCode , Hours , CalculatedHours , Overtime , CalculatedOvertime
                                   );
        }
    }
}
