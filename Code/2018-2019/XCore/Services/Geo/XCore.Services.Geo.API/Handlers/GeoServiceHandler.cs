using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Utilities;
using XCore.Services.Geo.Core.Contracts;
using XCore.Services.Geo.Core.Handlers;
using XCore.Services.Geo.Core.Models.Commands;
using XCore.Services.Geo.Core.Models.Domain;
using XCore.Services.Geo.Core.Models.Search;

namespace XCore.Services.Geo.API.Handlers
{
    public class GeoServiceHandler
    {
        #region props.

        public bool Initialized { get; protected set; }
        public IGeoHandler Handler { get; set; }

        #endregion
        #region cst.

        public GeoServiceHandler()
        {
            this.Initialized = this.Initialize();
        }

        #endregion
        #region publics.

        public ExecutionResponse<LocationEvent> AddLocationEvent(AddLocationRequestDomain request)
        {
            try
            {
                var response = Handler.AddLocationEvent(request?.Location, SystemRequestContext.Instance);
                return response;
            }
            #region catch

            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return new ExecutionResponse<LocationEvent>()
                {
                    State = ResponseState.Error,
                    Result = null,
                    Exception = x,
                };
            }

            #endregion
        }
        public ExecutionResponse<LocationEventsSearchResults> GetLocations(LocationEventSearchCriteria criteria)
        {
            try
            {
                var response = Handler.GetLocations(criteria, SystemRequestContext.Instance);
                return response;
            }
            #region catch

            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return new ExecutionResponse<LocationEventsSearchResults>()
                {
                    State = ResponseState.Error,
                    Result = null,
                    Exception = x,
                };
            }

            #endregion
        }
        public ExecutionResponse<LocationEvent> GetCurrentLocation(string entityCode)
        {
            try
            {
                var response = Handler.GetCurrentLocation(entityCode, SystemRequestContext.Instance);
                return response;
            }
            #region catch

            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return new ExecutionResponse<LocationEvent>()
                {
                    State = ResponseState.Error,
                    Result = null,
                    Exception = x,
                };
            }

            #endregion
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            try
            {
                var isValid = true;

                this.Handler = new GeoHandler();

                isValid = this.Handler.Initialized.GetValueOrDefault();

                return isValid;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }

        #endregion
    }
}
