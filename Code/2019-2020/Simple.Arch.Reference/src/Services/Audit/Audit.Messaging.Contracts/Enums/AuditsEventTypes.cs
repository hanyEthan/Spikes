namespace Mcs.Invoicing.Services.Audit.Messaging.Contracts.Enums
{
    public enum AuditsEventTypes : int
    {
        None = 0,
        UserCreated = 1,
        UserLoggedIn = 2,
        UserBlocked = 3,
        UserUnblocked = 4,
        UserContactDetailsValidated = 5,
        UserDetailsUpdated = 6,
        TaxpayerContactDetailsValidated = 7,
        TaxpayerProfileUpdated = 8,
        TaxpayerBlocked = 9,
        TaxpayerUnblocked = 10,
        SystemCredentialsUpdated = 11,
        DelegationAccepted = 12,
        DelegateInvitedByTaxpayerAdministrator = 13,
        DelegateInvitedByETA = 14,
        CodeTypeCreated = 15,
        CodeTypeUpdated = 16,
        CodeTypeDeactivated = 17,

        CodeCreated = 18,
        CodeUpdated = 19,
        CodeDeactivated = 20,

        UserPasswordResetInitiated = 21,
        UserPasswordChanged = 22,
        DelegateInvitationCreated = 23,
        TaxpayerProfileCreated = 24,
        UserProfileCreated = 25

    }
}
