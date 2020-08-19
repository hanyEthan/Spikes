using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Handlers.Terminals
{
    public class PunchTerminalHandler : IBaseHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "PunchTerminalHandler"; } }

        #endregion
    }
}
