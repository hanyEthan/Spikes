using System.Collections.Generic;
using Peasy;
using spikes.Peasy.core.DTOs;
using spikes.Peasy.core.Rules;

namespace spikes.Peasy.core.Services
{
    public class PersonService : ServiceBase<Person, int>
    {
        #region cst.

        public PersonService(IDataProxy<Person, int> repo) : base(repo)
        {
        }

        #endregion
        #region ServiceBase

        protected override IEnumerable<IRule> GetBusinessRulesForInsert(Person entity, ExecutionContext<Person> context)
        {
            yield return new PersonNameRule(entity.Name);
            yield return new ValidCityRule(entity.Address.City);
        }

        #endregion
    }
}
