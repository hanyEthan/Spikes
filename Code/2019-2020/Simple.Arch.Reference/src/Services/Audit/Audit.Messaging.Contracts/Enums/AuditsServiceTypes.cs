namespace Mcs.Invoicing.Services.Audit.Messaging.Contracts.Enums
{
    public enum AuditsServiceTypes : int
    {
        None = 0,
        Notification = 1,
        CodeManagement = 2,
        CoreTaxpayerRegistry = 3,
        DocumentAlerting = 4,
        DocumentPublisher = 5,
        TaxpayerProfileManagement = 6,
        IdentityHub = 7
    }
}
