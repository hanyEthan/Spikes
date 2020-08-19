using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Unity;
using XCore.Framework.Utilities;
using XCore.Services.Geo.Core.Contracts;
using XCore.Services.Geo.Core.Models.Domain;
using XCore.Services.Geo.Core.Models.Search;
using XCore.Services.Geo.Core.Validators;

namespace XCore.Services.Geo.Core.Handlers
{
    public class GeoHandler : IGeoHandler
    {
        #region cst.

        public GeoHandler()
        {
            this.Initialized = this.Initialize();
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.GeoHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IGeoHandler

        //public ExecutionResponse<LocationEvent> AddLocationEvent(LocationEvent locationEvent, RequestContext requestContext)
        //{
        //    var context = new ExecutionContext<LocationEvent>();
        //    context.Process(() =>
        //    {
        //        #region Logic

        //        #region DL

        //        using (var dataHandler = new GeoDataUnity<GeoDataUnitySettings>())
        //        {
        //            dataHandler.LocationEvents.AddLocationEvent(locationEvent);
        //            //dataHandler.Save();
        //        }

        //        #endregion

        //        return context.Response.Set(ResponseState.Success, locationEvent);

        //        #endregion
        //    }
        //    #region context

        //    , new ActionContext()
        //    {
        //        Request = requestContext,
        //        Validation = new ValidationContext<LocationEvent>(new LocationEventValidator(), locationEvent , ValidationMode.Create),
        //    });
        //    return context.Response;

        //    #endregion
        //}
        //public ExecutionResponse<LocationEvent> GetCurrentLocation(string entityCode, RequestContext requestContext)
        //{
        //    var context = new ExecutionContext<LocationEvent>();
        //    context.Process(() =>
        //    {
        //        #region Logic

        //        #region DL

        //        using (var dataHandler = new GeoDataUnity<GeoDataUnitySettings>())
        //        {
        //            #region Get Current Location
        //            var result = dataHandler.LocationEventsLatest.GetFirstAsync(x => x.EntityCode == entityCode).GetAwaiter().GetResult();
        //            var currentLocation = LocationEvent.Map(result);
        //            #endregion

        //            return context.Response.Set(ResponseState.Success, currentLocation);
        //        }

        //        #endregion

        //        #endregion
        //    }
        //    #region context

        //    , new ActionContext()
        //    {
        //        Request = requestContext,
        //    });
        //    return context.Response;

        //    #endregion
        //}
        //public ExecutionResponse<LocationEventsSearchResults> GetLocations(LocationEventSearchCriteria criteria, RequestContext requestContext)
        //{
        //    var context = new ExecutionContext<LocationEventsSearchResults>();
        //    context.Process(() =>
        //    {
        //        #region Logic

        //        #region DL

        //        using (var dataHandler = new GeoDataUnity<GeoDataUnitySettings>())
        //        {
        //            #region Get Locations

        //            var result = dataHandler.LocationEvents.GetLocations(criteria);

        //            #endregion

        //            return context.Response.Set(ResponseState.Success, result);
        //        }

        //        #endregion

        //        #endregion
        //    }
        //    #region context

        //    , new ActionContext()
        //    {
        //        Request = requestContext,
        //    });
        //    return context.Response;

        //    #endregion
        //}
        public ExecutionResponse<LocationEvent> AddLocationEvent(LocationEvent locationEvent, RequestContext requestContext)
        {
            throw new NotImplementedException();
        }

        public ExecutionResponse<LocationEvent> GetCurrentLocation(string entityCode, RequestContext requestContext)
        {
            throw new NotImplementedException();
        }

        public ExecutionResponse<LocationEventsSearchResults> GetLocations(LocationEventSearchCriteria criteria, RequestContext requestContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region helpers.

        private bool Initialize()
        {
            try
            {
                return true;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }
        private LocationEventLatest MapUpdate(LocationEventLatest from , LocationEventLatest to)
        {
            to.EventCode = from.EventCode;
            to.Latitude = from.Latitude;
            to.Longitude = from.Longitude;
            to.MetaData = from.MetaData;

            return to;
        }

        #endregion
    }
}
