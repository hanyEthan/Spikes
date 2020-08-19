using System;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public string LoginDays { get; set; }
        public string LoginTime { get; set; }

        public bool IsLocked { get; set; }
        public bool IsSecurityAnswerLocked { get; set; }
        public bool MustChangePassword { get; set; }

        public IList<UserSecurityQuestion> SecurityQuestions { get; set; }
    }
}
