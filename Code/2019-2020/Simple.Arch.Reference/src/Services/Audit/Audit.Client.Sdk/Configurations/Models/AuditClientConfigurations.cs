namespace Mcs.Invoicing.Services.Audit.Client.Sdk.Configurations.Models
{
    public class AuditClientConfigurations
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
