using Peasy;

namespace spikes.Peasy.core.Rules
{
    public class PersonNameRule : RuleBase
    {
        #region props.

        private string _name;

        #endregion
        #region cst.

        public PersonNameRule(string name)
        {
            _name = name;
        }

        #endregion
        #region RuleBase

        protected override void OnValidate()
        {
            if (_name == "Fred Jones")
            {
                Invalidate("Name cannot be fred jones");
            }
        }

        #endregion
    }
}
