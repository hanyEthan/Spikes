using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Config.Core.Contracts;
using XCore.Services.Config.Core.DataLayer.Unity;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;
using XCore.Services.Config.Core.Validators;

namespace XCore.Services.Config.Core.Handlers
{
    public class ConfigHandler : IConfigHandlerBase
    {
        #region props.

        private string App_DataInclude_Basic { get; set; }
        private string App_DataInclude_Full { get; set; }

        private string Module_DataInclude_Basic { get; set; }
        private string Module_DataInclude_Full { get; set; }

        private string Config_DataInclude_Full { get; set; }
        private string Config_DataInclude_Basic { get; set; }

        #endregion
        #region cst.

        public ConfigHandler()
        {
            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.ConfigHandler.{Guid.NewGuid()}"; } }

        #endregion

        #region IConfigHandlerBase

        #region App

        public ExecutionResponse<SearchResults<App>> Get(AppSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<App>>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var apps = dataHandler.Apps.Get(criteria, this.App_DataInclude_Full);
                    return context.Response.Set(ResponseState.Success, apps);
                }

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
        public ExecutionResponse<App> Create(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<App>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    dataHandler.Apps.Create(app);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, app);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<App>(new AppsValidators(), app, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<App> Edit(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<App>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var existing = dataHandler.Apps.GetFirst(x => x.Id == app.Id || x.Code == app.Code);
                    MapUpdate(existing, app);

                    dataHandler.Apps.Update(existing);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, existing);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<App>(new AppsValidators(), app, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<bool> Delete(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    dataHandler.Apps.Delete(app);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<App>(new AppsValidators(), app, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<bool> DeleteApp(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var app = dataHandler.Apps.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new AppsValidators();
                    var validationResponse = validator.Validate(app, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    dataHandler.Apps.Delete(app);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeleteApp(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var app = dataHandler.Apps.GetFirst(x => x.Code == code);

                    #region validation
                    var validator = new AppsValidators();
                    var validationResponse = validator.Validate(app, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    dataHandler.Apps.Delete(app);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> ActivateApp(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var app = dataHandler.Apps.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new AppsValidators();
                    var validationResponse = validator.Validate(app, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    dataHandler.Apps.SetActivation(id, true);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> ActivateApp(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var app = dataHandler.Apps.GetFirst(x => x.Code == code);

                    #region validation

                    var validator = new AppsValidators();
                    var validationResponse = validator.Validate(app, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    dataHandler.Apps.SetActivation(app.Id, true);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeactivateApp(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var app = dataHandler.Apps.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new AppsValidators();
                    var validationResponse = validator.Validate(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    dataHandler.Apps.SetActivation(id, false);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeactivateApp(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var app = dataHandler.Apps.GetFirst(x => x.Code == code);

                    #region validation

                    var validator = new AppsValidators();
                    var validationResponse = validator.Validate(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    dataHandler.Apps.SetActivation(app.Id, false);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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

        public ExecutionResponse<bool> IsUnique(App app, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var isExisting = dataHandler.Apps.Any(x => (x.Name == app.Name.Trim() ||
                                                                x.NameCultured == app.NameCultured.Trim()
                                                                )
                                                                &&
                                                                (x.Code != app.Code)
                                                                &&
                                                                (x.IsActive == true));

                    return context.Response.Set(ResponseState.Success, isExisting);
                }

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
        public ExecutionResponse<bool> IsExists(AppSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var isExisting = dataHandler.Apps.Any(criteria);
                    return context.Response.Set(ResponseState.Success, isExisting);
                }

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

        public ExecutionResponse<SearchResults<Module>> Get(ModuleSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Module>>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var modules = dataHandler.Modules.Get(criteria, this.Module_DataInclude_Full);
                    return context.Response.Set(ResponseState.Success, modules);
                }

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
        public ExecutionResponse<Module> Create(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<Module>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    dataHandler.Modules.Create(module);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, module);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Module>(new ModuleValidators(), module, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<Module> Edit(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<Module>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var existing = dataHandler.Modules.GetFirst(x => x.Id == module.Id || x.Code == module.Code);
                    MapUpdate(existing, module);

                    dataHandler.Modules.Update(existing);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, existing);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Module>(new ModuleValidators(), module, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<bool> Delete(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    dataHandler.Modules.Delete(module);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Module>(new ModuleValidators(), module, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<bool> DeleteModule(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var module = dataHandler.Modules.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new ModuleValidators();
                    var validationResponse = validator.Validate(module, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Modules.Delete(id);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeleteModule(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var module = dataHandler.Modules.GetFirst(x => x.Code == code);

                    #region validation
                    var validator = new ModuleValidators();
                    var validationResponse = validator.Validate(module, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Modules.Delete(code);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> ActivateModule(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var module = dataHandler.Modules.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new ModuleValidators();
                    var validationResponse = validator.Validate(module, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Modules.SetActivation(id, true);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> ActivateModule(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var module = dataHandler.Modules.GetFirst(x => x.Code == code);

                    #region validation

                    var validator = new ModuleValidators();
                    var validationResponse = validator.Validate(module, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Modules.SetActivation(code, true);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeactivateModule(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var module = dataHandler.Modules.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new ModuleValidators();
                    var validationResponse = validator.Validate(module, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Modules.SetActivation(id, false);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeactivateModule(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var module = dataHandler.Modules.GetFirst(x => x.Code == code);

                    #region validation

                    var validator = new ModuleValidators();
                    var validationResponse = validator.Validate(module, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Modules.SetActivation(code, false);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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

        public ExecutionResponse<bool> IsUnique(Module module, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {

                    var isExisting = dataHandler.Modules.Any(x => (x.Name == module.Name.Trim() ||
                                                             x.NameCultured == module.NameCultured.Trim() ||
                                                             x.Code == module.Code.Trim())
                                                             &&
                                                             (x.Id != module.Id)
                                                             &&
                                                             (x.IsActive == true));

                    return context.Response.Set(ResponseState.Success, isExisting);
                }

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
        public ExecutionResponse<bool> IsExists(ModuleSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var isExisting = dataHandler.Modules.Any(criteria);
                    return context.Response.Set(ResponseState.Success, isExisting);
                }

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

        public ExecutionResponse<ConfigItem> Get(ConfigKey key, RequestContext requestContext)
        {
            var context = new ExecutionContext<ConfigItem>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => (x.Key == key.Key) &&
                                                                   (x.ModuleId == key.Module) &&
                                                                   (x.AppId == key.App), null, this.Config_DataInclude_Full);

                    return context.Response.Set(ResponseState.Success, config);
                }

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
        public ExecutionResponse<SearchResults<ConfigItem>> Get(ConfigSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<ConfigItem>>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var modules = dataHandler.Configs.Get(criteria, this.Config_DataInclude_Basic);
                    return context.Response.Set(ResponseState.Success, modules);
                }

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
        public ExecutionResponse<ConfigItem> Create(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<ConfigItem>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    dataHandler.Configs.Create(config);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, config);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(new ConfigValidators(), config, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<ConfigItem> Edit(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<ConfigItem>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var existing = dataHandler.Configs.GetFirst(x => x.Id == config.Id || x.Code == config.Code);
                    MapUpdate(existing, config);

                    dataHandler.Configs.Update(existing);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, existing);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(new ConfigValidators(), config, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<bool> Set(ConfigKey key, string value, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => (x.Key == key.Key) &&
                                               (x.ModuleId == key.Module) &&
                                               (x.AppId == key.App), null, this.Config_DataInclude_Full);
                    if (config == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    config.Value = value;

                    dataHandler.Configs.Update(config);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> Delete(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    dataHandler.Configs.Delete(config);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(new ConfigValidators(), config, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<bool> DeleteConfig(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new ConfigValidators();
                    var validationResponse = validator.Validate(config, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    dataHandler.Configs.Delete(id);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeleteConfig(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => x.Code == code);

                    #region validation
                    var validator = new ConfigValidators();
                    var validationResponse = validator.Validate(config, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Configs.Delete(code);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> ActivateConfig(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new ConfigValidators();
                    var validationResponse = validator.Validate(config, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Configs.SetActivation(id, true);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> ActivateConfig(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => x.Code == code);

                    #region validation

                    var validator = new ConfigValidators();
                    var validationResponse = validator.Validate(config, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Configs.SetActivation(code, true);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> DeactivateConfig(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => x.Id == id);

                    #region validation

                    var validator = new ConfigValidators();
                    var validationResponse = validator.Validate(config, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Configs.SetActivation(id, false);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<ConfigItem>(new ConfigValidators(), new ConfigItem() { Id = id }, ValidationMode.Deactivate),
            });
            return context.Response;

            #endregion
        }
        public ExecutionResponse<bool> DeactivateConfig(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var config = dataHandler.Configs.GetFirst(x => x.Code == code);

                    #region validation

                    var validator = new ConfigValidators();
                    var validationResponse = validator.Validate(config, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    dataHandler.Configs.SetActivation(code, false);
                    dataHandler.Save();

                    return context.Response.Set(ResponseState.Success, true);
                }

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
        public ExecutionResponse<bool> IsUnique(ConfigItem config, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL

                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var isExisting = dataHandler.Configs.Any(x => (x.Name == config.Name.Trim() ||
                                                                   x.Code == config.Code.Trim() ||
                                                                   x.Key == config.Key.Trim())
                                                                   &&
                                                                   x.AppId == config.AppId &&
                                                                   x.ModuleId == config.ModuleId &&

                                                                   (x.Id != config.Id)
                                                                   &&
                                                                    (x.IsActive == true));

                    return context.Response.Set(ResponseState.Success, isExisting);
                }

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
        public ExecutionResponse<bool> IsExists(ConfigSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Process(() =>
            {
                #region Logic

                #region DL
                using (var dataHandler = new ConfigDataUnity<ConfigDataUnitySettings>())
                {
                    var isExisting = dataHandler.Configs.Any(criteria);
                    return context.Response.Set(ResponseState.Success, isExisting);
                }
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
                    "App"
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
    }
}
