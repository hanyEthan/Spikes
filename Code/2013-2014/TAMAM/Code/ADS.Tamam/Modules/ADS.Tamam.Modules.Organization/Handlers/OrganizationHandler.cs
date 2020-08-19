using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Tamam.Common.Data;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Common.Bases.Events.Handlers;
using ADS.Tamam.Common.Data.Validation;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Modules.Organization.Events;
using ADS.Common.Handlers.License.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;

namespace ADS.Tamam.Modules.Organization.Handlers
{
    public partial class OrganizationHandler : IOrganizationHandler, ISystemOrganizationHandler, IReadOnlyOrganizationHandler, IDataProvider, IOrgDataProvider
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "OrganizationHandler"; } }

        private readonly OrganizationDataHandler dataHandler;

        #endregion
        #region cst.

        public OrganizationHandler()
        {
            XLogger.Info( string.Empty );

            this.dataHandler = new OrganizationDataHandler();
            this.Initialized = this.dataHandler.Initialized;

            if ( !this.dataHandler.Initialized )
            {
                XLogger.Error( "Initialization Failed, underlying handlers are not registered or not initialized successfully." );
                return;
            }
        }

        #endregion

        #region IOrganizationHandler

        #region Department

        public ExecutionResponse<bool> CreateDepartment( Department department, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.CreateOrganizationAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.CreateOrganizationActionKey,
                    messageForDenied:
                        string.Format( TamamConstants.AuthorizationConstants.CreateOrganizationAuditMessageFailure,
                            department.Name ),
                    messageForFailure:
                        string.Format( TamamConstants.AuthorizationConstants.CreateOrganizationAuditMessageFailure,
                            department.Name ),
                    messageForSuccess:
                        string.Format( TamamConstants.AuthorizationConstants.CreateOrganizationAuditMessageSuccessful,
                            department.Name )
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, false );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region update Hashcode

                department.Id = Guid.NewGuid();
                if ( department.ParentDepartmentId.HasValue )
                {
                    var getParentDepartmentResponse = dataHandler.GetDepartment( department.ParentDepartmentId.Value,
                        SystemSecurityContext.Instance );
                    if ( getParentDepartmentResponse.Type != ResponseState.Success )
                    {
                        context.Response.Set( ResponseState.Failure, false,
                            new List<ModelMetaPair> { new ModelMetaPair( "departmentId", "Invalid parent departmentId" ) } );
                        return;
                    }

                    department.Hashcode = string.Format( "{0},{1}", getParentDepartmentResponse.Result.Hashcode,
                        department.Id );
                }
                else
                {
                    department.Hashcode = department.Id.ToString();
                }

                #endregion
                #region Update Names

                department.Name = department.Name.Trim();
                department.NameCultureVarient = department.NameCultureVarient.Trim();
                department.NameCultureVarientAbstract = department.NameCultureVarient;

                #endregion
                #region Validation

                var validator = new OrganizationValidator( department, TamamConstants.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    TamamServiceBroker.Audit( requestContext, context.ActionContext,
                        context.ActionContext.MessageForFailure, department.Name );
                    return;
                }

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.CreateDepartment( department );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, department.Name ); context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new DepartmentCreateEvent( department.Id ) );

                #region audit ...

                TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForSuccess,
                    department.Name );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Department );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditDepartment( Department department, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.EditOrganizationAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.EditOrganizationActionKey,
                    messageForDenied:
                        string.Format( TamamConstants.AuthorizationConstants.EditOrganizationAuditMessageFailure,
                            department.Name ),
                    messageForFailure:
                        string.Format( TamamConstants.AuthorizationConstants.EditOrganizationAuditMessageFailure,
                            department.Name ),
                    messageForSuccess:
                        string.Format( TamamConstants.AuthorizationConstants.EditOrganizationAuditMessageSuccessful,
                            department.Name )
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, false );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Update Names

                department.Name = department.Name.Trim();
                department.NameCultureVarient = department.NameCultureVarient.Trim();
                department.NameCultureVarientAbstract = department.NameCultureVarient;

                #endregion
                #region Validation

                var validator = new OrganizationValidator( department, TamamConstants.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, department.Name ); context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region update Hashcode

                if ( department.ParentDepartmentId.HasValue )
                {
                    var getParentDepartmentResponse = dataHandler.GetDepartment( department.ParentDepartmentId.Value, SystemSecurityContext.Instance );
                    if ( getParentDepartmentResponse.Type != ResponseState.Success )
                    {
                        context.Response.Set( ResponseState.Failure, false );
                        return;
                    }

                    department.Hashcode = string.Format( "{0},{1}", getParentDepartmentResponse.Result.Hashcode, department.Id );
                }
                else
                {
                    department.Hashcode = department.Id.ToString();
                }

                #endregion
                #region DataLayer

                var result = dataHandler.EditDepartment( department );

                //? This expression is always false
                if ( result.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, department.Name );
                    context.Response.Set( result );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new DepartmentEditEvent( department.Id ) );

                #region Audit ...

                TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, department.Name );

                #endregion

                context.Response.Set( result );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Department );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> DeleteDepartment( Department department, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.DeleteOrganizationAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.DeleteOrganizationActionKey,
                    messageForDenied:
                        string.Format( TamamConstants.AuthorizationConstants.DeleteOrganizationAuditMessageFailure,
                            department.Name ),
                    messageForFailure:
                        string.Format( TamamConstants.AuthorizationConstants.DeleteOrganizationAuditMessageFailure,
                            department.Name ),
                    messageForSuccess:
                        string.Format( TamamConstants.AuthorizationConstants.DeleteOrganizationAuditMessageSuccessful,
                            department.Name )
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, false );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region validation ...

                var validator = new OrganizationValidator( department, TamamConstants.ValidationMode.Delete );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, department.Name );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region dataLayer ...

                var dataHandlerResponse = dataHandler.DeleteDepartment( department );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, department.Name );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion

                // audit ...
                TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForSuccess, department.Name );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Department );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<Department> GetDepartment( Guid id, RequestContext requestContext )
        {
            var context = new ExecutionContext<Department>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetDepartment" + id + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Department>( TamamCacheClusters.Department, cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success, cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetOrganizationAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetOrganizationActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region dataLayer ...

                var dataHandlerResponse = dataHandler.GetDepartment( id, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext,
                        context.ActionContext.MessageForFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Department>( TamamCacheClusters.Department, cacheKey, dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Department>> GetDepartments( RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Department>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetDepartments" + requestContext;
                var cached = Broker.Cache.Get<List<Department>>( TamamCacheClusters.Department, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                # region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetOrganizationAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetOrganizationActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                # endregion

                var dataHandlerResponse = dataHandler.GetDepartments( securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                // normalize : remove hierarchal references ...
                foreach ( var item in dataHandlerResponse.Result )
                {
                    if ( dataHandlerResponse.Result.All( p => p.Id != item.ParentDepartmentId ) )
                    {
                        item.ParentDepartmentId = null;
                        item.ParentDepartment = null;
                    }
                }

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<Department>>( TamamCacheClusters.Department, cacheKey, dataHandlerResponse.Result );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<object> GetDepartmentsByPerson( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<object>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetDepartmentsByPerson" + personId + requestContext;
                var cached = Broker.Cache.Get<List<Department>>( TamamCacheClusters.Department, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
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
                //if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.GetOrganizationActionKey ) )
                //{
                //    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetOrganizationAuditActionId , TamamConstants.AuthorizationConstants.OrganizationAuditModuleId , TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure , string.Empty );
                //    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                //    return;
                //}

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetDepartmentsByPersonScalar( personId );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<string>( TamamCacheClusters.Department, cacheKey, ( string )dataHandlerResponse.Result );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Department>> GetDepartmentsByParentId( Guid id, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Department>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetDepartmentsByParentId" + id + requestContext;
                var cached = Broker.Cache.Get<List<Department>>( TamamCacheClusters.Department, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetOrganizationAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetOrganizationActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region dataLayer ...

                var dataHandlerResponse = dataHandler.GetDepartmentsByParentId( id, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext,
                        context.ActionContext.MessageForFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<List<Department>>( TamamCacheClusters.Department, cacheKey, dataHandlerResponse.Result );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion
        #region Organization Detail

        public ExecutionResponse<OrganizationDetail> GetOrganizationDetail( RequestContext requestContext )
        {
            var context = new ExecutionContext<OrganizationDetail>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetOrganizationDetail" + requestContext;
                var cached = Broker.Cache.Get<OrganizationDetail>( TamamCacheClusters.OrganizationDetail, cacheKey );
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
                        TamamConstants.AuthorizationConstants.GetOrganizationDetailActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetOrganizationDetailAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetOrganizationDetailAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetOrganizationDetail();
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.GetOrganizationDetailAuditActionId, TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId, TamamConstants.AuthorizationConstants.GetOrganizationDetailAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<OrganizationDetail>( TamamCacheClusters.OrganizationDetail, cacheKey, dataHandlerResponse.Result );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> CreateOrganizationDetail( OrganizationDetail organizationDetail, RequestContext requestContext )
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
                        TamamConstants.AuthorizationConstants.CreateOrganizationDetailActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditMessageFailure,
                            organizationDetail.Name ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // validation ...
                var validator = new OrganizationDetailValidator( organizationDetail );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditMessageFailure,
                            organizationDetail.Name ), string.Empty );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.CreateOrganizationDetail( organizationDetail );

                //? This expression is always false
                //if (dataHandlerResponse.Result == null)
                //{
                //    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditActionId, TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId, string.Format(TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditMessageFailure, organizationDetail.Name), string.Empty);
                //    context.Response.Set(dataHandlerResponse);
                //    return;
                //}

                // audit ...
                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditActionId,
                    TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                    string.Format( TamamConstants.AuthorizationConstants.CreateOrganizationDetailAuditMessageSuccessful,
                        organizationDetail.Name ), string.Empty );

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.OrganizationDetail );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditOrganizationDetail( OrganizationDetail organizationDetail, RequestContext requestContext )
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
                        TamamConstants.AuthorizationConstants.EditOrganizationDetailActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                        string.Format( TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditMessageFailure,
                            organizationDetail.Name ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // validation ...
                var validator = new OrganizationDetailValidator( organizationDetail );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                        string.Format( TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditMessageFailure,
                            organizationDetail.Name ), string.Empty );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.EditOrganizationDetail( organizationDetail );

                //? This is expression is always true
                //if (dataHandlerResponse.Result == null)
                //{
                //    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditActionId, TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId, string.Format(TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditMessageFailure, organizationDetail.Name), string.Empty);
                //    context.Response.Set(dataHandlerResponse);
                //    return;
                //}

                // audit ...
                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditActionId,
                    TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                    string.Format( TamamConstants.AuthorizationConstants.EditOrganizationDetailAuditMessageSuccessful,
                        organizationDetail.Name ), string.Empty );

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.OrganizationDetail );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> DeleteOrganizationDetail( OrganizationDetail organizationDetail, RequestContext requestContext )
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
                        TamamConstants.AuthorizationConstants.DeleteOrganizationDetailActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DeleteOrganizationDetailAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.DeleteOrganizationDetailAuditMessageFailure,
                            organizationDetail.Name ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.DeleteOrganizationDetail( organizationDetail );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.DeleteOrganizationDetailAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.DeleteOrganizationDetailAuditMessageFailure,
                            organizationDetail.Name ), string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.DeleteOrganizationDetailAuditActionId,
                    TamamConstants.AuthorizationConstants.OrganizationDetailAuditModuleId,
                    string.Format( TamamConstants.AuthorizationConstants.DeleteOrganizationDetailAuditMessageSuccessful,
                        organizationDetail.Name ), string.Empty );

                context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.OrganizationDetail );

                #endregion

            }, requestContext );

            return context.Response;
        }

        #endregion
        # region Attendance Codes

        public ExecutionResponse<List<AttendanceCode>> GetAttendanceCodes( RequestContext requestContext )
        {
            var context = new ExecutionContext<List<AttendanceCode>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetAttendanceCodes" + requestContext;
                var cached = Broker.Cache.Get<List<AttendanceCode>>( TamamCacheClusters.Common, cacheKey );
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
                        TamamConstants.AuthorizationConstants.GetAttendanceCodesActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetAttendanceCodesAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetAttendanceCodesAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetAttendanceCodes();
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetAttendanceCodesAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetAttendanceCodesAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                //TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.GetAttendanceCodesAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.GetAttendanceCodesAuditMessageSuccessful, string.Empty);

                #endregion
                #region Cache

                Broker.Cache.Add<List<AttendanceCode>>( TamamCacheClusters.Common, cacheKey, dataHandlerResponse.Result );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditAttendanceCode( AttendanceCode attendanceCode, RequestContext requestContext )
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
                        TamamConstants.AuthorizationConstants.EditAttendanceCodesActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                #region Validation

                var validator = new AttendanceCodeValidator( attendanceCode,
                    AttendanceCodeValidator.AttendanceCodeValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    // audit : failure ...
                    Broker.AuditTrailHandler.Log( new AuditTrailLog( requestContext.SecurityToken.ToString(),
                        requestContext.CallerUsername,
                        int.Parse( TamamConstants.AuthorizationConstants.OrganizationAuditModuleId ),
                        int.Parse( TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditActionId ),
                        requestContext.IpAddress, requestContext.MachineName, string.Empty,
                        TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditMessageFailure ) );

                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.EditAttendanceCode( attendanceCode );
                if ( dataHandlerResponse.Result == false )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditMessageFailure, string.Empty );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                TamamServiceBroker.Audit( requestContext,
                    TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditActionId,
                    TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    TamamConstants.AuthorizationConstants.EditAttendanceCodesAuditMessageSuccessful, string.Empty );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Common );

                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion
        # region Dashboard WebParts

        public ExecutionResponse<List<DashboardWebPart>> GetDashboardWebParts( RequestContext requestContext )
        {
            var context = new ExecutionContext<List<DashboardWebPart>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetDashboardWebParts" + requestContext;
                var cached = Broker.Cache.Get<List<DashboardWebPart>>( TamamCacheClusters.Common, cacheKey );
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
                        TamamConstants.AuthorizationConstants.GetDashboardWebPartsActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetDashboardWebPartsAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetDashboardWebPartsAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null,
                        new List<ModelMetaPair>
                        {
                            new ModelMetaPair(string.Empty, Resources.Culture.Common.UnauthorizedDashboard)
                        } );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetDashboardWebParts();
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.GetDashboardWebPartsAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.GetDashboardWebPartsAuditMessageFailure, string.Empty );
                    return;
                }

                var allWebParts = dataHandlerResponse.Result;
                allWebParts = requestContext.PersonId.HasValue ? allWebParts : allWebParts.Where( x => x.IsSelfService == false ).ToList();

                if ( !requestContext.PersonId.HasValue )
                {
                    context.Response.Set( ResponseState.AccessDenied, null );
                    return;
                }

                var authorizedWebParts = allWebParts.Where( webPart => Broker.AuthorizationHandler.Authorize( requestContext.PersonId.Value, webPart.Privilege ) ).ToList();
                context.Response.Set( ResponseState.Success, authorizedWebParts );

                #endregion
                #region Cache

                Broker.Cache.Add<List<DashboardWebPart>>( TamamCacheClusters.Common, cacheKey, authorizedWebParts );

                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion
        # region Reports

        public ExecutionResponse<List<ReportCategory>> GetReports( RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ReportCategory>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetReports" + requestContext;
                var cached = Broker.Cache.Get<List<ReportCategory>>( TamamCacheClusters.Common, cacheKey );
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
                        TamamConstants.AuthorizationConstants.GetReportsActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetReportsAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetReportsAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // fill ReportCategoryFilters..
                var filters = new ReportCategoryFilters( null, false, false );
                var response = GetReports( filters, requestContext );
                context.Response.Set( response );

                #endregion
                #region Cache

                Broker.Cache.Add<List<ReportCategory>>( TamamCacheClusters.Common, cacheKey, response.Result );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<ReportCategory>> GetReports( ReportCategoryFilters filters, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ReportCategory>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetReports" + filters + requestContext;
                var cached = Broker.Cache.Get<List<ReportCategory>>( TamamCacheClusters.Common, cacheKey );
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
                        TamamConstants.AuthorizationConstants.GetReportsActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetReportsAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetReportsAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetReports();

                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.GetReportsAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.GetReportsAuditMessageFailure, string.Empty );
                    return;
                }

                // Filter Report Categories
                var categories = dataHandlerResponse.Result.Where( x => x.IsQuick == filters.IsQuick ).ToList();

                if ( filters.CategoryId.HasValue )
                    categories = categories.Where( x => x.Id == filters.CategoryId.Value ).ToList();

                // Authorize Reports
                if ( !filters.DisableAuthorizationFilter )
                {
                    // Get All Report Definitions
                    var allDefinitions = new List<ReportDefinition>();
                    foreach ( var category in categories )
                    {
                        allDefinitions.AddRange( category.Reports );
                    }
                    allDefinitions = allDefinitions.Distinct().ToList();

                    if ( !requestContext.PersonId.HasValue )
                    {
                        context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                        return;
                    }

                    // Get UnAuthorized Report Definitions
                    var unAuthorizedReports =
                        allDefinitions.Where(
                            report =>
                                !Broker.AuthorizationHandler.Authorize( requestContext.PersonId.Value, report.Privilege ) )
                            .ToList();

                    // Subtract UnAuthorized Reports
                    foreach ( var category in categories )
                    {
                        category.Reports = category.Reports.Except( unAuthorizedReports ).ToList();
                    }
                }

                //  Subtract categories that does not have any more reports
                categories = categories.Where( x => x.Reports.Count > 0 ).ToList();

                context.Response.Set( ResponseState.Success, categories );
                //context.Response.Set(result);

                //TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.GetReportsAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.GetReportsAuditMessageSuccessful, string.Empty);

                #endregion
                #region Cache

                Broker.Cache.Add<List<ReportCategory>>( TamamCacheClusters.Common, cacheKey, categories );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<ReportDefinition> GetReportReportDefinition( Guid id, RequestContext requestContext )
        {
            var context = new ExecutionContext<ReportDefinition>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "GetReportReportDefinition" + id + requestContext;
                var cached = Broker.Cache.Get<ReportDefinition>( TamamCacheClusters.Common, cacheKey );
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
                        TamamConstants.AuthorizationConstants.GetReportsActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetReportsAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetReportsAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var result = dataHandler.GetReportDefinition( id );

                if ( result.Result == null )
                {
                    context.Response.Set( result );
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetReportsAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetReportsAuditMessageFailure, string.Empty );
                    return;
                }

                // Authorize Report
                if ( !requestContext.PersonId.HasValue )
                {
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                var isAuthorized = Broker.AuthorizationHandler.Authorize( requestContext.PersonId.Value,
                    result.Result.Privilege );
                if ( !isAuthorized )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.GetReportsAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.GetReportsAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                context.Response.Set( result );

                //TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.GetReportsAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.GetReportsAuditMessageSuccessful, string.Empty);

                #endregion
                #region Cache

                Broker.Cache.Add<ReportDefinition>( TamamCacheClusters.Common, cacheKey, result.Result );

                #endregion
            }, requestContext );

            return context.Response;
        }

        # endregion
        # region Holidays

        public ExecutionResponse<List<Holiday>> GetNativeHolidays( RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Holiday>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetNativeHolidays" + requestContext;
                var cached = Broker.Cache.Get<List<Holiday>>( TamamCacheClusters.Holidays, cacheKey );
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
                        TamamConstants.AuthorizationConstants.HolidaysActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null,
                        new List<ModelMetaPair>
                        {
                            new ModelMetaPair(string.Empty, Resources.Culture.Common.UnauthorizedHolidays)
                        } );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetNativeHolidays();

                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    return;
                }

                context.Response.Set( ResponseState.Success, dataHandlerResponse.Result );

                // Audit ...
                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.HolidaysAuditActionId , TamamConstants.AuthorizationConstants.OrganizationAuditModuleId , TamamConstants.AuthorizationConstants.HolidaysAuditMessageSuccessful , string.Empty );

                #endregion
                #region Cache

                Broker.Cache.Add<List<Holiday>>( TamamCacheClusters.Holidays, cacheKey, dataHandlerResponse.Result );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<Holiday> GetNativeHoliday( Guid id, RequestContext requestContext )
        {
            var context = new ExecutionContext<Holiday>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetNativeHolidays" + id + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Holiday>( TamamCacheClusters.Holidays, cacheKey );
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
                        TamamConstants.AuthorizationConstants.HolidaysActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null,
                        new List<ModelMetaPair>
                        {
                            new ModelMetaPair(string.Empty, Resources.Culture.Common.UnauthorizedHolidays)
                        } );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetNativeHoliday( id );

                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    return;
                }

                context.Response.Set( ResponseState.Success, dataHandlerResponse.Result, new List<ModelMetaPair>() );

                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.HolidaysAuditActionId , TamamConstants.AuthorizationConstants.OrganizationAuditModuleId , TamamConstants.AuthorizationConstants.HolidaysAuditMessageSuccessful , string.Empty );

                #endregion
                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Holiday>( TamamCacheClusters.Holidays, cacheKey, dataHandlerResponse.Result );
                }

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<Guid> CreateNativeHoliday( Holiday holiday, RequestContext requestContext )
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
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.HolidaysActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, Guid.Empty, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Validation

                // validation ...
                var validator = new HolidayValidator( holiday, TamamConstants.ValidationMode.Create );

                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.ValidationError, Guid.Empty, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                # region Data layer

                // data layer ...
                var dataHandlerResponse = dataHandler.CreateNativeHoliday( holiday );

                //? This expression is always false
                //if (dataHandlerResponse.Result == null)
                //{
                //    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty);
                //    context.Response.Set(dataHandlerResponse);
                //    return;
                //}


                # endregion

                var state = EventsBroker.Handle( new NativeHolidayCreateEvent( holiday ) );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Holidays );

                #endregion

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.HolidaysAuditMessageSuccessful, string.Empty );

                context.Response.Set( dataHandlerResponse );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditNativeHoliday( Holiday holiday, RequestContext requestContext )
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
                        TamamConstants.AuthorizationConstants.HolidaysActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Validation

                // validation ...
                var validator = new HolidayValidator( holiday, TamamConstants.ValidationMode.Create );

                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                # region Data layer

                var oldHoliday = dataHandler.GetNativeHoliday( holiday.Id ).Result;
                // data layer ...
                var dataHandlerResponse = dataHandler.EditNativeHoliday( holiday );
                if ( dataHandlerResponse.Result == false )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                # endregion

                var state = EventsBroker.Handle( new NativeHolidayEditEvent( oldHoliday, holiday ) );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Holidays );

                #endregion

                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.HolidaysAuditMessageSuccessful, string.Empty );

                context.Response.Set( dataHandlerResponse );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> DeleteNativeHoliday( Guid id, RequestContext requestContext )
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
                        TamamConstants.AuthorizationConstants.HolidaysActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false,
                        new List<ModelMetaPair> { new ModelMetaPair( string.Empty, Resources.Culture.Common.UnauthorizedHolidays ) } );
                    return;
                }

                #endregion
                # region Data layer

                // data layer ...
                var nativeHoliday = dataHandler.GetNativeHoliday( id ).Result;
                var dataHandlerResponse = dataHandler.DeleteNativeHoliday( id );

                if ( dataHandlerResponse.Result == false )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    return;
                }

                # endregion

                var state = EventsBroker.Handle( new NativeHolidayDeleteEvent( nativeHoliday ) );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Holidays );

                #endregion

                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId, TamamConstants.AuthorizationConstants.OrganizationAuditModuleId, TamamConstants.AuthorizationConstants.HolidaysAuditMessageSuccessful, string.Empty );

                context.Response.Set( ResponseState.Success, dataHandlerResponse.Result );

                #endregion

            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<List<Holiday>> GetHolidays( RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Holiday>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetHolidays" + requestContext;
                var cached = Broker.Cache.Get<List<Holiday>>( TamamCacheClusters.Holidays, cacheKey );
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
                        TamamConstants.AuthorizationConstants.HolidaysActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null,
                        new List<ModelMetaPair>
                        {
                            new ModelMetaPair(string.Empty, Resources.Culture.Common.UnauthorizedHolidays)
                        } );
                    return;
                }

                #endregion

                // native holidays..
                var nativateHolidaysResponse = dataHandler.GetNativeHolidays();
                if ( nativateHolidaysResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( nativateHolidaysResponse );
                    return;
                }

                // policies holidays..
                var filters = new PolicyFilters( Guid.Parse( PolicyTypes.HolidayPolicyType ), true );
                var policiesHolidaysResponse = GetPolicies( filters, SystemRequestContext.Instance );
                if ( policiesHolidaysResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( policiesHolidaysResponse.Type, null, policiesHolidaysResponse.MessageDetailed );
                    return;
                }

                // combine native holidays and policies holidays..
                var holidays = nativateHolidaysResponse.Result;
                holidays = holidays ?? new List<Holiday>();

                var policies = policiesHolidaysResponse.Result.Select( HolidayPolicy.GetInstance ).ToList();
                foreach ( var holidayPolicy in policies )
                {
                    var holiday = new Holiday
                    {
                        Name = holidayPolicy.Name,
                        NameCultureVariant = holidayPolicy.NameCultureVarient,
                        StartDate = holidayPolicy.DateFrom.Value,
                        EndDate = holidayPolicy.DateTo.Value
                    };

                    holidays.Add( holiday );
                }

                context.Response.Set( ResponseState.Success, holidays );

                #region Cache

                Broker.Cache.Add<List<Holiday>>( TamamCacheClusters.Holidays, cacheKey, holidays );

                #endregion

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Holiday>> GetHolidays( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Holiday>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetHolidays" + personId + requestContext;
                var cached = Broker.Cache.Get<List<Holiday>>( TamamCacheClusters.Holidays, cacheKey );
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
                        TamamConstants.AuthorizationConstants.HolidaysActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.HolidaysAuditActionId,
                        TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                        TamamConstants.AuthorizationConstants.HolidaysAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null,
                        new List<ModelMetaPair>
                        {
                            new ModelMetaPair(string.Empty, Resources.Culture.Common.UnauthorizedHolidays)
                        } );
                    return;
                }

                #endregion

                // native holidays..
                var nativateHolidaysResponse = dataHandler.GetNativeHolidays();
                if ( nativateHolidaysResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( nativateHolidaysResponse );
                    return;
                }

                // policies holidays..
                var filters = new PolicyFilters( Guid.Parse( PolicyTypes.HolidayPolicyType ), true );
                var policiesHolidaysResponse = GetPolicies( personId, filters, SystemRequestContext.Instance );
                if ( policiesHolidaysResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( policiesHolidaysResponse.Type, null, policiesHolidaysResponse.MessageDetailed );
                    return;
                }

                // combine native holidays and policies holidays..
                var holidays = nativateHolidaysResponse.Result;
                holidays = holidays ?? new List<Holiday>();

                var policies = policiesHolidaysResponse.Result.Select( HolidayPolicy.GetInstance ).ToList();
                foreach ( var holidayPolicy in policies )
                {
                    var holiday = new Holiday
                    {
                        Name = holidayPolicy.Name,
                        NameCultureVariant = holidayPolicy.NameCultureVarient,
                        StartDate = holidayPolicy.DateFrom.Value,
                        EndDate = holidayPolicy.DateTo.Value
                    };

                    holidays.Add( holiday );
                }

                #region Cache

                Broker.Cache.Add<List<Holiday>>( TamamCacheClusters.Holidays, cacheKey, holidays );

                #endregion

                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.HolidaysAuditActionId , TamamConstants.AuthorizationConstants.OrganizationAuditModuleId , TamamConstants.AuthorizationConstants.HolidaysAuditMessageSuccessful , string.Empty );

                context.Response.Set( ResponseState.Success, holidays );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Holiday>> GetHolidays( Guid personId, DateTime from, DateTime to, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Holiday>>();
            context.Execute( () =>
            {
                var holidaysResponse = GetHolidays( personId, requestContext );
                if ( holidaysResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( holidaysResponse );
                    return;
                }

                var holidays = holidaysResponse.Result;
                var filteredHolidays = holidays.Where( x => from.Date <= x.EndDate.Date && to.Date >= x.StartDate.Date ).ToList();

                context.Response.Set( ResponseState.Success, filteredHolidays );

                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.HolidaysAuditActionId , TamamConstants.AuthorizationConstants.OrganizationAuditModuleId , TamamConstants.AuthorizationConstants.HolidaysAuditMessageSuccessful , string.Empty );

            }, requestContext );

            return context.Response;
        }

        # endregion

        # endregion
        #region ISystemOrganizationHandler

        public ExecutionResponse<List<Person>> GetSupervisors( Guid id, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Person>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "OrganizationHandler_GetSupervisors" + id + requestContext;
                var cached = Broker.Cache.Get<List<Person>>( TamamCacheClusters.Person, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetOrganizationAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetOrganizationActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.GetOrganizationAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region DataLayer ...

                var dataHandlerResponse = dataHandler.GetSupervisors( id );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext,
                        context.ActionContext.MessageForFailure, id.ToString() );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<List<Person>>( TamamCacheClusters.Person, cacheKey, dataHandlerResponse.Result );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<List<Guid>> GetDepartmentsPeople( List<Guid> departments, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Guid>>();
            context.Execute( () =>
            {
                departments = departments ?? new List<Guid>();
                departments = departments.Distinct().ToList();

                // get all departments ids..
                var allDepartments = new List<Guid>();
                foreach ( var id in departments )
                {
                    var ids = dataHandler.GetDepartmentsByRoot( id ).Result.Select( x => x.Id ).ToList();
                    allDepartments.AddRange( ids );
                }

                // get all peoples associated with these departments..
                var criteria = new PersonSearchCriteria { AllowPaging = false, Departments = allDepartments };

                var response = TamamServiceBroker.PersonnelHandler.GetPersonnel( criteria, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var presonnelIds = response.Result.Persons.Select( x => x.Id ).ToList();
                context.Response.Set( ResponseState.Success, presonnelIds );

            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> HandlePolicyGroupEvents( List<Guid> departments, List<Guid> personnel )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                var state = EventsBroker.Handle( new PolicyGroupEvent( departments, personnel ) );

                context.Response.Set( ResponseState.Success, true );

            }, SystemRequestContext.Instance );

            return context.Response;
        }

        public ExecutionResponse<bool> IsSystemPolicyGroupHasPolicyForLeaveType( int leaveType )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                var systemPolicyGroup = dataHandler.GetSystemPolicyGroup().Result;

                if ( systemPolicyGroup == null )
                {
                    context.Response.Set( ResponseState.NotFound, false );
                    return;
                }

                var leavePolicy = systemPolicyGroup.Policies.FirstOrDefault( p => p.PolicyTypeId == new Guid( PolicyTypes.LeavePolicyType ) && p.Values.Any( y => y.PolicyFieldId == PolicyFields.LeavePolicy.LeaveType && y.Value == leaveType.ToString() ) );
                var havePolicy = leavePolicy != null;

                context.Response.Set( ResponseState.Success, havePolicy );

            }, SystemRequestContext.Instance );

            return context.Response;
        }

        public ExecutionResponse<PolicyGroup> GetSystemPolicyGroup()
        {
            var context = new ExecutionContext<PolicyGroup>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "OrganizationHandler_GetSystemPolicyGroup";
                var cached = Broker.Cache.Get<PolicyGroup>( TamamCacheClusters.Policies, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region logic ...



                // data layer ...
                var response = dataHandler.GetSystemPolicyGroup();
                if ( response.Result == null )
                {
                    context.Response.Set( response );
                    return;
                }

                context.Response.Set( response );

                #endregion
                #region Cache

                Broker.Cache.Add<PolicyGroup>( TamamCacheClusters.Policies, cacheKey, response.Result );

                #endregion

            }, SystemRequestContext.Instance );

            return context.Response;
        }

        public ExecutionResponse<object> getInfo()
        {
            var context = new ExecutionContext<object>();
            context.Execute(() =>
            {
                #region logic ...

                // data layer ...
                var response = dataHandler.GetTerminalsCount();
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, response.Result, response.MessageDetailed);
                    return;
                }

                context.Response.Set(response.Type, response.Result, response.MessageDetailed);

                #endregion

            }, SystemRequestContext.Instance);

            return context.Response;
        }

        public ExecutionResponse<object> getOrgInfo()
        {
            var context = new ExecutionContext<object>();
            context.Execute(() =>
            {
                #region logic ...

                // data layer ...
                var response = dataHandler.GetOrganizationDetail();
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null, response.MessageDetailed);
                    return;
                }

                context.Response.Set(response.Type, response.Result.Name, response.MessageDetailed);

                #endregion

            }, SystemRequestContext.Instance);

            return context.Response;
        }

        #endregion
    }
}