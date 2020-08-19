using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.Event;
using XCore.Services.Organizations.API.Models.Venue;

namespace XCore.Services.Organizations.API.Models.VenueEvent
{
    public class VenueEventDTO
    {
        public virtual int Id { get; set; }
        public int VenueId { get; set; }
        public int EventId { get; set; }
        public VenueDTO VenueDTO { get; set; }
        public EventDTO EventDTO { get; set; }
        public virtual string Description { get; set; }
        public virtual string Code { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string MetaData { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
    


     
    }
}
