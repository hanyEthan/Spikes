using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;

namespace XCore.Services.Personnel.Models.Settings
{

    public class Setting : Entity<int>
    {
        public int AccountId { get; set; }
        public virtual AccountBase Account { get; set; }
    }
}
