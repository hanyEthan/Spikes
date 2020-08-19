using System.Collections.Generic;

namespace spikes.NRules.core.Support
{
    public class RuleValidationResults
    {
        public bool IsValid { get; set; } = true;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
