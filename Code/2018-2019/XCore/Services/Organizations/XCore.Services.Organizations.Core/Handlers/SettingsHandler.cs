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
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;
using XCore.Services.Organizations.Core.Utilities;

namespace XCore.Services.Organizations.Core.Handlers
{
    public class SettingsHandler : ISettingsHandler
    {
        #region props.
        private string Settings_DataInclude_Full { get; set; }
        private string Settings_DataInclude_Basic { get; set; }
        private string Settings_DataInclude_Search { get; set; }

        private readonly IOrganizationDataUnity dataHandler;
        private readonly IModelValidator<Settings> SettingsValidator;
        #endregion
        #region cst.

        public SettingsHandler(IOrganizationDataUnity DataHandler,
                               IModelValidator<Settings> settingsValidators)
        {
            this.dataHandler = DataHandler;
            this.SettingsValidator = settingsValidators;
            this.Initialized = Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SettingsHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region ISettingsHandler

        public async Task<ExecutionResponse<SearchResults<Settings>>> Get(SettingsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Settings>>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var Settings = await dataHandler.Settings.GetAsync(criteria, this.Settings_DataInclude_Search);
                 return context.Response.Set(ResponseState.Success, Settings);

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
        public async Task<ExecutionResponse<Settings>> Create(Settings Settings, RequestContext requestContext)
        {
            var context = new ExecutionContext<Settings>();
            await context.Process(async () =>
             {
                #region Logic

                #region DL

                await dataHandler.Settings.CreateAsync(Settings);
                 await dataHandler.SaveAsync();

                 return context.Response.Set(ResponseState.Success, Settings);

                #endregion

                #endregion
            }
            #region context

              , new ActionContext()
              {
                  Request = requestContext,
                  Validation = new ValidationContext<Settings>(this.SettingsValidator, Settings, ValidationMode.Create),
              });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Settings>> Edit(Settings Settings, RequestContext requestContext)
        {
            var context = new ExecutionContext<Settings>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await dataHandler.Settings.GetFirstAsync(x => x.Id == Settings.Id || x.Code == Settings.Code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, null);

                OrganizationsHelpers.MapUpdate(existing, Settings);

                dataHandler.Settings.Update(existing);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Settings>(this.SettingsValidator, Settings, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.Settings.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SettingsValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Settings.SetActivationAsync(id, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.Settings.GetFirstAsync(x => x.Code == code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SettingsValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Settings.SetActivationAsync(existing.Id, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.Settings.GetFirstAsync(x => x.Code == code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SettingsValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Settings.SetActivationAsync(existing.Id, false);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.Settings.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SettingsValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Settings.SetActivationAsync(id, false);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.Settings.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SettingsValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Settings.DeleteAsync(existing);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.Settings.GetFirstAsync(x => x.Code == code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.SettingsValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Settings.DeleteAsync(existing);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(Settings settings, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Settings.DeleteAsync(settings);
                await dataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Settings>(this.SettingsValidator, settings, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }

        public async Task<ExecutionResponse<bool>> IsUnique(Settings Settings, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Settings.AnyAsync(x => ((Settings.Name != null && x.Name == Settings.Name.Trim()) ||
                                                                            (Settings.NameCultured != null && x.NameCultured == Settings.NameCultured.Trim()))
                                                                            &&
                                                                            (x.Code != Settings.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(SettingsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Settings.AnyAsync(criteria);
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

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;
            try
            {
                isValid = isValid && (dataHandler?.Initialized ?? false);
                isValid = isValid && this.SettingsValidator != null;

                isValid = isValid && InitializeIncludes();

                return isValid;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }

        private bool InitializeIncludes()
        {
            #region Settings : basic

            this.Settings_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region Settings : search

            this.Settings_DataInclude_Search = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region Organization : full

            this.Settings_DataInclude_Full = string.Join(",", new List<string>()
            {
                "Organization",
            });

            #endregion

            return true;
        }

        #endregion
    }
}
