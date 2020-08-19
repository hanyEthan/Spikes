namespace XCore.Utilities.Infrastructure.Entities.Validation.Models
{
    public enum ValidationMode
    {
        Create,
        Edit,
        Delete,

        Activate,
        Deactivate,
        ChangePassword,
        RefreshFromService,
        OverridePassword,
        Draft,
        Review,
    }
}
