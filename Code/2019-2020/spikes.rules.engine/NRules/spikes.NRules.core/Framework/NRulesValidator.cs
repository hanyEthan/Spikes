using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using application.Contracts;
using core.Utilities;
using domain.Support;
using NRules;
using NRules.RuleSharp;
using spikes.NRules.core.Support;

namespace spikes.NRules.core.Framework
{
    public class NRulesValidator : IRulesValidator
    {
        #region props.

        protected List<string> Rules { get; set; } = new List<string>();

        #endregion
        #region cst.

        public NRulesValidator()
        {
            Initialize();
        }

        #endregion
        #region IRulesValidator

        public async Task<BaseResponse> Validate(params string[] jsons)
        {
            try
            {
                var repository = new RuleRepository();
                repository.AddNamespace("System");
                repository.AddReference(typeof(RuleContent).Assembly);
                repository.AddReference(typeof(StringUtilities).Assembly);

                foreach (var rule in this.Rules)
                {
                    using (var stream = StringUtilities.ToStream(rule))
                    {
                        repository.Load(stream);
                    };
                }

                var factory = repository.Compile();
                var session = factory.CreateSession();

                var instance = Map(jsons);
                var result = new RuleValidationResults();

                session.Insert(instance);
                session.Insert(result);

                session.Fire();

                return Map(result);
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
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.invoice.amounts.rul"));
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.invoice.totals.rul"));

            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.person.name.rul"));
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.person.email.rul"));
            this.Rules.Add(File.ReadAllText(@"Rules\business.rule.person.city.rul"));
        }
        private RuleContent Map(string[] from)
        {
            var fromList = new List<string>(from);
            return new RuleContent() { Contents = fromList.Select(x => x).ToList() };
        }
        private BaseResponse Map(RuleValidationResults from)
        {
            return new BaseResponse()
            {
                IsValidRequest = from.IsValid,
                Messages = from.Errors,
                Content = null,
            };
        }

        #endregion
    }
}
