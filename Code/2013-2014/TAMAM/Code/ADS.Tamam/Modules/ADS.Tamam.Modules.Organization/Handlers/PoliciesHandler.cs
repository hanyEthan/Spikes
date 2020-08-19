using System;
using System.Linq;
using System.Collections.Generic;
using ADS.Common.Bases.Events.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Validation;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Common.Validation;
using ADS.Tamam.Modules.Organization.Events;
using OrganizationConstants = ADS.Tamam.Resources.Culture.Organization;
using PoliciesConstants = ADS.Tamam.Resources.Culture.Policies;

namespace ADS.Tamam.Modules.Organization.Handlers
{
    public partial class OrganizationHandler : IOrganizationHandler, ISystemOrganizationHandler
    {
        #region IOrganizationHandler

        #region Policies

        #region policy type

        public ExecutionResponse<PolicyType> GetPolicyType( Guid id , RequestContext requestContext )
        {
            var context = new ExecutionContext<PolicyType>();
            context.Execute( () =>
            {
                #region logic ...

                #region cache

                var cacheKey = "OrganizationHandler_GetPolicyType" + id + requestContext;
                var cached = Broker.Cache.Get<PolicyType>( TamamCacheClusters.Policies , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.PolicyGetActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyGetAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region data layer

                var dataHandlerResponse = dataHandler.GetPolicyType( id );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyGetAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                var policyType = dataHandlerResponse.Result;

                #endregion
                #region cache

                Broker.Cache.Add<PolicyType>( TamamCacheClusters.Policies , cacheKey , policyType );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion

            } , requestContext );

            return context.Response;
        }

        #endregion
        #region policy

        public ExecutionResponse<Policy> GetPolicy( Guid id , RequestContext requestContext )
        {
            var context = new ExecutionContext<Policy>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicy" + id + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Policy>( TamamCacheClusters.Policies , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
                }

                #endregion
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.PolicyGetActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.PolicyGetAuditActionId ,
                        TamamConstants.AuthorizationConstants.PoliciesAuditModuleId ,
                        TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetPolicy( id );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyGetAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Policy>( TamamCacheClusters.Policies , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<Guid> CreatePolicy( Policy policy , RequestContext requestContext )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.PolicyCreateActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyCreateAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyCreateAuditMessageFailure , policy == null ? string.Empty : policy.Name );
                    context.Response.Set( ResponseState.AccessDenied , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                // validation ...
                var validator = new PolicyValidator( policy , PolicyValidator.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyCreateAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyCreateAuditMessageFailure , policy == null ? string.Empty : policy.Name );
                    context.Response.Set( ResponseState.ValidationError , Guid.Empty , validator.ErrorsDetailed );
                    return;
                }

                // custom validation ...
                var policyType = GetPolicyType( policy.PolicyTypeId , requestContext ).Result;
                if ( !string.IsNullOrEmpty( policyType.CustomValidatorType ) )
                {
                    var customValidator = XReflector.GetInstance<IModelValidator>( policyType.CustomValidatorType , policy , TamamConstants.ValidationMode.Create );
                    if ( customValidator.IsValid.HasValue && customValidator.IsValid.Value == false )
                    {
                        context.Response.Set( ResponseState.ValidationError , Guid.Empty , customValidator.ErrorsDetailed );
                        return;
                    }
                }

                #endregion
                #region Data Layer ...

                var dataHandlerResponse = dataHandler.CreatePolicy( policy );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyCreateAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyCreateAuditMessageFailure , policy.Name );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion
                #region audit ...

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyCreateAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.PolicyCreateAuditMessageSuccessful , policy.PolicyType == null ? string.Empty : PolicyTypes.Names[policy.PolicyType.Id.ToString()] ) , dataHandlerResponse.Result.ToString() );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditPolicy( Policy policy , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.PolicyEditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyEditAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.PolicyEditAuditMessageFailure , policy == null || policy.PolicyType == null ? string.Empty : PolicyTypes.Names[policy.PolicyType.Id.ToString()] , policy == null ? string.Empty : policy.Id.ToString() ) , policy == null ? string.Empty : policy.Id.ToString() );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation

                // validation ...
                var validator = new PolicyValidator( policy , PolicyValidator.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyEditAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.PolicyEditAuditMessageFailure , policy == null || policy.PolicyType == null ? string.Empty : PolicyTypes.Names[policy.PolicyType.Id.ToString()] , policy == null ? string.Empty : policy.Id.ToString() ) , policy == null ? string.Empty : policy.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                    return;
                }
                //check for custom validation
                var policyType = GetPolicyType( policy.PolicyTypeId , requestContext ).Result;
                if ( !string.IsNullOrEmpty( policyType.CustomValidatorType ) )
                {
                    var customValidator = XReflector.GetInstance<IModelValidator>( policyType.CustomValidatorType , policy , TamamConstants.ValidationMode.Edit );
                    if ( customValidator.IsValid.HasValue && customValidator.IsValid.Value == false )
                    {
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyEditAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.PolicyEditAuditMessageFailure , policy.PolicyType == null ? string.Empty : PolicyTypes.Names[policy.PolicyType.Id.ToString()] , policy.Id ) , policy.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError , false , customValidator.ErrorsDetailed );
                        return;
                    }
                }

                #endregion
                #region Data Layer

                var dataHandlerResponse = dataHandler.EditPolicy( policy );
                if ( dataHandlerResponse.Result == false )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyEditAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.PolicyEditAuditMessageFailure , policy.PolicyType == null ? string.Empty : PolicyTypes.Names[policy.PolicyType.Id.ToString()] , policy.Id ) , policy.Id.ToString() );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new PolicyEditEvent( policy ) );

                #region Audit

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyEditAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.PolicyEditAuditMessageSuccessful , policy.PolicyType == null ? string.Empty : PolicyTypes.Names[policy.PolicyType.Id.ToString()] , policy.Id ) , policy.Id.ToString() );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> DeletePolicy( Guid id , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.PolicyDeleteActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyDeleteAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyDeleteAuditMessageFailure , id.ToString() );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region prepare.

                var response_get = dataHandler.GetPolicy( id );
                if ( response_get.Type != ResponseState.Success )
                {
                    context.Response.Set( response_get.Type , false , response_get.MessageDetailed );
                    return;
                }

                #endregion
                #region validation ...

                var validator = new PolicyValidator( response_get.Result , PolicyValidator.ValidationMode.Delete );
                if ( validator.IsValid.HasValue && validator.IsValid.Value == false )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.PolicyDeleteAuditActionId ,
                        TamamConstants.AuthorizationConstants.PoliciesAuditModuleId ,
                        TamamConstants.AuthorizationConstants.PolicyDeleteAuditMessageFailure , id.ToString() );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                    return;
                }
                // custom validation ...
                var policyType = GetPolicyType( response_get.Result.PolicyTypeId , requestContext ).Result;
                if ( !string.IsNullOrEmpty( policyType.CustomValidatorType ) )
                {
                    var customValidator = XReflector.GetInstance<IModelValidator>( policyType.CustomValidatorType ,
                        response_get.Result , TamamConstants.ValidationMode.Delete );
                    if ( customValidator.IsValid != null && !customValidator.IsValid.Value )
                    {
                        context.Response.Set( ResponseState.ValidationError , false , customValidator.ErrorsDetailed );
                        return;
                    }
                }

                #endregion
                #region data layer

                var dataHandlerResponse = dataHandler.DeletePolicy( id );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyDeleteAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyDeleteAuditMessageFailure , id.ToString() );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion
                #region audit ...

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.PolicyDeleteAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId , TamamConstants.AuthorizationConstants.PolicyDeleteAuditMessageSuccessful , id.ToString() );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            } , requestContext );
            return context.Response;
        }

        #endregion

        public ExecutionResponse<List<Policy>> GetPolicies( Person person, RequestContext requestContext )
        {
            return GetPolicies( person, new PolicyFilters( Guid.Empty, true ), requestContext );
        }
        public ExecutionResponse<List<Policy>> GetPolicies( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Policy>>();
            context.Execute( () =>
            {
                #region logic ...

                #region cache

                var cacheKey = "OrganizationHandler_GetPolicies" + personId + requestContext;
                var cached = Broker.Cache.Get<List<Policy>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region data layer

                var personnelDataHandler = new PersonnelDataHandler();
                var person = personnelDataHandler.GetPerson( personId, SystemSecurityContext.Instance ).Result;
                var response = GetPolicies( person, new PolicyFilters( Guid.Empty, true ), requestContext );

                #endregion
                #region cache

                if ( response.Type == ResponseState.Success ) Broker.Cache.Add<List<Policy>>( TamamCacheClusters.Policies, cacheKey, response.Result );

                #endregion

                context.Response.Set( response );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Policy>> GetPolicies( Person person, PolicyFilters filter, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Policy>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicies" + person.Id + filter + requestContext;
                var cached = Broker.Cache.Get<List<Policy>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region logic ...

                var policies = new List<Policy>();

                // from policy group ...
                if ( person.AccountInfo.PolicyGroupId.HasValue )
                {
                    foreach ( var policy in GetPolicyGroupPolicies( person.AccountInfo.PolicyGroupId.Value, filter, requestContext ).Result )
                    {
                        AddPolicyToUniqueList( policies, policy );
                    }
                }

                // from departments ...                
                var department = person.AccountInfo.Department;
                do
                {
                    if ( department.PolicyGroupId.HasValue )
                    {
                        var response = GetPolicyGroupPolicies( department.PolicyGroupId.Value, filter, requestContext );
                        if ( response.Type != ResponseState.Success )
                        {
                            context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                            return;
                        }

                        foreach ( var item in response.Result )
                        {
                            AddPolicyToUniqueList( policies, item );
                        }
                    }
                    var parentId = department.ParentDepartmentId;

                    department = parentId.HasValue ? dataHandler.GetDepartment( parentId.Value ).Result : null;

                } while ( department != null );

                context.Response.Set( ResponseState.Success, policies );

                #endregion
                #region Cache

                Broker.Cache.Add<List<Policy>>( TamamCacheClusters.Policies, cacheKey, policies );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Policy>> GetPolicies( Guid personId, PolicyFilters filter, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Policy>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicies" + personId + filter + requestContext;
                var cached = Broker.Cache.Get<List<Policy>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region logic ...

                var personnelDataHandler = new PersonnelDataHandler();
                var personResult = personnelDataHandler.GetPerson( personId, SystemSecurityContext.Instance );
                var person = personResult.Result;
                var response = GetPolicies( person, filter, requestContext );

                context.Response.Set( response );

                #endregion
                #region Cache

                if ( response.Type == ResponseState.Success ) Broker.Cache.Add<List<Policy>>( TamamCacheClusters.Policies, cacheKey, response.Result );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Policy>> GetPolicies( PolicyFilters filters, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Policy>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicies" + filters + requestContext;
                var cached = Broker.Cache.Get<List<Policy>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.PolicyGetActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.PolicyGetAuditActionId, TamamConstants.AuthorizationConstants.PoliciesAuditModuleId, TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null );
                    return;
                }

                #endregion

                // validation ...
                if ( filters == null )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.PolicyGetAuditActionId, TamamConstants.AuthorizationConstants.PoliciesAuditModuleId, TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.ValidationError, null );
                    return;
                }

                // data layer ...
                var response = dataHandler.GetPolicies( filters.PolicyTypeId, filters.Active );
                if ( response.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.PolicyGetAuditActionId, TamamConstants.AuthorizationConstants.PoliciesAuditModuleId, TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure, string.Empty );
                    context.Response.Set( response );
                    return;
                }

                context.Response.Set( response );

                #endregion
                #region Cache

                Broker.Cache.Add<List<Policy>>( TamamCacheClusters.Policies, cacheKey, response.Result );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Policy>> GetPolicies( List<string> PolicyCodes, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Policy>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicies" + XModel.ListToString( PolicyCodes ) + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<List<Policy>>( TamamCacheClusters.Policies, cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success, cached );
                        return;
                    }
                }

                #endregion
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.PolicyGetActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.PolicyGetAuditActionId,
                        TamamConstants.AuthorizationConstants.PoliciesAuditModuleId,
                        TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetPolicies( PolicyCodes );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.PolicyGetAuditActionId, TamamConstants.AuthorizationConstants.PoliciesAuditModuleId, TamamConstants.AuthorizationConstants.PolicyGetAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<List<Policy>>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );
                }

                #endregion

            }, requestContext );

            return context.Response;
        }

        #region Policy Fields

        public ExecutionResponse<List<PolicyField>> GetPolicyFields( Guid policyTypeId, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<PolicyField>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicyFields" + policyTypeId + requestContext;
                var cached = Broker.Cache.Get<List<PolicyField>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Data Layer ...

                var dataHandlerResponse = dataHandler.GetPolicyFields( policyTypeId );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion
                #region Cache

                Broker.Cache.Add<List<PolicyField>>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            } );
            return context.Response;
        }
        public ExecutionResponse<PolicyField> GetPolicyField( Guid policyFieldId, RequestContext requestContext )
        {
            var context = new ExecutionContext<PolicyField>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicyField" + policyFieldId + requestContext;
                var cached = Broker.Cache.Get<PolicyField>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Data Layer ...

                var dataHandlerResponse = dataHandler.GetPolicyField( policyFieldId );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsGetAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion
                #region Cache

                Broker.Cache.Add<PolicyField>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            } );
            return context.Response;
        }
        public ExecutionResponse<bool> AddPolicyField( PolicyField policyField, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsCreateActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditMessageFailure,
                        policyField == null ? string.Empty : policyField.Name );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                // validation ...
                var validator = new PolicyFieldValidator( policyField, TamamConstants.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditMessageFailure,
                        policyField == null ? string.Empty : policyField.Name );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region data layer ...

                var response = dataHandler.AddPolicyField( policyField );
                if ( response.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditMessageFailure,
                        policyField == null ? string.Empty : policyField.Name );
                    context.Response.Set( response );
                    return;
                }

                #endregion
                #region Audit ...

                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditActionId,
                    TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId,
                    TamamConstants.AuthorizationConstants.DynamicFieldsCreateAuditMessageSuccessful,
                    policyField == null ? string.Empty : policyField.Name );

                #endregion

                context.Response.Set( response );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                #endregion
            } );

            return context.Response;
        }
        public ExecutionResponse<bool> EditPolicyField( PolicyField policyField, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsEditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                        , TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditMessageFailure,
                        policyField == null ? string.Empty : policyField.Id.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                // validation ...
                var validator = new PolicyFieldValidator( policyField, TamamConstants.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                        , TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditMessageFailure,
                        policyField == null ? string.Empty : policyField.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion                // data layer ...
                #region Model

                policyField.PolicyType = null;

                #endregion
                #region data layer ...

                var response = dataHandler.EditPolicyField( policyField );
                if ( response.Result == false )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                        , TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditMessageFailure,
                        policyField == null ? string.Empty : policyField.Id.ToString() );
                    context.Response.Set( response );
                    return;
                }

                #endregion
                #region Audit ...

                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditActionId,
                    TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                    , TamamConstants.AuthorizationConstants.DynamicFieldsEditAuditMessageSuccessful,
                    policyField == null ? string.Empty : policyField.Id.ToString() );

                #endregion

                context.Response.Set( response );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                #endregion
            } );

            return context.Response;
        }
        public ExecutionResponse<bool> SwapPolicyFieldSequence( PolicyField policyFieldOne, PolicyField policyFieldTwo, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                //// authorization ...
                //if ( !TamamServiceBroker.Authorize ( requestContext , TamamConstants.AuthorizationConstants.PolicyEditActionKey ) )
                //{
                //    TamamServiceBroker.Audit ( requestContext , TamamConstants.AuthorizationConstants.PolicyEditAuditActionId , TamamConstants.AuthorizationConstants.PoliciesAuditModuleId
                //        , TamamConstants.AuthorizationConstants.PolicyEditAuditMessageFailure , policyField == null ? string.Empty : policyField.Id.ToString () );
                //    context.Response.Set ( ResponseState.AccessDenied , false , new List<ModelMetaPair> () );
                //    return;
                //}

                #endregion
                #region Data Layer ...

                var response = dataHandler.SwapPolicyFieldSequence( policyFieldOne, policyFieldTwo );
                if ( response.Result == false )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.PolicyEditAuditActionId,
                        TamamConstants.AuthorizationConstants.PoliciesAuditModuleId
                        , TamamConstants.AuthorizationConstants.PolicyEditAuditMessageFailure,
                        policyFieldOne == null ? string.Empty : policyFieldOne.Id.ToString() );
                    context.Response.Set( response );
                    return;
                }

                #endregion
                #region Audit ...

                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.PolicyEditAuditActionId,
                    TamamConstants.AuthorizationConstants.PoliciesAuditModuleId
                    , TamamConstants.AuthorizationConstants.PolicyEditAuditMessageSuccessful,
                    policyFieldOne == null ? string.Empty : policyFieldOne.Id.ToString() );

                #endregion

                context.Response.Set( response );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                #endregion
            } );

            return context.Response;
        }
        public ExecutionResponse<bool> DeletePolicyField( Guid policyFieldId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsDeleteActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                        , TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditMessageFailure,
                        policyFieldId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                // validation ...
                var field = dataHandler.GetPolicyField( policyFieldId ).Result;
                var validator = new PolicyFieldValidator( field, TamamConstants.ValidationMode.Delete );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                        , TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditMessageFailure,
                        policyFieldId.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region data layer ...

                var response = dataHandler.DeletePolicyField( policyFieldId );
                if ( response.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditActionId,
                        TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                        , TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditMessageFailure,
                        policyFieldId.ToString() );
                    context.Response.Set( response );
                    return;
                }

                #endregion
                #region Audit ...

                // audit trail ...
                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditActionId,
                    TamamConstants.AuthorizationConstants.DynamicFieldsAuditModuleId
                    , TamamConstants.AuthorizationConstants.DynamicFieldsDeleteAuditMessageSuccessful,
                    policyFieldId.ToString() );

                #endregion

                context.Response.Set( response );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                #endregion
            } );

            return context.Response;
        }

        #endregion

        #endregion
        #region Policy Groups

        public ExecutionResponse<PolicyGroup> GetPolicyGroup( Guid policygroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<PolicyGroup>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicyGroup" + policygroupId + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<PolicyGroup>( TamamCacheClusters.Policies, cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success, cached );
                        return;
                    }
                }

                #endregion
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetPolicyGroup( policygroupId );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<PolicyGroup>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );
                }

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<PolicyGroup>> GetPolicyGroups( RequestContext requestContext )
        {
            var context = new ExecutionContext<List<PolicyGroup>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicyGroups" + requestContext;
                var cached = Broker.Cache.Get<List<PolicyGroup>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetPolicyGroups();
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<List<PolicyGroup>>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );

                #endregion

            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> EditPolicyGroup( PolicyGroup group, List<Guid> updatedDepartments, List<Guid> updatedPersonnel, List<Guid> updatedPolicies, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Prep..

                var originalPolicyGroup = GetPolicyGroup( group.Id, requestContext ).Result;
                var originalDepartments = originalPolicyGroup.Departments.Select( x => x.Id ).ToList();
                var originalPersonnel = GetPolicyGroupPersons( group.Id, requestContext ).Result.Select( x => x.Id ).ToList();
                var originalPolicies = originalPolicyGroup.Policies.Select( x => x.Id ).ToList();

                // departments ...
                var deletedDepartments = GetItemsInFirstNotInSecond( originalDepartments, updatedDepartments );
                var addedDepartments = GetItemsInFirstNotInSecond( updatedDepartments, originalDepartments );

                // personnel ...
                var deletedPersonnel = GetItemsInFirstNotInSecond( originalPersonnel, updatedPersonnel );
                var addedPersonnel = GetItemsInFirstNotInSecond( updatedPersonnel, originalPersonnel );

                // policies ...
                var deletedPolicies = GetItemsInFirstNotInSecond( originalPolicies, updatedPolicies );
                var addedPolicies = GetItemsInFirstNotInSecond( updatedPolicies, originalPolicies );

                #endregion

                #region Data Layer..

                var Response_PolicyGroup = EditPolicyGroup( group, requestContext );
                var Response_Departments = UpdateDepartments( group.Id, addedDepartments, deletedDepartments, requestContext );
                var Response_Personnel = UpdatePersonnel( group.Id, addedPersonnel, deletedPersonnel, requestContext );
                var Response_Policies = UpdatePolicies( group.Id, addedPolicies, deletedPolicies, requestContext );

                var state = Response_PolicyGroup.Type == ResponseState.Success;
                state = state && Response_Departments.Type == ResponseState.Success;
                state = state && Response_Personnel.Type == ResponseState.Success;
                state = state && Response_Policies.Type == ResponseState.Success;

                #endregion

                #region Event..

                // combine events data..
                var P = new List<Guid>();
                var D = new List<Guid>();

                if ( addedPolicies.Count > 0 || deletedPolicies.Count > 0 )
                {
                    P.AddRange( originalPersonnel );
                    D.AddRange( originalDepartments );
                }

                if ( addedPersonnel.Count > 0 ) P.AddRange( addedPersonnel );
                if ( deletedPersonnel.Count > 0 ) P.AddRange( deletedPersonnel );
                if ( addedDepartments.Count > 0 ) D.AddRange( addedDepartments );
                if ( deletedDepartments.Count > 0 ) D.AddRange( deletedDepartments );

                P = P.Distinct().ToList();
                D = D.Distinct().ToList();

                EventsBroker.Handle( new PolicyGroupEvent( D, P ) );

                #endregion

                #region Response..


                if ( state == false )
                {
                    var failedResponses = new List<ExecutionResponse<bool>>();
                    if ( Response_PolicyGroup.Type != ResponseState.Success ) failedResponses.Add( Response_PolicyGroup );
                    if ( Response_Departments.Type != ResponseState.Success ) failedResponses.Add( Response_Departments );
                    if ( Response_Personnel.Type != ResponseState.Success ) failedResponses.Add( Response_Personnel );
                    if ( Response_Policies.Type != ResponseState.Success ) failedResponses.Add( Response_Policies );

                    var totalResponse = CombineFailedResponses( failedResponses );
                    context.Response.Set( totalResponse );
                    return;
                }

                context.Response.Set( ResponseState.Success, true );

                #endregion

            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<Guid> CreatePolicyGroup( PolicyGroup group, RequestContext requestContext )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, Guid.Empty, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditMessageFailure, group.Name ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, Guid.Empty, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                #region Validation

                // validation ...
                var validator = new PolicyGroupValidator( group, TamamConstants.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditMessageFailure, group.Name ), string.Empty );
                    context.Response.Set( ResponseState.ValidationError, Guid.Empty, validator.ErrorsDetailed );
                    return;
                }

                #endregion

                // data layer ...
                var result = dataHandler.CreatePolicyGroup( group );

                if ( result.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditMessageFailure, group.Name ), string.Empty );
                    context.Response.Set( result );
                    return;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreatePolicyGroupAuditMessageSuccessful, group.Name ), string.Empty );

                context.Response.Set( result );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

            }, requestContext );

            return context.Response;
        }
        
        public ExecutionResponse<bool> EditPolicyGroup( PolicyGroup group, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, group.Name ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // validation ...
                var validator = new PolicyGroupValidator( group, TamamConstants.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, group.Name ), string.Empty );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                // data layer ...
                var result = dataHandler.EditPolicyGroup( group );

                if ( result.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, group.Name ), string.Empty );
                    context.Response.Set( result );
                    return;
                }

                // audit trail ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageSuccessful, group.Name ), string.Empty );

                context.Response.Set( result );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> DeletePolicyGroup( Guid policyGroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.DeletePolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // validation ...
                var policyGroupObj = dataHandler.GetPolicyGroup( policyGroupId ).Result;
                var validator = new PolicyGroupValidator( policyGroupObj, TamamConstants.ValidationMode.Deactivate );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                // data layer ...
                var result = dataHandler.DeletePolicyGroup( policyGroupId );

                //? This expression is always false
                if ( result.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( result );
                    return;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.DeletePolicyGroupAuditMessageSuccessful, string.Empty );

                context.Response.Set( result );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<List<Person>> GetPolicyGroupPersons( Guid policyGroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Person>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicyGroupPersons" + policyGroupId + requestContext;
                var cached = Broker.Cache.Get<List<Person>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetPolicyGroupPersons( policyGroupId );

                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<List<Person>>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );

                #endregion

            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> AssociateGroupToPerson( List<Guid> personIds, Guid policyGroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey ) || !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                var personHandler = new PersonnelDataHandler();

                var errors = new List<ModelMetaPair>();
                foreach ( var personId in personIds )
                {
                    var person = personHandler.GetPerson( personId, SystemSecurityContext.Instance ).Result;
                    if ( person.AccountInfo.PolicyGroupId != null && person.AccountInfo.PolicyGroupId != policyGroupId )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                        errors.Add( new ModelMetaPair( string.Empty, string.Format( Resources.Culture.Policies.PersonHaveAlreadyPolicyGroup, person.GetLocalizedFullName() ) ) );
                    }


                    // old Code..
                    //if ( response.Type != ResponseState.Success || ( response.Result.AccountInfo.PolicyGroupId != null && response.Result.AccountInfo.PolicyGroupId != policyGroupId ) )

                    //    // Person not found or has another group
                    //{
                    //    TamamServiceBroker.Audit( requestContext ,
                    //        TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId ,
                    //        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId ,
                    //        TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure , string.Empty );
                    //    context.Response.Set( ResponseState.ValidationError , false ,
                    //        new List<ModelMetaPair>
                    //        {
                    //            new ModelMetaPair {Meta = PersonnelConstants.InvalidPerson, PropertyName = string.Empty}
                    //        } );
                    //    return;
                    //}
                }

                // check errors..
                if ( errors.Count > 0 )
                {
                    context.Response.Set( ResponseState.ValidationError, false, errors );
                    return;
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.AssociateGroupToPerson( personIds, policyGroupId );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                # region Events: To be Deleted

                /*
                // Recalculate Leaves Credit
                foreach ( var id in personIds )
                {
                    var response = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit(id, SystemRequestContext.Instance);
                    if ( response.Type != ResponseState.Success )
                    {
                        TamamServiceBroker.Audit( requestContext ,
                            TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId ,
                            TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId ,
                            TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure , string.Empty );
                        context.Response.Set( dataHandlerResponse );
                        return;
                    }

                    // Update Excuses Duration
                    var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration(id, SystemRequestContext.Instance);
                    if ( responseExcuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Validate Work flow
                    //TamamServiceBroker.OrganizationHandler.CheckPersonWorkFlowIntegrity( id , requestContext );
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id , SystemRequestContext.Instance );

                    //Handle Attendance in Holiday
                    var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( id ,
                        SystemRequestContext.Instance );
                    if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                    {
                        var personHolidays =
                            personPoliciesResult.Result.Where(
                                h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                        foreach ( var holiday in personHolidays )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id , holiday ,
                                SystemRequestContext.Instance );
                        }
                    }
                }
                */

                # endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> RemoveGroupAssociationFromPerson( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext,
                    TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey )
                    ||
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.RemoveGroupAssociationFromPerson( personId );

                //? This expression is always false
                //if (dataHandlerResponse.Result == null)
                //{
                //    context.Response.Set(dataHandlerResponse);
                //    return;
                //}

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                // Update Leaves Credit
                var response = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( personId, SystemRequestContext.Instance );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                // Update Excuses Duration
                var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( personId,
                    SystemRequestContext.Instance );
                if ( responseExcuses.Type != ResponseState.Success )
                {
                    // TODO: Cannot Recalculate Excuses
                }

                // Validate Work flow
                //TamamServiceBroker.OrganizationHandler.CheckPersonWorkFlowIntegrity( personId , requestContext );
                TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( personId,
                    SystemRequestContext.Instance );

                //Handle Attendance in Holiday
                var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( personId,
                    SystemRequestContext.Instance );
                if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                {
                    var personHolidays =
                        personPoliciesResult.Result.Where(
                            h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                    foreach ( var holiday in personHolidays )
                    {
                        TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( personId, holiday,
                            SystemRequestContext.Instance );
                    }
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> RemoveGroupAssociationFromPerson( List<Guid> personIds, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext,
                    TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey )
                    ||
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.RemoveGroupAssociationFromPerson( personIds );

                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                # region Events: To be Deleted

                /*
                foreach ( var id in personIds )
                {
                    // Recalculate Leaves Credit
                    var response = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit(id, SystemRequestContext.Instance);
                    if ( response.Type != ResponseState.Success )
                    {
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId , TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId , TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure , string.Empty );
                        context.Response.Set( dataHandlerResponse );
                        return;
                    }

                    // Update Excuses Duration
                    var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration(id, SystemRequestContext.Instance);
                    if ( responseExcuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Validate Work flow
                    //TamamServiceBroker.OrganizationHandler.CheckPersonWorkFlowIntegrity( id , requestContext );
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id , SystemRequestContext.Instance );

                    //Handle Attendance in Holiday
                    var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( id , SystemRequestContext.Instance );
                    if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                    {
                        var personHolidays = personPoliciesResult.Result.Where( h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                        foreach ( var holiday in personHolidays )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id , holiday , SystemRequestContext.Instance );
                        }
                    }
                }
                */

                # endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<List<Department>> GetPolicyGroupDepartments( Guid policyGroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Department>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicyGroupDepartments" + policyGroupId + requestContext;
                var cached = Broker.Cache.Get<List<Department>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion

                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetPolicyGroupDepartments( policyGroupId );

                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion

                #region Cache

                Broker.Cache.Add<List<Department>>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> AssociateGroupToDepartment( List<Guid> departmentIds, Guid policyGroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region validate ...

                if ( departmentIds == null || departmentIds.Count == 0 )
                {
                    context.Response.Set( ResponseState.Success, true );
                    return;
                }

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey ) || !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                var org = new OrganizationDataHandler();
                foreach ( var departmentId in departmentIds )
                {
                    var orgResult = org.GetDepartment( departmentId );
                    if ( orgResult.Type != ResponseState.Success || ( orgResult.Result.PolicyGroupId != null && orgResult.Result.PolicyGroupId != policyGroupId ) )
                    {
                        // Person not found or has another group
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair { Meta = OrganizationConstants.InvalidDepartment, PropertyName = string.Empty } } );
                        return;
                    }
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.AssociateGroupToDepartment( departmentIds, policyGroupId );

                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> RemoveGroupAssociationFromDepartment( List<Guid> departmentIds, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey ) || !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.RemoveGroupAssociationFromDepartment( departmentIds );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                # region Events: To be Deleted..

                /*
                var people = GetDepartmentsPeople( departmentIds , requestContext ).Result;
                foreach ( var id in people )
                {
                    // Update Leaves Credit
                    var response = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit(id, SystemRequestContext.Instance);
                    if ( response.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Leaves Credit
                    }

                    // Update Excuses Duration
                    var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration(id, SystemRequestContext.Instance);
                    if ( responseExcuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Validate Work flow
                    //TamamServiceBroker.OrganizationHandler.CheckPersonWorkFlowIntegrity( id , requestContext );
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id , SystemRequestContext.Instance );

                    //Handle Attendance in Holiday
                    var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( id ,
                        SystemRequestContext.Instance );
                    if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                    {
                        var personHolidays =
                            personPoliciesResult.Result.Where(
                                h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                        foreach ( var holiday in personHolidays )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id , holiday ,
                                SystemRequestContext.Instance );
                        }
                    }
                }
                */

                # endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<List<Policy>> GetPolicyGroupPolicies( Guid policyGroupId, PolicyFilters filter, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Policy>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetPolicyGroupPolicies" + policyGroupId + filter + requestContext;
                var cached = Broker.Cache.Get<List<Policy>>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion

                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                filter = filter ?? new PolicyFilters();

                var dataHandlerResponse = dataHandler.GetPolicyGroupPolicies( policyGroupId, filter.PolicyTypeId,
                    filter.Active );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion

                #region Cache

                Broker.Cache.Add<List<Policy>>( TamamCacheClusters.Policies, cacheKey, dataHandlerResponse.Result );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> AssociateGroupToPolicies( List<Guid> policyIds, Guid policyGroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region validate ...

                if ( policyIds == null || policyIds.Count == 0 )
                {
                    context.Response.Set( ResponseState.Success, true );
                    return;
                }

                #endregion

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey ) || !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                #region Validate : This part is to be implemented into the validator when we refactor the policy groups

                // models ...
                var policies = GetPolicies( policyIds );
                var policyGroup = GetPolicyGroup( policyGroupId );

                if ( policies == null )
                {
                    //context.Response.Set(ResponseState.NotFound, false, "Invalid Policy / Policy Group", null);
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    return;
                }

                // validate ...
                if ( !PolicyGroupRulesValidator.Validate( policyGroup, policies ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, PoliciesConstants.AssociateFail ) } );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.AssociateGroupToPolicies( policyIds, policyGroupId );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                # region Events: To be Deleted

                /*
                // Update Leaves Credit
                var groupPeople = GetPolicyGroupPersons( policyGroupId , requestContext ).Result.Select( x => x.Id ).ToList();
                var groupDepartments = GetPolicyGroupDepartments( policyGroupId , requestContext ).Result.Select( x => x.Id ).ToList();
                var groupDepartmentsPeople = GetDepartmentsPeople( groupDepartments , requestContext ).Result;
                var allPeople = groupPeople.Union( groupDepartmentsPeople ).Distinct().ToList();
                foreach ( var id in allPeople )
                {
                    var responseRecalculate = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit(id, SystemRequestContext.Instance);
                    if ( responseRecalculate.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Leaves Credit
                    }

                    // Update Excuses Duration
                    var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration(id, SystemRequestContext.Instance);
                    if ( responseExcuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Validate Work flow
                    //TamamServiceBroker.OrganizationHandler.CheckPersonWorkFlowIntegrity( id , requestContext );
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id , SystemRequestContext.Instance );

                    //Handle Attendance in Holiday
                    var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( id ,
                        SystemRequestContext.Instance );
                    if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                    {
                        var personHolidays =
                            personPoliciesResult.Result.Where(
                                h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                        foreach ( var holiday in personHolidays )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id , holiday ,
                                SystemRequestContext.Instance );
                        }
                    }
                }
                */

                # endregion

                #region audit

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> RemoveGroupAssociationFromPolicies( List<Guid> policyIds, Guid policyGroupId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CreatePolicyGroupActionKey ) || !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                #region validation ...

                // ...

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.RemoveGroupAssociationFromPolicies( policyIds, policyGroupId );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditActionId, TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId, TamamConstants.AuthorizationConstants.EditPolicyGroupAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion

                # region Events: To be Deleted

                /*
                // Update Leaves Credit
                var groupPeople = GetPolicyGroupPersons( policyGroupId , requestContext ).Result.Select( x => x.Id ).ToList();
                var groupDepartments = GetPolicyGroupDepartments( policyGroupId , requestContext ).Result.Select( x => x.Id ).ToList();
                var groupDepartmentsPeople = GetDepartmentsPeople( groupDepartments , requestContext ).Result;
                var allPeople = groupPeople.Union( groupDepartmentsPeople ).Distinct().ToList();
                foreach ( var id in allPeople )
                {
                    var responseRecalculate = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit(id, SystemRequestContext.Instance);
                    if ( responseRecalculate.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Leaves Credit
                    }

                    // Update Excuses Duration
                    var responseExcuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration(id, SystemRequestContext.Instance);
                    if ( responseExcuses.Type != ResponseState.Success )
                    {
                        // TODO: Cannot Recalculate Excuses
                    }

                    // Validate Work flow
                    //TamamServiceBroker.OrganizationHandler.CheckPersonWorkFlowIntegrity( id , requestContext );
                    TamamServiceBroker.LeavesHandler.ApprovalIntegrityMaintainByOwner( id , SystemRequestContext.Instance );

                    //Handle Attendance in Holiday
                    var personPoliciesResult = TamamServiceBroker.OrganizationHandler.GetPolicies( id ,
                        SystemRequestContext.Instance );
                    if ( personPoliciesResult.Type == ResponseState.Success && personPoliciesResult.Result != null )
                    {
                        var personHolidays =
                            personPoliciesResult.Result.Where(
                                h => h.PolicyTypeId == Guid.Parse( PolicyTypes.HolidayPolicyType ) ).ToList();
                        foreach ( var holiday in personHolidays )
                        {
                            TamamServiceBroker.AttendanceHandler.HandleHolidayPolicy( id , holiday ,
                                SystemRequestContext.Instance );
                        }
                    }
                }
                */

                # endregion

                #region audit ...


                #endregion

                context.Response.Set( dataHandlerResponse );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ChangePolicyGroupActiveState( Guid policyGroupId, bool activeStatus, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditMessageFailure,
                            policyGroupId ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // validation ...
                var policyGroupObj = dataHandler.GetPolicyGroup( policyGroupId ).Result;
                var validationMode = activeStatus
                    ? TamamConstants.ValidationMode.Activate
                    : TamamConstants.ValidationMode.Deactivate;
                var validator = new PolicyGroupValidator( policyGroupObj, validationMode );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditMessageFailure,
                            policyGroupId ), string.Empty );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.ChangePolicyGroupActiveState( policyGroupId, activeStatus );

                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditActionId,
                        TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditMessageFailure,
                            policyGroupId ), string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditActionId,
                    TamamConstants.AuthorizationConstants.PolicyGroupAuditModuleId,
                    string.Format(
                        TamamConstants.AuthorizationConstants.ChangePolicyGroupActiveStateAuditMessageSuccessful,
                        policyGroupId ), string.Empty );

                context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );

                #endregion

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Policies );

                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion

        #endregion
        #region Helpers

        private Policy GetPolicy( Guid id )
        {
            var response = dataHandler.GetPolicy( id );
            return response.Type == ResponseState.Success ? response.Result : null;
        }
        private List<Policy> GetPolicies( List<Guid> ids )
        {
            var response = dataHandler.GetPolicies( ids );
            return response.Type == ResponseState.Success ? response.Result : null;
        }
        private PolicyGroup GetPolicyGroup( Guid id )
        {
            var response = dataHandler.GetPolicyGroup( id );
            return response.Type == ResponseState.Success ? response.Result : null;
        }
        private LeavePolicy GetLeavePolicy( Policy policy )
        {
            try
            {
                // validate ...
                if ( policy == null || policy.PolicyTypeId.ToString() != PolicyTypes.LeavePolicyType ) return null;

                // compose ...
                var leavePolicy = new LeavePolicy( policy );

                // associated : accrual ...
                /*var accrual = GetPolicy( leavePolicy.AccrualTypeId.Value );
                if ( accrual == null ) return null;

                // compose ...
                leavePolicy.AccrualPolicy = new AccrualPolicy( accrual );*/

                return leavePolicy;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        private void AddPolicyToUniqueList( List<Policy> list, Policy policy )
        {
            // new policy type, then add ...
            if ( list.All( x => x.PolicyTypeId != policy.PolicyTypeId ) ) list.Add( policy );
            else
            {
                // check support for multi association
                if ( policy.PolicyType.SupportMultiAssociation )
                {
                    // check support for sub category restrictions (exclusive rule)
                    if ( policy.PolicyType.Rules != null && policy.PolicyType.Rules.Any( x => x.Condition == PolicyRule.PolicyRulesConditions.Exclusive ) )
                    {
                        //for the time been we support Exclusive rule only.
                        //loop on Exclusive Conditions in case there is more than one.
                        bool isRuleViolated = false;
                        foreach ( var exclusiveCondition in policy.PolicyType.Rules.Where( ex => ex.Condition == PolicyRule.PolicyRulesConditions.Exclusive ) )
                        {
                            isRuleViolated = list.Any( pl => ( pl.PolicyTypeId == policy.PolicyTypeId ) && ( pl.Values.Where( v => v.PolicyFieldId == exclusiveCondition.FieldId ).FirstOrDefault().Value == policy.Values.Where( v2 => v2.PolicyFieldId == exclusiveCondition.FieldId ).FirstOrDefault().Value ) );
                            if ( isRuleViolated ) break;
                        }

                        if ( !isRuleViolated )
                        {
                            if ( list.All( p => p.Id != policy.Id ) ) list.Add( policy );
                        }
                    }
                    else
                    {
                        // support for multi association, with no field exclusive rules (like Holidays)
                        if ( list.All( p => p.Id != policy.Id ) ) list.Add( policy );
                    }
                }
            }
        }
        private List<Guid> GetAllPeople()
        {
            var criteria = new PersonSearchCriteria { ActivationStatus = true };
            var people =
                TamamServiceBroker.PersonnelHandler.GetPersonnel( criteria, SystemRequestContext.Instance ).Result;
            var allPeople = people.Persons.Select( x => x.Id ).ToList();

            return allPeople;
        }

        private List<Guid> GetItemsInFirstNotInSecond( List<Guid> first , List<Guid> second )
        {
            var result = new List<Guid>();

            foreach ( var item in first )
            {
                if ( !second.Exists( x => x == item ) )
                {
                    result.Add( item );
                }
            }

            return result;
        }

        private ExecutionResponse<bool> UpdateDepartments( Guid policyGroupId , List<Guid> added , List<Guid> deleted , RequestContext requestContext )
        {
            if ( added != null && added.Count > 0 )
            {
                var addResoponse = AssociateGroupToDepartment( added , policyGroupId , requestContext );
                if ( addResoponse.Type != ResponseState.Success ) return addResoponse;
            }

            if ( deleted != null && deleted.Count > 0 )
            {
                var removeResponse = RemoveGroupAssociationFromDepartment( deleted , requestContext );
                if ( removeResponse.Type != ResponseState.Success ) return removeResponse;
            }

            return SuccessResponse();
        }
        private ExecutionResponse<bool> UpdatePersonnel( Guid policyGroupId , List<Guid> added , List<Guid> deleted , RequestContext requestContext )
        {
            if ( added != null && added.Count > 0 )
            {
                var addResponse = AssociateGroupToPerson( added , policyGroupId , requestContext );
                if ( addResponse.Type != ResponseState.Success ) return addResponse;
            }

            if ( deleted != null && deleted.Count > 0 )
            {
                var removeResponse = RemoveGroupAssociationFromPerson( deleted , requestContext );
                if ( removeResponse.Type != ResponseState.Success ) return removeResponse;
            }

            return SuccessResponse();
        }
        private ExecutionResponse<bool> UpdatePolicies( Guid policyGroupId , List<Guid> added , List<Guid> deleted , RequestContext requestContext )
        {
            if ( deleted != null && deleted.Count > 0 )
            {
                var removeResponse = RemoveGroupAssociationFromPolicies( deleted , policyGroupId , requestContext );
                if ( removeResponse.Type != ResponseState.Success ) return removeResponse;
            }

            if ( added != null && added.Count > 0 )
            {
                var addResponse = AssociateGroupToPolicies( added , policyGroupId , requestContext );
                if ( addResponse.Type != ResponseState.Success ) return addResponse;
            }

            return SuccessResponse();
        }
        private ExecutionResponse<bool> SuccessResponse()
        {
            var R = new ExecutionResponse<bool>();
            R.Set( ResponseState.Success , true );
            return R;
        }
        private ExecutionResponse<bool> CombineFailedResponses( List<ExecutionResponse<bool>> list )
        {
            var response = new ExecutionResponse<bool>();
            response.Set( ResponseState.ValidationError , false );

            foreach ( var item in list )
            {
                if ( item.Type == ResponseState.ValidationError ) response.MessageDetailed.AddRange( item.MessageDetailed );
            }

            return response;
        }

        #endregion
    }
}
