using Peasy;

namespace spikes.Peasy.core.DTOs
{
    public class Person : domain.DTOs.Person, IDomainObject<int>
    {
    }
}
