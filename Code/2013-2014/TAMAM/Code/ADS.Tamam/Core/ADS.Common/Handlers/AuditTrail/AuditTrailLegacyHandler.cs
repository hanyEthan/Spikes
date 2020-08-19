using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Contracts.AuditTrail;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;

using KFS.Components.Logging.BLL;
using KFS.Components.Logging.DAL;

namespace ADS.Common.Handlers.AuditTrail
{
    /// <summary>
    /// KFS Legacy custom audit trail handler, providing logs support
    /// </summary>
    public class AuditTrailLegacyHandler : IAuditTrailHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "AuditTrailLegacyHandler"; } }

        #endregion
        #region cst.

        public AuditTrailLegacyHandler( IAuditTrailDataHandler dataHandler )
        {
            XLogger.Info("");

            try
            {
                Initialized = true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: " + x );
                //ExceptionHandler.Handle( x );
            }
        }

        #endregion

        # region IAuditTrailHandler

        /// <summary>
        /// audit trail post ...
        /// </summary>
        public Guid? Log( AuditTrailLog item )
        {
            XLogger.Trace( "" );

            try
            {
                // map to old data structures ...
                var logItem = MapLegacyModel( item );
                if ( logItem == null ) return null;

                // log ...
                var status = LoggingMain.LogUserAction( logItem );

                // return ...
                return status == LogUserActionStatus.Success ? Guid.Empty : ( Guid? ) null;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public List<AuditTrailLog> GetLogs( AuditTrailLogCriteria criteria , out int totalCount )
        {
            XLogger.Info( "" );

            totalCount = 0;

            try
            {
                // validate ...
                if ( criteria != null ) return null;

                string outMessage;
                IQueryable<LogUserAction> logs;
                string userId = criteria.UserId.HasValue ? criteria.UserId.Value.ToString() : null;

                // search ...
                var status = LoggingMain.GetUserActionsPaged( out outMessage , out logs , out totalCount , criteria.PageIndex , criteria.PageSize , userId , criteria.Username , criteria.DateFrom , criteria.DateTo , criteria.IPAddress , criteria.MachineName , ( Modules ) criteria.ModuleCode , ( ActionTypes ) criteria.ActionCode , criteria.Details );
                if ( status == LogUserActionStatus.Failure ) return null;

                // map to new data structures ...
                var items = new List<AuditTrailLog>();
                foreach (var item in logs)
	            {
                    items.Add( MapLegacyModel( item ) );		            
	            }

                // return ...
                return items;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public bool DeleteLog( Guid id )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// get all audit trail registered actions
        /// </summary>
        public List<AuditTrailAction> GetActions()
        {
            XLogger.Info( "" );

            try
            {
                // fetch ...
                var actions = LoggingMain.GetAllActions();
                if ( actions == null ) return null;

                // map to new data structures ...
                var items = new List<AuditTrailAction>();
                foreach ( var item in actions )
                {
                    items.Add( MapLegacyModel( item ) );
                }

                // return ...
                return items;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public List<AuditTrailAction> GetActions( AuditTrailActionCriteria criteria )
        {
            XLogger.Info( "" );

            try
            {
                // validate ...
                if ( criteria != null ) return null;

                // search ...
                var action = LoggingMain.GetActionByName( criteria.Name );
                if ( action == null ) return null;

                // map to new data structures ...
                return new List<AuditTrailAction>() { MapLegacyModel( action ) };
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public AuditTrailAction GetAction( Guid id )
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public AuditTrailAction AddAction( AuditTrailAction item )
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public bool DeleteAction( Guid id )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// get all audit trail registered modules
        /// </summary>
        public List<AuditTrailModule> GetModules()
        {
            XLogger.Info( "" );

            try
            {
                // fetch ...
                var modules = LoggingMain.GetAllModules();
                if ( modules == null ) return null;

                // map to new data structures ...
                var items = new List<AuditTrailModule>();
                foreach ( var item in modules )
                {
                    items.Add( MapLegacyModel( item ) );
                }

                // return ...
                return items;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public List<AuditTrailModule> GetModules( AuditTrailModuleCriteria criteria )
        {
            XLogger.Info( "" );

            try
            {
                // validate ...
                if ( criteria != null ) return null;

                // search ...
                var module = LoggingMain.GetModuleByName( criteria.Name );
                if ( module == null ) return null;

                // map to new data structures ...
                return new List<AuditTrailModule>() { MapLegacyModel( module ) };
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public AuditTrailModule GetModule( Guid id )
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public AuditTrailModule AddModule( AuditTrailModule item )
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public bool DeleteModule( Guid id )
        {
            throw new NotSupportedException();
        }

        # endregion

        #region Helpers

        private LogUserAction MapLegacyModel( AuditTrailLog item )
        {
            // validation ...
            if ( item == null || item.Action == null || item.Module == null ) return null;

            var logItem = new LogUserAction();

            // map ...
            logItem.UserName = item.Username;
            logItem.UserId = item.UserCode;
            logItem.MachineName = item.MachineName;
            logItem.ActionDate = item.ActionDate;
            logItem.Details = item.Details;
            logItem.RefKey = item.RefKey;
            logItem.IPAddress = item.IpAddress;
            logItem.Action = item.Action.Code;            
            logItem.ModuleId = item.Module.Code;

            return logItem;
        }
        private AuditTrailLog MapLegacyModel( LogUserAction item )
        {
            // validation ...
            if ( item == null ) return null;

            var logItem = new AuditTrailLog();

            // map ...
            logItem.Username = item.UserName;
            logItem.UserCode = item.UserId;
            logItem.MachineName = item.MachineName;
            logItem.ActionDate = item.ActionDate;
            logItem.Details = item.Details;
            logItem.RefKey = item.RefKey;
            logItem.IpAddress = item.IPAddress;
            logItem.Action = new AuditTrailAction() { Code = item.Action };
            logItem.Module = new AuditTrailModule() { Code = item.ModuleId };

            return logItem;
        }
        private AuditTrailAction MapLegacyModel( LogAction item )
        {
            // validation ...
            if ( item == null ) return null;

            var action = new AuditTrailAction();

            // map ...
            action.Code = item.Id;
            action.Name = item.Name;

            return action;
        }
        private AuditTrailModule MapLegacyModel( LogModule item )
        {
            // validation ...
            if ( item == null ) return null;

            var module = new AuditTrailModule();

            // map ...
            module.Code = item.Id;
            module.Name = item.Name;

            return module;
        }

        #endregion
    }
}
