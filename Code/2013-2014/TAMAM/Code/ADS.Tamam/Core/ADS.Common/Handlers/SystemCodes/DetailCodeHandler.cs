using System;
using System.Collections.Generic;
using ADS.Common.Contracts.SystemCodes;
using ADS.Common.Handlers.Cache;
using ADS.Common.Models.Domain;
using ADS.Common.Models.DTO;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.SystemCodes
{
    public class DetailCodeHandler : IDetailCodeHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "DetailCodeHandler"; } }

        private IDetailCodeDataHandler _DataHandler;

        #endregion
        #region cst.

        public DetailCodeHandler(IDetailCodeDataHandler dataHandler)
        {
            XLogger.Info("DetailCodeHandler ...");

            try
            {
                _DataHandler = dataHandler;
                Initialized = _DataHandler != null && _DataHandler.Initialized;

                //Initialized = true; // waiting for the datastores to be set ...
            }
            catch (Exception x)
            {
                XLogger.Error("DetailCodeHandler ... Exception: " + x);
                //ExceptionHandler.Handle( x );
            }
        }

        #endregion
        # region IDetailCodeHandler

        public List<DetailCode> GetDetailCodes()
        {
            return _DataHandler.GetDetailCodes();
        }
        public DetailCode GetDetailCode(int id)
        {
            return _DataHandler.GetDetailCode(id);
        }
        public DetailCode GetDetailCode(int id, bool underlyingCollections)
        {
            return _DataHandler.GetDetailCode(id, underlyingCollections);
        }
        public DetailCode GetDetailCode(string code)
        {
            return _DataHandler.GetDetailCode(code);
        }
        public DetailCode GetDetailCode(string code, bool underlyingCollections)
        {
            return _DataHandler.GetDetailCode(code, underlyingCollections);
        }
        public DetailCode CreateDetailCode(DetailCode detailCode)
        {
            var detailCodeUpdated = _DataHandler.CreateDetailCode( detailCode );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Common );

            #endregion

            return detailCodeUpdated;
        }
        public DetailCode UpdateDetailCode(DetailCode detailCode)
        {
            var detailCodeUpdated = _DataHandler.UpdateDetailCode( detailCode );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Common );

            #endregion

            return detailCodeUpdated;
        }
        public bool DeleteDetailCode(int id)
        {
            var status = _DataHandler.DeleteDetailCode( id );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Common );

            #endregion

            return status;
        }
        public List<DetailCode> GetViewableDetailCodes()
        {
            return _DataHandler.GetViewableDetailCodes();
        }
        public bool IsDetailCode_CodeUnique(string code)
        {
            return _DataHandler.IsDetailCode_CodeUnique(code);
        }
        public bool IsDetailCode_NameUnique(string name)
        {
            return _DataHandler.IsDetailCode_NameUnique(name);
        }
        public bool IsDetailCode_NameCultureVariantUnique(string nameCultureVariant)
        {
            return _DataHandler.IsDetailCode_NameCultureVariantUnique(nameCultureVariant);
        }
        public List<DetailCodeDTO> GetDetailCodeParentsList(int id)
        {
            return _DataHandler.GetDetailCodeParentsList(id);
        }
        public List<DetailCode> SearchDetailCodes(DetailCodeCriteria criteria)
        {
            return _DataHandler.SearchDetailCodes(criteria);
        }

        public bool CheckDetailCodeExistance( int id )
        {
            return _DataHandler.CheckDetailCodeExistance( id );
        }

        public List<DetailCode> GetDetailCodesByMasterCode( string masterCode, bool allDetailCodes )
        {
            #region Cache

            var cacheKey = "DetailCodeHandler_GetDetailCodesByMasterCode" + masterCode + allDetailCodes;
            var cached = Broker.Cache.Get<List<DetailCode>>( CacheClusters.Common, cacheKey );
            if ( cached != null ) return cached;

            #endregion

            var detailCodes = _DataHandler.GetDetailCodesByMasterCode( masterCode, allDetailCodes );

            #region Cache

            Broker.Cache.Add<List<DetailCode>>( CacheClusters.Common, cacheKey, detailCodes );

            #endregion

            return detailCodes;
        }
        public List<DetailCode> GetDetailCodesByMasterCode( string masterCode )
        {
            #region Cache

            var cacheKey = "DetailCodeHandler_GetDetailCodesByMasterCode" + masterCode;
            var cached = Broker.Cache.Get<List<DetailCode>>( CacheClusters.Common , cacheKey );
            if ( cached != null ) return cached;

            #endregion

            var detailCodes = _DataHandler.GetDetailCodesByMasterCode( masterCode );

            #region Cache

            Broker.Cache.Add<List<DetailCode>>( CacheClusters.Common , cacheKey , detailCodes );

            #endregion

            return detailCodes;
        }
        public List<DetailCode> GetDetailCodesByMasterCode( int masterCode )
        {
            #region Cache

            var cacheKey = "DetailCodeHandler_GetDetailCodesByMasterCode" + masterCode;
            var cached = Broker.Cache.Get<List<DetailCode>>( CacheClusters.Common , cacheKey );
            if ( cached != null ) return cached;

            #endregion

            var detailCodes = _DataHandler.GetDetailCodesByMasterCode( masterCode );

            #region Cache

            Broker.Cache.Add<List<DetailCode>>( CacheClusters.Common , cacheKey , detailCodes );

            #endregion

            return detailCodes;
        }

        # endregion
    }
}