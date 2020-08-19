using XCore.Services.Personnel.Models.Organizations;

namespace XCore.Services.Personnel.Models.Accounts
{
    public class OrganizationAccount : AccountBase
    {
        public OrganizationAccount()
        {
            AccountType = Constants.Constants.AccountTypes.Organization;
        }
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
