using System;
using System.Collections.Generic;
using ADS.Common.Contracts.SystemCodes;
using ADS.Common.Handlers.Cache;
using ADS.Common.Models.Domain;
using ADS.Common.Models.DTO;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.SystemCodes
{
    public class MasterCodeHandler : IMasterCodeHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "MasterCodeHandler"; } }

        private IMasterCodeDataHandler _DataHandler;

        #endregion
        #region cst.

        public MasterCodeHandler(IMasterCodeDataHandler dataHandler)
        {
            XLogger.Info("MasterCodeHandler ...");

            try
            {
                _DataHandler = dataHandler;
                Initialized = _DataHandler != null && _DataHandler.Initialized;

                //Initialized = true; // waiting for the datastores to be set ...
            }
            catch (Exception x)
            {
                XLogger.Error("MasterCodeHandler ... Exception: " + x);
                //ExceptionHandler.Handle( x );
            }
        }

        #endregion
        # region IMasterCodeHandler

        public List<MasterCode> GetMasterCodes()
        {
            return _DataHandler.GetMasterCodes();
        }
        public MasterCode GetMasterCode(int id)
        {
            return _DataHandler.GetMasterCode(id);
        }
        public MasterCode GetMasterCode(int id, bool underlyingCollections)
        {
            return _DataHandler.GetMasterCode(id, underlyingCollections);
        }
        public MasterCode GetMasterCode(string code)
        {
            return _DataHandler.GetMasterCode(code);
        }
        public MasterCode GetMasterCode(string code, bool underlyingCollections)
        {
            return _DataHandler.GetMasterCode(code, underlyingCollections);
        }
        public MasterCode CreateMasterCode(MasterCode masterCode)
        {
            var masterCodeUpdated = _DataHandler.CreateMasterCode( masterCode );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Common );

            #endregion

            return masterCodeUpdated;
        }
        public MasterCode UpdateMasterCode(MasterCode masterCode)
        {
            var masterCodeUpdated = _DataHandler.UpdateMasterCode( masterCode );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Common );

            #endregion

            return masterCodeUpdated;
        }
        public bool DeleteMasterCode(int id)
        {
            var status = _DataHandler.DeleteMasterCode(id);

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Common );

            #endregion

            return status;
        }
        public List<MasterCode> GetViewableMasterCodes()
        {
            return _DataHandler.GetViewableMasterCodes();
        }
        public bool IsMasterCode_CodeUnique(string code)
        {
            return _DataHandler.IsMasterCode_CodeUnique(code);
        }
        public bool IsMasterCode_NameUnique(string name)
        {
            return _DataHandler.IsMasterCode_NameUnique(name);
        }
        public bool IsMasterCode_NameCultureVariantUnique(string nameCultureVariant)
        {
            return _DataHandler.IsMasterCode_NameCultureVariantUnique(nameCultureVariant);
        }
        public bool IsMasterCode_CodeEditUnique(int id, string code)
        {
            return _DataHandler.IsMasterCode_CodeEditUnique(id, code);
        }
        public bool IsMasterCode_NameEditUnique(int id, string name)
        {
            return _DataHandler.IsMasterCode_NameEditUnique(id, name);
        }
        public bool IsMasterCode_NameCultureVariantEditUnique(int id, string nameCultureVariant)
        {
            return _DataHandler.IsMasterCode_NameCultureVariantEditUnique(id, nameCultureVariant);
        }
        public List<MasterCodeDTO> GetMasterCodeParentsList(int id)
        {
            return _DataHandler.GetMasterCodeParentsList(id);
        }
        public List<MasterCode> SearchByName(string name)
        {
            return _DataHandler.SearchByName(name);
        }

        # endregion
    }
}
