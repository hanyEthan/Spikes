using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using application.Contracts;
using domain.Support;

namespace spikes.MSRE.core.Framework
{
    public class MSREValidator : IRulesValidator
    {
        #region props.

        private List<RuleDefinition> _rules = new List<RuleDefinition>();

        #endregion
        #region cst.

        public MSREValidator()
        {
            Initialize();
        }

        #endregion
        #region IRulesValidator

        public async Task<BaseResponse> Validate(params string[] jsons)
        {
            try
            {
                foreach (var rule in this._rules)
                {
                    var isValid = await rule.Validate(jsons);
                    if (!isValid.IsValidRequest) return isValid;
                }

                return new BaseResponse();
            }
            catch (System.Exception x)
            {
                throw;
            }
        }

        #endregion
        #region helpers.

        private void Initialize()
        {
            this._rules.Add(new RuleDefinition("Person Name Validation Rule", "person.insert", File.ReadAllText(@"Rules\\business.rule.person.name.json")));
        }

        #endregion
    }
}
