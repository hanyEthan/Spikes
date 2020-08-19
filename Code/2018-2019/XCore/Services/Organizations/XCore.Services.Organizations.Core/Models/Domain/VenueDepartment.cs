using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class VenueDepartment
    {
        #region props.
        
        public int? Capacity { get; set; }

        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        #endregion
    }
}
