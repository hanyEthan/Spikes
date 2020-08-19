namespace XCore.Framework.Framework.Services.Rest
{
    public class RestResponse<TContent>
    {
        #region props.

        public string Code { get; set; }
        public TContent Response { get; set; }

        #endregion
        #region cst.

        public RestResponse()
        {
        }
        public RestResponse(string code, TContent response) : this()
        {
            this.Code = code;
            this.Response = response;
        }

        #endregion
    }
}
