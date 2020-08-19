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
    public class EventHandler : IEventHandler
    {
        #region props.

        private string Event_DataInclude_Basic { get; set; }
        private string Event_DataInclude_Search { get; set; }
        private string Event_DataInclude_Full { get; set; }
        private string Event_DataInclude_SearchRecursive { get; set; }

        private readonly IOrganizationDataUnity dataHandler;
        private readonly IModelValidator<Event> EventValidator;

        #endregion
        #region cst.
        public EventHandler(IOrganizationDataUnity DataHandler,
                                    IModelValidator<Event> roleValidator)
        {
            this.dataHandler = DataHandler;
            this.EventValidator = roleValidator;

            this.Initialized = Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.RoleHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IEventHandler

        public async Task<ExecutionResponse<SearchResults<Event>>> Get(EventSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Event>>();
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
                              ? this.Event_DataInclude_SearchRecursive
                              : this.Event_DataInclude_Search;
 
                 var events = await dataHandler.Event.GetAsync(criteria, includes);
                 return context.Response.Set(ResponseState.Success, events);

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
        public async Task<ExecutionResponse<Event>> Create(Event Event, RequestContext requestContext)
        {
            var context = new ExecutionContext<Event>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                await dataHandler.Event.CreateAsync(Event);
                await dataHandler.SaveAsync();

                return context.Response.Set(ResponseState.Success, Event);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Event>(this.EventValidator, Event, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Event>> Edit(Event Event, RequestContext requestContext)
        {
            var context = new ExecutionContext<Event>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 // var existing = await dataHandler.Event.GetFirstAsync(x => x.Id == Event.Id || x.Code == Event.Code, null, this.Event_DataInclude_Full);
                 var existing = await dataHandler.Event.GetFirstAsync(x => x.Id == Event.Id || x.Code == Event.Code);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, null);

                 OrganizationsHelpers.MapUpdate(existing, Event);

                 dataHandler.Event.Update(existing);
                 await dataHandler.SaveAsync();

                 return context.Response.Set(ResponseState.Success, existing);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<Event>(this.EventValidator, Event, ValidationMode.Edit),
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(Event Event, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 await dataHandler.Event.DeleteAsync(Event);
                 await dataHandler.SaveAsync();

                 return context.Response.Set(ResponseState.Success, true);

                 #endregion

                 #endregion
             }
             #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<Event>(this.EventValidator, Event, ValidationMode.Delete),
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

                 var existing = await dataHandler.Event.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.EventValidator.ValidateAsync(existing, ValidationMode.Delete);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Event.DeleteAsync(existing);
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

                 var existing = await dataHandler.Event.GetFirstAsync(x => x.Code == code);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);
              
                 var validationResponse = await this.EventValidator.ValidateAsync(existing, ValidationMode.Delete);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Event.DeleteAsync(existing.Id);
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

                 var existing = await dataHandler.Event.GetFirstAsync(x => x.Id == id);
                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                 var validationResponse = await this.EventValidator.ValidateAsync(existing, ValidationMode.Activate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Event.SetActivationAsync(id, true);
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

                var Event = await dataHandler.Event.GetFirstAsync(x => x.Code == code);
                var validationResponse = await EventValidator.ValidateAsync(Event, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await dataHandler.Event.SetActivationAsync(Event.Id, true);
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

                 var Event = await dataHandler.Event.GetFirstAsync(x => x.Id == id);
                 var validationResponse = await EventValidator.ValidateAsync(Event, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Event.SetActivationAsync(id, false);
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

                 var Event = await dataHandler.Event.GetFirstAsync(x => x.Code == code);
                 var validationResponse = await EventValidator.ValidateAsync(Event, ValidationMode.Deactivate);
                 if (!validationResponse.IsValid)
                 {
                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                 }

                 #endregion

                 await dataHandler.Event.SetActivationAsync(Event.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Event Event, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Event.AnyAsync(x => ((Event.Name != null && x.Name == Event.Name.Trim()) ||
                                                             (Event.NameCultured != null && x.NameCultured == Event.NameCultured.Trim()))
                                                             &&
                                                             (x.Code != Event.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(EventSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
             {
                 #region Logic

                 #region DL

                 var isExisting = await dataHandler.Event.AnyAsync(criteria);
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
                isValid = isValid && EventValidator!=null;
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
            #region Event : basic

            this.Event_DataInclude_Basic = string.Join(",", new List<string>()
            {
            });

            #endregion
            #region Event : search

            this.Event_DataInclude_Search = string.Join(",", new List<string>()
            { 
                //"Departments",
                //"VenueEvents"
            });

            #endregion
            #region Event : Recursive

            this.Event_DataInclude_SearchRecursive = string.Join(",", new List<string>()
                {
                   
                });

            #endregion
            #region Event : full

            this.Event_DataInclude_Full = string.Join(",", new List<string>()
                {
                "Departments",
                "VenueEvents"

                });

            #endregion

            return true;
        }

        #endregion
    }
}
