using System;
using System.Collections.Generic;

using ADS.Common.Contracts.AuditTrail;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.AuditTrail
{
    /// <summary>
    /// default custom audit trail handler, providing logs support
    /// </summary>
    public class AuditTrailHandler : IAuditTrailHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "AuditTrailHandler"; } }

        private IAuditTrailDataHandler _DataHandler;

        #endregion
        #region cst.

        public AuditTrailHandler(IAuditTrailDataHandler dataHandler)
        {
            XLogger.Info("AuditTrailHandler ...");

            try
            {
                _DataHandler = dataHandler;
                Initialized = _DataHandler != null && _DataHandler.Initialized;
            }
            catch (Exception x)
            {
                XLogger.Error("AuditTrailHandler ... Exception: " + x);
                //ExceptionHandler.Handle( x );
            }
        }

        #endregion

        # region IAuditTrailHandler

        public Guid? Log( AuditTrailLog item )
        {
            return _DataHandler.Log( item ).Id;
        }
        public List<AuditTrailLog> GetLogs( AuditTrailLogCriteria criteria , out int totalCount )
        {
            return _DataHandler.GetLogs( criteria , out totalCount );
        }
        public bool DeleteLog( Guid id )
        {
            return _DataHandler.DeleteLog( id );
        }

        public List<AuditTrailAction> GetActions()
        {
            return _DataHandler.GetAuditTrailActions();
        }
        public List<AuditTrailAction> GetActions(AuditTrailActionCriteria criteria)
        {
            return _DataHandler.GetAuditTrailActions(criteria);
        }

        public AuditTrailAction GetAction(Guid id)
        {
            return _DataHandler.GetAuditTrailAction(id);
        }
        public AuditTrailAction AddAction(AuditTrailAction item)
        {
            return _DataHandler.AddAuditTrailAction(item);
        }
        public bool DeleteAction(Guid id)
        {
            return _DataHandler.DeleteAuditTrailAction(id);
        }

        public List<AuditTrailModule> GetModules(AuditTrailModuleCriteria criteria)
        {
            return _DataHandler.GetAuditTrailModules(criteria);
        }
        public List<AuditTrailModule> GetModules()
        {
            return _DataHandler.GetAuditTrailModules();
        }
        public AuditTrailModule GetModule(Guid id)
        {
            return _DataHandler.GetAuditTrailModule(id);
        }
        public AuditTrailModule AddModule(AuditTrailModule item)
        {
            return _DataHandler.AddAuditTrailModule(item);
        }
        public bool DeleteModule(Guid id)
        {
            return _DataHandler.DeleteAuditTrailModule(id);
        }
        
        # endregion
    }
}