using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Personnel.Models.Personnels;

namespace XCore.Services.Personnel.Models.Accounts
{
   public class PersonnelAccount : AccountBase
    {
        public PersonnelAccount()
        {
            AccountType = Constants.Constants.AccountTypes.Personnel;
        }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
