using System.Threading.Tasks;
using application.Contracts;
using core.Utilities;
using domain.DTOs;
using domain.Support;
using infrastructure.Repositories;

namespace application.Services
{
    public class PersonService
    {
        #region props.

        private PersonMockRepository _repo { get; set; }
        private IRulesValidator _validator { get; set; }

        #endregion
        #region cst.

        public PersonService(PersonMockRepository repo, IRulesValidator validator)
        {
            _repo = repo;
            _validator = validator;

            Initialize();
        }

        #endregion
        #region publics.

        public async Task<BaseResponse> InsertCommand(string json)
        {
            // ...
            var response = await this._validator.Validate(json);
            if (!response.IsValidRequest) return response;

            // ...
            var command = JsonUtilities.Deserialize<Person>(json);
            response.Content = _repo.Insert(command);

            // ...
            return response;
        }

        #endregion
        #region helpers.

        private void Initialize()
        {
        }

        #endregion
    }
}
