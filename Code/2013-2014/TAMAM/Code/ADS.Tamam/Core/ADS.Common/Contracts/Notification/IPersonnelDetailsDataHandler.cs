using System.Text;
using ADS.Common.Context;

namespace ADS.Common.Contracts.Notification
{
    public interface IPersonnelDetailsDataHandler : IBaseHandler
    {
        ExecutionResponse<StringBuilder> GetEmailAddress( string id );
        ExecutionResponse<StringBuilder> GetName( string id );
        ExecutionResponse<StringBuilder> GetArabicName( string id );
        ExecutionResponse<StringBuilder> GetCellNumber( string id );
    }
}
