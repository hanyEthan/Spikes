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
    public class CityHandler : ICityHandler
    {
        #region props.
        private string City_DataInclude_Basic { get; set; }
        private string City_DataInclude_Search { get; set; }
        private string City_DataInclude_Full { get; set; }

        private string city_DataInclude_SearchRecursive;

        private string GetCity_DataInclude_SearchRecursive()
        {
            return city_DataInclude_SearchRecursive;
        }

        private void City_DataInclude_SearchRecursive(string value)
        {
            city_DataInclude_SearchRecursive = value;
        }

        private readonly IModelValidator<City> CityValidator;
        private readonly  IOrganizationDataUnity dataHandler;


        #endregion
        #region cst.

        public CityHandler(IOrganizationDataUnity _dataHandler,  IModelValidator<City> CityValidator)
        {
            this.dataHandler = _dataHandler;
         
            this.CityValidator = CityValidator;
           
            this.Initialized = Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.CityHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region ICityHandler
        public async Task<ExecutionResponse<SearchResults<City>>> Get(CitySearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<City>>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var includes = criteria.IncludeRecursive
                             ? this.city_DataInclude_SearchRecursive
                             : this.City_DataInclude_Search;
                var Cities = await dataHandler.city.GetAsync(criteria, includes);
                return context.Response.Set(ResponseState.Success, Cities);

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
        public async Task<ExecutionResponse<City>> Create(City city, RequestContext requestContext)
        {
            var context = new ExecutionContext<City>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.city.CreateAsync(city);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, city);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<City>(this.CityValidator, city, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<City>> Edit(City city, RequestContext requestContext)
        {
            var context = new ExecutionContext<City>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL
                // var existing = await dataHandler.city.GetFirstAsync(x => x.Id == city.Id || x.Code == city.Code, null, this.City_DataInclude_Full);
                var existing = await dataHandler.city.GetFirstAsync(x => x.Id == city.Id || x.Code == city.Code);
                OrganizationsHelpers.MapUpdate(existing, city);

                dataHandler.city.Update(existing);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, existing);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<City>(this.CityValidator, city, ValidationMode.Edit),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(City city, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.city.DeleteAsync(city);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<City>(this.CityValidator, city, ValidationMode.Delete),
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

                #region validate.

                var city = await dataHandler.city.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.CityValidator.ValidateAsync(city, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.city.DeleteAsync(city);
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
        public async Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region validate.

                var existing = await dataHandler.city.GetFirstAsync(x => x.Code == code);
                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                var validationResponse = await this.CityValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.city.DeleteAsync(existing.Id);
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

                #region validate.

                var city = await dataHandler.city.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.CityValidator.ValidateAsync(city, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.city.SetActivationAsync(id, true);
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
        public async Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                var city = await dataHandler.city.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.CityValidator.ValidateAsync(city, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.city.SetActivationAsync(city.Id, true);
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

                #region validate.

                var city = await dataHandler.city.GetFirstAsync(x => x.Id == id);
                var validationResponse = await this.CityValidator.ValidateAsync(city, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.city.SetActivationAsync(id, false);
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

                #region validate.

                var city = await dataHandler.city.GetFirstAsync(x => x.Code == code);
                var validationResponse = await this.CityValidator.ValidateAsync(city, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await dataHandler.city.SetActivationAsync(city.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsExists(CitySearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.city.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(City city, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await dataHandler.Venue.AnyAsync(x => (x.Name == city.Name.Trim() ||
                                                                       x.NameCultured == city.NameCultured.Trim())
                                                                       &&
                                                                       (x.Code != city.Code)
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

        private bool Initialize()
        {
            bool isValid = true;
            try
            {
                isValid = isValid && (dataHandler?.Initialized ?? false);
                isValid = isValid && CityValidator != null;
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
            #region City : basic

            this.City_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region City : search

            this.City_DataInclude_Search = string.Join(",", new List<string>()
            {

            });

            #endregion
            #region City : Recursive

            this.city_DataInclude_SearchRecursive = string.Join(",", new List<string>()
            {

            });

            #endregion
            #region City : full

            this.City_DataInclude_Full = string.Join(",", new List<string>()
            {
                "VenueCity"

            });

            #endregion

            return true;
        }

        #endregion
    }
}
