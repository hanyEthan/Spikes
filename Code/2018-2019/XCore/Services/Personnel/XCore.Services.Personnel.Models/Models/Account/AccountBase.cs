using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.Models.Accounts
{
    public class AccountBase : Entity<int>
    {
        public AccountBase()
        {
            Settings = new List<Setting>();
        }
        public virtual List<Setting> Settings { get; set; }
        public string AccountType { get; set; }
    }
}
