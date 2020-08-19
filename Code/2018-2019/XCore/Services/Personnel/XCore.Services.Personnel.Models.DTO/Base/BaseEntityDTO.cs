
namespace XCore.Services.Personnel.Models.DTO.Base
{
    public class BaseEntityDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultured { get;  set; }
        public bool? IsActive { get; set; }
        public string MetaData { get; set; }
    }

}
