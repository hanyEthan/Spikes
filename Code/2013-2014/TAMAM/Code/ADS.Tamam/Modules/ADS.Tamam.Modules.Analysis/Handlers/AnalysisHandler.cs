using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Contracts;

namespace ADS.Tamam.Modules.Analysis.Handlers
{
    public class AnalysisHandler : IAnalysisHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "AnalysisHandler"; } }

        #endregion
        #region cst.

        public AnalysisHandler()
        {
            XLogger.Info( "AnalysisHandler ... Initializing ..." );
            Initialized = true;
        }

        #endregion
        #region publics


        #endregion
    }
}
