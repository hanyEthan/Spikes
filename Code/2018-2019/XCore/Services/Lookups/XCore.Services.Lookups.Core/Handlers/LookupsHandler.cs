using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Utilities;
using XCore.Services.Lookups.Core.Contracts;
using XCore.Services.Lookups.Core.DataLayer.Contracts;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.Core.Handlers
{
    public class LookupsHandler : ILookupsHandler
    {
        #region props.

        private readonly ILookupsDataUnity _DataLayer;

        private string LookupCategory_DataInclude_Full { get; set; }
        private string LookupCategory_DataInclude_Search { get; set; }
        private string LookupCategory_DataInclude_Basic { get; set; }

        #endregion
        #region cst.

        public LookupsHandler(ILookupsDataUnity dataLayer)
        {
            this._DataLayer = dataLayer;
            this.Initialized = Initialize();
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.LookupsHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region ILookupsHandler

        public async Task<ExecutionResponse<LookupCategory>> Create(LookupCategory lookupCategory, RequestContext requestContext)
        {
            var context = new ExecutionContext<LookupCategory>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, null);
                }

                #endregion
                #region DL

                await this._DataLayer.LookupCategories.CreateAsync(lookupCategory);
                await this._DataLayer.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, lookupCategory);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Edit(LookupCategory lookupCategory, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, false);
                }

                #endregion
                #region DL

                var existing = await _DataLayer.LookupCategories.GetFirstAsync(x => x.Id == lookupCategory.Id || x.Code == lookupCategory.Code, null, this.LookupCategory_DataInclude_Full);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                MapUpdate(existing, lookupCategory);

                _DataLayer.LookupCategories.Update(existing);
                await _DataLayer.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
            });
            return context.Response;

            #endregion
        }

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && ( this._DataLayer?.Initialized ?? false );
            isValid = isValid && InitializeIncludes();

            return isValid;
        }
        private bool InitializeIncludes()
        {
            try
            {
                #region LookupCategory : Search

                this.LookupCategory_DataInclude_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region LookupCategory : basic

                this.LookupCategory_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region LookupCategory : Full

                this.LookupCategory_DataInclude_Full = string.Join(",", new List<string>()
                {
                });

                #endregion

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }

        #region Map Updates.

        private bool MapUpdate(LookupCategory existing, LookupCategory updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.IsActive = updated.IsActive;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;

            bool state = true;

            state = MapUpdate(existing.Lookups, updated.Lookups) && state;

            return state;
        }
        private bool MapUpdate(IList<Lookup> existing, IList<Lookup> updated)
        {
            // ...
            if (updated == null || existing == null) return false;

            // get [deleted records] ...
            foreach (var existingItem in existing)
            {
                var isDeleted = !updated.Any(x => x.Id == existingItem.Id);

                // delete the [existingItem]
                if (isDeleted)
                {
                    this._DataLayer.Lookups.Delete(existingItem);
                    existing.Remove(existingItem);
                }
            }

            // get [inserted records] ...
            foreach (var updatedItem in updated)
            {
                var isInserted = !existing.Any(x => x.Id == updatedItem.Id);

                // insert the [updatedItem]
                if (isInserted)
                {
                    existing.Add(updatedItem);
                }
            }

            return true;
        }

        #endregion

        #endregion
    }
}
