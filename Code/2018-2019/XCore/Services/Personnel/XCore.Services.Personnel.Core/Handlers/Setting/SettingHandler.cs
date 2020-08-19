using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Personnel.Core.Contracts.Settings;
using XCore.Services.Personnel.DataLayer.Contracts;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.Core.Handlers.Settings
{

    public class SettingHandler : ISettingHandler
    {
        #region props.
        private string Setting_DataInclude_Basic { get; set; }
        private string Setting_DataInclude_Search { get; set; }
        private string Setting_DataInclude_Full { get; set; }
        private readonly IModelValidator<Setting> SettingModelValidator;
        private readonly IPersonnelDataUnity dataHandler;
        #endregion
        #region cst.
        public SettingHandler(IPersonnelDataUnity DataHandler, IModelValidator<Setting> SettingModelValidator)
        {
            this.dataHandler = DataHandler;
            this.SettingModelValidator = SettingModelValidator;

            this.Initialized = Initialize();
        }
        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SettingHandler.{Guid.NewGuid()}"; } }

        #endregion

        #region ISettingHandler

        public async Task<ExecutionResponse<SearchResults<Setting>>> Get(SettingSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Setting>>();
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

                    var SettingModels = await dataHandler.Setting.GetAsync(criteria, GetIncludes(criteria.SearchIncludes));
                    return context.Response.Set(ResponseState.Success, SettingModels);

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
        public async Task<ExecutionResponse<Setting>> Create(Setting Setting, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Setting>();
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

                    await dataHandler.Setting.CreateAsync(Setting);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, Setting);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Setting>(this.SettingModelValidator, Setting, ValidationMode.Create),
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
        public async Task<ExecutionResponse<Setting>> Edit(Setting Setting, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Setting>();
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

                    var existing = await dataHandler.Setting.GetFirstAsync(x => x.Id == Setting.Id || x.Code == Setting.Code, null, this.Setting_DataInclude_Full);
                    MapUpdate(existing, Setting);

                    dataHandler.Setting.Update(existing);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Setting>(this.SettingModelValidator, Setting, ValidationMode.Edit),
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
        public async Task<ExecutionResponse<bool>> Delete(Setting Setting, RequestContext requestContext)
        {
            try
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

                    await dataHandler.Setting.DeleteAsync(Setting);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Setting>(this.SettingModelValidator, Setting, ValidationMode.Delete),
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
        public async Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext)
        {
            try
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
                    var app = await dataHandler.Setting.GetFirstAsync(x => x.Id == id);
                    var validationResponse = await this.SettingModelValidator.ValidateAsync(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    #region DL

                    await dataHandler.Setting.DeleteAsync(app);
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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> IsUnique(Setting Setting, RequestContext requestContext)
        {
            try
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

                    var isExisting = await dataHandler.Setting.AnyAsync(x => (x.Name == Setting.Name.Trim() ||
                                                                           x.NameCultured == Setting.NameCultured.Trim())
                                                                           &&
                                                                           (x.Code != Setting.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(SettingSearchCriteria criteria, RequestContext requestContext)
        {
            try
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

                    var isExisting = await dataHandler.Setting.AnyAsync(criteria);
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

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (dataHandler?.Initialized ?? false);
            isValid = isValid && (SettingModelValidator != null);
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Setting : basic

                this.Setting_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Setting : Search

                this.Setting_DataInclude_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Setting : full

                this.Setting_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "Account",
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
        private bool MapUpdate(Setting existing, Setting updated)
        {
            if (updated == null || existing == null) return false;

            existing.AccountId = updated.AccountId;
            #region Common
            existing.Code = updated.Code;
            existing.IsActive = updated.IsActive;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            #endregion
            return true;
        }
        private string GetIncludes(SearchIncludesEnum searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludesEnum.Search:
                    return this.Setting_DataInclude_Search;
                case SearchIncludesEnum.Full:
                    return this.Setting_DataInclude_Full;
                case SearchIncludesEnum.Basic:
                default:
                    return this.Setting_DataInclude_Basic;
            }
        }
        #endregion
    }
}
