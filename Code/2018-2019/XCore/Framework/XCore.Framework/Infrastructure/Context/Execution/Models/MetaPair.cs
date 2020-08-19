namespace XCore.Framework.Infrastructure.Context.Execution.Models
{
    public class MetaPair
    {
        public string Property { get; set; }
        public string Meta { get; set; }

        #region cst.

        public MetaPair()
        {
        }
        public MetaPair( string property , string meta )
        {
            this.Property = property;
            this.Meta = meta;
        }

        #endregion
    }
}
