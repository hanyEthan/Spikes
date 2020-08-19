using System;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Audit.Models.Contracts;
using XCore.Services.Audit.Models.model;
using XCore.Services.Audit.SDK.Models;
using XCore.Services.Audit.Test.Console.Support;

namespace XCore.Services.Audit.Test.Console.Tests
{
    public static class AuditTests
    {
        #region ...

        public static void start()
        {
            //Test01();
            Test02();
        }

        #endregion

        private static async void Test01()
        {
            try
            {
                //var xx = AuditClient.AuditHandler;
                var Requet = new ServiceExecutionRequestDTO<IAuditMessage>();
                Requet.Content = new AuditTrailMessage { Action = "01010" };

                var sdk = new AuditCustomClient();

                await sdk.SDK.CreateAsync(Requet);
            }
            catch (Exception x)
            {
                throw;
            }
        }
        private static async void Test02()
        {
            try
            {
                #region prep.

                var Audit = new AuditTrailDTO()
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
                
                var sender = new AuditCustomClient();

                #endregion

                for ( ; ; )
                {
                    System.Console.ReadLine();
                    sender.SDK.CreateAsync(new ServiceExecutionRequestDTO<IAuditMessage>() { Content = Audit }).GetAwaiter().GetResult();
                }
            }
            catch (Exception x)
            {
                throw;
            }
        }
    }
}
