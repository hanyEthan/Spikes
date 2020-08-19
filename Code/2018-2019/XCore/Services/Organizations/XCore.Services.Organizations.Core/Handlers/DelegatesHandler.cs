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
    public class DelegatesHandler :  IOrganizationDelegationHandler
    {
        #region props.

        private string OrganizationDelegation_DataInclude_Basic { get; set; }
        private string OrganizationDelegation_DataInclude_Search { get; set; }
        private string OrganizationDelegation_DataInclude_Full { get; set; }
        private string OrganizationDelegation_DataInclude_SearchRecursive { get; set; }
        
        private readonly IOrganizationDataUnity dataHandler;
        private readonly IModelValidator<OrganizationDelegation> OrganizationDelegationValidator;
        
        #endregion
        #region cst.

        public DelegatesHandler(IOrganizationDataUnity DataHandler,
                                   IModelValidator<OrganizationDelegation> OrganizationDelegationValidators)

        {
            this.dataHandler = DataHandler;
            this.OrganizationDelegationValidator = OrganizationDelegationValidators;
            this.Initialized = Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.OrganizationDelegationHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region OrganizationDelegation

        public async Task<ExecutionResponse<SearchResults<OrganizationDelegation>>> Get(OrganizationDelegationSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<OrganizationDelegation>>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL
                var includes = criteria.IncludeRecursive
                             ? this.OrganizationDelegation_DataInclude_SearchRecursive
                             : this.OrganizationDelegation_DataInclude_Search;
                var OrganizationDelegation = await dataHandler.OrganizationDelegation.GetAsync(criteria, includes);
                return context.Response.Set(ResponseState.Success, OrganizationDelegation);

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
        public async Task<ExecutionResponse<OrganizationDelegation>> Create(OrganizationDelegation DelegatesOrganization, RequestContext requestContext)
        {
            var context = new ExecutionContext<OrganizationDelegation>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.OrganizationDelegation.CreateAsync(DelegatesOrganization);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, DelegatesOrganization);

                #endregion

                #endregion
            }
            #region context

              , new ActionContext()
              {
                  Request = requestContext,
                  Validation = new ValidationContext<OrganizationDelegation>(this.OrganizationDelegationValidator, DelegatesOrganization, ValidationMode.Create),
              });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<OrganizationDelegation>> Edit(OrganizationDelegation DelegatesOrganization, RequestContext requestContext)
        {
            var context = new ExecutionContext<OrganizationDelegation>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await dataHandler.OrganizationDelegation.GetFirstAsync(x => x.DelegateId == DelegatesOrganization.DelegateId && x.DelegatorId == DelegatesOrganization.DelegatorId);
                if(existing == null) return context.Response.Set(ResponseState.NotFound, null);

                OrganizationsHelpers.MapUpdate(existing, DelegatesOrganization);

                dataHandler.OrganizationDelegation.Update(existing);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<OrganizationDelegation>(this.OrganizationDelegationValidator, DelegatesOrganization, ValidationMode.Edit),
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

                var existing = await dataHandler.OrganizationDelegation.GetFirstAsync(x => x.Code == code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.OrganizationDelegationValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.OrganizationDelegation.SetActivationAsync(existing.Id, true);
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

                var existing = await dataHandler.OrganizationDelegation.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.OrganizationDelegationValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.OrganizationDelegation.SetActivationAsync(id, true);
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

                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.OrganizationDelegation.GetFirstAsync(x => x.Code == code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.OrganizationDelegationValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.OrganizationDelegation.SetActivationAsync(existing.Id, false);
                await dataHandler.SaveAsync();

                #endregion

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

                #region validate.

                var existing = await dataHandler.OrganizationDelegation.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.OrganizationDelegationValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.OrganizationDelegation.SetActivationAsync(id, false);
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

                var existing = await dataHandler.OrganizationDelegation.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);
                
                var validationResponse = await this.OrganizationDelegationValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.OrganizationDelegation.DeleteAsync(existing);
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

                var existing = await dataHandler.OrganizationDelegation.GetFirstAsync(x => x.Code == code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);
                
                var validationResponse = await this.OrganizationDelegationValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.OrganizationDelegation.DeleteAsync(existing);
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
        public async Task<ExecutionResponse<bool>> Delete(OrganizationDelegation organizationDelegation, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.OrganizationDelegation.DeleteAsync(organizationDelegation);
                await dataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<OrganizationDelegation>(this.OrganizationDelegationValidator, organizationDelegation, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        
        public async Task<ExecutionResponse<bool>> IsUnique(OrganizationDelegation OrganizationDelegation, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.OrganizationDelegation.AnyAsync(x => 
                                                             (x.DelegateId != OrganizationDelegation.DelegateId)
                                                             &&
                                                             (x.DelegatorId != OrganizationDelegation.DelegatorId)
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
        public async Task<ExecutionResponse<bool>> IsExists(OrganizationDelegationSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.OrganizationDelegation.AnyAsync(criteria);
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
        #region Helpers.

        private bool Initialize()
        {
            bool isValid = true;
            try
            {
                isValid = isValid && (dataHandler?.Initialized ?? false);
                isValid = isValid && this.OrganizationDelegationValidator != null;
                isValid= isValid && InitializeIncludes();
              
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
            #region OrganizationDelegation : basic

            this.OrganizationDelegation_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region OrganizationDelegation : search

            this.OrganizationDelegation_DataInclude_Search = string.Join(",", new List<string>()
            {

            });

            #endregion
            #region OrganizationDelegation : full

            this.OrganizationDelegation_DataInclude_Full = string.Join(",", new List<string>()
            {
                "Delegate",
                "Delegator",
            });

            #endregion
           
            #region OrganizationDelegation : Recursive

            this.OrganizationDelegation_DataInclude_SearchRecursive = string.Join(",", new List<string>()
            {

            });

           

            #endregion

            return true;
        }

        #endregion
    }
}
