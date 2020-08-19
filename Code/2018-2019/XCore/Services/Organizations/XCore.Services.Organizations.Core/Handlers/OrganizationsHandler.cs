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
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;
using XCore.Services.Organizations.Core.Utilities;
using XCore.Services.Organizations.Core.Validators;

namespace XCore.Services.Organizations.Core.Handlers
{
    public class OrganizationsHandler : IOrganizationHandler
    {
        #region props.

        private string Organization_DataInclude_Basic { get; set; }
        private string Organization_DataInclude_Search { get; set; }
        private string Organization_DataInclude_Full { get; set; }
        private string Organization_DataInclude_SearchRecursive { get; set; }

        private readonly IOrganizationDataUnity dataHandler;
        private readonly IModelValidator<Organization> OrganizationValidator;

        #endregion
        #region cst.
        public OrganizationsHandler(IOrganizationDataUnity DataHandler,
                                    IModelValidator<Organization> organizationValidator)
        {
            this.dataHandler = DataHandler;
            this.OrganizationValidator = organizationValidator;

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

                 var includes = criteria.IncludeRecursive
                              ? this.Organization_DataInclude_SearchRecursive
                              : this.Organization_DataInclude_Search;
 
                 var Organizations = await dataHandler.Organization.GetAsync(criteria, includes);
                 return context.Response.Set(ResponseState.Success, Organizations);

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
        public async Task<ExecutionResponse<Organization>> Create(Organization organization, RequestContext requestContext)
        {
            var context = new ExecutionContext<Organization>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Organization.CreateAsync(organization);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, organization);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Organization>(this.OrganizationValidator, organization, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Organization>> Edit(Organization organization, RequestContext requestContext)
        {
            var context = new ExecutionContext<Organization>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var existing = await dataHandler.Organization.GetFirstAsync(x => x.Id == organization.Id || x.Code == organization.Code, null, this.Organization_DataInclude_Full);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, null);

                 OrganizationsHelpers.MapUpdate(existing, organization);

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
                 Validation = new ValidationContext<Organization>(this.OrganizationValidator, organization, ValidationMode.Edit),
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Organization organization, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 await dataHandler.Organization.DeleteAsync(organization);
                 await dataHandler.SaveAsync();

                 return context.Response.Set(ResponseState.Success, true);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<Organization>(this.OrganizationValidator, organization, ValidationMode.Delete),
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

                 var existing = await dataHandler.Organization.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.OrganizationValidator.ValidateAsync(existing, ValidationMode.Delete);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Organization.DeleteAsync(existing.Id);
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

                 var existing = await dataHandler.Organization.GetFirstAsync(x => x.Code == code);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);
              
                 var validationResponse = await this.OrganizationValidator.ValidateAsync(existing, ValidationMode.Delete);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Organization.DeleteAsync(existing.Id);
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
        public async Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 #region validate.

                 var existing = await dataHandler.Organization.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.OrganizationValidator.ValidateAsync(existing, ValidationMode.Activate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Organization.SetActivationAsync(id, true);
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

                #region validation

                var organization = await dataHandler.Organization.GetFirstAsync(x => x.Code == code);
                var validationResponse = await OrganizationValidator.ValidateAsync(organization, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Organization.SetActivationAsync(organization.Id, true);
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
        public async Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 #region validation

                 var organization = await dataHandler.Organization.GetFirstAsync(x => x.Id == id);
                 var validationResponse = await OrganizationValidator.ValidateAsync(organization, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Organization.SetActivationAsync(id, false);
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
        public async Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 #region validation

                 var organization = await dataHandler.Organization.GetFirstAsync(x => x.Code == code);
                 var validationResponse = await OrganizationValidator.ValidateAsync(organization, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Organization.SetActivationAsync(organization.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Organization organization, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Organization.AnyAsync(x => ((organization.Name != null && x.Name == organization.Name.Trim()) ||
                                                             (organization.NameCultured != null && x.NameCultured == organization.NameCultured.Trim()))
                                                             &&
                                                             (x.Code != organization.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(OrganizationSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

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

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;
            try
            {
                isValid = isValid && (dataHandler?.Initialized ?? false)&&this.OrganizationValidator!=null;
                isValid = isValid && OrganizationValidator != null;
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
            #region Organization : basic

            this.Organization_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region Organization : search

            this.Organization_DataInclude_Search = string.Join(",", new List<string>()
            {
                //"Departments",
                //"Settings",
                //"ContactInfo",
                //"ContactPersonnel",
                //"SubOrganizations",
                //"OrganizationDelegates",
                //"OrganizationDelegators",
            });

            #endregion
            #region Organization : Recursive

            this.Organization_DataInclude_SearchRecursive = string.Join(",", new List<string>()
                {
                    "Departments.SubDepartments",
                    //"Settings",
                    //"ContactInfo",
                    //"ContactPersonnel",
                    "SubOrganizations.SubOrganizations",
                    "OrganizationDelegates",
                    "OrganizationDelegators",
                });

            #endregion
            #region Organization : full

            this.Organization_DataInclude_Full = string.Join(",", new List<string>()
                {
                    //"Departments",
                    //"Settings",
                    //"ContactInfo",
                    //"ContactPersonnel",
                    //"SubOrganizations",
                    //"OrganizationDelegates",
                    //"OrganizationDelegators",
                });

            #endregion

            return true;
        }

        #endregion
    }
}
