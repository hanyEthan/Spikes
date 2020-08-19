using System;

namespace ADS.Common.Models.Domain
{
    public class AuditTrailLogCriteria
    {
        public Guid? LogId { get; set; }
        public Guid? UserId { get; set; }
        public string Username { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string IPAddress { get; set; }
        public string MachineName { get; set; }
        public Guid? ModuleId { get; set; }
        public Guid? ActionId { get; set; }
        public int ModuleCode { get; set; }
        public int ActionCode { get; set; }
        public string Details { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }

        # region cst.

        public AuditTrailLogCriteria( Guid? logId , Guid? userId , string username , DateTime? dateFrom , DateTime? dateTo , string ipAddress , string machineName , Guid? moduleId , Guid? actionId , string details , int pageIndex , int pageSize )
        {
            LogId = logId;
            UserId = userId;
            Username = username;
            DateFrom = dateFrom;
            DateTo = dateTo;
            IPAddress = ipAddress;
            MachineName = machineName;
            ModuleId = moduleId;
            ActionId = actionId;
            Details = details;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public AuditTrailLogCriteria()
        {
        }

        # endregion
    }
}
