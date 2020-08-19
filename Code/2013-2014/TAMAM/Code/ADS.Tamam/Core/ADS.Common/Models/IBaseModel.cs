namespace ADS.Common.Models
{
    public interface IBaseModel
    {
        object Id { get; }
        string Name { get; }
        string NameCultureVariant { get; }
    }

    public class BaseModel : IBaseModel
    {
        public BaseModel( object id , string name , string nameCultureVariant )
        {
            Id = id;
            Name = name;
            NameCultureVariant = nameCultureVariant;
        }

        public object Id { get; private set; }
        public string Name { get; private set; }
        public string NameCultureVariant { get; private set; }
    }
}