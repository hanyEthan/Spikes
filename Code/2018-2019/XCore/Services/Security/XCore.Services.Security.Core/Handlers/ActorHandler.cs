using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
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
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Events.Domain;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Handlers
{
    public class ActorHandler : IActorHandler
    {
        #region props.

        private string Actor_DataInclude_Basic { get; set; }
        private string Actor_DataInclude_Full { get; set; }

        private readonly ISecurityDataUnity _DataHandler;
        private readonly IModelValidator<Actor> ActorsValidators;
        private IMediator _mediator;

        #endregion
        #region cst.

        public ActorHandler(ISecurityDataUnity dataHandler, IModelValidator<Actor> ActorsValidators, IMediator mediator)
        {
            this.ActorsValidators = ActorsValidators;
            this._DataHandler = dataHandler;
            this._mediator = mediator;
            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SecurityHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IActorHandler

        public async Task<ExecutionResponse<SearchResults<Actor>>> Get(ActorSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Actor>>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    #region mode.

                    string includes = criteria.InquiryMode == InquiryMode.Basic ? this.Actor_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Search ? this.Actor_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Full ? this.Actor_DataInclude_Full
                                    : null;

                    #endregion

                    var Actors = await _DataHandler.Actors.GetAsync(criteria, includes);
                    return context.Response.Set(ResponseState.Success, Actors);

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
        public async Task<ExecutionResponse<Actor>> Create(Actor actor, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Actor>();
                await context.Process(async () =>
                {
                    #region Logic

                    #region check.

                    Check();

                    #endregion
                    #region DL

                    await _DataHandler.Actors.CreateAsync(actor);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RolesAssociatedToActor(actor, actor.Roles?.Select(x => x.RoleId)?.ToList());
                    await RaiseEvent_PrivilegesAssociatedToActor(actor, actor.Privileges?.Select(x => x.PrivilegeId)?.ToList());

                    #endregion

                    return context.Response.Set(ResponseState.Success, actor);

                    #endregion
                }
                #region context

                 , new ActionContext()
                 {
                     Request = requestContext,
                     Validation = new ValidationContext<Actor>(this.ActorsValidators, actor, ValidationMode.Create),
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
        public async Task<ExecutionResponse<Actor>> Edit(Actor actor, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Actor>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    var existing = await _DataHandler.Actors.GetFirstAsync(x => x.Id == actor.Id || x.Code == actor.Code, null, this.Actor_DataInclude_Full);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, actor);

                    MapUpdate(existing, actor, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedRoles, out List<int> removedRoles, out List<int> addedClaims, out List<int> removedClaims);

                    _DataHandler.Actors.Update(existing);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_PrivilegesAssociatedToActor(existing, addedPrivileges.Distinct().ToList());
                    await RaiseEvent_PrivilegesDeassociatedFromActor(existing, removedPrivileges.Distinct().ToList());
                    await RaiseEvent_RolesAssociatedToActor(existing, addedRoles.Distinct().ToList());
                    await RaiseEvent_RolesDeassociatedFromActor(existing, removedRoles.Distinct().ToList());
                    await RaiseEvent_ClaimsAssociatedToActor(existing, addedClaims.Distinct().ToList());
                    await RaiseEvent_ClaimsDeassociatedFromActor(existing, removedClaims.Distinct().ToList());

                    #endregion

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion
                }
                #region context

                 , new ActionContext()
                 {
                     Request = requestContext,
                     Validation = new ValidationContext<Actor>(this.ActorsValidators, actor, ValidationMode.Edit),
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
        public async Task<ExecutionResponse<bool>> DeleteActor(int id, RequestContext requestContext)
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
                    var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Id == id);
                    if (actor == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }
                    #region validation.
                    var validationResponse = await this.ActorsValidators.ValidateAsync(actor, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Actors.DeleteAsync(actor.Id);
                    await _DataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateActor(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic
                #region check.

                Check();

                #endregion

                #region DL
                var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Id == id);
                if (actor == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }
                #region validation.
                var validationResponse = await this.ActorsValidators.ValidateAsync(actor, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.Actors.SetActivationAsync(actor.Id, true);
                await _DataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeactivateActor(int id, RequestContext requestContext)
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


                    var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Id == id);
                    if (actor == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.ActorsValidators.ValidateAsync(actor, ValidationMode.Deactivate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Actors.SetActivationAsync(actor.Id, false);
                    await _DataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeleteActor(string code, RequestContext requestContext)
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


                    var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Code == code);
                    if (actor == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }
                    #region validation.
                    var validationResponse = await this.ActorsValidators.ValidateAsync(actor, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Actors.DeleteAsync(actor.Id);
                    await _DataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> ActivateActor(string code, RequestContext requestContext)
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


                    var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Code == code);
                    if (actor == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.ActorsValidators.ValidateAsync(actor, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Actors.SetActivationAsync(actor.Id, true);
                    await _DataHandler.SaveAsync();

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
        public async Task<ExecutionResponse<bool>> DeactivateActor(string code, RequestContext requestContext)
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


                    var actor = await _DataHandler.Actors.GetFirstAsync(x => x.Code == code);
                    if (actor == null)
                    {
                        return context.Response.Set(ResponseState.NotFound, false);
                    }

                    #region validation.
                    var validationResponse = await this.ActorsValidators.ValidateAsync(actor, ValidationMode.Activate);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Actors.SetActivationAsync(actor.Id, false);
                    await _DataHandler.SaveAsync();

                    return context.Response.Set(ResponseState.Success, true);

                    #endregion

                    #endregion
                }
                #region context

                 , new ActionContext()
                 {
                     Request = requestContext,
                     //Validation = new ValidationContext<Role>(new RolesValidators(), new Role() { Code = code }, ValidationMode.Delete),
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
        public async Task<ExecutionResponse<bool>> IsUnique(Actor actor, RequestContext requestContext)
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


                    var isExisting = await _DataHandler.Actors.AnyAsync(x => ((actor.Name != null && x.Name == actor.Name.Trim()) ||
                                                                 (actor.NameCultured != null && x.NameCultured == actor.NameCultured.Trim()))
                                                                 &&
                                                                 (x.Code == actor.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(ActorSearchCriteria criteria, RequestContext requestContext)
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


                    var isExisting = await _DataHandler.Actors.AnyAsync(criteria);
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
        #region Helpers
        private bool MapUpdate(Actor existing, Actor updated, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedRoles, out List<int> removedRoles, out List<int> addedClaims, out List<int> removedClaims)
        {
            // ...
            addedPrivileges = new List<int>();
            removedPrivileges = new List<int>();
            addedRoles = new List<int>();
            removedRoles = new List<int>();
            addedClaims = new List<int>();
            removedClaims = new List<int>();

            // ...
            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.Description = updated.Description;
            existing.IsActive = updated.IsActive;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.AppId = updated.AppId;

            bool state = true;

            state = ExtractUpdates(existing.Privileges, updated.Privileges, out addedPrivileges, out removedPrivileges) && state;
            state = ExtractUpdates(existing.Roles, updated.Roles, out addedRoles, out removedRoles) && state;
            state = ExtractUpdates(existing.Claims, updated.Claims, out addedClaims, out removedClaims) && state;

            existing.Roles = updated.Roles;
            existing.Privileges = updated.Privileges;
            existing.Claims = updated.Claims;

            return state;
        }
        private bool ExtractUpdates(IList<ActorClaim> existing, IList<ActorClaim> updated, out List<int> addedClaims, out List<int> removedClaims)
        {
            // ...
            addedClaims = new List<int>();
            removedClaims = new List<int>();

            if (existing == null || updated == null) return false;

            addedClaims = updated.Except(updated.Where(u => existing.Any(e => e.ClaimId == u.ClaimId && e.ActorId == u.ActorId))).Select(x=>x.ClaimId).ToList();
            removedClaims = existing.Except(existing.Where(u => updated.Any(e => e.ClaimId == u.ClaimId && e.ActorId == u.ActorId))).Select(x=>x.ClaimId).ToList();

            #region obsolete

            //// ...
            //if (updated == null || existing == null) return false;

            //// get [deleted records] ...
            //foreach (var existingItem in existing)
            //{
            //    var isDeleted = !updated.Any(x => x.ClaimId == existingItem.ClaimId
            //                                   && x.ActorId == existingItem.ActorId);

            //    // delete the [existingItem]
            //    if (isDeleted)
            //    {
            //        this._DataHandler.Claims.DeletedAssociatedActor(existingItem);
            //        existing.Remove(existingItem);

            //        removedClaims.Add(existingItem.ClaimId);
            //    }
            //}

            //// get [inserted records] ...
            //foreach (var updatedItem in updated)
            //{
            //    var isInserted = !existing.Any(x => x.ClaimId == updatedItem.ClaimId
            //                                     && x.ActorId == updatedItem.ActorId);

            //    // insert the [updatedItem]
            //    if (isInserted)
            //    {
            //        existing.Add(updatedItem);

            //        addedClaims.Add(updatedItem.ClaimId);
            //    }
            //}

            #endregion

            return true;
        }
        private bool ExtractUpdates(IList<ActorRole> existing, IList<ActorRole> updated, out List<int> addedRoles, out List<int> removedRoles)
        {
            // ...
            addedRoles = new List<int>();
            removedRoles = new List<int>();
 

            // ...
            if (updated == null || existing == null) return false;
            addedRoles = updated.Except(updated.Where(u => existing.Any(e => e.RoleId == u.RoleId && e.ActorId == u.ActorId))).Select(x => x.RoleId).ToList();
            removedRoles = existing.Except(existing.Where(u => updated.Any(e => e.RoleId == u.RoleId && e.ActorId == u.ActorId))).Select(x => x.RoleId).ToList();
            #region obsolete

            // get [deleted records] ...
            //foreach (var existingItem in existing)
            //{
            //    var isDeleted = !updated.Any(x => x.RoleId == existingItem.RoleId
            //                                   && x.ActorId == existingItem.ActorId);

            //    // delete the [existingItem]
            //    if (isDeleted)
            //    {
            //        this._DataHandler.Roles.DeletedAssociatedActor(existingItem);
            //        existing.Remove(existingItem);

            //        removedRoles.Add(existingItem.RoleId);
            //    }
            //}

            //// get [inserted records] ...
            //foreach (var updatedItem in updated)
            //{
            //    var isInserted = !existing.Any(x => x.RoleId == updatedItem.RoleId
            //                                     && x.ActorId == updatedItem.ActorId);

            //    // insert the [updatedItem]
            //    if (isInserted)
            //    {
            //        existing.Add(updatedItem);

            //        addedRoles.Add(updatedItem.RoleId);
            //    }
            //}
            #endregion

            return true;
        }
        private bool ExtractUpdates(IList<ActorPrivilege> existing, IList<ActorPrivilege> updated, out List<int> addedPrivileges, out List<int> removedPrivileges)
        {
            // ...
            addedPrivileges = new List<int>();
            removedPrivileges = new List<int>();

            // ...
            if (updated == null || existing == null) return false;
            addedPrivileges = updated.Except(updated.Where(u => existing.Any(e => e.PrivilegeId == u.PrivilegeId && e.ActorId == u.ActorId))).Select(x => x.PrivilegeId).ToList();
            removedPrivileges = existing.Except(existing.Where(u => updated.Any(e => e.PrivilegeId == u.PrivilegeId && e.ActorId == u.ActorId))).Select(x => x.PrivilegeId).ToList();

            #region obsolete
            //foreach (var existingItem in existing)
            //{
            //    var isDeleted = !updated.Any(x => x.ActorId == existingItem.ActorId
            //                                   && x.PrivilegeId == existingItem.PrivilegeId);

            //    // delete the [existingItem]
            //    if (isDeleted)
            //    {
            //        this._DataHandler.Actors.DeletedAssociatedPrivilege(existingItem);
            //        existing.Remove(existingItem);
            //        removedPrivileges.Add(existingItem.PrivilegeId);
            //    }
            //}

            //// get [inserted records] ...
            //foreach (var updatedItem in updated)
            //{
            //    var isInserted = !existing.Any(x => x.ActorId == updatedItem.ActorId
            //                                     && x.PrivilegeId == updatedItem.PrivilegeId);

            //    // insert the [updatedItem]
            //    if (isInserted)
            //    {
            //        existing.Add(updatedItem);
            //        addedPrivileges.Add(updatedItem.PrivilegeId);

            //    }
            //}
            #endregion

            return true;
        }

        #region events.
        private async Task RaiseEvent_ClaimsAssociatedToActor(Actor actor, List<int> addedClaims)
        {
            if (actor == null) return;
            if (_mediator == null) return;
            if ((actor?.Id).GetValueOrDefault() == 0) return;
            if (!addedClaims?.Any() ?? true) return;

            await _mediator.Publish(new ClaimsAssociatedToActorDomainEvent()
            {
                App = actor.AppId.ToString(),
                ActorId = actor.Id,
                Claims = addedClaims
            });
        }

        private async Task RaiseEvent_ClaimsDeassociatedFromActor(Actor actor, List<int> removedClaims)
        {
            if (actor == null) return;
            if (_mediator == null) return;
            if ((actor?.Id).GetValueOrDefault() == 0) return;
            if (!removedClaims?.Any() ?? true) return;

            await this._mediator.Publish(new ClaimsDeassociatedFromActorDomainEvent()
            {
                ActorId = actor.Id,
                Claims = removedClaims,
            });
        }
        private async Task RaiseEvent_RolesAssociatedToActor(Actor actor, List<int> addedRoles)
        {
            if (actor == null) return;
            if (_mediator == null) return;
            if ((actor?.Id).GetValueOrDefault() == 0) return;
            if (!addedRoles?.Any() ?? true) return;

            await _mediator.Publish(new RolesAssociatedToActorDomainEvent()
            {
                App = actor.AppId.ToString(),
                ActorId = actor.Id,
                Roles = addedRoles
            });
        }

        private async Task RaiseEvent_RolesDeassociatedFromActor(Actor actor, List<int> removedRoles)
        {
            if (actor == null) return;
            if (_mediator == null) return;
            if ((actor?.Id).GetValueOrDefault() == 0) return;
            if (!removedRoles?.Any() ?? true) return;

            await this._mediator.Publish(new RolesDeassociatedFromActorDomainEvent()
            {
                ActorId = actor.Id,
                Roles = removedRoles,
            });
        }

        private async Task RaiseEvent_PrivilegesAssociatedToActor(Actor actor, List<int> addedPrivileges)
        {
            if (actor == null) return;
            if (_mediator == null) return;
            if ((actor?.Id).GetValueOrDefault() == 0) return;
            if (!addedPrivileges?.Any() ?? true) return;

            await this._mediator.Publish(new PrivilegeAssociatedToActorDomainEvent()
            {
                App = actor.AppId.ToString(),
                ActorId = actor.Id,
                Privileges = addedPrivileges,
            });
        }
        private async Task RaiseEvent_PrivilegesDeassociatedFromActor(Actor actor, List<int> removedPrivileges)
        {
            if (actor == null) return;
            if (_mediator == null) return;
            if ((actor?.Id).GetValueOrDefault() == 0) return;
            if (!removedPrivileges?.Any() ?? true) return;

            await this._mediator.Publish(new PrivilegeDeassociatedFromActorDomainEvent()
            {
                ActorId = actor.Id,
                Privileges = removedPrivileges,
            });
        }

        #endregion
        
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && InitializeIncludes();

            isValid = isValid && (this._DataHandler?.Initialized ?? false);
            isValid = isValid && ActorsValidators != null;
            isValid = isValid && _mediator != null;

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }
        private bool InitializeIncludes()
        {
            try
            {
                #region Actor : basic

                this.Actor_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Actor : full

                this.Actor_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "App",
                    "Roles",
                    "Privileges",
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
    }
}
