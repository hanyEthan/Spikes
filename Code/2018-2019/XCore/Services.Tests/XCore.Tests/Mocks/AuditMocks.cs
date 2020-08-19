using XCore.Services.Audit.Core.Models;

namespace XCore.Tests.Mocks
{
    public static class AuditMocks
    {
        #region Config

        public static AuditTrail Audit_01 = new AuditTrail()
        {
            Action = "AC.00",
            App = "AP.00",
            //Code = "",
            ConnectionMethod = "CNN.00",
            DestinationAddress = "DAD.00",
            DestinationIP = "DIP.00",
            DestinationPort = "DPO.00",
            Entity = "EN.00",
            MetaData = "MT.00",
            Module = "MD.00",
            SourceClient = "SC.00",
            SourceIP = "SIP.00",
            SourceOS = "SOS.00",
            Text = "TXT.00",
            SourcePort = "SPO.00",
            UserId = "SID.00",
            UserName = "USN.00",
        };

        #endregion
    }
}
