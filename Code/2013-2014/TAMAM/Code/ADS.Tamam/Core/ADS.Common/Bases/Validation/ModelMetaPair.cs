namespace ADS.Common.Validation
{
    public class ModelMetaPair
    {
        public string PropertyName { get; set; }
        public string Meta { get; set; }

        #region cst ...

        public ModelMetaPair()
        {
        }
        public ModelMetaPair( string propertyName , string meta )
        {
            PropertyName = propertyName;
            Meta = meta;
        }

        #endregion
    }
}
