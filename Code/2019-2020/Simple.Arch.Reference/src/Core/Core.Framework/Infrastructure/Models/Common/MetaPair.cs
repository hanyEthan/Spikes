namespace Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common
{
    public class MetaPair
    {
        public string code { get; set; }
        public string target { get; set; }
        public string message { get; set; }

        #region cst.

        public MetaPair()
        {
        }
        public MetaPair(string property, string description)
        {
            this.target = property;
            this.message = description;
        }
        public MetaPair(string property, string description, string code)
        {
            this.target = property;
            this.message = description;
            this.code = code;
        }

        #endregion
    }
}
