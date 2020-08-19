using System;
using System.Linq;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Utilities;
using XCore.Services.Attachments.SDK.Models;
using XCore.Services.Attachments.Test.Console.Support;

namespace XCore.Services.Attachments.Test.Console.Tests
{
    public static class AttachmentsTests
    {
        #region ...

        public static void start()
        {
            Test01();
            //Test02();
        }

        #endregion

        private static async void Test01()
        {
            try
            {
                #region prep.

                //var status = AttachmentsNativeClient.BL.Initialized;

                byte[] sampleFile = System.IO.File.ReadAllBytes(@"C:\Users\marwa.saleh\Desktop\test.txt");
                string B64 = Convert.ToBase64String(sampleFile);

                var request = new Attachments.Core.Models.Attachment()
                {
                    Id = "3",
                    Code = "3",
                    Name = "2",
                    Extension = ".txt",
                    MimeType = "text",
                    Body = sampleFile
                };
                var request2 = new AttachmentDTO()
                {
                    Id = "3",
                    Code = "3",
                    Name = "2",
                    Extension = ".txt",
                    MimeType = "text",
                    Body = sampleFile
                };

                string json = XSerialize.JSON.Serialize(request);

                #endregion

                var response = await AttachmentsNativeClient.BL.Create(request, SystemRequestContext.Instance);
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
                //var criteria = new Core.Models.Support.AttachmentSearchCriteria()
                //{
                //    Id = "3",
                //};

                //var json = XSerialize.JSON.Serialize(criteria);
            }
            catch (Exception x)
            {
                throw;
            }
        }

    }
}
