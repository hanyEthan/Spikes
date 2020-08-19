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
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Handlers
{
   public class ClaimHandler : IClaimHandler
    {
        #region props.

        private string Claim_DataInclude_Full { get; set; }
        private string Claim_DataInclude_Basic { get; set; }


        private readonly ISecurityDataUnity _DataHandler;
        private readonly IModelValidator<Claim> ClaimValidators;


        #endregion
        #region cst.

        public ClaimHandler(ISecurityDataUnity dataHandler,  IModelValidator<Claim> claimValidators)
        {

            this.ClaimValidators = claimValidators;

            this._DataHandler = dataHandler;

            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SecurityHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IClaimHandler

        public async Task<ExecutionResponse<SearchResults<Claim>>> Get(ClaimSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Claim>>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    #region mode.

                    string includes = criteria.InquiryMode == InquiryMode.Basic ? this.Claim_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Search ? this.Claim_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Full ? this.Claim_DataInclude_Full
                                    : null;

                    #endregion

                    var Claims = await _DataHandler.Claims.GetAsync(criteria, includes);
                    return context.Response.Set(ResponseState.Success, Claims);

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
        public async Task<ExecutionResponse<Claim>> Create(Claim Claim, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Claim>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    await _DataHandler.Claims.CreateAsync(Claim);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    //await RaiseEvent_ClaimCreated(Claim);
                    //await RaiseEvent_PrivilegesAssociatedToClaim(Claim, Claim.Privileges?.Select(x => x.PrivilegeId)?.ToList());

                    #endregion

                    return context.Response.Set(ResponseState.Success, Claim);

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                    Validation = new ValidationContext<Claim>(this.ClaimValidators, Claim, ValidationMode.Create),
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
        public async Task<ExecutionResponse<Claim>> Edit(Claim Claim, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<Claim>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL

                    var existing = await _DataHandler.Claims.GetFirstAsync(x => x.Id == Claim.Id || x.Code == Claim.Code, null, this.Claim_DataInclude_Full);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, Claim);

                    MapUpdate(existing, Claim, out List<int> addedPrivileges, out List<int> removedPrivileges, out List<int> addedActors, out List<int> removedActors);

                    _DataHandler.Claims.Update(existing);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    //await RaiseEvent_PrivilegesAssociatedToClaim(Claim, addedPrivileges.Distinct().ToList());
                    //await RaiseEvent_PrivilegesDeassociatedFromClaim(Claim, removedPrivileges.Distinct().ToList());
                    //await RaiseEvent_ClaimsAssociatedToActor(Claim, addedActors.Distinct().ToList());
                    //await RaiseEvent_ClaimsDeassociatedFromActor(Claim, removedActors.Distinct().ToList());

                    #endregion

                    return context.Response.Set(ResponseState.Success, existing);

                    #endregion
                }
                #region context

                 , new ActionContext()
                 {
                     Request = requestContext,
                     Validation = new ValidationContext<Claim>(this.ClaimValidators, Claim, ValidationMode.Edit),
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
        public async Task<ExecutionResponse<bool>> DeleteClaim(int id, RequestContext requestContext)
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

                    var existing = await _DataHandler.Claims.GetFirstAsync(x => x.Id == id);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.ClaimValidators.ValidateAsync(existing, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Claims.DeleteAsync(existing.Id);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    //await RaiseEvent_ClaimDeleted(existing);

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
        public async Task<ExecutionResponse<bool>> DeleteClaim(string code, RequestContext requestContext)
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

                    var existing = await _DataHandler.Claims.GetFirstAsync(x => x.Code == code);
                    if (existing == null) return context.Response.Set(ResponseState.NotFound, false);

                    var validationResponse = await this.ClaimValidators.ValidateAsync(existing, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }

                    #endregion

                    await _DataHandler.Claims.DeleteAsync(existing.Id);
                    await _DataHandler.SaveAsync();

                    #endregion
                    #region events.

                    //await RaiseEvent_ClaimDeleted(existing);

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
        public async Task<ExecutionResponse<bool>> IsUnique(Claim Claim, RequestContext requestContext)
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


                    var isExisting = await _DataHandler.Claims.AnyAsync(x => ((Claim.Name != null && x.Name == Claim.Name.Trim()) ||
                                                                 (Claim.NameCultured != null && x.NameCultured == Claim.NameCultured.Trim()))
                                                                 &&
                                                                 (x.Code == Claim.Code)
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
        public async Task<ExecutionResponse<bool>> IsExists(ClaimSearchCriteria criteria, RequestContext requestContext)
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

                    var isExisting = await _DataHandler.Claims.AnyAsync(criteria);
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

        #region Map Updates.

        //private bool MapUpdate(App existing, App updated)
        //{
        //    if (updated == null || existing == null) return false;

        //    existing.Code = updated.Code;
        //    existing.Description = updated.Description;
        //    existing.IsActive = updated.IsActive;
        //    existing.MetaData = updated.MetaData;
        //    existing.ModifiedBy = updated.ModifiedBy;
        //    existing.ModifiedDate = updated.ModifiedDate;
        //    existing.Name = updated.Name;
        //    existing.NameCultured = updated.NameCultured;
        //    existing.Actors = updated.Actors;
        //    existing.Privileges = updated.Privileges;
        //    existing.Roles = updated.Roles;
        //    existing.Targets = updated.Targets;

        //    return true;
        //}
        private bool MapUpdate(Claim existing, Claim updated, out List<int> addedRoles, out List<int> removedRoles, out List<int> addedActors, out List<int> removedActors)
        {
            // ...
            addedRoles = new List<int>();
            removedRoles = new List<int>();
            addedActors = new List<int>();
            removedActors = new List<int>();

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
            //existing.Roles = updated.Roles;
            //existing.Actors = updated.Actors;

            //state = MapUpdate(existing.App, updated.App) && state;
            //state = MapUpdate(existing.Roles, updated.Roles, out addedRoles, out removedRoles) && state;
            //state = MapUpdate(existing.Actors, updated.Actors, out List<int> addedClaims, out List<int> removedClaims, out addedActors, out removedActors) && state;

            return state;
        }

        //private bool MapUpdate(IList<RoleClaim> existing, IList<RoleClaim> updated, out List<int> addedRoles, out List<int> removedRoles)
        //{
        //    // ...
        //    addedRoles = new List<int>();
        //    removedRoles = new List<int>();

        //    // ...
        //    if (updated == null || existing == null) return false;

        //    // get [deleted records] ...
        //    foreach (var existingItem in existing)
        //    {
        //        var isDeleted = !updated.Any(x => x.RoleId == existingItem.RoleId
        //                                       && x.RoleId == existingItem.RoleId);

        //        // delete the [existingItem]
        //        if (isDeleted)
        //        {
        //            this._DataHandler.Claims.DeletedAssociatedRole(existingItem);
        //            existing.Remove(existingItem);
        //            removedRoles.Add(existingItem.RoleId);
        //        }
        //    }

        //    // get [inserted records] ...
        //    foreach (var updatedItem in updated)
        //    {
        //        var isInserted = !existing.Any(x => x.RoleId == updatedItem.RoleId
        //                                         && x.RoleId == updatedItem.RoleId);

        //        // insert the [updatedItem]
        //        if (isInserted)
        //        {
        //            existing.Add(updatedItem);
        //            addedRoles.Add(updatedItem.RoleId);
        //        }
        //    }

        //    return true;
        //}
        //private bool MapUpdate(IList<ActorClaim> existing, IList<ActorClaim> updated, out List<int> addedClaims, out List<int> removedClaims, out List<int> addedActors, out List<int> removedActors)
        //{
        //    // ...
        //    addedClaims = new List<int>();
        //    removedClaims = new List<int>();
        //    addedActors = new List<int>();
        //    removedActors = new List<int>();

        //    // ...
        //    if (updated == null || existing == null) return false;

        //    // get [deleted records] ...
        //    foreach (var existingItem in existing)
        //    {
        //        var isDeleted = !updated.Any(x => x.ClaimId == existingItem.ClaimId
        //                                       && x.ActorId == existingItem.ActorId);

        //        // delete the [existingItem]
        //        if (isDeleted)
        //        {
        //            this._DataHandler.Claims.DeletedAssociatedActor(existingItem);
        //            existing.Remove(existingItem);

        //            removedClaims.Add(existingItem.ClaimId);
        //            removedActors.Add(existingItem.ActorId);
        //        }
        //    }

        //    // get [inserted records] ...
        //    foreach (var updatedItem in updated)
        //    {
        //        var isInserted = !existing.Any(x => x.ClaimId == updatedItem.ClaimId
        //                                         && x.ActorId == updatedItem.ActorId);

        //        // insert the [updatedItem]
        //        if (isInserted)
        //        {
        //            existing.Add(updatedItem);

        //            addedClaims.Add(updatedItem.ClaimId);
        //            addedActors.Add(updatedItem.ClaimId);
        //        }
        //    }

        //    return true;
        //}

        #endregion

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && InitializeIncludes();

            isValid = isValid && (this._DataHandler?.Initialized ?? false);
            isValid = isValid && ClaimValidators != null;

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
               
                #region Claim : basic

                this.Claim_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Claim : full

                this.Claim_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "App",
                    "Roles",
                    "Actors",
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
