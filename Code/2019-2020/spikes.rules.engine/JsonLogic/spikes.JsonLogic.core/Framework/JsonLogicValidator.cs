using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using application.Contracts;
using core.Utilities;
using domain.Support;
using JsonLogic.Net;
using Newtonsoft.Json.Linq;

namespace spikes.NRules.core.Framework
{
    public class JsonLogicValidator : IRulesValidator
    {
        #region props.

        protected List<string> Rules { get; set; } = new List<string>();

        #endregion
        #region cst.

        public JsonLogicValidator()
        {
            Initialize();
        }

        #endregion
        #region IRulesValidator

        public async Task<BaseResponse> Validate(params string[] jsons)
        {
            try
            {
                var evaluator = new JsonLogicEvaluator(EvaluateOperators.Default);
                var data = JsonUtilities.DeserializeExpando(jsons);

                foreach (var ruleJson in this.Rules)
                {
                    var rule = JObject.Parse(ruleJson);
                    bool isValid = evaluator.Apply(rule, data[0]);
                    if (!isValid) return new BaseResponse() { IsValidRequest = false };
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
            InitializeRules();
            InitializeCustomOperations();
        }
        private void InitializeRules()
        {
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.person.email.json"));
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.person.name.json"));
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.person.city.json"));

            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.invoice.amounts.json"));
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.invoice.totals.json"));
        }
        private void InitializeCustomOperations()
        {
            #region regex.

            Func<IProcessJsonLogic, JToken[], object, object> regex = (p, args, data) => { return StringUtilities.MatchPattern(p.Apply(args[0], data), p.Apply(args[1], data)); };
            EvaluateOperators.Default.AddOperator("regex", regex);

            #endregion
        }

        #endregion
    }
}
