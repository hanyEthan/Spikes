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
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Handlers
{
    public class OrganizationsHandler : IOrganizationsHandler
    {
        #region props.

        private readonly IHiringDataUnity DataLayer;
        private readonly IModelValidator<Organization> OrganizationValidator;
        private string Organization_IncludeProperties_Basic { get; set; }
        private string Organization_IncludeProperties_Search { get; set; }
        private string Organization_IncludeProperties_Full { get; set; }

        #endregion
        #region cst.

        public OrganizationsHandler(IHiringDataUnity dataLayer, IModelValidator<Organization> OrganizationValidator)
        {
            this.DataLayer = dataLayer;
            this.OrganizationValidator = OrganizationValidator;
            this.Initialized = Initialize();
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.HiringHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IOrganizationsHandler

        public async Task<ExecutionResponse<Organization>> Create(Organization request, RequestContext requestContext)
        {
            var context = new ExecutionContext<Organization>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await this.DataLayer.Organizations.CreateAsync(request);
                await this.DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, request);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Organization>(this.OrganizationValidator, request, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<SearchResults<Organization>>> Get(OrganizationsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Organization>>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var results = await this.DataLayer.Organizations.Get(criteria, GetIncludes(criteria.SearchIncludes));
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
        public async Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                #region validate.

                var existing = await DataLayer.Organizations.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.OrganizationValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Organizations.SetActivationAsync(existing.Id, true);
                await DataLayer.SaveAsync();

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

                Check();

                #region DL

                #region validate.

                var existing = await DataLayer.Organizations.GetFirstAsync(x => x.Id == id);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.OrganizationValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                #endregion

                await DataLayer.Organizations.SetActivationAsync(existing.Id, false);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<Organization>> Edit(Organization organization, RequestContext requestContext)
        {
            var context = new ExecutionContext<Organization>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var existing = await DataLayer.Organizations.GetFirstAsync(x => x.Id == organization.Id || x.Code == organization.Code, null, this.Organization_IncludeProperties_Full);
                MapUpdate(existing, organization);

                DataLayer.Organizations.Update(existing);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(Organization Organization, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                await DataLayer.Organizations.DeleteAsync(Organization.Id);
                await DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Organization>(this.OrganizationValidator, Organization, ValidationMode.Delete),
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

                Check();

                #region validate.

                var Organization = await DataLayer.Organizations.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.OrganizationValidator.ValidateAsync(Organization, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Organizations.DeleteAsync(Organization.Id);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var Organization = await DataLayer.Organizations.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.OrganizationValidator.ValidateAsync(Organization, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Organizations.DeleteAsync(Organization.Id);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region validate.

                var venue = await DataLayer.Organizations.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.OrganizationValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Organizations.SetActivationAsync(venue.Id, true);
                await DataLayer.SaveAsync();

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

                Check();

                #region validate.

                var venue = await DataLayer.Organizations.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.OrganizationValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await DataLayer.Organizations.SetActivationAsync(venue.Id, false);
                await DataLayer.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> IsExists(OrganizationsSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.Organizations.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Organization organization, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                Check();

                #region DL

                var isExisting = await DataLayer.Organizations.AnyAsync(x => ((organization.Name != null && x.Name == organization.Name.Trim()) ||
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

        #endregion

        #region helpers. 

        #region initialize
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.DataLayer?.Initialized ?? false);
            isValid = isValid && OrganizationValidator != null;
            isValid = isValid && InitializeModelIncludes();

            return isValid;
        }
        private bool InitializeModelIncludes()
        {
            try
            {
                #region Organization : basic

                this.Organization_IncludeProperties_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Organization : search

                this.Organization_IncludeProperties_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Organization : full

                this.Organization_IncludeProperties_Full = string.Join(",", new List<string>()
                {
                    "HiringProcesses",
                    "Roles"
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
        #endregion
        #region mapUpdate
        private bool MapUpdate(Organization existing, Organization updated)
        {
            if (updated == null || existing == null) return false;

            #region Common

            existing.Code = updated.Code;
            //existing.IsActive = updated.IsActive;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.AppId = updated.AppId;
            existing.ModuleId = updated.ModuleId;
            existing.OrganizationReferenceId = updated.OrganizationReferenceId;

            #endregion

            //existing.HiringProcesses = updated.HiringProcesses;   // don't map
            MapUpdate(existing.Roles, updated.Roles);

            return true;
        }
        private bool MapUpdate(Role existing, Role updated)
        {
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;

            return true;
        }
        private bool MapUpdate(IList<Role> existing, IList<Role> updated)
        {
            // ...
            if (existing == null) return false;
            updated = updated ?? new List<Role>();

            // get [deleted records] ...
            foreach (var existingItem in existing)
            {
                var isDeleted = !updated.Any(x => x.Id == existingItem.Id);

                // delete the [existingItem]
                if (isDeleted)
                {
                    existing.Remove(existingItem);
                }
            }

            // get [updated records] ...
            foreach (var updatedItem in updated)
            {
                var existingItem = existing.Where(x => x.Id == updatedItem.Id && updatedItem.Id != 0).FirstOrDefault();

                if (existingItem != null)
                {
                    MapUpdate(existingItem, updatedItem);
                }
            }

            // get [inserted records] ...
            foreach (var updatedItem in updated)
            {
                var isInserted = !existing.Any(x => x.Id == updatedItem.Id && x.Id != 0);

                // insert the [updatedItem]
                if (isInserted)
                {
                    existing.Add(updatedItem);
                }
            }

            return true;
        }

        #endregion
        #region includes

        private string GetIncludes(SearchIncludes? searchIncludes)
        {
            switch (searchIncludes)
            {
                case SearchIncludes.Search:
                    return this.Organization_IncludeProperties_Search;
                case SearchIncludes.Full:
                    return this.Organization_IncludeProperties_Full;
                case SearchIncludes.Basic:
                    return this.Organization_IncludeProperties_Basic;
                default:
                    return null;
            }
        }

        #endregion

        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("handler is not initialized properly.");
            }
        }

        #endregion
    }
}
