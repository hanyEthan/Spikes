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
using XCore.Services.Personnel.Core.Contracts.Organizations;
using XCore.Services.Personnel.DataLayer.Contracts;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.Organizations;

namespace XCore.Services.Personnel.Core.Handlers.Organizations
{
    public class OrganizationHandler : IOrganizationHandler
    {
        #region props.
        
        private string Organization_DataInclude_Basic { get; set; }
        private string Organization_DataInclude_Search { get; set; }
        private string Organization_DataInclude_Full { get; set; }

        private readonly IModelValidator<Organization> OrganizationModelValidator;
        private readonly IPersonnelDataUnity dataHandler;
        #endregion
        #region cst.
        public OrganizationHandler(IPersonnelDataUnity DataHandler, IModelValidator<Organization> OrganizationModelValidator)
        {
            this.dataHandler = DataHandler;
            this.OrganizationModelValidator = OrganizationModelValidator;

            this.Initialized = Initialize();
        }
        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.OrganizationHandler.{Guid.NewGuid()}"; } }

        #endregion

        #region IOrganizationHandler

        public async Task<ExecutionResponse<SearchResults<Organization>>> Get(OrganizationSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Organization>>();
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

                    var OrganizationModels = await dataHandler.Organization.GetAsync(criteria, GetIncludes(criteria.SearchIncludes));
                    return context.Response.Set(ResponseState.Success, OrganizationModels);

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
        public async Task<ExecutionResponse<Organization>> Create(Organization Organization, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Organization>();
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

                    await dataHandler.Organization.CreateAsync(Organization);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, Organization);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Organization>(this.OrganizationModelValidator, Organization, ValidationMode.Create),
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
        public async Task<ExecutionResponse<Organization>> Edit(Organization Organization, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Organization>();
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

                    var existing = await dataHandler.Organization.GetFirstAsync(x => x.Id == Organization.Id || x.Code == Organization.Code, null, this.Organization_DataInclude_Full);
                    MapUpdate(existing, Organization);

                    dataHandler.Organization.Update(existing);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Organization>(this.OrganizationModelValidator, Organization, ValidationMode.Edit),
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
        public async Task<ExecutionResponse<bool>> Delete(Organization Organization, RequestContext requestContext)
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

                    await dataHandler.Organization.DeleteAsync(Organization);
                    await dataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Organization>(this.OrganizationModelValidator, Organization, ValidationMode.Delete),
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

            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic
                #region check.

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, false);
                }

                #endregion
                #region validate.

                var app = await dataHandler.Organization.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.OrganizationModelValidator.ValidateAsync(app, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Organization.DeleteAsync(app);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Organization Organization, RequestContext requestContext)
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

                    var isExisting = await dataHandler.Organization.AnyAsync(x => (x.Name == Organization.Name.Trim() ||
                                                                           x.NameCultured == Organization.NameCultured.Trim())
                                                                           &&
                                                                           (x.Code != Organization.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(OrganizationSearchCriteria criteria, RequestContext requestContext)
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

                    var isExisting = await dataHandler.Organization.AnyAsync(criteria);
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
            isValid = isValid && (OrganizationModelValidator != null);
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Organization : basic

                this.Organization_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Organization : Search

                this.Organization_DataInclude_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Organization : full

                this.Organization_DataInclude_Full = string.Join(",", new List<string>()
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
        private bool MapUpdate(Organization existing, Organization updated)
        {
            if (updated == null || existing == null) return false;
            existing.OrganizationReferenceId = updated.OrganizationReferenceId;
            
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
                    return this.Organization_DataInclude_Search;
                case SearchIncludesEnum.Full:
                    return this.Organization_DataInclude_Full;
                case SearchIncludesEnum.Basic:
                default:
                    return this.Organization_DataInclude_Basic;
            }
        }
        #endregion
    }
}
