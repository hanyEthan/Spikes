using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Personnel.Core.Contracts.Accounts;
using XCore.Services.Personnel.DataLayer.Contracts;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.Core.Handlers.Accounts
{

    public class OrganizationAccountHandler : IOrganizationAccountHandler
    {
        #region props.
        private string Account_DataInclude_Basic { get; set; }
        private string Account_DataInclude_Search { get; set; }
        private string Account_DataInclude_Full { get; set; }
        private readonly IModelValidator<OrganizationAccount> OrganizationAccountValidator;
        private readonly IPersonnelDataUnity dataHandler;
        #endregion
        #region cst.

        public OrganizationAccountHandler(IPersonnelDataUnity DataHandler, IModelValidator<OrganizationAccount> OrganizationAccountValidator)
        {
            this.dataHandler = DataHandler;
            this.OrganizationAccountValidator = OrganizationAccountValidator;

            this.Initialized = Initialize();
        }
        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.OrganizationAccountHandler.{Guid.NewGuid()}"; } }

        #endregion

        #region IAccountHandler
        public async Task<ExecutionResponse<SearchResults<OrganizationAccount>>> Get(OrganizationAccountSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<OrganizationAccount>>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    var AccountBases = await dataHandler.OrganizationAccount.GetAsync(criteria, GetIncludes(criteria.SearchIncludes));
                    return context.Response.Set(ResponseState.Success, AccountBases);

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

      

        public async Task<ExecutionResponse<OrganizationAccount>> Create(OrganizationAccount Account, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<OrganizationAccount>();
                await context.Process(async () =>
                {
                    
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region validate.

                    if (!this.Initialized.GetValueOrDefault())
                    {
                        return context.Response.Set(ResponseState.Error, null);
                    }

                    #endregion
                    #region DL

                    await dataHandler.OrganizationAccount.CreateAsync(Account);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, Account);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<OrganizationAccount>(this.OrganizationAccountValidator, Account, ValidationMode.Create),
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
        public async Task<ExecutionResponse<OrganizationAccount>> Edit(OrganizationAccount Account, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<OrganizationAccount>();
                await context.Process(async () =>
                {

                    #region Logic
                    #region check.

                    Check();

                    #endregion
                   
                    #region DL

                    var existing = await dataHandler.OrganizationAccount.GetFirstAsync(x => x.Id == Account.Id || x.Code == Account.Code, null, this.Account_DataInclude_Full);
                    MapUpdate(existing, Account, out List<int> addedSettings, out List<int> removedSettings);

                    dataHandler.OrganizationAccount.Update(existing);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<OrganizationAccount>(this.OrganizationAccountValidator, Account, ValidationMode.Edit),
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
        public async Task<ExecutionResponse<bool>> Delete(OrganizationAccount Account, RequestContext requestContext)
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

                    await dataHandler.OrganizationAccount.DeleteAsync(Account);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<OrganizationAccount>(this.OrganizationAccountValidator, Account, ValidationMode.Delete),
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
                    #region check.

                    Check();

                    #endregion
                   
                    #region validate.

                    var app = await dataHandler.OrganizationAccount.GetFirstAsync(x => x.Id == id);
                    var validationResponse = await this.OrganizationAccountValidator.ValidateAsync(app, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion
                    #region DL

                    await dataHandler.OrganizationAccount.DeleteAsync(app);
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
        public async Task<ExecutionResponse<bool>> IsUnique(OrganizationAccount Account, RequestContext requestContext)
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

                    var isExisting = await dataHandler.OrganizationAccount.AnyAsync(x => (x.Name == Account.Name.Trim() ||
                                                                           x.NameCultured == Account.NameCultured.Trim())
                                                                           &&
                                                                           (x.Code != Account.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(OrganizationAccountSearchCriteria criteria, RequestContext requestContext)
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

                    var isExisting = await dataHandler.OrganizationAccount.AnyAsync(criteria);
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
            isValid = isValid && (OrganizationAccountValidator != null);
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }

        private bool InitializeModelIncludes()
        {
            try
            {
                #region Account : basic

                this.Account_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Account : Search

                this.Account_DataInclude_Search = string.Join(",", new List<string>()
                {
                });

                #endregion

                #region Account : full

                this.Account_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "Organization",
                    "Settings"
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
        private bool MapUpdate(OrganizationAccount existing, OrganizationAccount updated, out List<int> addedSettings, out List<int> removedSettings)
        {
            addedSettings = new List<int>();
            removedSettings = new List<int>();
            if (updated == null || existing == null) return false;
            existing.OrganizationId = updated.OrganizationId;
            #region Common
            existing.Code = updated.Code;
            existing.IsActive = updated.IsActive;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            #endregion
            bool state = true;

            state = MapUpdate(existing.Settings, updated.Settings, out addedSettings, out removedSettings) && state;

            return true;
        }
        private bool MapUpdate(IList<Setting> existing, IList<Setting> updated, out List<int> addedSettings, out List<int> removedSettings)
        {
            // ...
            addedSettings = new List<int>();
            removedSettings = new List<int>();

            // ...
            if (updated == null || existing == null) return false;

            // get [deleted records] ...
            foreach (var existingItem in existing)
            {
                var isDeleted = !updated.Any(x => x.Id == existingItem.Id
                                               && x.Id == existingItem.Id);

                // delete the [existingItem]
                if (isDeleted)
                {
                    this.dataHandler.Setting.DeleteAsync(existingItem);
                    existing.Remove(existingItem);
                    removedSettings.Add(existingItem.Id);
                }
            }

            // get [inserted records] ...
            foreach (var updatedItem in updated)
            {
                var isInserted = !existing.Any(x => x.Id == updatedItem.Id
                                                 && x.Id == updatedItem.Id);

                // insert the [updatedItem]
                if (isInserted)
                {
                    existing.Add(updatedItem);
                    addedSettings.Add(updatedItem.Id);
                }
            }

            return true;
        }
        private string GetIncludes(SearchIncludesEnum searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludesEnum.Search:
                    return this.Account_DataInclude_Search;
                case SearchIncludesEnum.Full:
                    return this.Account_DataInclude_Full;
                case SearchIncludesEnum.Basic:
                default:
                    return this.Account_DataInclude_Basic;
            }
        }
        #endregion
    }
}
