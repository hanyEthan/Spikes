using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Configurations.Core.Contracts;
using XCore.Services.Configurations.Core.DataLayer.Contracts;
using XCore.Services.Configurations.Core.Mappers;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Events.Domain;
using XCore.Services.Configurations.Core.Models.Support;

namespace XCore.Services.Configurations.Core.Handlers
{
    public class ConfigHandler : IConfigHandler
    {
        #region props.

        private string App_DataInclude_Basic { get; set; }
        private string App_DataInclude_Full { get; set; }

        private string Module_DataInclude_Basic { get; set; }
        private string Module_DataInclude_Full { get; set; }

        private string Config_DataInclude_Full { get; set; }
        private string Config_DataInclude_Basic { get; set; }

        private readonly IConfigurationDataUnity dataHandler;
        private readonly IModelValidator<App> AppsValidators;
        private readonly IModelValidator<Module> ModuleValidators;
        private readonly IModelValidator<ConfigItem> ConfigValidators;

        private IMediator _mediator;

        #endregion

        #region cst.

        public ConfigHandler(IConfigurationDataUnity DataHandler, 
                             IModelValidator<App> appsValidators, 
                             IModelValidator<Module> moduleValidators, 
                             IModelValidator<ConfigItem> configValidators,
                             IMediator mediator)
        {
            this.dataHandler = DataHandler;
            this.AppsValidators = appsValidators;
            this.ModuleValidators = moduleValidators;
            this.ConfigValidators = configValidators;
            this.ConfigValidators = configValidators;
            this._mediator = mediator;

            this.Initialized = Initialize();
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.AuditHandler.{Guid.NewGuid()}"; } }

        #endregion

        #region IConfigHandlerBase

        #region App

        public async Task<ExecutionResponse<SearchResults<App>>> Get(AppSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<App>>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var apps = await dataHandler.Apps.GetAsync(criteria, this.App_DataInclude_Full);
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
        public async Task<ExecutionResponse<App>> Create(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<App>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Apps.CreateAsync(app);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<App>> Edit(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<App>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await dataHandler.Apps.GetFirstAsync(x => x.Id == app.Id || x.Code == app.Code);
                MapUpdate(existing, app);

                dataHandler.Apps.Update(existing);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Apps.DeleteAsync(app);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<App>(this.AppsValidators, app, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> DeleteApp(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var app = await dataHandler.Apps.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Apps.DeleteAsync(app);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeleteApp(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var app = await dataHandler.Apps.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Apps.DeleteAsync(app);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateApp(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var app = await dataHandler.Apps.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Apps.SetActivationAsync(id, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateApp(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var app = await dataHandler.Apps.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Apps.SetActivationAsync(app.Id, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeactivateApp(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var app = await dataHandler.Apps.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Apps.SetActivationAsync(id, false);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeactivateApp(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var app = await dataHandler.Apps.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Apps.SetActivationAsync(app.Id, false);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> IsUnique(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Apps.AnyAsync(x => (x.Name == app.Name.Trim() ||
                                                                       x.NameCultured == app.NameCultured.Trim())
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
        public async Task<ExecutionResponse<bool>> IsExists(AppSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Apps.AnyAsync(criteria);
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
        #region Module

        public async Task<ExecutionResponse<SearchResults<Module>>> Get(ModuleSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Module>>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var modules = await dataHandler.Modules.GetAsync(criteria, this.Module_DataInclude_Full);
                return context.Response.Set(ResponseState.Success, modules);

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
        public async Task<ExecutionResponse<Module>> Create(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<Module>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Modules.CreateAsync(module);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, module);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Module>(this.ModuleValidators, module, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Module>> Edit(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<Module>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await dataHandler.Modules.GetFirstAsync(x => x.Id == module.Id || x.Code == module.Code);
                MapUpdate(existing, module);

                dataHandler.Modules.Update(existing);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Module>(this.ModuleValidators, module, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Modules.DeleteAsync(module);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Module>(this.ModuleValidators, module, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> DeleteModule(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var module = await dataHandler.Modules.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.ModuleValidators.ValidateAsync(module, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Modules.DeleteAsync(id);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeleteModule(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var module = await dataHandler.Modules.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ModuleValidators.ValidateAsync(module, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Modules.DeleteAsync(code);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateModule(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var module = await dataHandler.Modules.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.ModuleValidators.ValidateAsync(module, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Modules.SetActivationAsync(id, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateModule(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var module = await dataHandler.Modules.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ModuleValidators.ValidateAsync(module, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Modules.SetActivationAsync(code, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeactivateModule(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var module = await dataHandler.Modules.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.ModuleValidators.ValidateAsync(module, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region Logic

                await dataHandler.Modules.SetActivationAsync(id, false);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeactivateModule(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var module = await dataHandler.Modules.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ModuleValidators.ValidateAsync(module, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Modules.SetActivationAsync(code, false);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> IsUnique(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Modules.AnyAsync(x => (x.Name == module.Name.Trim() ||
                                                                    x.NameCultured == module.NameCultured.Trim() ||
                                                                    x.Code == module.Code.Trim())
                                                                    &&
                                                                    (x.Id != module.Id)
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
        public async Task<ExecutionResponse<bool>> IsExists(ModuleSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL


                var isExisting = await dataHandler.Modules.AnyAsync(criteria);
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
        #region Config

        public async Task<ExecutionResponse<SearchResults<ConfigItem>>> Get(ConfigSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<ConfigItem>>();
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

                var results = await this.dataHandler.Configs.GetAsync(criteria);
                return context.Response.Set(ResponseState.Success, results);

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
        public async Task<ExecutionResponse<ConfigItem>> Get(ConfigKey key, RequestContext requestContext)
        {
            var context = new ExecutionContext<ConfigItem>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var config = await dataHandler.Configs.GetFirstAsync(x => (x.Key == key.Key) &&
                                                                    (x.ModuleId == key.Module) &&
                                                                    (x.AppId == key.App), null, this.Config_DataInclude_Full);

                return context.Response.Set(ResponseState.Success, config);

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
        public async Task<ExecutionResponse<ConfigItem>> Create(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<ConfigItem>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Configs.CreateAsync(config);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, config);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(this.ConfigValidators, config, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<ConfigItem>> Edit(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<ConfigItem>();
            await context.Process(async () =>
            {
                #region Logic

                #region Events.

                //var externalValidationResponse = await Event_ConfigEditing(config, requestContext);
                //if (externalValidationResponse?.Result != true)
                //{
                //    return context.Response.Set(ResponseState.ValidationError, null, externalValidationResponse.DetailedMessages);
                //}

                #endregion
                #region DL

                var existing = await dataHandler.Configs.GetFirstAsync(x => x.Id == config.Id || x.Code == config.Code);
                MapUpdate(existing, config);

                dataHandler.Configs.Update(existing);
                await dataHandler.SaveAsync();

                #endregion
                #region Events.

                await Event_ConfigEdited(config, requestContext);

                #endregion

                return context.Response.Set(ResponseState.Success, existing);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(this.ConfigValidators, config, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Set(ConfigKey key, string value, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var config = await dataHandler.Configs.GetFirstAsync(x => (x.Key == key.Key) &&
                                                                    (x.ModuleId == key.Module) &&
                                                                    (x.AppId == key.App), null, this.Config_DataInclude_Full);
                if (config == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                config.Value = value;

                dataHandler.Configs.Update(config);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Configs.DeleteAsync(config);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(this.ConfigValidators, config, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> DeleteConfig(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var config = await dataHandler.Configs.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.ConfigValidators.ValidateAsync(config, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Configs.DeleteAsync(id);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeleteConfig(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var config = await dataHandler.Configs.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ConfigValidators.ValidateAsync(config, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Configs.DeleteAsync(code);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateConfig(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var config = await dataHandler.Configs.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.ConfigValidators.ValidateAsync(config, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Configs.SetActivationAsync(id, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateConfig(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var config = await dataHandler.Configs.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ConfigValidators.ValidateAsync(config, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Configs.SetActivationAsync(code, true);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeactivateConfig(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var config = await dataHandler.Configs.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.ConfigValidators.ValidateAsync(config, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Configs.SetActivationAsync(id, false);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(this.ConfigValidators, new ConfigItem() { Id = id }, ValidationMode.Deactivate),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> DeactivateConfig(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validation

                var config = await dataHandler.Configs.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.ConfigValidators.ValidateAsync(config, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Configs.SetActivationAsync(code, false);
                await dataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> IsUnique(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Configs.AnyAsync(x => (x.Name == config.Name.Trim() ||
                                                                          x.Code == config.Code.Trim() ||
                                                                          x.Key == config.Key.Trim())
                                                                         &&
                                                                         x.AppId == config.AppId &&
                                                                         x.ModuleId == config.ModuleId &&
                                                                         x.Id != config.Id &&
                                                                         x.IsActive == true);

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
        public async Task<ExecutionResponse<bool>> IsExists(ConfigSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Configs.AnyAsync(criteria);
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

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (dataHandler?.Initialized ?? false);
            isValid = isValid && _mediator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
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
                    "Modules.Configurations",
                });

                #endregion

                #region Module : basic

                this.Module_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Module : full

                this.Module_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "App",
                    "Configurations",
                });

                #endregion

                #region Config : basic

                this.Config_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Config : full

                this.Config_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "App",
                    "Module",
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

        #region MapUpdate.

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

            return true;
        }
        private bool MapUpdate(Module existing, Module updated)
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
            existing.AppId = updated.AppId;

            bool state = true;
            state = MapUpdate(existing.App, updated.App) && state;

            return state;
        }
        private bool MapUpdate(ConfigItem existing, ConfigItem updated)
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
            existing.Key = updated.Key;
            existing.ReadOnly = updated.ReadOnly;
            existing.Version = updated.Version;
            existing.Value = updated.Value;
            existing.Type = updated.Type;


            existing.AppId = updated.AppId;
            existing.ModuleId = updated.ModuleId;

            bool state = true;
            state = MapUpdate(existing.App, updated.App) && state;
            state = MapUpdate(existing.Module, updated.Module) && state;

            return state;
        }

        #endregion
        #region events.

        private async Task Event_ConfigEdited(ConfigItem config, RequestContext requestContext)
        {
            if (config == null) return;
            if (_mediator == null) return;

            var domainEvent = ConfigEditedDomainEventMapper.Instance.Map(config, requestContext);
            await _mediator.Publish(domainEvent);
        }
        private async Task<ExecutionResponse<bool>> Event_ConfigEditing(ConfigItem config, RequestContext requestContext)
        {
            if (config == null) return null;
            if (_mediator == null) return null;

            var domainEvent = ConfigEditingDomainEventMapper.Instance.Map(config, requestContext);
            return await _mediator.Send(domainEvent);
        }

        #endregion

        #endregion
    }
}
