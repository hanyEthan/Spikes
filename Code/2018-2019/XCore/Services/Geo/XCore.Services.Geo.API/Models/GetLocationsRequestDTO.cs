namespace XCore.Services.Geo.API.Models
{
    public class GetLocationsRequestDTO
    {
        public string EntityCode { get; set; }
        public GetLocationsRequestDTOOrderBy? OrderBy { get; set; }
        public GetLocationsRequestDTOOrderDirection? OrderDirection { get; set; }
        public bool PagingEnabled { get; set; }
        public virtual int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }
}