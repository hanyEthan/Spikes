using System;

namespace ADS.Common.Models.Domain
{
    public class AuditTrailLog
    {
        public virtual Guid Id { get; set; }        
        public virtual int Code { get; set; }
        public virtual string IpAddress { get; set; }
        public virtual string MachineName { get; set; }
        public virtual DateTime ActionDate { get; set; }
        public virtual string RefKey { get; set; }
        public virtual string Details { get; set; }
        
        public virtual Guid ModuleId { get; set; }
        public virtual string ModuleName { get; set; }
        public virtual AuditTrailModule Module { get; set; }

        public virtual Guid ActionId { get; set; }
        public virtual string ActionName { get; set; }
        public virtual AuditTrailAction Action { get; set; }

        public virtual Guid UserId { get; set; }
        public virtual string UserCode { get; set; }
        public virtual string Username { get; set; }

        # region cst.

        public AuditTrailLog()
        {
        }
        public AuditTrailLog( Guid id , Guid userId , string username , Guid moduleId , string moduleName , Guid actionId , string actionName , string ipAddress , string machineName , string refKey , string details )
        {
            Id = id;
            UserId = userId;
            Username = username;
            ActionDate = DateTime.Now;
            ModuleId = moduleId;
            ModuleName = moduleName;
            ActionId = actionId;
            ActionName = actionName;
            IpAddress = ipAddress;
            MachineName = machineName;
            RefKey = refKey;
            Details = details;
        }
        public AuditTrailLog( string userCode , string username , int moduleId , int actionId , string ipAddress , string machineName , string refKey , string details )
        {
            Action = new AuditTrailAction() { Code = actionId };
            Module = new AuditTrailModule() { Code = moduleId };
            ActionDate = DateTime.Now;
            Details = details;
            IpAddress = ipAddress;
            MachineName = machineName;
            RefKey = refKey;
            UserCode = userCode;
            Username = username;
        }

        # endregion
    }
}
