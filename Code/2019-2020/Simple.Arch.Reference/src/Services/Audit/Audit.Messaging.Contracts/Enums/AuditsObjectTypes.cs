namespace Mcs.Invoicing.Services.Audit.Messaging.Contracts.Enums
{
    public enum AuditsObjectTypes : int
    {
        None = 0,
        User = 1,
        TaxPayer = 2,
        Document = 3,
        Notification = 4,
        DocumentType = 5,
        DocumentTypeVersion = 6,
        CodeType = 7,
        Code = 8,
    }
}
