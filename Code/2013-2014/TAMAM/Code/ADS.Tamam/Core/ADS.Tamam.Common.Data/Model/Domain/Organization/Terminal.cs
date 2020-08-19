namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    public class Terminal
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int? LocationId { get; set; }
    }
}
