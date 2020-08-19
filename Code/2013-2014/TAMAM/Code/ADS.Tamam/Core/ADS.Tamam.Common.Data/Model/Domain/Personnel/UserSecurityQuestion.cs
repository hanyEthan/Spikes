using System;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    public  class UserSecurityQuestion
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
    }
}
