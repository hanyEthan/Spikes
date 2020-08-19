namespace XCore.Services.Geo.API.Models
{
    public class LocationEventDTO
    {
        public int Id { get; set; }

        public string EntityCode { get; set; }
        public string EntityType { get; set; }
        public string EventCode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string MetaData { get; set; }
    }
}
