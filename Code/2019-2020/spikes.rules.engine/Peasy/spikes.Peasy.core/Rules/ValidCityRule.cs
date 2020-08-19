using Peasy;

namespace spikes.Peasy.core.Rules
{
    public class ValidCityRule : RuleBase
    {
        #region props.

        private string _city;

        #endregion
        #region cst.

        public ValidCityRule(string city)
        {
            _city = city;
        }

        #endregion
        #region RuleBase

        protected override void OnValidate()
        {
            if (_city == "Nowhere")
            {
                Invalidate("Nowhere is not a city");
            }
        }

        #endregion
    }
}
