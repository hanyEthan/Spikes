using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.ModelBinding;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Config.API.Mappers;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;
using XCore.Services.Config.Core.Unity;

namespace XCore.Services.Config.API.APIs
{
    public class ConfigurationsAPI : NancyModule
    {
        #region props.
        
        public bool Initialized { get; private set; }
        private AppMapper AppMapper { get; set; }
        
        private AppSearchCriteriaMapper AppSearchCriteriaMapper { get; set; }
        private AppSearchResultsMapper AppSearchResultsMapper { get; set; }

        private ModuleMapper ModuleMapper { get; set; }
        private ModuleSearchCriteriaMapper ModuleSearchCriteriaMapper { get; set; }
        private ModuleSearchResultsMapper ModuleSearchResultsMapper { get; set; }

        private ConfigMapper ConfigMapper { get; set; }

        private ConfigSearchCriteriaMapper ConfigSearchCriteriaMapper { get; set; }
        private ConfigSearchResultsMapper ConfigSearchResultsMapper { get; set; }

        private NativeMapper<int> IntMapper { get; set; }
        private NativeMapper<bool> BoolMapper { get; set; }
        private NativeMapper<string> stringMapper { get; set; }

        private NativeMapper <ConfigSetRequest> ConfigSetRequestMapper { get; set; }

        #endregion
        #region cst.

        public ConfigurationsAPI(ILoggerFactory loggerFactory)
        {
            Initialized = Initialize();
        }

        #endregion

        #region API.

        #region App : Create

