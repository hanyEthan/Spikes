using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Models.Domain.Authorization;
using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Validation;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Models.Domain;
using ADS.Common.Bases.Events.Handlers;
using ADS.Tamam.Modules.Personnel.Events;
using ADS.Common.Handlers.License.Contracts;

namespace ADS.Tamam.Modules.Personnel.Handlers
{
    public class PersonnelHandler : IPersonnelHandler , ISystemPersonnelHandler , IReadOnlyPersonnelHandler, IDataProvider
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "PersonnelHandler"; } }

        private readonly PersonnelDataHandler dataHandler;

        #endregion

        #region cst.

        public PersonnelHandler()
        {
            XLogger.Info("PersonnelHandler ... Initializing ...");

            //if (TamamDataBroker.Initialized)
            //{
            //    dataHandler = TamamDataBroker.GetRegisteredDataLayer<PersonnelDataHandler>(TamamConstants.PersonnelDataHandlerName);
            //    if (dataHandler != null && dataHandler.Initialized)
            //    {
            //        XLogger.Info("PersonnelHandler ... Initialized");
            //        Initialized = true;
            //    }
            //    else
            //    {
            //        XLogger.Error("PersonnelHandler ... Initialization Failed, underlying handlers are not registered or initialized successfully.");
            //        Initialized = false;
            //    }
            //}
            //else
            {
                this.dataHandler = new PersonnelDataHandler();
                this.Initialized = this.dataHandler.Initialized;

                if (!this.dataHandler.Initialized)
                {
                    XLogger.Error("PersonnelHandler ... Initialization Failed, underlying handlers are not registered or initialized successfully.");
                    return;
                }
            }
        }

        #endregion

        #region IPersonnelHandler

        public ExecutionResponse<bool> CreatePerson(Person person, List<PolicyFieldValue> customFields, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.CreatePersonAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.CreatePersonActionKey,
                                        messageForDenied: string.Format(TamamConstants.AuthorizationConstants.CreatePersonAuditMessageFailure, person.FullName),
                                        messageForFailure: string.Format(TamamConstants.AuthorizationConstants.CreatePersonAuditMessageFailure, person.FullName),
                                        messageForSuccess: string.Format(TamamConstants.AuthorizationConstants.CreatePersonAuditMessageSuccessful, person.FullName)
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Validation

                var validator = new PersonValidator(person, TamamConstants.ValidationMode.Create);

                if (validator.IsValid != null && !validator.IsValid.Value)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person == null ? string.Empty : person.FullName);
                    context.Response.Set(ResponseState.ValidationError, false, validator.ErrorsDetailed);

                    return;
                }

                #endregion
                #region Custom Fields

                if (customFields != null && customFields.Count > 0)   //CF is defined.
                {
                    var policy = new Policy { Active = true, Code = person.Code + XString.RandomString(5), Name = person.Code + XString.RandomString(5), NameCultureVarient = person.Code+XString.RandomString(5), PolicyTypeId = Guid.Parse(PolicyTypes.PersonCustomFields), Values = customFields };

                    var createPolicyResult = TamamServiceBroker.OrganizationHandler.CreatePolicy(policy, SystemRequestContext.Instance);
                    if (createPolicyResult.Type != ResponseState.Success)
                    {
                        TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                        context.Response.Set(createPolicyResult.Type, false, createPolicyResult.MessageDetailed);
                        return;
                    }
                    person.DetailedInfo.CustomFieldId = createPolicyResult.Result;
                }

                #endregion
                #region Pre-Events

                person.Id = Guid.NewGuid();

                if (person.AccountInfo.ReportingToId.HasValue && person.AccountInfo.ReportingToId.Value != Guid.Empty)
                {
                    var managerResponse = GetPerson(person.AccountInfo.ReportingToId.Value, SystemRequestContext.Instance);
                    if (managerResponse.Type != ResponseState.Success)
                    {
                        TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                        context.Response.Set(ResponseState.ValidationError, false);
                        return;
                    }
                    var manager = managerResponse.Result;

                    person.AccountInfo.ManagerName = manager.FullName;
                    person.AccountInfo.ManagerNameCultureVarient = manager.DetailedInfo.FullNameCultureVarient;
                }
                else
                {
                    person.AccountInfo.ManagerName = null;
                    person.AccountInfo.ManagerNameCultureVarient = null;
                }

                #endregion
                #region dataLayer ...

                var result = dataHandler.CreatePerson(person);
                if (!result.Result)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                    context.Response.Set(result);
                    return;
                }

                // Create Authorization Actor..
                var securityActor = new Actor { Id = person.Id };
                var Actor_Response = Broker.AuthorizationHandler.CreateActor( securityActor );

                #endregion
             
                var state = EventsBroker.Handle( new PersonCreateEvent( person.Id ) );

                // audit ...
                TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, person.FullName);

                #region Cache
              
                Broker.Cache.Invalidate( TamamCacheClusters.Person );
              
                #endregion

                context.Response.Set(ResponseState.Success, true);

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<bool> EditPerson(Person person, List<PolicyFieldValue> customFields, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.EditPersonAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.EditPersonActionKey,
                                        messageForDenied: string.Format(TamamConstants.AuthorizationConstants.EditPersonAuditMessageFailure, person.FullName),
                                        messageForFailure: string.Format(TamamConstants.AuthorizationConstants.EditPersonAuditMessageFailure, person.FullName),
                                        messageForSuccess: string.Format(TamamConstants.AuthorizationConstants.EditPersonAuditMessageSuccessful, person.FullName)
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Validation

                var validator = new PersonValidator(person, TamamConstants.ValidationMode.Edit);
                if (validator.IsValid != null && !validator.IsValid.Value)
                {
                    context.Response.Set(ResponseState.ValidationError, false, validator.ErrorsDetailed);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                    return;
                }

                #endregion
                #region Authentication

                //// get old Person object..
                //var oldPersonResponse = GetPerson(person.Id, SystemRequestContext.Instance);
                //if (oldPersonResponse.Type != ResponseState.Success)
                //{
                //    context.Response.Set(ResponseState.Failure, false, new List<ModelMetaPair>() { new ModelMetaPair("Id", "Cannot get person object") });
                //    return;
                //}

                //var oldPerson = oldPersonResponse.Result;

                //// (1) Tamam --> Tamam  ,   Tamam --> LDAP  ,   LDAP --> LDAP
                //if ((oldPerson.AuthenticationInfo.AuthenticationMode == AuthenticationMode.Tamam && person.AuthenticationInfo.AuthenticationMode == AuthenticationMode.Tamam) ||
                //    (oldPerson.AuthenticationInfo.AuthenticationMode == AuthenticationMode.Tamam && person.AuthenticationInfo.AuthenticationMode == AuthenticationMode.LDAP) ||
                //    (oldPerson.AuthenticationInfo.AuthenticationMode == AuthenticationMode.LDAP && person.AuthenticationInfo.AuthenticationMode == AuthenticationMode.LDAP))
                //{
                //    // must update the associated identity (username)
                //    var identities = Broker.AuthenticationService.GetIdentities(oldPerson.AuthenticationInfo.Username, Constants.AuthenticationHandlers.Tamam);
                //    foreach (BaseIdentity identity in identities)
                //    {
                //        identity.Username = person.AuthenticationInfo.Username;
                //        var done = Broker.Initialized && Broker.AuthenticationService.UpdateIdentity(identity);
                //        if (!done)
                //        {
                //            context.Response.Set(ResponseState.Failure, false,
                //                new List<ModelMetaPair>()
                //            {
                //                new ModelMetaPair("AuthenticationInfo.Username", "Cannot update identity")
                //            });
                //            return;
                //        }
                //    }
                //}
                //// (2) LDAP --> Tamam
                //else if (oldPerson.AuthenticationInfo.AuthenticationMode == AuthenticationMode.LDAP && person.AuthenticationInfo.AuthenticationMode == AuthenticationMode.Tamam)
                //{
                //    var identities = Broker.AuthenticationService.GetIdentities(oldPerson.AuthenticationInfo.Username, Constants.AuthenticationHandlers.Tamam);
                //    if (identities == null || identities.Count == 0)
                //    {
                //        // create a new identity ..
                //        BaseIdentity identity = new BaseIdentity();
                //        identity.Id = Guid.NewGuid();
                //        identity.Username = person.AuthenticationInfo.Username;
                //        identity.Password = XCrypto.EncryptToMd5Hash(person.AuthenticationInfo.Password);
                //        identity.ProviderName = Constants.AuthenticationHandlers.Tamam;

                //        var done = Broker.Initialized && Broker.AuthenticationService.CreateIdentity(identity);
                //        if (!done)
                //        {
                //            context.Response.Set(ResponseState.Failure, false,
                //            new List<ModelMetaPair>()
                //            {
                //                new ModelMetaPair("AuthenticationInfo.Username", "Cannot create identity")
                //            });
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        // update existing identities..
                //        foreach (BaseIdentity identity in identities)
                //        {
                //            identity.Username = person.AuthenticationInfo.Username;
                //            var done = Broker.Initialized && Broker.AuthenticationService.UpdateIdentity(identity);
                //            if (!done)
                //            {
                //                context.Response.Set(ResponseState.Failure, false,
                //                    new List<ModelMetaPair>()
                //            {
                //                new ModelMetaPair("AuthenticationInfo.Username", "Cannot update identity")
                //            });
                //                return;
                //            }
                //        }
                //    }
                //}

                //if ( person.AuthenticationInfo.AuthenticationMode == AuthenticationMode.Tamam )
                //{
                //    person.AuthenticationInfo.Password = XCrypto.EncryptToMd5Hash( person.AuthenticationInfo.Password );
                //}

                #endregion
                #region Pre-Events

                #region original Person

                var personOriginalResponse = TamamServiceBroker.PersonnelHandler.GetPerson(person.Id, requestContext);
                if (personOriginalResponse.Type != ResponseState.Success)
                {
                    context.Response.Set(personOriginalResponse.Type, false, personOriginalResponse.MessageDetailed);
                    return;
                }

                var personOriginal = personOriginalResponse.Result;

                #endregion

                #region Manager Name

                if (person.AccountInfo.ReportingToId.HasValue && person.AccountInfo.ReportingToId.Value != Guid.Empty)
                {
                    var managerResponse = GetPerson(person.AccountInfo.ReportingToId.Value, SystemRequestContext.Instance);
                    if (managerResponse.Type != ResponseState.Success)
                    {
                        TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                        context.Response.Set(ResponseState.ValidationError, false);
                        return;
                    }
                    var manager = managerResponse.Result;

                    person.AccountInfo.ManagerName = manager.FullName;
                    person.AccountInfo.ManagerNameCultureVarient = manager.DetailedInfo.FullNameCultureVarient;
                }
                else
                {
                    person.AccountInfo.ManagerName = null;
                    person.AccountInfo.ManagerNameCultureVarient = null;
                }

                #endregion
                #region Custom Fields

                if (customFields != null && customFields.Count > 0)   //CF is defined.
                {
                    if (person.DetailedInfo.CustomFieldId.HasValue)
                    {
                        var policy = new Policy
                        {
                            Id = person.DetailedInfo.CustomFieldId.Value,
                            Active = true,
                            Code =person.Code+ XString.RandomString(5),
                            Name = person.Code + XString.RandomString(5),
                            NameCultureVarient = person.Code + XString.RandomString(5),
                            PolicyTypeId = Guid.Parse(PolicyTypes.PersonCustomFields),
                            Values = customFields
                        };

                        var updatePolicyResult = TamamServiceBroker.OrganizationHandler.EditPolicy(policy, SystemRequestContext.Instance);
                        if (updatePolicyResult.Type != ResponseState.Success)
                        {
                            // audit : failure ...
                            TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                            context.Response.Set(updatePolicyResult.Type, false, updatePolicyResult.MessageDetailed);
                            return;
                        }
                    }
                    else
                    {
                        var policy = new Policy
                        {
                            Active = true,
                            Code = person.Code + XString.RandomString(5),
                            Name = person.Code + XString.RandomString(5),
                            NameCultureVarient = person.Code + XString.RandomString(5),
                            PolicyTypeId = Guid.Parse(PolicyTypes.PersonCustomFields),
                            Values = customFields
                        };
                        var createPolicyResult = TamamServiceBroker.OrganizationHandler.CreatePolicy(policy, SystemRequestContext.Instance);
                        if (createPolicyResult.Type != ResponseState.Success)
                        {
                            // audit : failure ...
                            TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                            context.Response.Set(createPolicyResult.Type, false, createPolicyResult.MessageDetailed);
                            return;
                        }
                        person.DetailedInfo.CustomFieldId = createPolicyResult.Result;
                    }
                }

                #endregion

                #endregion
                #region dataLayer ...

                var result = dataHandler.EditPerson(person);
                if (!result.Result)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                    context.Response.Set(result);
                    return;
                }

                #endregion
                
                var state = EventsBroker.Handle( new PersonEditEvent( person.Id , personOriginal ) );
                
                // audit ...
                TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, person.FullName);

                #region Cache
              
                Broker.Cache.Invalidate( TamamCacheClusters.Person );
               
                #endregion

                context.Response.Set(ResponseState.Success, true);

                #endregion
            }, requestContext);

            return context.Response;
        }

        public ExecutionResponse<bool> EditPersonPassword(Person person, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.EditPersonPasswordActionAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.EditPersonPasswordActionKey,
                                        messageForDenied: string.Format(TamamConstants.AuthorizationConstants.EditPersonPasswordAuditMessageFailure, person.Id),
                                        messageForFailure: string.Format(TamamConstants.AuthorizationConstants.EditPersonPasswordAuditMessageFailure, person.Id),
                                        messageForSuccess: string.Format(TamamConstants.AuthorizationConstants.EditPersonPasswordAuditMessageSuccessful, person.Id)
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Validation

                var validator = new PersonValidator(person, TamamConstants.ValidationMode.EditIdentity);
                if (validator.IsValid != null && !validator.IsValid.Value)
                {
                    context.Response.Set(ResponseState.ValidationError, false, validator.ErrorsDetailed);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, person.FullName);
                    return;
                }

                #endregion
                #region dataLayer

                if (person.AuthenticationInfo.AuthenticationMode == AuthenticationMode.Tamam)
                    person.AuthenticationInfo.Password = XCrypto.EncryptToMd5Hash(person.AuthenticationInfo.Password);

                var result = dataHandler.EditPerson(person).Result;

                if (!result)
                {
                    context.Response.Set(ResponseState.Failure, false, new List<ModelMetaPair>() { new ModelMetaPair("Id", "Failed to edit Person Password") });
                    return;
                }

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion

                // audit ...
                TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, person.Id.ToString());
                context.Response.Set(ResponseState.Success, true, null);

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<bool> EditPersonSecurity(Actor actor, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.EditPersonSecurityActionAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.EditPersonSecurityActionKey,
                                        messageForDenied: string.Format(TamamConstants.AuthorizationConstants.EditPersonSecurityAuditMessageFailure, actor.Id),
                                        messageForFailure: string.Format(TamamConstants.AuthorizationConstants.EditPersonSecurityAuditMessageFailure, actor.Id),
                                        messageForSuccess: string.Format(TamamConstants.AuthorizationConstants.EditPersonSecurityAuditMessageSuccessful, actor.Id)
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region dataLayer

                var result = Broker.AuthorizationHandler.UpdateActor(actor);
                if (!result)
                {
                    context.Response.Set(ResponseState.Failure, false, new List<ModelMetaPair>() { new ModelMetaPair("Id", "Failed to edit Person Security") });
                    return;
                }

                #endregion
                #region Cache

                //Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion

                // audit ...
                TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, actor.Id.ToString());
                context.Response.Set(ResponseState.Success, true, null);

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<bool> EditPersonStatus(Guid personId, bool activeStatus, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.ChangePersonActiveStateAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.ChangePersonActiveStateActionKey,
                                        messageForDenied: string.Format(TamamConstants.AuthorizationConstants.ChangePersonActiveStateAuditMessageFailure, personId.ToString()),
                                        messageForFailure: string.Format(TamamConstants.AuthorizationConstants.ChangePersonActiveStateAuditMessageFailure, personId.ToString()),
                                        messageForSuccess: string.Format(TamamConstants.AuthorizationConstants.ChangePersonActiveStateAuditMessageSuccessful, personId.ToString())
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Validation

                var response_person = dataHandler.GetPerson(personId, SystemSecurityContext.Instance);
                if (response_person.Type != ResponseState.Success)
                {
                    context.Response.Set(response_person.Type, false, response_person.MessageDetailed);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personId.ToString());
                    return;
                }

                var validator = new PersonValidator(response_person.Result, activeStatus ? TamamConstants.ValidationMode.Activate : TamamConstants.ValidationMode.Deactivate);
                if (validator.IsValid != null && !validator.IsValid.Value)
                {
                    context.Response.Set(ResponseState.ValidationError, false, validator.ErrorsDetailed);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personId.ToString());
                    return;
                }

                // Check if the Logged In Person is the same activated / de-activated person
                if (requestContext.PersonId.HasValue)
                {
                    var isTheSameLoggedInPerson = personId == requestContext.PersonId.Value;
                    if (isTheSameLoggedInPerson)
                    {
                        context.Response.Set(ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair(string.Empty, Resources.Culture.Personnel.InvalidPersonToActivateOrDeactivate) });
                        TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personId.ToString());
                        return;
                    }
                }

                #endregion
                #region DataLayer ...

                // Activate / Deactivate associated User
                var person = dataHandler.GetPerson(personId, SystemSecurityContext.Instance).Result;

                var result = dataHandler.EditPersonStatus(personId, activeStatus);
                if (!result.Result)
                {
                    context.Response.Set(result);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personId.ToString());
                    return;
                }

                #endregion

                bool state = false;

                if ( activeStatus )
                {
                    state = EventsBroker.Handle( new PersonActivateEvent( personId ) );
                }
                else
                {
                    state = EventsBroker.Handle( new PersonDeactivateEvent( personId ) );
                }

                // audit ...
                TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, personId.ToString());

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion
                
                context.Response.Set(ResponseState.Success, true);

                #endregion
            }, requestContext);

            return context.Response;
        }

        public ExecutionResponse<Person> GetPerson(Guid personId, RequestContext requestContext)
        {
            var context = new ExecutionContext<Person>();
            context.Execute(() =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "PersonnelHandler_GetPerson" + personId + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Person>( TamamCacheClusters.Person , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.GetPersonAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.GetPersonActionKey,
                                        messageForDenied: string.Format(TamamConstants.AuthorizationConstants.GetPersonAuditMessageFailure, personId.ToString()),
                                        messageForFailure: string.Format(TamamConstants.AuthorizationConstants.GetPersonAuditMessageFailure, personId.ToString()),
                                        messageForSuccess: string.Empty
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetPerson(personId, securityContext);

                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure,string.Empty);

                    return;
                }

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Person>( TamamCacheClusters.Person , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<Person> GetPerson(string username, RequestContext requestContext)
        {
            var context = new ExecutionContext<Person>();
            context.Execute(() =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "PersonnelHandler_GetPerson" + username + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Person>( TamamCacheClusters.Person , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetPersonAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetPersonActionKey,
                    messageForDenied: string.Format(TamamConstants.AuthorizationConstants.GetPersonAuditMessageFailure, username),
                    messageForFailure: string.Format(TamamConstants.AuthorizationConstants.GetPersonAuditMessageFailure, username),
                    messageForSuccess: string.Empty
                    );

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetPerson(username);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, string.Empty);

                    return;
                }

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Person>( TamamCacheClusters.Person , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<Person> GetPersonByIdentifier(string identifier, RequestContext requestContext)
        {
            var context = new ExecutionContext<Person>();
            context.Execute(() =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "PersonnelHandler_GetPersonByIdentifier" + identifier + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Person>( TamamCacheClusters.Person , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetPersonAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetPersonActionKey,
                    messageForDenied: string.Format(TamamConstants.AuthorizationConstants.GetPersonAuditMessageFailure, identifier),
                    messageForFailure: string.Format(TamamConstants.AuthorizationConstants.GetPersonAuditMessageFailure, identifier),
                    messageForSuccess: string.Empty
                    );

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetPersonByIdentifier(identifier);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure,string.Empty);

                    return;
                }

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Person>( TamamCacheClusters.Person , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<PersonSearchResult> GetPersonnel(PersonSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<PersonSearchResult>();
            context.Execute(() =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "PersonnelHandler_SearchPersons" + criteria + requestContext;
                var cached = Broker.Cache.Get<PersonSearchResult>(TamamCacheClusters.Person, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }
                
                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.SearchPersonnelAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.SearchPersonnelActionKey,
                                        messageForDenied: TamamConstants.AuthorizationConstants.SearchPersonnelAuditMessageAccessDenied,
                                        messageForFailure: TamamConstants.AuthorizationConstants.SearchPersonnelAuditMessageFailure,
                                        messageForSuccess: string.Empty
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetPersonnel(criteria, securityContext);
                if (dataHandlerResponse.Result == null)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure,string.Empty);
                    context.Response.Set(dataHandlerResponse);
                    return;
                }

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<PersonSearchResult>(TamamCacheClusters.Person, cacheKey, dataHandlerResponse.Result);

                #endregion

                #endregion

            }, requestContext);

            return context.Response;
        }

        public ExecutionResponse<PersonnelDelegatesSearchResult> GetPersonnelDelegates( PersonnelDelegatesSearchCriteria criteria , RequestContext requestContext )
        {
            var context = new ExecutionContext<PersonnelDelegatesSearchResult>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "PersonnelHandler_GetPersonnelDelegates" + criteria + requestContext;
                var cached = Broker.Cache.Get<PersonnelDelegatesSearchResult>( TamamCacheClusters.Person , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId ,
                                        actionId: TamamConstants.AuthorizationConstants.SearchDelegatesAuditActionId ,
                                        actionKey: TamamConstants.AuthorizationConstants.SearchDelegatesActionKey ,
                                        messageForDenied: TamamConstants.AuthorizationConstants.SearchDelegatesAuditMessageAccessDenied ,
                                        messageForFailure: TamamConstants.AuthorizationConstants.SearchDelegatesAuditMessageFailure ,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.SearchDelegatesAuditMessageSuccessful
                                        );

                var response = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetPersonnelDelegates( criteria , securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext , context.ActionContext , context.ActionContext.MessageForFailure , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<PersonnelDelegatesSearchResult>( TamamCacheClusters.Person , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> CreateDelegate( PersonDelegate personDelegate , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.CreateDelegateAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.CreateDelegateActionKey,
                                        messageForDenied: TamamConstants.AuthorizationConstants.CreateDelegateAuditMessageFailure,
                                        messageForFailure: TamamConstants.AuthorizationConstants.CreateDelegateAuditMessageFailure,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.CreateDelegateAuditMessageSuccessful
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Validation

                var validator = new DelegateValidator(personDelegate, TamamConstants.ValidationMode.Create);

                if (validator.IsValid != null && !validator.IsValid.Value)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personDelegate.Code);
                    context.Response.Set(ResponseState.ValidationError, false, validator.ErrorsDetailed);
                    return;
                }

                #endregion
                #region Authentication

                #endregion
                #region dataLayer ...

                var dataHandlerResult = dataHandler.CreateDelegate(personDelegate);
                if (!dataHandlerResult.Result)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personDelegate.Code);
                    context.Response.Set(dataHandlerResult);
                    return;
                }

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion
                #region events

                #endregion

                // audit ...
                TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, personDelegate.Code);
                context.Response.Set(ResponseState.Success, true);

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<bool> EditDelegate(PersonDelegate personDelegate, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.PersonAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.EditDelegateAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.EditDelegateActionKey,
                                        messageForDenied: TamamConstants.AuthorizationConstants.EditDelegateAuditMessageFailure,
                                        messageForFailure: TamamConstants.AuthorizationConstants.EditDelegateAuditMessageFailure,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.EditDelegateAuditMessageSuccessful
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Validation

                var validator = new DelegateValidator(personDelegate, TamamConstants.ValidationMode.Create);

                if (validator.IsValid != null && !validator.IsValid.Value)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personDelegate.Code);
                    context.Response.Set(ResponseState.ValidationError, false, validator.ErrorsDetailed);
                    return;
                }

                #endregion
                #region Authentication

                #endregion
                #region dataLayer ...

                var result = dataHandler.EditDelegate(personDelegate);
                if (!result.Result)
                {
                    TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForFailure, personDelegate.Code);
                    context.Response.Set(result);
                    return;
                }

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion
                #region events


                #endregion

                // audit ...
                TamamServiceBroker.Audit(requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, personDelegate.Code);
                context.Response.Set(ResponseState.Success, true);

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<bool> CanActFor( Guid delegateId , Guid personId , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "PersonnelHandler_CanActFor" + delegateId + personId + requestContext;
                var cached = Broker.Cache.Get<bool?>( TamamCacheClusters.Person , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached.Value );
                    return;
                }

                #endregion
                #region Logic ...

                var canActFor = true;

                if ( delegateId != personId )
                {
                    var canActResponse = dataHandler.CanActFor( delegateId , personId , SystemSecurityContext.Instance );
                    if ( canActResponse.Type != ResponseState.Success )
                    {
                        context.Response.Set( canActResponse.Type , false );
                        return;
                    }

                    canActFor = canActResponse.Result;
                }

                #endregion
                #region Cache

                Broker.Cache.Add<bool?>( TamamCacheClusters.Person , cacheKey , canActFor );

                #endregion

                context.Response.Set( ResponseState.Success , canActFor );
            } );

            return context.Response;
        }

        public ExecutionResponse<object> getInfo()
        {
            var context = new ExecutionContext<object>();
            context.Execute(() =>
            {

                #region Logic ...

                var response = dataHandler.GetPersonnelCount();
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }


                #endregion


                context.Response.Set(ResponseState.Success, response.Result);
            });

            return context.Response;
            //return dataHandler.GetPersonnelCount();
        }

        #endregion
        #region ISystemPersonnelHandler

        public ExecutionResponse<object> GetPersonnelByRoot(Guid rootId, RequestContext requestContext)
        {
            var context = new ExecutionContext<object>();
            context.Execute(() =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "PersonnelHandler_GetPersonnelByRoot" + rootId + requestContext;
                var cached = Broker.Cache.Get<string>( TamamCacheClusters.Person , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                //// authentication ...
                //if ( !TamamServiceBroker.Authenticate( requestContext ) )
                //{
                //    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                //    return;
                //}

                //// authorization ...
                //if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.GetPersonActionKey ) )
                //{
                //    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetPersonAuditActionId , TamamConstants.AuthorizationConstants.PersonAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.GetPersonAuditMessageFailure , rootId ) , string.Empty );
                //    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                //    return;
                //}

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetPersonnelByRoot(rootId);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);

                    return;
                }

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<string>(TamamCacheClusters.Person, cacheKey, (string)dataHandlerResponse.Result);

                #endregion

                #endregion
            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<List<Guid>> GetPersonnelWithUnTransferredCredits()
        {
            var context = new ExecutionContext<List<Guid>>();
            context.Execute(() =>
            {
                #region Cache

                //var cacheKey = "PersonnelHandler_GetUnTransferredCreditsPeople";
                //var cached = Broker.Cache.Get<List<Guid>>( TamamCacheClusters.Person , cacheKey );
                //if ( cached != null )
                //{
                //    context.Response.Set( ResponseState.Success , cached );
                //    return;
                //}

                #endregion
                #region logic ...

                // get people ...
                var response_people = dataHandler.GetPersonnel();
                if (response_people.Type != ResponseState.Success)
                {
                    context.Response.Set(ResponseState.Failure, null, new List<ModelMetaPair> { new ModelMetaPair(string.Empty, Resources.Culture.Personnel.SearchPersonsFailed) });
                    return;
                }

                var ids = response_people.Result.Select(x => x.Id).ToList();

                // check people credits
                var list = new List<Guid>();
                foreach (var id in ids)
                {
                    var response = TamamServiceBroker.LeavesHandler.GetCurrentLeaveCredit(id, SystemRequestContext.Instance);
                    if ( response.Result == null ) list.Add( id );
                }

                context.Response.Set( ResponseState.Success , list );

                #endregion
                #region Cache

                //Broker.Cache.Add<List<Guid>>( TamamCacheClusters.Person , cacheKey , list );

                #endregion
            });

            return context.Response;
        }

        #endregion
    }
}