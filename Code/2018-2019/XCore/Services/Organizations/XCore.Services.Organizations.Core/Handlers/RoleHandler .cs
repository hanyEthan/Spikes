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
    public class RoleHandler : IRoleHandler
    {
        #region props.

        private string Role_DataInclude_Basic { get; set; }
        private string Role_DataInclude_Search { get; set; }
        private string Role_DataInclude_Full { get; set; }
        private string Role_DataInclude_SearchRecursive { get; set; }

        private readonly IOrganizationDataUnity dataHandler;
        private readonly IModelValidator<Role> RoleValidator;

        #endregion
        #region cst.
        public RoleHandler(IOrganizationDataUnity DataHandler,
                                    IModelValidator<Role> roleValidator)
        {
            this.dataHandler = DataHandler;
            this.RoleValidator = roleValidator;

            this.Initialized = Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.RoleHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IRoleHandler

        public async Task<ExecutionResponse<SearchResults<Role>>> Get(RoleSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Role>>();
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
                              ? this.Role_DataInclude_SearchRecursive
                              : this.Role_DataInclude_Search;
 
                 var roles = await dataHandler.Role.GetAsync(criteria, includes);
                 return context.Response.Set(ResponseState.Success, roles);

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
        public async Task<ExecutionResponse<Role>> Create(Role role, RequestContext requestContext)
        {
            var context = new ExecutionContext<Role>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Role.CreateAsync(role);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, role);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Role>(this.RoleValidator, role, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Role>> Edit(Role role, RequestContext requestContext)
        {
            var context = new ExecutionContext<Role>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var existing = await dataHandler.Role.GetFirstAsync(x => x.Id == role.Id || x.Code == role.Code, null, this.Role_DataInclude_Full);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, null);

                 OrganizationsHelpers.MapUpdate(existing, role);

                 dataHandler.Role.Update(existing);
                 await dataHandler.SaveAsync();

                 return context.Response.Set(ResponseState.Success, existing);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<Role>(this.RoleValidator, role, ValidationMode.Edit),
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Role role, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 await dataHandler.Role.DeleteAsync(role);
                 await dataHandler.SaveAsync();

                 return context.Response.Set(ResponseState.Success, true);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<Role>(this.RoleValidator, role, ValidationMode.Delete),
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

                 var existing = await dataHandler.Role.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.RoleValidator.ValidateAsync(existing, ValidationMode.Delete);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Role.DeleteAsync(existing);
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

                 var existing = await dataHandler.Role.GetFirstAsync(x => x.Code == code);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);
              
                 var validationResponse = await this.RoleValidator.ValidateAsync(existing, ValidationMode.Delete);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Role.DeleteAsync(existing.Id);
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

                 var existing = await dataHandler.Role.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.RoleValidator.ValidateAsync(existing, ValidationMode.Activate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Role.SetActivationAsync(id, true);
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

                var role = await dataHandler.Role.GetFirstAsync(x => x.Code == code);
                var validationResponse = await RoleValidator.ValidateAsync(role, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Role.SetActivationAsync(role.Id, true);
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

                 var role = await dataHandler.Role.GetFirstAsync(x => x.Id == id);
                 var validationResponse = await RoleValidator.ValidateAsync(role, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Role.SetActivationAsync(id, false);
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

                 var role = await dataHandler.Role.GetFirstAsync(x => x.Code == code);
                 var validationResponse = await RoleValidator.ValidateAsync(role, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Role.SetActivationAsync(role.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Role role, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Role.AnyAsync(x => ((role.Name != null && x.Name == role.Name.Trim()) ||
                                                             (role.NameCultured != null && x.NameCultured == role.NameCultured.Trim()))
                                                             &&
                                                             (x.Code != role.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(RoleSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Role.AnyAsync(criteria);
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
                isValid = isValid && RoleValidator!=null;
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
            #region Role : basic

            this.Role_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region Role : search

            this.Role_DataInclude_Search = string.Join(",", new List<string>()
            {

            });

            #endregion
            #region Role : Recursive

            this.Role_DataInclude_SearchRecursive = string.Join(",", new List<string>()
                {
                   
                });

            #endregion
            #region Role : full

            this.Role_DataInclude_Full = string.Join(",", new List<string>()
                {
                   
                });

            #endregion

            return true;
        }

        #endregion
    }
}
