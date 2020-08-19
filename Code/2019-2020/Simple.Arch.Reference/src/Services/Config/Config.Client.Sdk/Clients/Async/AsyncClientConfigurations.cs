namespace Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Async
{
    public class AsyncClientConfigurations
    {
        public bool IsValid
        {
            get
            {
                bool isValid = true;

                isValid = isValid && !string.IsNullOrWhiteSpace(this.ServiceEndpoint);

                return isValid;
            }
        }

        public string ServiceEndpoint { get; set; }
    }
}
