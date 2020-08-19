using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Attachments.SDK.Contracts;
using XCore.Services.Attachments.SDK.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using XCore.Services.Attachments.SDK.Models.Enums;
using System.Collections.Generic;

namespace XCore.Services.Attachments.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class AttachmentController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        protected IAttachmentClient AttachmentClient { get; set; }

        #endregion
        #region cst.

        public AttachmentController(IAttachmentClient attachmentClient)
        {
            this.AttachmentClient = attachmentClient;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.
        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<AttachmentDTO>>> Get(ServiceExecutionRequestDTO<AttachmentSearchCriteriaDTO> request)
        
        {
            try
            {
                Check();
                var response =await this.AttachmentClient.Get(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<AttachmentDTO>> Create(ServiceExecutionRequestDTO<AttachmentDTO> request)
        {
            try
            {
                Check();
                var response = await this.AttachmentClient.Create(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<string> request)
        {
            try
            {
                Check();
                var response = await this.AttachmentClient.Delete(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        [HttpPost]
        [Route("DeleteObj")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeleteObj()
        {
            try
            {
                var File_Id = HttpContext.Request.Form["ID"];

                #region Validation.
                if (string.IsNullOrWhiteSpace(File_Id))
                {
                    throw new Exception("File Cannot Be Empty.");
                }
                #endregion

                #region AttachmentClient.
                ServiceExecutionRequestDTO<string> request = Mapreq(File_Id);
                var response = await this.AttachmentClient.DeleteSoft(request);
                #endregion
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }


        }
        [HttpPost]
        [Route("DeleteSoft")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeleteSoft(ServiceExecutionRequestDTO<string> request)
        {
            try
            {
                Check();
                var response = await this.AttachmentClient.DeleteSoft(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
        }
        //[HttpPost]
        //[Route("ConfirmStatus")]
        //public async Task<ServiceExecutionResponseDTO<bool>> ConfirmStatus(ServiceExecutionRequestDTO<AttachmentConfirmationAction> request)
        //{
        //    try
        //    {
        //        Check();
        //        var response = await this.AttachmentClient.ConfirmStatus(request);
        //        return response?.Response;
        //    }
        //    catch (Exception x)
        //    {
        //        XLogger.Error($"Exception : {x}");
        //        throw;
        //    }
        //}
        [HttpPost]
        [Route("CreateObj")]
        public async Task<ServiceExecutionResponseDTO<AttachmentDTO>> CreateObj()
        {

            var File = HttpContext.Request.Form.Files[0];
            ServiceExecutionRequestDTO<AttachmentDTO> request = Map(File);

            try
            {
                Check();
                var response = await this.AttachmentClient.Create(request);
                return response?.Response;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }

        }
        [HttpPost]
        [Route("Download")]
        public async Task<IActionResult> DownloadAttachment(ServiceExecutionRequestDTO<AttachmentSearchCriteriaDTO> request)
        {
            try
            {
                var response = await this.AttachmentClient.Get(request);

                return File(fileContents: response?.Response.Content.Results[0].Body,
                            contentType: response?.Response.Content.Results[0].MimeType.ToLower(),
                            fileDownloadName: response?.Response.Content.Results[0].Name);
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }


        }
        #endregion
        #region helpers.
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.AttachmentClient?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault()) throw new Exception("gateway service is not initialized correctly.");
        }
        protected ServiceExecutionRequestDTO<string> Mapreq(string Id, object metadata = null)
        {

            var to = new ServiceExecutionRequestDTO<string>();

            if (Id == null) return null;
            to.RequestClientToken = "634a5d14-81c1-4c9b-9484-0b3c21bb2299";
            to.RequestSessionCode = null;
            to.RequestCorrelationCode = null;
            to.RequestMetadata = "4557d16f-3dd0-4bfc-a913-70db20e43e08";
            to.RequestTime = "20191008 13:13:34";
            to.RequestCulture = "en-US";
            to.Content = Id ;//new AttachmentConfirmationAction() {AttachmentId = new List<string>() { Id } , ConfirmationAction=AttachmentConfirmationStatus.RequestDelete ,};
            return to;
        }
        protected AttachmentDTO MapContent(IFormFile from, object metadata = null)
        {
            var to = new AttachmentDTO();
            to.CreatedBy = null;
            to.CreatedDate = null; // DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.MetaData = null;
            to.MimeType = from.ContentType;
            FileInfo file_Info = new FileInfo(from.FileName);
            string ext = file_Info.Extension;

            var fileStream = from.OpenReadStream();
            byte[] bytes = new byte[from.Length];
            fileStream.Read(bytes, 0, (int)from.Length);
            to.Body = bytes;

            //using (var ms = new MemoryStream())
            //{
            //    from.CopyToAsync(ms);
            //    to.Body = ms.ToArray();

            //}

            to.Name = from.FileName;
            to.Extension = ext;
            to.Status = AttachmentStatus.MarkedForAddtion;

            return to;
        }
        protected ServiceExecutionRequestDTO<AttachmentDTO> Map(IFormFile from, object metadata = null)
        {

            var to = new ServiceExecutionRequestDTO<AttachmentDTO>();

            if (from == null) return null;
            to.RequestClientToken = "634a5d14-81c1-4c9b-9484-0b3c21bb2299";
            to.RequestSessionCode = null;
            to.RequestCorrelationCode = null;
            to.RequestMetadata = "4557d16f-3dd0-4bfc-a913-70db20e43e08";
            to.RequestTime = "20191008 13:13:34";
            to.RequestCulture = "en-US";
            to.Content = MapContent(from);
            return to;
        }
        #endregion
    }
}
