//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using XCore.Framework.Infrastructure.Context.Execution.Extensions;
//using XCore.Framework.Infrastructure.Context.Execution.Handler;
//using XCore.Framework.Infrastructure.Context.Execution.Models;
//using XCore.Framework.Infrastructure.Context.Execution.Support;
//using XCore.Framework.Infrastructure.Entities.Repositories.Models;
//using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
//using XCore.Framework.Infrastructure.Entities.Validation.Models;
//using XCore.Framework.Utilities;
//using XCore.Services.Security.Core.Contracts;
//using XCore.Services.Security.Core.Models.Domain;
//using XCore.Services.Security.Core.Models.Events.Domain;
//using XCore.Services.Security.Core.Models.Relations;
//using XCore.Services.Security.Core.Models.Support;

//namespace XCore.Services.Security.Core.Handlers
//{
//    public class SecurityHandler : ISecurityHandler
//    {
//        #region props.

//        private string App_DataInclude_Basic { get; set; }
//        private string App_DataInclude_Full { get; set; }

//        private string Target_DataInclude_Basic { get; set; }
//        private string Target_DataInclude_Full { get; set; }

//        private string Privilege_DataInclude_Full { get; set; }
//        private string Privilege_DataInclude_Basic { get; set; }
//        private string Claim_DataInclude_Full { get; set; }
//        private string Claim_DataInclude_Basic { get; set; }

//        private string Actor_DataInclude_Full { get; set; }
//        private string Actor_DataInclude_Basic { get; set; }

//        private string Role_DataInclude_Full { get; set; }
//        private string Role_DataInclude_Basic { get; set; }

//        private readonly ISecurityDataUnity _DataHandler;
//        private readonly ISecurityEventsPublisher _EventsPublisher;

//        private readonly IModelValidator<App> AppsValidators;
//        private readonly IModelValidator<Role> RoleValidators;
//        private readonly IModelValidator<Actor> ActorValidators;
//        private readonly IModelValidator<Claim> ClaimValidators;


//        #endregion
//        #region cst.

//        public SecurityHandler(ISecurityDataUnity dataHandler,
//                               IModelValidator<App> appsValidators,
//                               IModelValidator<Role> roleValidators,
//                               IModelValidator<Actor> actorValidators,
//                               ISecurityEventsPublisher eventsPublisher,
//                               IModelValidator<Claim> claimValidators)
//        {
//            this.AppsValidators = appsValidators;
//            this.RoleValidators = roleValidators;
//            this.ActorValidators = actorValidators;
//            this.ClaimValidators = claimValidators;

//            this._DataHandler = dataHandler;
//            this._EventsPublisher = eventsPublisher;

//            this.Initialized = this.Initialize();
//        }

//        #endregion
//        #region IUnityService

//        public bool? Initialized { get; protected set; }
//        public string ServiceId { get { return $"XCore.SecurityHandler.{Guid.NewGuid()}"; } }

//        #endregion

//        #region ISecurityHandler

//        #region App

//        public async Task<ExecutionResponse<SearchResults<App>>> Get(AppSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic)
//        {
//            var context = new ExecutionContext<SearchResults<App>>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 #region mode.

//                 string includes = inquiryMode == InquiryMode.Basic ? this.App_DataInclude_Basic
//                                 : inquiryMode == InquiryMode.Search ? this.App_DataInclude_Basic
//                                 : inquiryMode == InquiryMode.Full ? this.App_DataInclude_Full
//                                 : null;

//                 #endregion

