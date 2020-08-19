using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IUsersHandler : IBaseHandler
    {
        void CreatePerson( Person person );
    }
}