        private void AppCreate()
        {
            Post("/App/Create/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<AppDTO>>();
                    var responseDTO = AppCreate(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<AppDTO> AppCreate(ServiceExecutionRequestDTO<AppDTO> request)
        {
            string serviceName = "xcore.config.apps.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<AppDTO> requestDTO = request;
                ServiceExecutionRequest<App> requestDMN;
                ServiceExecutionResponse<App> responseDMN;
                ServiceExecutionResponseDTO<AppDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppDTO, App, AppDTO, App>(requestDTO, this.AppMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AppDTO, App, AppDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.AppMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AppDTO, AppDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region App : Edit
        private void AppEdit()
        {
            Put("/App/Edit/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<AppDTO>>();
                    var responseDTO = AppEdit(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<AppDTO> AppEdit(ServiceExecutionRequestDTO<AppDTO> request)
        {
            string serviceName = "xcore.config.apps.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<AppDTO> requestDTO = request;
                ServiceExecutionRequest<App> requestDMN;
                ServiceExecutionResponse<App> responseDMN;
                ServiceExecutionResponseDTO<AppDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppDTO, App, AppDTO, App>(requestDTO, this.AppMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AppDTO, App, AppDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.AppMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AppDTO, AppDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region App : Delete
        private void AppDelete()
        {
            Post("/App/Delete/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = AppDelete(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> AppDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.apps.delete";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>() ;
                ServiceExecutionResponseDTO<bool> responseDTO;
              
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool,bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.DeleteApp(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region App : Get

        private void AppGet()
        {
            Get("/App/get/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<AppSearchCriteriaDTO>>();
                    var responseDTO = AppGet(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>> AppGet(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.config.apps.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<AppSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<AppSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<App>> responseDMN = new ServiceExecutionResponse<SearchResults<App>>() ;
                ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppSearchCriteriaDTO, AppSearchCriteria, SearchResultsDTO<AppDTO>, SearchResults<App>>(requestDTO, this.AppSearchCriteriaMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AppSearchCriteriaDTO, SearchResults<App>, SearchResultsDTO<AppDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.AppSearchResultsMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AppSearchCriteriaDTO, SearchResultsDTO<AppDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region App : Activate

        private void ActivateApp()
        {
            Put("/App/Activate/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = ActivateApp(requestDTO);
                    return responseDTO;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> ActivateApp(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.apps.activate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>(); 
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.ActivateApp(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region App : DeActivate

        private void DeActivateApp()
        {
            Put("/App/DeActivate/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = DeActivateApp(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> DeActivateApp(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.apps.deactivate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>(); 
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.DeactivateApp(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion

        #region Module : Create

        private void ModuleCreate()
        {
            Post("/Module/Create/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<ModuleDTO>>();
                    var responseDTO = ModuleCreate(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<ModuleDTO> ModuleCreate(ServiceExecutionRequestDTO<ModuleDTO> request)
        {
            string serviceName = "xcore.config.module.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ModuleDTO> requestDTO = request;
                ServiceExecutionRequest<Module> requestDMN;
                ServiceExecutionResponse<Module> responseDMN;
                ServiceExecutionResponseDTO<ModuleDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ModuleDTO, Module, ModuleDTO, Module>(requestDTO, this.ModuleMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ModuleDTO, Module, ModuleDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.ModuleMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ModuleDTO, ModuleDTO>(request, serviceName);
            }

            #endregion
        }
        #endregion
        #region Module : Edit

        private void ModuleEdit()
        {
            Put("/Module/Edit/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<ModuleDTO>>();
                    var responseDTO = ModuleEdit(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<ModuleDTO> ModuleEdit(ServiceExecutionRequestDTO<ModuleDTO> request)
        {
            string serviceName = "xcore.config.module.edit";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ModuleDTO> requestDTO = request;
                ServiceExecutionRequest<Module> requestDMN;
                ServiceExecutionResponse<Module> responseDMN;
                ServiceExecutionResponseDTO<ModuleDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ModuleDTO, Module, ModuleDTO, Module>(requestDTO, this.ModuleMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ModuleDTO, Module, ModuleDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.ModuleMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ModuleDTO, ModuleDTO>(request, serviceName);
            }

            #endregion
        }
        #endregion
        #region Module : Delete
        private void ModuleDelete()
        {
            Delete("/Module/Delete/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = ModuleDelete(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> ModuleDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.module.delete";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = null;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.DeleteModule(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Module : Activate

        private void ActivateModule()
        {
            Put("/Module/ActivateModule/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = ActivateModule(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> ActivateModule(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.modules.activate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = null;
                ServiceExecutionResponseDTO<bool> responseDTO;
                int x = 0;
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.ActivateModule(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Module : DeActivate

        private void DeActivateModule()
        {
            Put("/Module/DeActivateModule/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = DeActivateModule(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> DeActivateModule(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.modules.deactivate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = null;
                ServiceExecutionResponseDTO<bool> responseDTO;
                int x = 0;
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.DeactivateModule(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Module : Get

        private void ModuleGet()
        {
            Get("/Module/get/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO>>();
                    var responseDTO = ModuleGet(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>> ModuleGet(ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.config.modules.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<ModuleSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Module>> responseDMN = null;
                ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ModuleSearchCriteriaDTO, ModuleSearchCriteria, SearchResultsDTO<ModuleDTO>, SearchResults<Module>>(requestDTO, this.ModuleSearchCriteriaMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ModuleSearchCriteriaDTO, SearchResults<Module>, SearchResultsDTO<ModuleDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.ModuleSearchResultsMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ModuleSearchCriteriaDTO, SearchResultsDTO<ModuleDTO>>(request, serviceName);
            }
            #endregion
        }
       
        #endregion

        #region Config : Create

        private void ConfigCreate()
        {
            Post("/Config/Create/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<ConfigDTO>>();
                    var responseDTO = ModuleCreate(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<ConfigDTO> ModuleCreate(ServiceExecutionRequestDTO<ConfigDTO> request)
        {
            string serviceName = "xcore.config.config.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigDTO> requestDTO = request;
                ServiceExecutionRequest<ConfigItem> requestDMN;
                ServiceExecutionResponse<ConfigItem> responseDMN;
                ServiceExecutionResponseDTO<ConfigDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigDTO, ConfigItem, ConfigDTO, ConfigItem>(requestDTO, this.ConfigMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigDTO, ConfigItem, ConfigDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.ConfigMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigDTO, ConfigDTO>(request, serviceName);
            }

            #endregion
        }
        #endregion
        #region Config : Edit

        private void ConfigEdit()
        {
            Put("/Config/Edit/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<ConfigDTO>>();
                    var responseDTO = ConfigEdit(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<ConfigDTO> ConfigEdit(ServiceExecutionRequestDTO<ConfigDTO> request)
        {
            string serviceName = "xcore.config.config.edit";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigDTO> requestDTO = request;
                ServiceExecutionRequest<ConfigItem> requestDMN;
                ServiceExecutionResponse<ConfigItem> responseDMN;
                ServiceExecutionResponseDTO<ConfigDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigDTO, ConfigItem, ConfigDTO, ConfigItem>(requestDTO, this.ConfigMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigDTO, ConfigItem, ConfigDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.ConfigMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigDTO, ConfigDTO>(request, serviceName);
            }

            #endregion
        }
        #endregion
        #region Config : Delete
        private void ConfigDelete()
        {
            Delete("/Config/Delete/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = ConfigDelete(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> ConfigDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.config.delete";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = null;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.DeleteConfig(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Config : Activate

        private void ActivateConfig()
        {
            Put("/Config/ActivateConfig/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = ActivateConfig(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> ActivateConfig(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.config.activate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = null;
                ServiceExecutionResponseDTO<bool> responseDTO;
                int x = 0;
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.ActivateConfig(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Config : DeActivate

        private void DeActivateConfig()
        {
            Put("/Config/DeActivateConfig/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<string>>();
                    var responseDTO = DeActivateConfig(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> DeActivateConfig(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.config.deactivate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = null;
                ServiceExecutionResponseDTO<bool> responseDTO;
                int x = 0;
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, this.stringMapper, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.DeactivateConfig(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Config : Get

        private void ConfigGet()
        {
            Post("/Config/get/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO>>();
                    var responseDTO = ConfigGet(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<SearchResultsDTO<ConfigDTO>> ConfigGet(ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.config.config.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<ConfigSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<ConfigItem>> responseDMN = null;
                ServiceExecutionResponseDTO<SearchResultsDTO<ConfigDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigSearchCriteriaDTO, ConfigSearchCriteria, SearchResultsDTO<ConfigDTO>, SearchResults<ConfigItem>>(requestDTO, this.ConfigSearchCriteriaMapper, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                //var domainResponse = ConfigUnity.Configs.Get(requestDMN.Content, requestDMN.ToRequestContext());

                var domainResponse = new ExecutionResponse<SearchResults<ConfigItem>>()
                {
                    State = Framework.Infrastructure.Context.Execution.Models.ResponseState.Success,
                    Result = new SearchResults<ConfigItem>()
                    {
                        Results = new List<ConfigItem>()
                        {
                            new ConfigItem()
                            {
                                AppId = 1,
                                ModuleId = 1,
                                Key = "XCore.Services.Audit.Endpoint",
                                Value = "http://localhost:14170/",
                                CreatedDate = DateTime.Now,
                            }
                        }
                    }
                };

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigSearchCriteriaDTO, SearchResults<ConfigItem>, SearchResultsDTO<ConfigDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.ConfigSearchResultsMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigSearchCriteriaDTO, SearchResultsDTO<ConfigDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Config : Set

        private void ConfigSet()
        {
            Post("/Config/Set/", args =>
            {
                try
                {
                    var requestDTO = this.Bind<ServiceExecutionRequestDTO<ConfigSetRequest>>();

                    var responseDTO = ConfigSet(requestDTO);
                    return responseDTO;
                }
                catch (System.Exception)
                {
                    throw;
                }
            });
        }
        private ServiceExecutionResponseDTO<bool> ConfigSet(ServiceExecutionRequestDTO<ConfigSetRequest> request)
        {
            string serviceName = "xcore.config.apps.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigSetRequest> requestDTO = request;
                ServiceExecutionRequest<ConfigSetRequest> requestDMN;

                

                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigSetRequest, ConfigSetRequest, bool, bool>(requestDTO,this.ConfigSetRequestMapper, out requestDMN, out bool isValidRequest);
                
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = ConfigUnity.Configs.Set(requestDMN.Content.key, requestDMN.Content.value, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigSetRequest, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, this.BoolMapper, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigSetRequest, bool>(request, serviceName);
            }

            #endregion
        }
        #endregion


       


        public IList<app> gridtest()
        {

            List<config> configlist = new List<config>() {
                new config() { id = 1, name = "config1", code = "config1" },
                new config() { id = 2, name = "config2", code = "config2" },
                new config() { id = 3, name = "config3", code = "config3" },
                new config() { id = 4, name = "config4", code = "config4" },
                new config() { id = 5, name = "config5", code = "config5" }
            };




            List<module> modulelist = new List<module>() {
                new module() { id = 1, name = "module1", code = "module1",config=configlist},
                new module() { id = 2, name = "module2", code = "module2",config=configlist },
                new module() { id = 3, name = "module3", code = "module3",config=configlist },
                new module() { id = 4, name = "module4", code = "module4",config=configlist },
                new module() { id = 5, name = "module5", code = "module5",config=configlist }
            };

          


            List<app> applist = new List<app>() {
                new app() { id = 1, name = "app1", code = "app1",module=modulelist },
                new app() { id = 2, name = "app2", code = "app2",module=modulelist },
                new app() { id = 3, name = "app3", code = "app3",module=modulelist },
                new app() { id = 4, name = "app4", code = "app4",module=modulelist },
                new app() { id = 5, name = "app5", code = "app5",module=modulelist }
            };

          
            return applist;

        }


       

        public class app
        {
            public int id;
            public string name;
            public string code;
            public List<module> module;

        }
        public class module
        {
            public int id;
            public string name;
            public string code;
            public List<config> config;

        }
        public class config
        {
            public int id;
            public string name;
            public string code;
            

        }




        #endregion
        #region helpers.

        private bool Initialize()
        {
            // api's
            AppCreate();
            AppEdit();
            AppDelete();
            AppGet();
            ActivateApp();
            DeActivateApp();


            //modules
            ModuleCreate();
            ModuleEdit();
            ModuleDelete();
            ActivateModule();
            DeActivateModule();
            ModuleGet();

            //configs
            ConfigCreate();
            ConfigEdit();
            ConfigDelete();
            ActivateConfig();
            DeActivateConfig();
            ConfigGet();
            ConfigSet();

            // mappers
            this.AppMapper = new AppMapper();
            this.AppSearchCriteriaMapper = new AppSearchCriteriaMapper();
            this.AppSearchResultsMapper = new AppSearchResultsMapper();

            this.ModuleMapper = new ModuleMapper();
            this.ModuleSearchCriteriaMapper = new ModuleSearchCriteriaMapper();
            this.ModuleSearchResultsMapper = new ModuleSearchResultsMapper();

            this.ConfigMapper = new ConfigMapper();
            this.ConfigSearchCriteriaMapper = new ConfigSearchCriteriaMapper();
            this.ConfigSearchResultsMapper = new ConfigSearchResultsMapper();

            this.IntMapper = NativeMapper<int>.Instance;
            this.BoolMapper = NativeMapper<bool>.Instance;
            this.stringMapper = NativeMapper<string>.Instance;
            this.ConfigSetRequestMapper = NativeMapper<ConfigSetRequest>.Instance;

            this.ConfigSearchResultsMapper = new ConfigSearchResultsMapper();

            // ...
            return true;
        }

        #endregion
    }
}
