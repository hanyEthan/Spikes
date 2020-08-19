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

namespace XCore.Services.Organizations.Core.Handlers
{
    public class VenueHandler : IVenueHandler
    {
        #region props.
        private string Venue_DataInclude_Basic { get; set; }
        private string Venue_DataInclude_Search { get; set; }
        private string Venue_DataInclude_Full { get; set; }
        private string Venue_DataInclude_SearchRecursive { get; set; }
        private readonly IOrganizationDataUnity dataHandler;
        private readonly IModelValidator<Venue> VenueValidator;

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.VenueHandler.{Guid.NewGuid()}"; } }

        #endregion


        #endregion
        #region cst.

        public VenueHandler(IOrganizationDataUnity _dataHandler, IModelValidator<Venue> _VenueValidator)
        {
            this.dataHandler = _dataHandler;
            this.VenueValidator = _VenueValidator;
            this.Initialized = Initialize();


        }

        #endregion
        #region IVenueHandler

        public async Task<ExecutionResponse<SearchResults<Venue>>> Get(VenueSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Venue>>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL
                var includes = criteria.IncludeRecursive
                           ? this.Venue_DataInclude_SearchRecursive
                           : this.Venue_DataInclude_Search;

                var Venues = await dataHandler.Venue.GetAsync(criteria, includes);
                return context.Response.Set(ResponseState.Success, Venues);

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
        public async Task<ExecutionResponse<Venue>> Create(Venue Venue, RequestContext requestContext)
        {
            var context = new ExecutionContext<Venue>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Venue.CreateAsync(Venue);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, Venue);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Venue>(this.VenueValidator, Venue, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Venue>> Edit(Venue Venue, RequestContext requestContext)
        {
            var context = new ExecutionContext<Venue>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await dataHandler.Venue.GetFirstAsync(x => x.Id == Venue.Id || x.Code == Venue.Code);
                OrganizationsHelpers.MapUpdate(existing, Venue);

                dataHandler.Venue.Update(existing);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Venue>(this.VenueValidator, Venue, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Venue Venue, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Venue.DeleteAsync(Venue);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Venue>(this.VenueValidator, Venue, ValidationMode.Delete),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> DeleteVenue(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var venue = await dataHandler.Venue.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.VenueValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Venue.DeleteAsync(venue);
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

        public async Task<ExecutionResponse<bool>> DeleteVenue(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var Venue = await dataHandler.Venue.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.VenueValidator.ValidateAsync(Venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Venue.DeleteAsync(Venue.Id);
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
        public async Task<ExecutionResponse<bool>> ActivateVenue(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var venue = await dataHandler.Venue.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.VenueValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Venue.SetActivationAsync(id, true);
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
        public async Task<ExecutionResponse<bool>> ActivateVenue(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var venue = await dataHandler.Venue.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.VenueValidator.ValidateAsync(venue, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Venue.SetActivationAsync(venue.Id, true);
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
        public async Task<ExecutionResponse<bool>> DeactivateVenue(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var venue = await dataHandler.Venue.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.VenueValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Venue.SetActivationAsync(id, false);
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
        public async Task<ExecutionResponse<bool>> DeactivateVenue(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var venue = await dataHandler.Venue.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.VenueValidator.ValidateAsync(venue, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.Venue.SetActivationAsync(venue.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsExists(VenueSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Venue.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Venue venue, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Venue.AnyAsync(x => (x.Name == venue.Name.Trim() ||
                                                                       x.NameCultured == venue.NameCultured.Trim())
                                                                       ||
                                                                       (x.Code == venue.Code));

                // (x.IsActive == true));

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
                isValid = isValid && VenueValidator != null ;
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
            #region Venue : basic

            this.Venue_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region Venue : search

            this.Venue_DataInclude_Search = string.Join(",", new List<string>()
            {

                  //"VenueCity",
                  //"VenueDepartments",
                  //"VenueEvents",
                  //"SubVenues",
                  //"ParentVenue"

            });

            #endregion
            #region Venue : Recursive

            this.Venue_DataInclude_SearchRecursive = string.Join(",", new List<string>()
            {

                  //"VenueCity",
                  //"VenueDepartments",
                  //"VenueEvents",
                  //"SubVenues",
                  //"ParentVenue"

            });

            #endregion
            #region Venue : full

            this.Venue_DataInclude_Full = string.Join(",", new List<string>()
            {
                  "VenueCity",
                  "VenueDepartments",
                  "VenueEvents",
                  "SubVenues",
                  "ParentVenue"

            });

            #endregion

            return true;
        }

        #endregion
    }
}
