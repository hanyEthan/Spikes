using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Handlers
{
   public class AppHandler : IAppHandler
    {
        #region props.

        private string App_DataInclude_Basic { get; set; }
        private string App_DataInclude_Full { get; set; }

        private readonly ISecurityDataUnity _DataHandler;

        private readonly IModelValidator<App> AppsValidators;

        #endregion
        #region cst.

        public AppHandler(ISecurityDataUnity dataHandler, IModelValidator<App> appsValidators)
        {
            this.AppsValidators = appsValidators;
            this._DataHandler = dataHandler;
            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SecurityHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IAppHandler

        public async Task<ExecutionResponse<SearchResults<App>>> Get(AppSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<App>>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    #region mode.

                    string includes = criteria.InquiryMode == InquiryMode.Basic ? this.App_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Search ? this.App_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Full ? this.App_DataInclude_Full
                                    : null;

                    #endregion

                    var apps = await _DataHandler.Apps.GetAsync(criteria, includes);
                    return context.Response.Set(ResponseState.Success, apps);


                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<App>> Register(App app, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<App>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    //await dataHandler.Apps.CreateAsync(app);
                    await _DataHandler.Apps.CreateAsync(app);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, app);

                    #endregion

                    #endregion
                }
                #region context

                 , new ActionContext()
                 {
                     Request = requestContext,
                     Validation = new ValidationContext<App>(this.AppsValidators, app, ValidationMode.Create),
                 });
                return context.Response;

                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<App>> Edit(App app, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<App>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    var existing = await _DataHandler.Apps.GetFirstAsync(x => x.Id == app.Id || x.Code == app.Code, null, this.App_DataInclude_Full);
                    if (existing == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, app);
                    }
                    MapUpdate(existing, app);

                    if (existing == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, null);
                    }

                    _DataHandler.Apps.Update(existing);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, existing);


                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<App>(this.AppsValidators, app, ValidationMode.Edit),
                });
                return context.Response;

                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> UnregisterApp(int id, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    var app = await _DataHandler.Apps.GetFirstAsync(x => x.Id == id);
                    if (app == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Apps.DeleteAsync(id);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);


                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> UnregisterApp(string code, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    var app = await _DataHandler.Apps.GetFirstAsync(x => x.Code == code);
                    if (app == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Apps.DeleteAsync(app.Id);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> ActivateApp(int id, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    var app = await _DataHandler.Apps.GetFirstAsync(x => x.Id == id);
                    if (app == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Apps.SetActivationAsync(id, true);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);


                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> ActivateApp(string code, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    var app = await _DataHandler.Apps.GetFirstAsync(x => x.Code == code);
                    if (app == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Apps.SetActivationAsync(app.Id, true);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }

          
        }
        public async Task<ExecutionResponse<bool>> DeactivateApp(int id, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    var app = await _DataHandler.Apps.GetFirstAsync(x => x.Id == id);
                    if (app == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Apps.SetActivationAsync(id, false);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> DeactivateApp(string code, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    var app = await _DataHandler.Apps.GetFirstAsync(x => x.Code == code);
                    if (app == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }
                    #region validation.
                    var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Apps.SetActivationAsync(app.Id, false);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> IsUnique(App app, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    var isExisting = await _DataHandler.Apps.AnyAsync(x => ((app.Name != null && x.Name == app.Name.Trim()) ||
                                                                (app.NameCultured != null && x.NameCultured == app.NameCultured.Trim()))
                                                                &&
                                                                (x.Code != app.Code)
                                                                &&
                                                                (x.IsActive == true));

                    return context.Response.Set(ResponseState.Success, isExisting);


                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> IsExists(AppSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    var isExisting = await _DataHandler.Apps.AnyAsync(criteria);
                    return context.Response.Set(ResponseState.Success, isExisting);


                    #endregion

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }

        #endregion
        #region helpers.

        #region Map Updates.

        private bool MapUpdate(App existing, App updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.Description = updated.Description;
            existing.IsActive = updated.IsActive;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.Actors = updated.Actors;
            existing.Privileges = updated.Privileges;
            existing.Roles = updated.Roles;
            existing.Targets = updated.Targets;

            return true;
        }


        #endregion

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && InitializeIncludes();

            isValid = isValid && (this._DataHandler?.Initialized ?? false);
            isValid = isValid && AppsValidators != null;


            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }
        private bool InitializeIncludes()
        {
            try
            {
                #region App : basic

                this.App_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region App : full

                this.App_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "Privileges",
                    "Roles",
                    "Actors",
                    "Targets",
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

        #endregion
    }
}
