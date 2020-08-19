using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Utilities;
using domain.Support;
using Newtonsoft.Json;
using RulesEngine.Extensions;
using RulesEngine.Models;

namespace spikes.MSRE.core.Framework
{
    public class RuleDefinition
    {
        #region props.

        public string Name { get; private set; }
        public string Key { get; private set; }
        public string RulesJson { get; private set; }

        #endregion
        #region cst.

        public RuleDefinition(string name, string key, string rulesJson)
        {
            this.Name = name;
            this.Key = key;
            this.RulesJson = rulesJson;
        }

        #endregion
        #region publics.

        public async Task<BaseResponse> Validate(params string[] jsons)
        {
            var data = JsonUtilities.DeserializeExpando(jsons);

            var workflowRules = JsonConvert.DeserializeObject<List<WorkflowRules>>(this.RulesJson);
            var BRE = new RulesEngine.RulesEngine(workflowRules.ToArray(), null);
            var resultList = BRE.ExecuteRule(this.Key, data);

            var result = new BaseResponse();

            resultList.OnSuccess((eventName) =>
            {
                result.IsValidRequest = true;
            });

            resultList.OnFail(() =>
            {
                result.IsValidRequest = false;
                result.Messages = resultList.Select(x => x.ExceptionMessage).ToList();
            });

            return result;
        }

        #endregion
    }
}
