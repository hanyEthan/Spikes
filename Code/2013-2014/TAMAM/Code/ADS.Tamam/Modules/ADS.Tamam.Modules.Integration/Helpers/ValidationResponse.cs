namespace ADS.Tamam.Modules.Integration.Helpers
{
    public class ValidationResponse
    {
        public ValidationResponse( bool isValid, string message )
        {
            this.IsValid = isValid;
            this.Message = message;
        }

        public bool IsValid { get; private set; }
        public string Message { get; private set; }
    }
}