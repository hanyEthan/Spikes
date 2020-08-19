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
   public class RoleHandler : IRoleHandler
    {
        #region props.

        private string Role_DataInclude_Basic { get; set; }
        private string Role_DataInclude_Full { get; set; }

        private readonly ISecurityDataUnity _DataHandler;
        private readonly IMediator _mediator;

        private readonly IModelValidator<Role> RolesValidators;

        #endregion
        #region cst.

        public RoleHandler(ISecurityDataUnity dataHandler, IMediator mediator, IModelValidator<Role> RolesValidators)
        {
            this.RolesValidators = RolesValidators;
            this._DataHandler = dataHandler;
            this._mediator = mediator;
            this.Initialized = this.Initialize();

        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SecurityHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region Role

        public async Task<ExecutionResponse<SearchResults<Role>>> Get(RoleSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Role>>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    #region mode.

                    string includes = criteria.InquiryMode == InquiryMode.Basic ? this.Role_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Search ? this.Role_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Full ? this.Role_DataInclude_Full
                                    : null;

                    #endregion

                    var Roles = await _DataHandler.Roles.GetAsync(criteria, includes);
                    return context.Response.Set(ResponseState.Success, Roles);

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
        public async Task<ExecutionResponse<Role>> Create(Role role, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Role>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    await _DataHandler.Roles.CreateAsync(role);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RoleCreated(role);
                    await RaiseEvent_PrivilegesAssociatedToRole(role, role.Privileges?.Select(x => x.PrivilegeId)?.ToList());

                    #endregion

                    return context.Response.Set(ResponseState.Success, role);

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Role>(this.RolesValidators, role, ValidationMode.Create),
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
        public async Task<ExecutionResponse<Role>> Edit(Role role, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Role>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == role.Id || x.Code == role.Code, null, this.Role_DataInclude_Full);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, role);

                    MapUpdate(existing, role, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedActors, out List<int> removedActors, out List<int> addedClaims, out List<int> removedClaims);

                    _DataHandler.Roles.Update(existing);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_PrivilegesAssociatedToRole(role, addedPrivileges.Distinct().ToList());
                    await RaiseEvent_PrivilegesDeassociatedFromRole(role, removedPrivileges.Distinct().ToList());
                    await RaiseEvent_RolesAssociatedToActor(role, addedActors.Distinct().ToList());
                    await RaiseEvent_RolesDeassociatedFromActor(role, removedActors.Distinct().ToList());

                    #endregion

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion
                }
                #region context

                 , new ActionContext()
                 {
                     Request = requestContext,
                     Validation = new ValidationContext<Role>(this.RolesValidators, role, ValidationMode.Edit),
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
        public async Task<ExecutionResponse<bool>> DeleteRole(int id, RequestContext requestContext)
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

                    #region validate.

                    var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == id);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.RolesValidators.ValidateAsync(existing, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Roles.DeleteAsync(existing.Id);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RoleDeleted(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> DeleteRole(string code, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic

                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    #region validate.

                    var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Code == code);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.RolesValidators.ValidateAsync(existing, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Roles.DeleteAsync(existing.Id);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RoleDeleted(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> ActivateRole(int id, RequestContext requestContext)
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

                    #region validate.

                    var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == id);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.RolesValidators.ValidateAsync(existing, ValidationMode.Activate);
                    if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                    #endregion

                    await _DataHandler.Roles.SetActivationAsync(existing.Id, true);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RoleActivated(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> ActivateRole(string code, RequestContext requestContext)
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

                    #region validate.

                    var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Code == code);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.RolesValidators.ValidateAsync(existing, ValidationMode.Activate);
                    if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                    #endregion

                    await _DataHandler.Roles.SetActivationAsync(existing.Id, true);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RoleActivated(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> DeactivateRole(int id, RequestContext requestContext)
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

                    #region validate.

                    var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Id == id);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.RolesValidators.ValidateAsync(existing, ValidationMode.Activate);
                    if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                    #endregion

                    await _DataHandler.Roles.SetActivationAsync(existing.Id, false);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RoleDeactivated(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }
        public async Task<ExecutionResponse<bool>> DeactivateRole(string code, RequestContext requestContext)
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

                    #region validate.

                    var existing = await _DataHandler.Roles.GetFirstAsync(x => x.Code == code);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.RolesValidators.ValidateAsync(existing, ValidationMode.Activate);
                    if (!validationResponse.IsValid) return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);

                    #endregion

                    await _DataHandler.Roles.SetActivationAsync(existing.Id, false);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    await RaiseEvent_RoleDeactivated(existing);

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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> IsUnique(Role role, RequestContext requestContext)
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


                    var isExisting = await _DataHandler.Roles.AnyAsync(x => ((role.Name != null && x.Name == role.Name.Trim()) ||
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
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> IsExists(RoleSearchCriteria criteria, RequestContext requestContext)
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

                    var isExisting = await _DataHandler.Roles.AnyAsync(criteria);
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

        #region events.
        private async Task RaiseEvent_ClaimsAssociatedTorole(Role role, List<int> addedClaims)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;
            if (!addedClaims?.Any() ?? true) return;

            await _mediator.Publish(new ClaimsAssociatedToroleDomainEvent()
            {
                App = role.AppId.ToString(),
                RoleId = role.Id,
                Claims = addedClaims
            });
        }

        private async Task RaiseEvent_ClaimsDeassociatedFromrole(Role role, List<int> removedClaims)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;
            if (!removedClaims?.Any() ?? true) return;

            await this._mediator.Publish(new ClaimsDeassociatedFromRoleDomainEvent()
            {
                RoleId = role.Id,
                Claims = removedClaims,
            });
        }
        private async Task RaiseEvent_RoleCreated(Role role)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;

            // ...
            await this._mediator.Publish(new RoleCreatedDomainEvent()
            {
                RoleId = role.Id,
                App = role.AppId.ToString(),
            });
        }
        private async Task RaiseEvent_RoleDeleted(Role role)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;

            // ...
            await this._mediator.Publish(new RoleDeletedDomainEvent()
            {
                Code = role.Code,
                App = role.AppId.ToString(),
            });
        }
        private async Task RaiseEvent_RoleActivated(Role role)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;

            // ...
            await this._mediator.Publish(new RoleActivatedDomainEvent()
            {
                Code = role.Code,
                App = role.AppId.ToString(),
            });
        }
        private async Task RaiseEvent_RoleDeactivated(Role role)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;

            // ...
            await this._mediator.Publish(new RoleDeactivatedDomainEvent()
            {
                Code = role.Code,
                App = role.AppId.ToString(),
            });
        }

        private async Task RaiseEvent_RolesAssociatedToActor(Role role, List<int> addedActors)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;
            if (!addedActors?.Any() ?? true) return;

            var addedRoles = new List<int>() { role.Id };

            foreach (var addedActorId in addedActors)
            {
                await this._mediator.Publish(new RolesAssociatedToActorDomainEvent()
                {
                    App = role.AppId.ToString(),
                    ActorId = addedActorId,
                    Roles = addedRoles,
                });
            }
        }
        private async Task RaiseEvent_RolesDeassociatedFromActor(Role role, List<int> removedActors)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;
            if (!removedActors?.Any() ?? true) return;

            var addedRoles = new List<int>() { role.Id };

            foreach (var removedActorId in removedActors)
            {
                await this._mediator.Publish(new RolesDeassociatedFromActorDomainEvent()
                {
                    ActorId = removedActorId,
                    Roles = addedRoles,
                });
            }
        }

        private async Task RaiseEvent_PrivilegesAssociatedToRole(Role role, List<int> addedPrivileges)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;
            if (!addedPrivileges?.Any() ?? true) return;

            await this._mediator.Publish(new PrivilegeAssociatedToRoleDomainEvent()
            {
                App = role.AppId.ToString(),
                RoleId = role.Id,
                PrivilegeIds = addedPrivileges,
            });
        }
        private async Task RaiseEvent_PrivilegesDeassociatedFromRole(Role role, List<int> removedPrivileges)
        {
            if (role == null) return;
            if (_mediator == null) return;
            if ((role?.Id).GetValueOrDefault() == 0) return;
            if (!removedPrivileges?.Any() ?? true) return;

            await this._mediator.Publish(new PrivilegeDeassociatedFromRoleDomainEvent()
            {
                RoleId = role.Id,
                Privileges = removedPrivileges,
            });
        }


        #endregion
        #region Map Updates.

        private bool MapUpdate(Role existing, Role updated, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedActors, out List<int> removedActors, out List<int> addedClaims, out List<int> removedClaims)
        {
            // ...
            addedPrivileges = new List<int>();
            removedPrivileges = new List<int>();
            addedActors = new List<int>();
            removedActors = new List<int>();
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
            state = ExtractUpdates(existing.Actors, updated.Actors, out addedActors, out removedActors) && state;
            state = ExtractUpdates(existing.Claims, updated.Claims, out addedClaims, out removedClaims) && state;

            existing.Privileges = updated.Privileges;
            existing.Actors = updated.Actors;
            existing.Claims = updated.Claims;

            return state;
        }


        private bool ExtractUpdates(IList<RolePrivilege> existing, IList<RolePrivilege> updated, out List<int> addedPrivileges, out List<int> removedPrivileges)
        {
            // ...
            addedPrivileges = new List<int>();
            removedPrivileges = new List<int>();

            // ...
            if (updated == null || existing == null) return false;
            addedPrivileges = new List<int>();
            removedPrivileges = new List<int>();

            // ...
            if (updated == null || existing == null) return false;
            addedPrivileges = updated.Except(updated.Where(u => existing.Any(e => e.PrivilegeId == u.PrivilegeId && e.RoleId == u.RoleId))).Select(x => x.PrivilegeId).ToList();
            removedPrivileges = existing.Except(existing.Where(u => updated.Any(e => e.PrivilegeId == u.PrivilegeId && e.RoleId == u.RoleId))).Select(x => x.PrivilegeId).ToList();
            #region obsolete

            // get [deleted records] ...
            //foreach (var existingItem in existing)
            //{
            //    var isDeleted = !updated.Any(x => x.RoleId == existingItem.RoleId
            //                                   && x.PrivilegeId == existingItem.PrivilegeId);

            //    // delete the [existingItem]
            //    if (isDeleted)
            //    {
            //        this._DataHandler.Roles.DeletedAssociatedPrivilege(existingItem);
            //        existing.Remove(existingItem);
            //        removedPrivileges.Add(existingItem.PrivilegeId);
            //    }
            //}

            //// get [inserted records] ...
            //foreach (var updatedItem in updated)
            //{
            //    var isInserted = !existing.Any(x => x.RoleId == updatedItem.RoleId
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
        private bool ExtractUpdates(IList<RoleClaim> existing, IList<RoleClaim> updated, out List<int> addedClaims, out List<int> removedClaims)
        {
            // ...
            addedClaims = new List<int>();
            removedClaims = new List<int>();

            // ...
            if (updated == null || existing == null) return false;
            addedClaims = updated.Except(updated.Where(u => existing.Any(e => e.ClaimId == u.ClaimId && e.RoleId == u.RoleId))).Select(x => x.ClaimId).ToList();
            removedClaims = existing.Except(existing.Where(u => updated.Any(e => e.ClaimId == u.ClaimId && e.RoleId == u.RoleId))).Select(x => x.ClaimId).ToList();
            #region obsolete

            // get [deleted records] ...
            //foreach (var existingItem in existing)
            //{
            //    var isDeleted = !updated.Any(x => x.RoleId == existingItem.RoleId
            //                                   && x.RoleId == existingItem.RoleId);

            //    // delete the [existingItem]
            //    if (isDeleted)
            //    {
            //        this._DataHandler.Claims.DeletedAssociatedRole(existingItem);
            //        existing.Remove(existingItem);
            //        removedClaims.Add(existingItem.RoleId);
            //    }
            //}

            //// get [inserted records] ...
            //foreach (var updatedItem in updated)
            //{
            //    var isInserted = !existing.Any(x => x.RoleId == updatedItem.RoleId
            //                                     && x.RoleId == updatedItem.RoleId);

            //    // insert the [updatedItem]
            //    if (isInserted)
            //    {
            //        existing.Add(updatedItem);
            //        addedClaims.Add(updatedItem.RoleId);
            //    }
            //}
            #endregion

            return true;
        }
        private bool ExtractUpdates(IList<ActorRole> existing, IList<ActorRole> updated,out List<int> addedActors, out List<int> removedActors)
        {
            // ...
       
            addedActors = new List<int>();
            removedActors = new List<int>();

            // ...
            if (updated == null || existing == null) return false;
            addedActors = updated.Except(updated.Where(u => existing.Any(e => e.ActorId == u.ActorId && e.RoleId == u.RoleId))).Select(x => x.ActorId).ToList();
            removedActors = existing.Except(existing.Where(u => updated.Any(e => e.ActorId == u.ActorId && e.RoleId == u.RoleId))).Select(x => x.ActorId).ToList();

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

            //        removedActors.Add(existingItem.ActorId);
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

            //        addedActors.Add(updatedItem.ActorId);
            //    }
            //}
            #endregion

            return true;
        }

        #endregion

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && InitializeIncludes();
            isValid = isValid && (this._DataHandler?.Initialized ?? false);
            isValid = isValid && RolesValidators != null;
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

                #region Role : basic

                this.Role_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Role : full

                this.Role_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "App",
                    "Actors",
                    "Privileges"
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