//                 var apps = await _DataHandler.Apps.GetAsync(criteria, includes);
//                 return context.Response.Set(ResponseState.Success, apps);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<App>> Register(App app, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<App>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 //await dataHandler.Apps.CreateAsync(app);
//                 await _DataHandler.Apps.CreateAsync(app);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, app);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//                 Validation = new ValidationContext<App>(this.AppsValidators, app, ValidationMode.Create),
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<App>> Edit(App app, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<App>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                var existing = await _DataHandler.Apps.GetFirstAsync(x => x.Id == app.Id || x.Code == app.Code);
//                if (existing == null)
//                {
//                    return context.Response.Set(ResponseState.NotFound, app);
//                }
//                MapUpdate(existing, app);

//                if (existing == null)
//                {
//                    return context.Response.Set(ResponseState.NotFound, null);
//                }

//                _DataHandler.Apps.Update(existing);
//                await _DataHandler.SaveAsync();

//                return context.Response.Set(ResponseState.Success, existing);


//                #endregion

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//                Validation = new ValidationContext<App>(this.AppsValidators, app, ValidationMode.Edit),
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> UnregisterApp(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var app = await _DataHandler.Apps.GetFirstAsync(x => x.Id == id);
//                 if (app == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }

//                 #region validation.
//                 var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Delete);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Apps.DeleteAsync(id);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> UnregisterApp(string code, RequestContext requestContext)
//        {

//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                var app = await _DataHandler.Apps.GetFirstAsync(x => x.Code == code);
//                if (app == null)
//                {
//                    return context.Response.Set(ResponseState.NotFound, false);
//                }

//                #region validation.
//                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Delete);
//                if (!validationResponse.IsValid)
//                {
//                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                }

//                #endregion

//                await _DataHandler.Apps.DeleteAsync(app.Id);
//                await _DataHandler.SaveAsync();

//                return context.Response.Set(ResponseState.Success, true);

//                #endregion

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> ActivateApp(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var app = await _DataHandler.Apps.GetFirstAsync(x => x.Id == id);
//                 if (app == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }

//                 #region validation.
//                 var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Activate);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Apps.SetActivationAsync(id, true);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> ActivateApp(string code, RequestContext requestContext)
//        {

//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var app = await _DataHandler.Apps.GetFirstAsync(x => x.Code == code);
//                 if (app == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }

//                 #region validation.
//                 var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Activate);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Apps.SetActivationAsync(app.Id, true);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeactivateApp(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var app = await _DataHandler.Apps.GetFirstAsync(x => x.Id == id);
//                 if (app == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }

//                 #region validation.
//                 var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Apps.SetActivationAsync(id, false);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeactivateApp(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL


//                var app = await _DataHandler.Apps.GetFirstAsync(x => x.Code == code);
//                if (app == null)
//                {
//                    return context.Response.Set(ResponseState.NotFound, false);
//                }
//                #region validation.
//                var validationResponse = await this.AppsValidators.ValidateAsync(app, ValidationMode.Deactivate);
//                if (!validationResponse.IsValid)
//                {
//                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                }

//                #endregion

//                await _DataHandler.Apps.SetActivationAsync(app.Id, false);
//                await _DataHandler.SaveAsync();

//                return context.Response.Set(ResponseState.Success, true);

//                #endregion

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsUnique(App app, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 var isExisting = await _DataHandler.Apps.AnyAsync(x => ((app.Name != null && x.Name == app.Name.Trim()) ||
//                                                             (app.NameCultured != null && x.NameCultured == app.NameCultured.Trim()))
//                                                             &&
//                                                             (x.Code != app.Code)
//                                                             &&
//                                                             (x.IsActive == true));

//                 return context.Response.Set(ResponseState.Success, isExisting);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsExists(AppSearchCriteria criteria, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var isExisting = await _DataHandler.Apps.AnyAsync(criteria);
//                 return context.Response.Set(ResponseState.Success, isExisting);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }

//        #endregion
//        #region Privilege

//        public async Task<ExecutionResponse<SearchResults<Privilege>>> Get(PrivilegeSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic)
//        {
//            var context = new ExecutionContext<SearchResults<Privilege>>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL


//                #region mode.

//                string includes = inquiryMode == InquiryMode.Basic ? this.Privilege_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Search ? this.Privilege_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Full ? this.Privilege_DataInclude_Full
//                                : null;

//                #endregion

//                var privileges = await _DataHandler.Privileges.GetAsync(criteria, includes);
//                return context.Response.Set(ResponseState.Success, privileges);


//                #endregion

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsExists(PrivilegeSearchCriteria criteria, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var isExisting = await _DataHandler.Privileges.AnyAsync(criteria);
//                 return context.Response.Set(ResponseState.Success, isExisting);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }

//        #endregion
//        #region Target

//        public async Task<ExecutionResponse<SearchResults<Target>>> Get(TargetSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic)
//        {
//            var context = new ExecutionContext<SearchResults<Target>>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL


//                #region mode.

//                string includes = inquiryMode == InquiryMode.Basic ? this.App_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Search ? this.App_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Full ? this.App_DataInclude_Full
//                                : null;

//                #endregion

//                var Targets = await _DataHandler.Targets.GetAsync(criteria, includes);
//                return context.Response.Set(ResponseState.Success, Targets);

//                #endregion

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsExists(TargetSearchCriteria criteria, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL


//                var isExisting = await _DataHandler.Targets.AnyAsync(criteria);
//                return context.Response.Set(ResponseState.Success, isExisting);


//                #endregion

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//            });
//            return context.Response;

//            #endregion

//        }

//        #endregion
//        #region Role

//        public async Task<ExecutionResponse<SearchResults<Role>>> Get(RoleSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic)
//        {
//            var context = new ExecutionContext<SearchResults<Role>>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 #region mode.

//                 string includes = inquiryMode == InquiryMode.Basic ? this.Role_DataInclude_Basic
//                                 : inquiryMode == InquiryMode.Search ? this.Role_DataInclude_Basic
//                                 : inquiryMode == InquiryMode.Full ? this.Role_DataInclude_Full
//                                 : null;

//                 #endregion

//                 var Roles = await _DataHandler.Roles.GetAsync(criteria, includes);
//                 return context.Response.Set(ResponseState.Success, Roles);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<Role>> Create(Role role, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<Role>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                await _DataHandler.Roles.CreateAsync(role);
//                await _DataHandler.SaveAsync();

//                #endregion
//                #region events.

//                await RaiseEvent_RoleCreated(role);
//                await RaiseEvent_PrivilegesAssociatedToRole(role, role.Privileges?.Select(x => x.PrivilegeId)?.ToList());

//                #endregion

//                return context.Response.Set(ResponseState.Success, role);

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//                Validation = new ValidationContext<Role>(this.RoleValidators, role, ValidationMode.Create),
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<Role>> Edit(Role role, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<Role>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == role.Id || x.Code == role.Code, null, this.Role_DataInclude_Full);
//                 if (existing == null) return context.Response.Set(ResponseState.NotFound, role);

//                 MapUpdate(existing, role, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedActors, out List<int> removedActors);

//                 _DataHandler.Roles.Update(existing);
//                 await _DataHandler.SaveAsync();

//                 #endregion
//                 #region events.

//                 await RaiseEvent_PrivilegesAssociatedToRole(role, addedPrivileges.Distinct().ToList());
//                 await RaiseEvent_PrivilegesDeassociatedFromRole(role, removedPrivileges.Distinct().ToList());
//                 await RaiseEvent_RolesAssociatedToActor(role, addedActors.Distinct().ToList());
//                 await RaiseEvent_RolesDeassociatedFromActor(role, removedActors.Distinct().ToList());

//                 #endregion

//                 return context.Response.Set(ResponseState.Success, existing);

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//                 Validation = new ValidationContext<Role>(this.RoleValidators, role, ValidationMode.Edit),
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeleteRole(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 #region validate.

//                 var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == id);
//                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                 var validationResponse = await this.RoleValidators.ValidateAsync(existing, ValidationMode.Delete);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Roles.DeleteAsync(existing.Id);
//                 await _DataHandler.SaveAsync();

//                 #endregion
//                 #region events.

//                 await RaiseEvent_RoleDeleted(existing);

//                 #endregion

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeleteRole(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region Logic

//                #region DL

//                #region validate.

//                var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Code == code);
//                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                var validationResponse = await this.RoleValidators.ValidateAsync(existing, ValidationMode.Delete);
//                if (!validationResponse.IsValid)
//                {
//                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                }

//                #endregion

//                await _DataHandler.Roles.DeleteAsync(existing.Id);
//                await _DataHandler.SaveAsync();

//                #endregion
//                #region events.

//                await RaiseEvent_RoleDeleted(existing);

//                #endregion

//                return context.Response.Set(ResponseState.Success, true);

//                #endregion

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> ActivateRole(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 #region validate.

//                 var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == id);
//                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                 var validationResponse = await this.RoleValidators.ValidateAsync(existing, ValidationMode.Activate);
//                 if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

//                 #endregion

//                 await _DataHandler.Roles.SetActivationAsync(existing.Id, true);
//                 await _DataHandler.SaveAsync();

//                 #endregion
//                 #region events.

//                 await RaiseEvent_RoleActivated(existing);

//                 #endregion

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> ActivateRole(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                #region validate.

//                var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Code == code);
//                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                var validationResponse = await this.RoleValidators.ValidateAsync(existing, ValidationMode.Activate);
//                if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

//                #endregion

//                await _DataHandler.Roles.SetActivationAsync(existing.Id, true);
//                await _DataHandler.SaveAsync();

//                #endregion
//                #region events.

//                await RaiseEvent_RoleActivated(existing);

//                #endregion

//                return context.Response.Set(ResponseState.Success, true);

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeactivateRole(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 #region validate.

//                 var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == id);
//                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                 var validationResponse = await this.RoleValidators.ValidateAsync(existing, ValidationMode.Activate);
//                 if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

//                 #endregion

//                 await _DataHandler.Roles.SetActivationAsync(existing.Id, false);
//                 await _DataHandler.SaveAsync();

//                 #endregion
//                 #region events.

//                 await RaiseEvent_RoleDeactivated(existing);

//                 #endregion

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeactivateRole(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 #region validate.

//                 var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Code == code);
//                 if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                 var validationResponse = await this.RoleValidators.ValidateAsync(existing, ValidationMode.Activate);
//                 if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

//                 #endregion

//                 await _DataHandler.Roles.SetActivationAsync(existing.Id, false);
//                 await _DataHandler.SaveAsync();

//                 #endregion
//                 #region events.

//                 await RaiseEvent_RoleDeactivated(existing);

//                 #endregion

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsUnique(Role role, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var isExisting = await _DataHandler.Roles.AnyAsync(x => ((role.Name != null && x.Name == role.Name.Trim()) ||
//                                                              (role.NameCultured != null && x.NameCultured == role.NameCultured.Trim()))
//                                                              &&
//                                                              (x.Code != role.Code)
//                                                              &&
//                                                              (x.IsActive == true));

//                 return context.Response.Set(ResponseState.Success, isExisting);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsExists(RoleSearchCriteria criteria, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 var isExisting = await _DataHandler.Roles.AnyAsync(criteria);
//                 return context.Response.Set(ResponseState.Success, isExisting);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }

//        #endregion
//        #region Claim

//        public async Task<ExecutionResponse<SearchResults<Claim>>> Get(ClaimSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic)
//        {
//            var context = new ExecutionContext<SearchResults<Claim>>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                #region mode.

//                string includes = inquiryMode == InquiryMode.Basic ? this.Claim_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Search ? this.Claim_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Full ? this.Claim_DataInclude_Full
//                                : null;

//                #endregion

//                var Claims = await _DataHandler.Claims.GetAsync(criteria, includes);
//                return context.Response.Set(ResponseState.Success, Claims);

//                #endregion

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<Claim>> Create(Claim Claim, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<Claim>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                await _DataHandler.Claims.CreateAsync(Claim);
//                await _DataHandler.SaveAsync();

//                #endregion
//                #region events.

//                //await RaiseEvent_ClaimCreated(Claim);
//                //await RaiseEvent_PrivilegesAssociatedToClaim(Claim, Claim.Privileges?.Select(x => x.PrivilegeId)?.ToList());

//                #endregion

//                return context.Response.Set(ResponseState.Success, Claim);

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//                Validation = new ValidationContext<Claim>(this.ClaimValidators, Claim, ValidationMode.Create),
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<Claim>> Edit(Claim Claim, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<Claim>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                var existing = await _DataHandler.Claims.GetFirstAsync(x => x.Id == Claim.Id || x.Code == Claim.Code, null, this.Claim_DataInclude_Full);
//                if (existing == null) return context.Response.Set(ResponseState.NotFound, Claim);

//                MapUpdate(existing, Claim, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedActors, out List<int> removedActors);

//                _DataHandler.Claims.Update(existing);
//                await _DataHandler.SaveAsync();

//                #endregion
//                #region events.

//                //await RaiseEvent_PrivilegesAssociatedToClaim(Claim, addedPrivileges.Distinct().ToList());
//                //await RaiseEvent_PrivilegesDeassociatedFromClaim(Claim, removedPrivileges.Distinct().ToList());
//                //await RaiseEvent_ClaimsAssociatedToActor(Claim, addedActors.Distinct().ToList());
//                //await RaiseEvent_ClaimsDeassociatedFromActor(Claim, removedActors.Distinct().ToList());

//                #endregion

//                return context.Response.Set(ResponseState.Success, existing);

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//                 Validation = new ValidationContext<Claim>(this.ClaimValidators, Claim, ValidationMode.Edit),
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeleteClaim(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                #region validate.

//                var existing = await _DataHandler.Claims.GetFirstAsync(x => x.Id == id);
//                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                var validationResponse = await this.ClaimValidators.ValidateAsync(existing, ValidationMode.Delete);
//                if (!validationResponse.IsValid)
//                {
//                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                }

//                #endregion

//                await _DataHandler.Claims.DeleteAsync(existing.Id);
//                await _DataHandler.SaveAsync();

//                #endregion
//                #region events.

//                //await RaiseEvent_ClaimDeleted(existing);

//                #endregion

//                return context.Response.Set(ResponseState.Success, true);

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeleteClaim(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region Logic

//                #region DL

//                #region validate.

//                var existing = await _DataHandler.Claims.GetFirstAsync(x => x.Code == code);
//                if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

//                var validationResponse = await this.ClaimValidators.ValidateAsync(existing, ValidationMode.Delete);
//                if (!validationResponse.IsValid)
//                {
//                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                }

//                #endregion

//                await _DataHandler.Claims.DeleteAsync(existing.Id);
//                await _DataHandler.SaveAsync();

//                #endregion
//                #region events.

//                //await RaiseEvent_ClaimDeleted(existing);

//                #endregion

//                return context.Response.Set(ResponseState.Success, true);

//                #endregion

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsUnique(Claim Claim, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL


//                var isExisting = await _DataHandler.Claims.AnyAsync(x => ((Claim.Name != null && x.Name == Claim.Name.Trim()) ||
//                                                             (Claim.NameCultured != null && x.NameCultured == Claim.NameCultured.Trim()))
//                                                             &&
//                                                             (x.Code == Claim.Code)
//                                                             &&
//                                                             (x.IsActive == true));

//                return context.Response.Set(ResponseState.Success, isExisting);


//                #endregion

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsExists(ClaimSearchCriteria criteria, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL

//                var isExisting = await _DataHandler.Claims.AnyAsync(criteria);
//                return context.Response.Set(ResponseState.Success, isExisting);


//                #endregion

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }

//        #endregion
//        #region Actor

//        public async Task<ExecutionResponse<SearchResults<Actor>>> Get(ActorSearchCriteria criteria, RequestContext requestContext, InquiryMode inquiryMode = InquiryMode.Basic)
//        {
//            var context = new ExecutionContext<SearchResults<Actor>>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL


//                #region mode.

//                string includes = inquiryMode == InquiryMode.Basic ? this.Actor_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Search ? this.Actor_DataInclude_Basic
//                                : inquiryMode == InquiryMode.Full ? this.Actor_DataInclude_Full
//                                : null;

//                #endregion

//                var Actors = await _DataHandler.Actors.GetAsync(criteria, includes);
//                return context.Response.Set(ResponseState.Success, Actors);

//                #endregion

//                #endregion
//            }
//            #region context

//            , new ActionContext()
//            {
//                Request = requestContext,
//            });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<Actor>> Create(Actor actor, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<Actor>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 await _DataHandler.Actors.CreateAsync(actor);
//                 await _DataHandler.SaveAsync();

//                 #endregion
//                 #region events.

//                 await RaiseEvent_RolesAssociatedToActor(actor, actor.Roles?.Select(x => x.RoleId)?.ToList());
//                 await RaiseEvent_PrivilegesAssociatedToActor(actor, actor.Privileges?.Select(x => x.PrivilegeId)?.ToList());

//                 #endregion

//                 return context.Response.Set(ResponseState.Success, actor);

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//                 Validation = new ValidationContext<Actor>(this.ActorValidators, actor, ValidationMode.Create),
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<Actor>> Edit(Actor actor, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<Actor>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL

//                 var existing = await _DataHandler.Actors.GetFirstAsync(x => x.Id == actor.Id || x.Code == actor.Code, null, this.Actor_DataInclude_Full);
//                 if (existing == null) return context.Response.Set(ResponseState.NotFound, actor);

//                 MapUpdate(existing, actor, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedRoles, out List<int> removedRoles);

//                 _DataHandler.Actors.Update(existing);
//                 await _DataHandler.SaveAsync();

//                 #endregion
//                 #region events.

//                 await RaiseEvent_PrivilegesAssociatedToActor(existing, addedPrivileges.Distinct().ToList());
//                 await RaiseEvent_PrivilegesDeassociatedFromActor(existing, removedPrivileges.Distinct().ToList());
//                 await RaiseEvent_RolesAssociatedToActor(existing, addedRoles.Distinct().ToList());
//                 await RaiseEvent_RolesDeassociatedFromActor(existing, removedRoles.Distinct().ToList());

//                 #endregion

//                 return context.Response.Set(ResponseState.Success, existing);

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//                 Validation = new ValidationContext<Actor>(this.ActorValidators, actor, ValidationMode.Edit),
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeleteActor(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL
//                 var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Id == id);
//                 if (actor == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }
//                 #region validation.
//                 var validationResponse = await this.ActorValidators.ValidateAsync(actor, ValidationMode.Delete);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Actors.DeleteAsync(actor.Id);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> ActivateActor(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//            {
//                #region Logic

//                #region DL
//                var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Id == id);
//                if (actor == null)
//                {
//                    return context.Response.Set(ResponseState.NotFound, false);
//                }
//                #region validation.
//                var validationResponse = await this.ActorValidators.ValidateAsync(actor, ValidationMode.Activate);
//                if (!validationResponse.IsValid)
//                {
//                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                }

//                #endregion

//                await _DataHandler.Actors.SetActivationAsync(actor.Id, true);
//                await _DataHandler.SaveAsync();

//                return context.Response.Set(ResponseState.Success, true);

//                #endregion

//                #endregion
//            }
//            #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeactivateActor(int id, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Id == id);
//                 if (actor == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }

//                 #region validation.
//                 var validationResponse = await this.ActorValidators.ValidateAsync(actor, ValidationMode.Deactivate);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Actors.SetActivationAsync(actor.Id, false);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeleteActor(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Code == code);
//                 if (actor == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }
//                 #region validation.
//                 var validationResponse = await this.ActorValidators.ValidateAsync(actor, ValidationMode.Delete);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Actors.DeleteAsync(actor.Id);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> ActivateActor(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Code == code);
//                 if (actor == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }

//                 #region validation.
//                 var validationResponse = await this.ActorValidators.ValidateAsync(actor, ValidationMode.Activate);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Actors.SetActivationAsync(actor.Id, true);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> DeactivateActor(string code, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Code == code);
//                 if (actor == null)
//                 {
//                     return context.Response.Set(ResponseState.NotFound, false);
//                 }

//                 #region validation.
//                 var validationResponse = await this.ActorValidators.ValidateAsync(actor, ValidationMode.Activate);
//                 if (!validationResponse.IsValid)
//                 {
//                     return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
//                 }

//                 #endregion

//                 await _DataHandler.Actors.SetActivationAsync(actor.Id, false);
//                 await _DataHandler.SaveAsync();

//                 return context.Response.Set(ResponseState.Success, true);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//                 //Validation = new ValidationContext<Role>(new RolesValidators(), new Role() { Code = code }, ValidationMode.Delete),
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsUnique(Actor actor, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var isExisting = await _DataHandler.Actors.AnyAsync(x => ((actor.Name != null && x.Name == actor.Name.Trim()) ||
//                                                              (actor.NameCultured != null && x.NameCultured == actor.NameCultured.Trim()))
//                                                              &&
//                                                              (x.Code == actor.Code)
//                                                              &&
//                                                              (x.IsActive == true));

//                 return context.Response.Set(ResponseState.Success, isExisting);

//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }
//        public async Task<ExecutionResponse<bool>> IsExists(ActorSearchCriteria criteria, RequestContext requestContext)
//        {
//            var context = new ExecutionContext<bool>();
//            await context.Process(async () =>
//             {
//                 #region Logic

//                 #region DL


//                 var isExisting = await _DataHandler.Actors.AnyAsync(criteria);
//                 return context.Response.Set(ResponseState.Success, isExisting);


//                 #endregion

//                 #endregion
//             }
//             #region context

//             , new ActionContext()
//             {
//                 Request = requestContext,
//             });
//            return context.Response;

//            #endregion
//        }

//        #endregion

//        #endregion
//        #region helpers.

//        #region events.

//        private async Task RaiseEvent_RoleCreated(Role role)
//        {
//            if ((role?.Id).GetValueOrDefault() == 0) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            // ...
//            await this._EventsPublisher.RoleCreatedEvent(new RoleCreatedDomainEvent()
//            {
//                RoleId = role.Id,
//                //AppId = role.AppId,
//            });
//        }
//        private async Task RaiseEvent_RoleDeleted(Role role)
//        {
//            if ((role?.Id).GetValueOrDefault() == 0) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            // ...
//            await this._EventsPublisher.RoleDeletedEvent(new RoleDeletedDomainEvent()
//            {
//                RoleId = role.Id,
//                //AppId = role.AppId,
//            });
//        }
//        private async Task RaiseEvent_RoleActivated(Role role)
//        {
//            if ((role?.Id).GetValueOrDefault() == 0) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            // ...
//            await this._EventsPublisher.RoleActivatedEvent(new RoleActivatedDomainEvent()
//            {
//                RoleId = role.Id,
//                //AppId = role.AppId,
//            });
//        }
//        private async Task RaiseEvent_RoleDeactivated(Role role)
//        {
//            if ((role?.Id).GetValueOrDefault() == 0) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            // ...
//            await this._EventsPublisher.RoleDeactivatedEvent(new RoleDeactivatedDomainEvent()
//            {
//                RoleId = role.Id,
//                //AppId = role.AppId,
//            });
//        }

//        private async Task RaiseEvent_RolesAssociatedToActor(Actor actor, List<int> addedRoles)
//        {
//            if ((actor?.Id).GetValueOrDefault() == 0) return;
//            if (!addedRoles?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            await this._EventsPublisher.RolesAssociatedToActorEvent(new RolesAssociatedToActorDomainEvent()
//            {
//                //AppId = actor.AppId,
//                ActorId = actor.Id,
//                Roles = addedRoles,
//            });
//        }
//        private async Task RaiseEvent_RolesAssociatedToActor(Role role, List<int> addedActors)
//        {
//            if ((role?.Id).GetValueOrDefault() == 0) return;
//            if (!role?.Actors?.Any() ?? true) return;
//            if (!addedActors?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            var addedRoles = new List<int>() { role.Id };

//            foreach (var addedActorId in addedActors)
//            {
//                await this._EventsPublisher.RolesAssociatedToActorEvent(new RolesAssociatedToActorDomainEvent()
//                {
//                    //AppId = role.AppId,
//                    ActorId = addedActorId,
//                    Roles = addedRoles,
//                });
//            }
//        }

//        private async Task RaiseEvent_RolesDeassociatedFromActor(Actor actor, List<int> removedRoles)
//        {
//            if ((actor?.Id).GetValueOrDefault() == 0) return;
//            if (!removedRoles?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            await this._EventsPublisher.RoleDeassociatedFromActorEvent(new RolesDeassociatedFromActorDomainEvent()
//            {
//                ActorId = actor.Id,
//                Roles = removedRoles,
//            });
//        }
//        private async Task RaiseEvent_RolesDeassociatedFromActor(Role role, List<int> removedActors)
//        {
//            if ((role?.Id).GetValueOrDefault() == 0) return;
//            if (!role?.Actors?.Any() ?? true) return;
//            if (!removedActors?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            var addedRoles = new List<int>() { role.Id };

//            foreach (var removedActorId in removedActors)
//            {
//                await this._EventsPublisher.RoleDeassociatedFromActorEvent(new RolesDeassociatedFromActorDomainEvent()
//                {
//                    ActorId = removedActorId,
//                    Roles = addedRoles,
//                });
//            }
//        }

//        private async Task RaiseEvent_PrivilegesAssociatedToRole(Role role, List<int> addedPrivileges)
//        {
//            if (role == null) return;
//            if (!addedPrivileges?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            await this._EventsPublisher.PrivilegeAssociatedToRoleEvent(new PrivilegeAssociatedToRoleDomainEvent()
//            {
//                //AppId = role.AppId,
//                RoleId = role.Id,
//                PrivilegeIds = addedPrivileges,
//            });
//        }
//        private async Task RaiseEvent_PrivilegesDeassociatedFromRole(Role role, List<int> removedPrivileges)
//        {
//            if ((role?.Id).GetValueOrDefault() == 0) return;
//            if (!removedPrivileges?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            await this._EventsPublisher.PrivilegeDeassociatedFromRoleEvent(new PrivilegeDeassociatedFromRoleDomainEvent()
//            {
//                RoleId = role.Id,
//                Privileges = removedPrivileges,
//            });
//        }

//        private async Task RaiseEvent_PrivilegesAssociatedToActor(Actor actor, List<int> addedPrivileges)
//        {
//            if (actor == null) return;
//            if (!addedPrivileges?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            await this._EventsPublisher.PrivilegeAssociatedToActorEvent(new PrivilegeAssociatedToActorDomainEvent()
//            {
//                //AppId = actor.AppId,
//                ActorId = actor.Id,
//                Privileges = addedPrivileges,
//            });
//        }
//        private async Task RaiseEvent_PrivilegesDeassociatedFromActor(Actor actor, List<int> removedPrivileges)
//        {
//            if ((actor?.Id).GetValueOrDefault() == 0) return;
//            if (!removedPrivileges?.Any() ?? true) return;
//            if (this._EventsPublisher?.Initialized != true) return;

//            await this._EventsPublisher.PrivilegeDeassociatedFromActorEvent(new PrivilegeDeassociatedFromActorDomainEvent()
//            {
//                ActorId = actor.Id,
//                Privileges = removedPrivileges,
//            });
//        }

//        #endregion
//        #region Map Updates.

//        private bool MapUpdate(App existing, App updated)
//        {
//            if (updated == null || existing == null) return false;

//            existing.Code = updated.Code;
//            existing.Description = updated.Description;
//            existing.IsActive = updated.IsActive;
//            existing.MetaData = updated.MetaData;
//            existing.ModifiedBy = updated.ModifiedBy;
//            existing.ModifiedDate = updated.ModifiedDate;
//            existing.Name = updated.Name;
//            existing.NameCultured = updated.NameCultured;
//            existing.Actors = updated.Actors;
//            existing.Privileges = updated.Privileges;
//            existing.Roles = updated.Roles;
//            existing.Targets = updated.Targets;

//            return true;
//        }
//        private bool MapUpdate(Role existing, Role updated, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedActors, out List<int> removedActors)
//        {
//            // ...
//            addedPrivileges = new List<int>();
//            removedPrivileges = new List<int>();
//            addedActors = new List<int>();
//            removedActors = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            existing.Code = updated.Code;
//            existing.Description = updated.Description;
//            existing.IsActive = updated.IsActive;
//            existing.MetaData = updated.MetaData;
//            existing.ModifiedBy = updated.ModifiedBy;
//            existing.ModifiedDate = updated.ModifiedDate;
//            existing.Name = updated.Name;
//            existing.NameCultured = updated.NameCultured;
//            existing.AppId = updated.AppId;

//            bool state = true;

//            state = MapUpdate(existing.App, updated.App) && state;
//            state = MapUpdate(existing.Privileges, updated.Privileges, out addedPrivileges, out removedPrivileges) && state;
//            state = MapUpdate(existing.Actors, updated.Actors, out List<int> addedRoles, out List<int> removedRoles, out addedActors, out removedActors) && state;

//            return state;
//        }
//        private bool MapUpdate(Claim existing, Claim updated, out List<int> addedRoles, out List<int> removedRoles, out List<int> addedActors, out List<int> removedActors)
//        {
//            // ...
//            addedRoles = new List<int>();
//            removedRoles = new List<int>();
//            addedActors = new List<int>();
//            removedActors = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            existing.Code = updated.Code;
//            existing.Description = updated.Description;
//            existing.IsActive = updated.IsActive;
//            existing.MetaData = updated.MetaData;
//            existing.ModifiedBy = updated.ModifiedBy;
//            existing.ModifiedDate = updated.ModifiedDate;
//            existing.Name = updated.Name;
//            existing.NameCultured = updated.NameCultured;
//            existing.AppId = updated.AppId;

//            bool state = true;

//            state = MapUpdate(existing.App, updated.App) && state;

//            return state;
//        }

//        private bool MapUpdate(Actor existing, Actor updated, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedRoles, out List<int> removedRoles)
//        {
//            // ...
//            addedPrivileges = new List<int>();
//            removedPrivileges = new List<int>();
//            addedRoles = new List<int>();
//            removedRoles = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            existing.Code = updated.Code;
//            existing.Description = updated.Description;
//            existing.IsActive = updated.IsActive;
//            existing.MetaData = updated.MetaData;
//            existing.ModifiedBy = updated.ModifiedBy;
//            existing.ModifiedDate = updated.ModifiedDate;
//            existing.Name = updated.Name;
//            existing.NameCultured = updated.NameCultured;
//            existing.AppId = updated.AppId;

//            bool state = true;

//            state = MapUpdate(existing.App, updated.App) && state;
//            state = MapUpdate(existing.Privileges, updated.Privileges, out addedPrivileges, out removedPrivileges) && state;
//            state = MapUpdate(existing.Roles, updated.Roles, out addedRoles, out removedRoles, out List<int> addedActors, out List<int> removedActors) && state;

//            return state;
//        }

//        private bool MapUpdate(IList<ActorPrivilege> existing, IList<ActorPrivilege> updated, out List<int> addedPrivileges, out List<int> removedPrivileges)
//        {
//            // ...
//            addedPrivileges = new List<int>();
//            removedPrivileges = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            // get [deleted records] ...
//            foreach (var existingItem in existing)
//            {
//                var isDeleted = !updated.Any(x => x.ActorId == existingItem.ActorId
//                                               && x.PrivilegeId == existingItem.PrivilegeId);

//                // delete the [existingItem]
//                if (isDeleted)
//                {
//                    this._DataHandler.Actors.DeletedAssociatedPrivilege(existingItem);
//                    existing.Remove(existingItem);
//                    removedPrivileges.Add(existingItem.PrivilegeId);
//                }
//            }

//            // get [inserted records] ...
//            foreach (var updatedItem in updated)
//            {
//                var isInserted = !existing.Any(x => x.ActorId == updatedItem.ActorId
//                                                 && x.PrivilegeId == updatedItem.PrivilegeId);

//                // insert the [updatedItem]
//                if (isInserted)
//                {
//                    existing.Add(updatedItem);
//                    addedPrivileges.Add(updatedItem.PrivilegeId);

//                }
//            }

//            return true;
//        }
//        private bool MapUpdate(IList<RolePrivilege> existing, IList<RolePrivilege> updated, out List<int> addedPrivileges, out List<int> removedPrivileges)
//        {
//            // ...
//            addedPrivileges = new List<int>();
//            removedPrivileges = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            // get [deleted records] ...
//            foreach (var existingItem in existing)
//            {
//                var isDeleted = !updated.Any(x => x.RoleId == existingItem.RoleId
//                                               && x.PrivilegeId == existingItem.PrivilegeId);

//                // delete the [existingItem]
//                if (isDeleted)
//                {
//                    this._DataHandler.Roles.DeletedAssociatedPrivilege(existingItem);
//                    existing.Remove(existingItem);
//                    removedPrivileges.Add(existingItem.PrivilegeId);
//                }
//            }

//            // get [inserted records] ...
//            foreach (var updatedItem in updated)
//            {
//                var isInserted = !existing.Any(x => x.RoleId == updatedItem.RoleId
//                                                 && x.PrivilegeId == updatedItem.PrivilegeId);

//                // insert the [updatedItem]
//                if (isInserted)
//                {
//                    existing.Add(updatedItem);
//                    addedPrivileges.Add(updatedItem.PrivilegeId);
//                }
//            }

//            return true;
//        }
//        private bool MapUpdate(IList<RoleClaim> existing, IList<RoleClaim> updated, out List<int> addedRoles, out List<int> removedRoles)
//        {
//            // ...
//            addedRoles = new List<int>();
//            removedRoles = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            // get [deleted records] ...
//            foreach (var existingItem in existing)
//            {
//                var isDeleted = !updated.Any(x => x.RoleId == existingItem.RoleId
//                                               && x.RoleId == existingItem.RoleId);

//                // delete the [existingItem]
//                if (isDeleted)
//                {
//                    this._DataHandler.Claims.DeletedAssociatedRole(existingItem);
//                    existing.Remove(existingItem);
//                    removedRoles.Add(existingItem.RoleId);
//                }
//            }

//            // get [inserted records] ...
//            foreach (var updatedItem in updated)
//            {
//                var isInserted = !existing.Any(x => x.RoleId == updatedItem.RoleId
//                                                 && x.RoleId == updatedItem.RoleId);

//                // insert the [updatedItem]
//                if (isInserted)
//                {
//                    existing.Add(updatedItem);
//                    addedRoles.Add(updatedItem.RoleId);
//                }
//            }

//            return true;
//        }
//        private bool MapUpdate(IList<ActorClaim> existing, IList<ActorClaim> updated, out List<int> addedClaims, out List<int> removedClaims, out List<int> addedActors, out List<int> removedActors)
//        {
//            // ...
//            addedClaims = new List<int>();
//            removedClaims = new List<int>();
//            addedActors = new List<int>();
//            removedActors = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            // get [deleted records] ...
//            foreach (var existingItem in existing)
//            {
//                var isDeleted = !updated.Any(x => x.ClaimId == existingItem.ClaimId
//                                               && x.ActorId == existingItem.ActorId);

//                // delete the [existingItem]
//                if (isDeleted)
//                {
//                    this._DataHandler.Claims.DeletedAssociatedActor(existingItem);
//                    existing.Remove(existingItem);

//                    removedClaims.Add(existingItem.ClaimId);
//                    removedActors.Add(existingItem.ActorId);
//                }
//            }

//            // get [inserted records] ...
//            foreach (var updatedItem in updated)
//            {
//                var isInserted = !existing.Any(x => x.ClaimId == updatedItem.ClaimId
//                                                 && x.ActorId == updatedItem.ActorId);

//                // insert the [updatedItem]
//                if (isInserted)
//                {
//                    existing.Add(updatedItem);

//                    addedClaims.Add(updatedItem.ClaimId);
//                    addedActors.Add(updatedItem.ClaimId);
//                }
//            }

//            return true;
//        }
//        private bool MapUpdate(IList<ActorRole> existing, IList<ActorRole> updated, out List<int> addedRoles, out List<int> removedRoles, out List<int> addedActors, out List<int> removedActors)
//        {
//            // ...
//            addedRoles = new List<int>();
//            removedRoles = new List<int>();
//            addedActors = new List<int>();
//            removedActors = new List<int>();

//            // ...
//            if (updated == null || existing == null) return false;

//            // get [deleted records] ...
//            foreach (var existingItem in existing)
//            {
//                var isDeleted = !updated.Any(x => x.RoleId == existingItem.RoleId
//                                               && x.ActorId == existingItem.ActorId);

//                // delete the [existingItem]
//                if (isDeleted)
//                {
//                    this._DataHandler.Roles.DeletedAssociatedActor(existingItem);
//                    existing.Remove(existingItem);

//                    removedRoles.Add(existingItem.RoleId);
//                    removedActors.Add(existingItem.ActorId);
//                }
//            }

//            // get [inserted records] ...
//            foreach (var updatedItem in updated)
//            {
//                var isInserted = !existing.Any(x => x.RoleId == updatedItem.RoleId
//                                                 && x.ActorId == updatedItem.ActorId);

//                // insert the [updatedItem]
//                if (isInserted)
//                {
//                    existing.Add(updatedItem);

//                    addedRoles.Add(updatedItem.RoleId);
//                    addedActors.Add(updatedItem.RoleId);
//                }
//            }

//            return true;
//        }

//        #endregion

//        private bool Initialize()
//        {
//            bool isValid = true;

//            isValid = isValid && InitializeIncludes();

//            isValid = isValid && (this._DataHandler?.Initialized ?? false);
//            isValid = isValid && (this._EventsPublisher?.Initialized ?? false);
//            isValid = isValid && AppsValidators != null;
//            isValid = isValid && RoleValidators != null;
//            isValid = isValid && ActorValidators != null;
//            isValid = isValid && ClaimValidators != null;

//            return isValid;
//        }
//        private bool InitializeIncludes()
//        {
//            try
//            {
//                #region App : basic

//                this.App_DataInclude_Basic = string.Join(",", new List<string>()
//                {
//                });

//                #endregion
//                #region App : full

//                this.App_DataInclude_Full = string.Join(",", new List<string>()
//                {
//                    "Privileges",
//                    "Roles",
//                    "Actors",
//                    "Targets",
//                });

//                #endregion

//                #region Privilege : basic

//                this.Privilege_DataInclude_Basic = string.Join(",", new List<string>()
//                {
//                });

//                #endregion
//                #region Privilege : full

//                this.Privilege_DataInclude_Full = string.Join(",", new List<string>()
//                {
//                    "App",
//                    "Roles",
//                    "Actors",
//                    "Target"
//                });

//                #endregion
//                #region Claim : basic

//                this.Claim_DataInclude_Basic = string.Join(",", new List<string>()
//                {
//                });

//                #endregion
//                #region Claim : full

//                this.Claim_DataInclude_Full = string.Join(",", new List<string>()
//                {
//                    "App",
//                    "Roles",
//                    "Actors",
//                });

//                #endregion

//                #region Actor : basic

//                this.Actor_DataInclude_Basic = string.Join(",", new List<string>()
//                {
//                });

//                #endregion
//                #region Actor : full

//                this.Actor_DataInclude_Full = string.Join(",", new List<string>()
//                {
//                    "App",
//                    "Roles",
//                    "Privileges",
//                });

//                #endregion

//                #region Role : basic

//                this.Role_DataInclude_Basic = string.Join(",", new List<string>()
//                {
//                });

//                #endregion
//                #region Role : full

//                this.Role_DataInclude_Full = string.Join(",", new List<string>()
//                {
//                    "App",
//                    "Actors",
//                    "Privileges"
//                });

//                #endregion

//                #region Target : basic

//                this.Target_DataInclude_Basic = string.Join(",", new List<string>()
//                {
//                });

//                #endregion
//                #region Target : full

//                this.Target_DataInclude_Full = string.Join(",", new List<string>()
//                {
//                    "App",
//                    "Privilege",
//                });

//                #endregion

//                return true;
//            }
//            catch (Exception x)
//            {
//                XLogger.Error($"Exception : {x}");
//                return false;
//            }
//        }

//        #endregion
//    }
//}
