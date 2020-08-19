using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Authorization;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Models.Enums;
using ADS.Common.Workflow.Enums;
using LinqKit;
using Telerik.OpenAccess;
using ADS.Common.Contracts.SystemCodes;
using ADS.Common.Utilities;
using ADS.Common.Contracts;
using ADS.Common.Contracts.Security;
using ADS.Common.Handlers.Data.ORM;
using ADS.Common.Contracts.AuditTrail;
using ADS.Common.Models.Domain;
using ADS.Common.Models.DTO;
using Action = ADS.Common.Models.Domain.Authorization.Action;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;

namespace ADS.Common.Handlers.Data
{
    public class DataHandler : IDataAccessHandler,
        IAuthorizationDataHandler, IAuditTrailDataHandler,
        IMasterCodeDataHandler, IDetailCodeDataHandler, INotificationsSenderDataHandler,
        INotificationsListnerDataHandler, INotificationsListSenderDataHandler, INotificationsDataHandler,
        IWorkflowDataHandler, ISMSNotificationsDataHandler, INotificationsEmailSenderDataHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "DataHandler"; } }

        //protected DomainDataContext DataContext;

        #endregion
        #region cst.

        public DataHandler()
        {
            try
            {
                //OpenDataSourceConnection();
                using (var context = GetDataContext())
                {
                    var conn = context.Connection;
                    Initialized = true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                Initialized = false;
            }
        }

        #endregion
        #region publics

        #region IAuthorizationDataHandler

        # region Privileges

        public Privilege GetPrivilege(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var privilege = context.Privileges.FirstOrDefault(x => x.Id == id);
                    privilege = context.CreateDetachedCopy(privilege);
                    return privilege;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<Privilege> GetPrivileges()
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var privileges = context.Privileges.ToList();
                    return context.CreateDetachedCopy(privileges);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Privilege AddPrivilege(Privilege item)
        {
            XLogger.Trace("");

            try
            {
                if (item == null) return null;

                using (var context = GetDataContext())
                {
                    context.Add(item);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(item);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Privilege UpdatePrivilege(Privilege item)
        {
            XLogger.Trace("");

            try
            {
                if (item == null) return null;

                using (var context = GetDataContext())
                {
                    context.AttachCopy(item);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(item);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public bool DeletePrivilege(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var privilege = context.Privileges.FirstOrDefault(x => x.Id == id);
                    context.Delete(privilege);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }
        public List<Privilege> GetPrivileges(IAuthorizationActor principle)
        {
            XLogger.Trace("");

            try
            {
                if (principle == null) return null;

                using (var context = GetDataContext())
                {
                    var actor = context.Actors.FirstOrDefault(x => x.Id == principle.Id);
                    if (actor == null) return null;

                    var privileges = actor.Privileges.Union(actor.Roles.SelectMany(x => x.Privileges)).Distinct();
                    return privileges.ToList();
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<Privilege> GetPrivileges(IAuthorizationTarget principle)
        {
            XLogger.Trace("");

            try
            {
                if (principle == null) return null;

                using (var context = GetDataContext())
                {
                    var action = context.Actions.FirstOrDefault(x => x.Id == principle.Id);
                    return action == null ? null : action.Privileges.Distinct().ToList();
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }

        # endregion
        # region Roles

        public List<Role> GetRoles()
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    return context.CreateDetachedCopy(context.Roles.ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<Role> GetRoles(RoleSearchCriteria criteria)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var query = context.Roles;

                    query = string.IsNullOrWhiteSpace(criteria.Name)
                            ? query
                            : IsOracle()
                            ? query = query.Where(x => x.Name.ToLower().Contains(criteria.Name.ToLower()))
                            : query = query.Where(x => x.Name.Contains(criteria.Name));

                    query = string.IsNullOrWhiteSpace(criteria.Code)
                            ? query
                            : IsOracle()
                            ? query = query.Where(x => x.Code.ToLower().Contains(criteria.Code.ToLower()))
                            : query = query.Where(x => x.Code.Contains(criteria.Code));

                    return context.CreateDetachedCopy(query.ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Role GetRole(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    return context.CreateDetachedCopy(context.Roles.FirstOrDefault(x => x.Id == id));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Role GetRole(Guid id, bool underlyingCollections)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    return underlyingCollections
                           ? context.CreateDetachedCopy(context.Roles.Include(x => x.Privileges).FirstOrDefault(x => x.Id == id), x => x.Privileges)
                           : context.CreateDetachedCopy(context.Roles.FirstOrDefault(x => x.Id == id));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Role AddRole(Role role)
        {
            XLogger.Trace("");

            try
            {
                if (role == null) return null;

                using (var context = GetDataContext())
                {
                    context.AttachCopy(role);
                    context.SaveChanges();
                    return role;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Role UpdateRole(Role item)
        {
            XLogger.Trace("");

            try
            {
                if (item == null) return null;

                using (var context = GetDataContext())
                {
                    context.AttachCopy(item);
                    context.SaveChanges();
                    return item;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public bool DeleteRole(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var role = context.Roles.FirstOrDefault(x => x.Id == id);
                    context.Delete(role);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        # endregion
        # region Actions

        public Action GetAction(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    return context.CreateDetachedCopy(context.Actions.FirstOrDefault(x => x.Id == id));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Action GetAction(Guid id, bool underlyingCollections)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    return underlyingCollections
                        ? context.CreateDetachedCopy(context.Actions.Include(x => x.Privileges).FirstOrDefault(x => x.Id == id), x => x.Privileges)
                        : context.CreateDetachedCopy(context.Actions.FirstOrDefault(x => x.Id == id));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<Privilege> GetActionPrivileges(Guid actionId)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var action = context.Actions.Include(x => x.Privileges).FirstOrDefault(x => x.Id == actionId);
                    return action != null ? action.Privileges.ToList() : null;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<Action> GetActions()
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    return context.CreateDetachedCopy(context.Actions.ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Action AddAction(Action item)
        {
            XLogger.Trace("");

            try
            {
                if (item == null) return null;

                using (var context = GetDataContext())
                {
                    context.Add(item);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(item);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public Action UpdateAction(Action item)
        {
            XLogger.Trace("");

            try
            {
                if (item == null) return null;

                using (var context = GetDataContext())
                {
                    context.AttachCopy(item);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(item);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public bool DeleteAction(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var action = context.Actions.FirstOrDefault(x => x.Id == id);
                    context.Delete(action);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        # endregion
        # region Authorize

        public bool Authorize(IAuthorizationActor actor, IAuthorizationTarget target)
        {
            XLogger.Trace("");

            try
            {
                if (actor == null || target == null) return false;

                using (var context = GetDataContext())
                {
                    var paramTargetName = XDatabase.GetSqlParameter("TargetName", DbType.String, ParameterDirection.Input, target.Name);
                    var paramActorId = XDatabase.GetSqlParameter("ActorId", DbType.Guid, ParameterDirection.Input, actor.Id);

                    var result = context.ExecuteQuery<bool>("AuthorizeTargetWithDelegates", paramTargetName, paramActorId);
                    context.SaveChanges();

                    var isAuthorized = result.FirstOrDefault();
                    return isAuthorized;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }
        public bool Authorize(Guid actorId, string targetName)
        {
            XLogger.Trace("");

            try
            {
                if (actorId == Guid.Empty || string.IsNullOrWhiteSpace(targetName)) return false;

                using (var context = GetDataContext())
                {
                    var paramTargetName = XDatabase.GetSqlParameter("TargetName", DbType.String, ParameterDirection.Input, targetName);
                    var paramActorId = XDatabase.GetSqlParameter("ActorId", DbType.Guid, ParameterDirection.Input, actorId);

                    var result = context.ExecuteQuery<bool>("AuthorizeTargetWithDelegates", paramTargetName, paramActorId);
                    context.SaveChanges();

                    var isAuthorized = result.FirstOrDefault();
                    return isAuthorized;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        #endregion
        # region Actors

        public Actor GetActor(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var actor = context.Actors.Include(x => x.Roles).Include(x => x.Privileges).FirstOrDefault(x => x.Id == id);
                    return context.CreateDetachedCopy(actor, x => x.Roles, x => x.Privileges);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public bool CreateActor(Actor actor)
        {
            XLogger.Trace("");

            try
            {
                if (actor == null || actor.Id == Guid.Empty) return false;

                using (var context = GetDataContext())
                {
                    context.Add(actor);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }
        public bool UpdateActor(Actor actor)
        {
            XLogger.Trace("");

            try
            {
                if (actor == null) return false;

                using (var context = GetDataContext())
                {
                    context.AttachCopy(actor);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        #endregion

        #endregion
        # region IAuditTrailDataHandler

        public List<AuditTrailAction> GetAuditTrailActions(AuditTrailActionCriteria criteria)
        {
            XLogger.Info("DataHandler.GetAuditTrailActions ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var predicate = PredicateBuilder.True<AuditTrailAction>();

                    if (criteria.Id.HasValue)
                    {
                        predicate = predicate.And(x => x.Id == criteria.Id);
                    }
                    if (!string.IsNullOrEmpty(criteria.Name))
                    {
                        predicate = predicate.And(x => x.Name.Trim().ToLower().Contains(criteria.Name.Trim().ToLower()));
                    }
                    return context.CreateDetachedCopy(context.AuditTrailActions.Where(predicate).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetAuditTrailActions() ... Exception: ", x);
                return null;
            }
        }
        public List<AuditTrailAction> GetAuditTrailActions()
        {
            XLogger.Trace("DataHandler.GetAuditTrailActions ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    return context.CreateDetachedCopy(context.AuditTrailActions.ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetAuditTrailActions ... Exception : ", x);
                return null;
            }
        }
        public AuditTrailAction GetAuditTrailAction(Guid id)
        {
            XLogger.Info("DataHandler.GetAuditTrailAction ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var action = context.AuditTrailActions.FirstOrDefault(x => x.Id == id);
                    action = context.CreateDetachedCopy(action);
                    return action;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetAuditTrailAction ... Exception : ", x);
                return null;
            }
        }
        public AuditTrailAction AddAuditTrailAction(AuditTrailAction item)
        {
            XLogger.Info("DataHandler.AddAuditTrailAction ... ");

            try
            {
                if (item == null) return null;

                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    context.Add(item);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(item);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.AddAuditTrailAction ... Exception : ", x);
                return null;
            }
        }
        public bool DeleteAuditTrailAction(Guid id)
        {
            XLogger.Info("DataHandler.DeleteAuditTrailAction ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var action = context.AuditTrailActions.FirstOrDefault(x => x.Id == id);
                    context.Delete(action);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.DeleteAuditTrailAction ... Exception : ", x);
                return false;
            }
        }

        public AuditTrailLog Log(AuditTrailLog item)
        {
            XLogger.Info("DataHandler.Log ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    context.Add(item);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(item);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.Log ... Exception: ", x);
                return null;
            }
        }
        public List<AuditTrailLog> GetLogs(AuditTrailLogCriteria criteria, out int totalCount)
        {
            XLogger.Info("DataHandler.GetLogs ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var predicate = PredicateBuilder.True<AuditTrailLog>();

                    if (criteria.LogId.HasValue)
                    {
                        predicate = predicate.And(x => x.Id == criteria.LogId);
                    }
                    if (criteria.UserId.HasValue)
                    {
                        predicate = predicate.And(x => x.UserId == criteria.UserId);
                    }
                    if (!string.IsNullOrEmpty(criteria.Username))
                    {
                        predicate = predicate.And(x => x.Username.Trim().ToLower().Contains(criteria.Username.Trim().ToLower()));
                    }
                    if (criteria.DateFrom.HasValue)
                    {
                        predicate = predicate.And(x => x.ActionDate >= criteria.DateFrom);
                    }
                    if (criteria.DateTo.HasValue)
                    {
                        predicate = predicate.And(x => x.ActionDate <= criteria.DateTo);
                    }
                    if (!string.IsNullOrEmpty(criteria.IPAddress))
                    {
                        predicate = predicate.And(x => x.IpAddress.Contains(criteria.IPAddress));
                    }
                    if (!string.IsNullOrEmpty(criteria.MachineName))
                    {
                        predicate = predicate.And(x => x.MachineName.Trim().ToLower().Contains(criteria.MachineName.Trim().ToLower()));
                    }
                    if (criteria.ModuleId.HasValue)
                    {
                        predicate = predicate.And(x => x.ModuleId == criteria.ModuleId);
                    }
                    if (criteria.ActionId.HasValue)
                    {
                        predicate = predicate.And(x => x.ActionId == criteria.ActionId);
                    }
                    if (!string.IsNullOrEmpty(criteria.Details))
                    {
                        predicate = predicate.And(x => x.Details.Trim().ToLower().Contains(criteria.Details.Trim().ToLower()));
                    }

                    if (criteria.PageSize == 0)
                        criteria.PageSize = 100;

                    var skipRowsCount = criteria.PageIndex * criteria.PageSize;
                    var searchResults = context.AuditTrailLogs.Where(predicate).ToList();
                    totalCount = searchResults.Count();
                    return
                        context.CreateDetachedCopy(
                            searchResults.OrderByDescending(x => x.ActionDate)
                                .Skip(skipRowsCount)
                                .Take(criteria.PageSize)
                                .ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetLogs ... Exception: ", x);
                totalCount = 0;
                return null;
            }
        }
        public bool DeleteLog(Guid id)
        {
            XLogger.Info("DataHandler.DeleteLog ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var log = context.AuditTrailLogs.FirstOrDefault(x => x.Id == id);
                    context.Delete(log);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.DeleteLog ... Exception: ", x);
                return false;
            }
        }

        public List<AuditTrailModule> GetAuditTrailModules(AuditTrailModuleCriteria criteria)
        {
            XLogger.Info("DataHandler.GetAuditTrailModules ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var predicate = PredicateBuilder.True<AuditTrailModule>();

                    if (criteria.Id.HasValue)
                    {
                        predicate = predicate.And(x => x.Id == criteria.Id);
                    }
                    if (!string.IsNullOrEmpty(criteria.Name))
                    {
                        predicate = predicate.And(x => x.Name.Trim().ToLower().Contains(criteria.Name.Trim().ToLower()));
                    }

                    return context.CreateDetachedCopy(context.AuditTrailModules.Where(predicate).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetAuditTrailModules ... Exception: ", x);
                return null;
            }
        }
        public List<AuditTrailModule> GetAuditTrailModules()
        {
            XLogger.Info("DataHandler.GetAuditTrailModules ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    return context.CreateDetachedCopy(context.AuditTrailModules.ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetAuditTrailModules ... Exception: ", x);
                return null;
            }
        }
        public AuditTrailModule GetAuditTrailModule(Guid id)
        {
            XLogger.Info("DataHandler.GetAuditTrailModule ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var module = context.AuditTrailModules.FirstOrDefault(x => x.Id == id);
                    module = context.CreateDetachedCopy(module);
                    return module;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetAuditTrailModule ... Exception : ", x);
                return null;
            }
        }
        public AuditTrailModule AddAuditTrailModule(AuditTrailModule item)
        {
            XLogger.Info("DataHandler.AddAuditTrailModule ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    context.Add(item);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(item);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.AddAuditTrailModule ... Exception: ", x);
                return null;
            }
        }
        public bool DeleteAuditTrailModule(Guid id)
        {
            XLogger.Info("DataHandler.DeleteAuditTrailModule ... ");

            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var module = context.AuditTrailModules.FirstOrDefault(x => x.Id == id);
                    context.Delete(module);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.DeleteAuditTrailModule ... Exception : ", x);
                return false;
            }
        }

        # endregion
        # region IMasterCodeDataHandler

        public List<MasterCode> GetMasterCodes()
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    return context.CreateDetachedCopy(context.MasterCodes.Where(m => !m.IsDeleted).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetMasterCodes() ... Exception: ", x);
                return null;
            }
        }
        public MasterCode GetMasterCode(int id)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    return context.CreateDetachedCopy(context.MasterCodes.FirstOrDefault(x => (x.Id == id) && (!x.IsDeleted)));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetMasterCode() ... Exception: ", x);
                return null;
            }
        }
        public MasterCode GetMasterCode(int id, bool underlyingCollections)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    if (underlyingCollections)
                    {
                        return
                            context.CreateDetachedCopy(
                                context.MasterCodes.Include(x => x.ParentMasterCode)
                                    .Include(x => x.ChildMasterCodes).Include(x => x.DetailCodes)
                                    .FirstOrDefault(x => (x.Id == id) && (!x.IsDeleted)), x => x.ParentMasterCode, x => x.ChildMasterCodes,
                                x => x.DetailCodes);
                    }
                    return context.CreateDetachedCopy(context.MasterCodes.FirstOrDefault(x => (x.Id == id) && (!x.IsDeleted)));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetMasterCode() ... Exception: ", x);
                return null;
            }
        }
        public MasterCode GetMasterCode(string code)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    return
                        context.CreateDetachedCopy(
                            context.MasterCodes.FirstOrDefault(x => (x.Code.Trim().ToLower() == code.Trim().ToLower()) && (!x.IsDeleted)));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetMasterCode(code) ... Exception: ", x);
                return null;
            }
        }
        public MasterCode GetMasterCode(string code, bool underlyingCollections)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    if (underlyingCollections)
                    {
                        return
                            context.CreateDetachedCopy(
                                context.MasterCodes.Include(x => x.ParentMasterCode)
                                    .Include(x => x.ChildMasterCodes)
                                    .Include(x => x.DetailCodes)
                                    .FirstOrDefault(x => (x.Code.Trim().ToLower() == code.Trim().ToLower()) && (!x.IsDeleted)),
                                x => x.ParentMasterCode, x => x.ChildMasterCodes, x => x.DetailCodes);
                    }

                    return
                        context.CreateDetachedCopy(
                            context.MasterCodes.FirstOrDefault(x => (x.Code.Trim().ToLower() == code.Trim().ToLower()) && (!x.IsDeleted)));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetMasterCode(code, underlyingCollections) ... Exception: ", x);
                return null;
            }
        }
        public MasterCode CreateMasterCode(MasterCode masterCode)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    context.Add(masterCode);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(masterCode);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.CreateMasterCode() ... Exception: ", x);
                return null;
            }
        }
        public MasterCode UpdateMasterCode(MasterCode masterCode)
        {
            try
            {
                if (masterCode == null) return null;

                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    context.AttachCopy(masterCode);
                    context.SaveChanges();
                    if (OpenAccessContextBase.PersistenceState.IsDetached(masterCode))
                    {
                        return masterCode;
                    }
                    return context.CreateDetachedCopy(masterCode);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.UpdateMasterCode() ... Exception: ", x);
                return null;
            }
        }
        public bool DeleteMasterCode(int id)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var item = context.MasterCodes.FirstOrDefault(x => x.Id == id);
                    context.Delete(item);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.DeleteMasterCode() ... Exception: ", x);
                return false;
            }
        }

        // Get All MasterCodes that have IsDeleted property (set to False)
        public List<MasterCode> GetViewableMasterCodes()
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    return context.CreateDetachedCopy(context.MasterCodes.Where(x => !x.IsDeleted).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetViewableMasterCodes() ... Exception: ", x);
                return null;
            }
        }

        // Check Master Code Uniqueness
        public bool IsMasterCode_CodeUnique(string code)
        {
            try
            {
                var masterCode = GetMasterCode(code);
                return masterCode == null;
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.IsMasterCode_CodeUnique() ... Exception: ", x);
                return false;
            }
        }
        public bool IsMasterCode_NameUnique(string name)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var masterCode = context.MasterCodes.FirstOrDefault(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
                    return masterCode == null;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.IsMasterCode_NameUnique() ... Exception: ", x);
                return false;
            }
        }
        public bool IsMasterCode_NameCultureVariantUnique(string nameCultureVariant)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var masterCode =
                        context.MasterCodes.FirstOrDefault(
                            x => x.NameCultureVariant.Trim().ToLower() == nameCultureVariant.Trim().ToLower());
                    return masterCode == null;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.IsMasterCode_NameCultureVariantUnique() ... Exception: ", x);
                return false;
            }
        }
        public bool CheckMasterCodeExistance(int id)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    return context.MasterCodes.Any(x => x.Id == id);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: " + x);
                return false;
            }
        }

        // check if entered "Code" Unique or not with other elements not edited item.
        public bool IsMasterCode_CodeEditUnique(int id, string code)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var masterCode =
                        context.MasterCodes.FirstOrDefault(
                            x => x.Code.Trim().ToLower() == code.Trim().ToLower() && x.Id != id);
                    return masterCode == null;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.IsMasterCode_CodeEditUnique() ... Exception: ", x);
                return false;
            }
        }

        // check if entered "Name" Unique or not with other elements not edited item.
        public bool IsMasterCode_NameEditUnique(int id, string name)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var masterCode =
                        context.MasterCodes.FirstOrDefault(
                            x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.Id != id);
                    return masterCode == null;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.IsMasterCode_NameEditUnique() ... Exception: ", x);
                return false;
            }
        }

        // check if entered "NameCultureVariant" Unique or not with other elements not edited item.
        public bool IsMasterCode_NameCultureVariantEditUnique(int id, string nameCultureVariant)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var masterCode =
                        context.MasterCodes.FirstOrDefault(
                            x =>
                                x.NameCultureVariant.Trim().ToLower() == nameCultureVariant.Trim().ToLower() &&
                                x.Id != id);
                    return masterCode == null;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.IsMasterCode_NameCultureVariantEditUnique() ... Exception: ", x);
                return false;
            }
        }

        // Get All MasterCode parent hierarchy as "List of MasterCodeDTO"
        public List<MasterCodeDTO> GetMasterCodeParentsList(int id)
        {
            try
            {
                var parents = new List<MasterCodeDTO>();
                var masterCode = GetMasterCode(id, true);
                if (masterCode == null) return null;
                while (masterCode.ParentId.HasValue)
                {
                    parents.Add(new MasterCodeDTO
                    {
                        Id = (int)masterCode.ParentId,
                        Name = masterCode.ParentMasterCode.Name,
                        NameCultureVariant = masterCode.ParentMasterCode.NameCultureVariant
                    });
                    masterCode = GetMasterCode((int)masterCode.ParentId, true);
                }
                return parents;
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetMasterCodeParentsList() ... Exception: ", x);
                return null;
            }
        }

        public List<MasterCode> SearchByName(string name)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var masterCodes =
                        context.MasterCodes.Where(x => x.Name.Trim().ToLower().Contains(name.Trim().ToLower()))
                            .ToList();
                    return context.CreateDetachedCopy(masterCodes);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.SearchByName() ... Exception: ", x);
                return null;
            }
        }

        # endregion
        # region IDetailCodeDataHandler

        public List<DetailCode> GetDetailCodes()
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    return context.CreateDetachedCopy(context.DetailCodes.Where(m => !m.IsDeleted).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCodes() ... Exception: ", x);
                return null;
            }
        }
        public DetailCode GetDetailCode(int id)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    var detailCode = context.DetailCodes.FirstOrDefault(x => (x.Id == id) && (!x.IsDeleted));
                    if (detailCode == null) return null;

                    return context.CreateDetachedCopy(detailCode);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCode() ... Exception: ", x);
                return null;
            }
        }
        public DetailCode GetDetailCode(int id, bool underlyingCollections)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    if (underlyingCollections)
                    {
                        return
                            context.CreateDetachedCopy(
                                context.DetailCodes.Include(x => x.ParentDetailCode)
                                    .Include(x => x.ChildDetailCodes).Include(x => x.MasterCode)
                                    .FirstOrDefault(x => (x.Id == id) && (!x.IsDeleted)), x => x.ParentDetailCode, x => x.ChildDetailCodes,
                                x => x.MasterCode);
                    }
                    return context.CreateDetachedCopy(context.DetailCodes.FirstOrDefault(x => x.Id == id));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCode() ... Exception: ", x);
                return null;
            }
        }
        public DetailCode GetDetailCode(string code)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    return
                        context.CreateDetachedCopy(
                            context.DetailCodes.FirstOrDefault(x => (x.Code.Trim().ToLower() == code.Trim().ToLower()) && (!x.IsDeleted)));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCode(code) ... Exception: ", x);
                return null;
            }
        }
        public DetailCode GetDetailCode(string code, bool underlyingCollections)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();
                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    if (underlyingCollections)
                    {
                        return
                            context.CreateDetachedCopy(
                                context.DetailCodes.Include(x => x.ParentDetailCode)
                                    .Include(x => x.ChildDetailCodes)
                                    .Include(x => x.MasterCode)
                                    .FirstOrDefault(x => (x.Code.Trim().ToLower() == code.Trim().ToLower()) && (!x.IsDeleted)),
                                x => x.ParentDetailCode, x => x.ChildDetailCodes, x => x.MasterCode);
                    }

                    return
                        context.CreateDetachedCopy(
                            context.DetailCodes.FirstOrDefault(x => (x.Code.Trim().ToLower() == code.Trim().ToLower()) && (!x.IsDeleted)));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCode(code, underlyingCollections) ... Exception: ", x);
                return null;
            }
        }
        public DetailCode CreateDetailCode(DetailCode detailCode)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    context.Add(detailCode);
                    context.SaveChanges();
                    return context.CreateDetachedCopy(detailCode);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.CreateDetailCode() ... Exception: ", x);
                return null;
            }
        }
        public DetailCode UpdateDetailCode(DetailCode detailCode)
        {
            try
            {
                if (detailCode == null) return null;

                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    context.AttachCopy(detailCode);
                    context.SaveChanges();
                    if (OpenAccessContextBase.PersistenceState.IsDetached(detailCode))
                    {
                        return detailCode;
                    }
                    return context.CreateDetachedCopy(detailCode);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.UpdateDetailCode() ... Exception: ", x);
                return null;
            }
        }
        public bool DeleteDetailCode(int id)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var item = context.DetailCodes.FirstOrDefault(x => x.Id == id);
                    context.Delete(item);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.DeleteDetailCode() ... Exception: ", x);
                return false;
            }
        }

        public List<DetailCode> GetDetailCodesByMasterCode( string masterCode, bool allDetailCodes )
        {
            try
            {
                if ( !allDetailCodes ) return GetDetailCodesByMasterCode( masterCode );
                else
                {
                    using ( var context = GetDataContext() )
                    {
                        return context.CreateDetachedCopy( context.DetailCodes.Where( m => m.MasterCode.Code == masterCode ).ToList() );
                    }
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "DataHandler.GetDetailCodes() ... Exception: ", x );
                return null;
            }
        }
        public List<DetailCode> GetDetailCodesByMasterCode(string masterCode)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    return context.CreateDetachedCopy(context.DetailCodes.Where(m => !m.IsDeleted && m.MasterCode.Code == masterCode).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCodes() ... Exception: ", x);
                return null;
            }
        }
        public List<DetailCode> GetDetailCodesByMasterCode(int masterCode)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    //MG,2-11-2014.Add condition to queries to get undeleted items only
                    return context.CreateDetachedCopy(context.DetailCodes.Where(m => !m.IsDeleted && m.MasterCode.Id == masterCode).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCodes() ... Exception: ", x);
                return null;
            }
        }
        // Get All DetailCodes that have IsDeleted property (set to False)
        public List<DetailCode> GetViewableDetailCodes()
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    return context.CreateDetachedCopy(context.DetailCodes.Where(x => !x.IsDeleted).ToList());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetViewableDetailCodes() ... Exception: ", x);
                return null;
            }
        }

        // Check Detail Code Uniqueness
        public bool IsDetailCode_CodeUnique(string code)
        {
            try
            {
                var detailCode = GetDetailCode(code);
                return detailCode == null;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: " + x);
                return false;
            }
        }
        public bool IsDetailCode_NameUnique(string name)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    return context.DetailCodes.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: " + x);
                return false;
            }
        }
        public bool IsDetailCode_NameCultureVariantUnique(string nameCultureVariant)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    return context.DetailCodes.Any(x => x.NameCultureVariant.Trim().ToLower() == nameCultureVariant.Trim().ToLower());
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: " + x);
                return false;
            }
        }

        public bool CheckDetailCodeExistance(int id)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    return context.DetailCodes.Any(x => x.Id == id);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: " + x);
                return false;
            }
        }

        public List<DetailCodeDTO> GetDetailCodeParentsList(int id)
        {
            try
            {
                var parents = new List<DetailCodeDTO>();
                var detailCode = GetDetailCode(id, true);
                if (detailCode == null) return null;
                while (detailCode.ParentId.HasValue)
                {
                    parents.Add(new DetailCodeDTO
                    {
                        Id = (int)detailCode.ParentId,
                        Name = detailCode.ParentDetailCode.Name,
                        NameCultureVariant = detailCode.ParentDetailCode.NameCultureVariant,
                        MasterCodeId = detailCode.ParentDetailCode.MasterCodeId
                    });
                    detailCode = GetDetailCode((int)detailCode.ParentId, true);
                }
                return parents;
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.GetDetailCodeParentsList() ... Exception: ", x);
                return null;
            }
        }
        public List<DetailCode> SearchDetailCodes(DetailCodeCriteria criteria)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //var context = OpenDataSourceConnection();

                    var skip = criteria.PageIndex * criteria.PageSize;

                    var predicate = PredicateBuilder.True<DetailCode>();
                    predicate = predicate.And(x => x.MasterCodeId == criteria.MasterCodeId &&
                                                   (!x.IsDeleted));
                    if (criteria.ParentId.HasValue)
                        predicate = predicate.And(x => x.ParentId == criteria.ParentId);

                    if (!(string.IsNullOrEmpty(criteria.Name)))
                        predicate = predicate.And(x => x.Name.Trim().ToLower().Contains(criteria.Name.Trim().ToLower()));

                    if (!(string.IsNullOrEmpty(criteria.NameCultureVariant)))
                        predicate =
                            predicate.And(
                                x =>
                                    x.NameCultureVariant.Trim()
                                        .ToLower()
                                        .Contains(criteria.NameCultureVariant.Trim().ToLower()));

                    if (!(string.IsNullOrEmpty(criteria.Code)))
                        predicate = predicate.And(x => x.Code.Trim().ToLower().Contains(criteria.Code.Trim().ToLower()));

                    if (!(string.IsNullOrEmpty(criteria.FieldOneValue)))
                        predicate =
                            predicate.And(
                                x => x.FieldOneValue.Trim().ToLower().Contains(criteria.FieldOneValue.Trim().ToLower()));

                    if (!(string.IsNullOrEmpty(criteria.FieldTwoValue)))
                        predicate =
                            predicate.And(
                                x => x.FieldTwoValue.Trim().ToLower().Contains(criteria.FieldTwoValue.Trim().ToLower()));

                    if (!(string.IsNullOrEmpty(criteria.FieldThreeValue)))
                        predicate =
                            predicate.And(
                                x =>
                                    x.FieldThreeValue.Trim()
                                        .ToLower()
                                        .Contains(criteria.FieldThreeValue.Trim().ToLower()));

                    // Order By
                    if (criteria.OrderBy == null)
                        criteria.OrderBy = x => x.Name;


                    criteria.TotalCount = context.DetailCodes.Where(predicate).Count();

                    if (criteria.PageSize == criteria.TotalCount)
                        skip = 0;

                    return
                        context.DetailCodes.Where(predicate)
                            .OrderBy(criteria.OrderBy)
                            .Skip(skip)
                            .Take(criteria.PageSize).ToList();
                }
            }
            catch (Exception x)
            {
                XLogger.Error("DataHandler.SearchDetailCodes() ... Exception: ", x);
                return null;
            }
        }

        #endregion
        # region INotificationsSenderDataHandler

        public bool Notify(NotificationMessage message)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    if (message.Attachments != null && message.Attachments.Any())
                        message.AttachmentsSerialized = XSerialize.Serialize(XSerialize.Mode.Xml, message.Attachments);

                    context.Add(message);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: ", x);
                return false;
            }
        }
        public bool DeleteRawNotification(string targetId)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var notifications = context.NotificationMessages.Where(x => x.TargetId == targetId).ToList();
                    context.Delete(notifications);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: ", x);
                return false;
            }
        }

        # endregion
        # region INotificationsListnerDataHandler

        public NotificationMessage GetNextNotification()
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var message = context.NotificationMessages.OrderBy(x => x.CreationTime).FirstOrDefault();

                    if (message != null && !string.IsNullOrEmpty(message.AttachmentsSerialized))
                        message.Attachments = XSerialize.Deserialize<List<NotificationAttachment>>(XSerialize.Mode.Xml, message.AttachmentsSerialized);

                    return message;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<NotificationMessage> GetNextNotifications()
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var messages = context.NotificationMessages.OrderBy(x => x.CreationTime).ToList();

                    for (int i = 0; i < messages.Count; i++)
                    {
                        if (messages[i] != null && !string.IsNullOrEmpty(messages[i].AttachmentsSerialized))
                        {
                            messages[i].Attachments = XSerialize.Deserialize<List<NotificationAttachment>>(XSerialize.Mode.Xml, messages[i].AttachmentsSerialized);
                        }
                    }

                    return messages;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public bool DeleteNotification(Guid id)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    //DataContext = OpenDataSourceConnection();

                    var message = context.NotificationMessages.FirstOrDefault(x => x.Id == id);
                    context.Delete(message);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        # endregion
        # region INotificationsListSenderDataHandler

        public bool Save(NotificationDetailedMessage message)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    //DataContext = OpenDataSourceConnection();
                    context.Add(message);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        # endregion

        # region INotificationsEmailSenderDataHandler

        public bool Save( EmailMessage message )
        {
            try
            {
                using ( var context = GetDataContext() )
                {
                    context.Add( message );
                    context.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : ", x );
                return false;
            }
        }
        public bool Delete( Guid id )
        {
            try
            {
                using ( var context = GetDataContext() )
                {
                    var notifications = context.EmailMessages.Where( x => x.Id == id ).ToList();
                    context.Delete( notifications );
                    context.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: ", x );
                return false;
            }
        }
        public List<EmailMessage> GetAll()
        {
            XLogger.Info( "" );

            try
            {
                using ( var context = GetDataContext() )
                {
                    var messages = context.EmailMessages.ToList();

                    for ( int i = 0; i < messages.Count; i++ )
                    {
                        if ( messages[i] != null && !string.IsNullOrWhiteSpace( messages[i].AttachmentsSerialized ) )
                        {
                            messages[i].Attachments = XSerialize.Deserialize<List<NotificationAttachment>>( XSerialize.Mode.Xml, messages[i].AttachmentsSerialized );
                        }
                    }

                    return messages;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : ", x );
                return null;
            }
        }

        # endregion

        # region INotificationDataHandler

        public NotificationDetailedMessage GetNotification(Guid id)
        {
            XLogger.Info("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    var notification = dbContext.NotificationDetailedMessages.Where(x => x.Id == id).FirstOrDefault();
                    return notification;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public NotificationDetailedMessage GetNotification(Guid id, string personId)
        {
            XLogger.Info("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    var notification = dbContext.NotificationDetailedMessages.FirstOrDefault(x => x.Id == id && x.PersonId == personId);
                    return notification;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public NotificationDetailedMessage GetNotification(string code, string personId)
        {
            XLogger.Info("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    var notification = IsOracle()
                                       ? dbContext.NotificationDetailedMessages.SingleOrDefault(x => x.Code.ToLower() == code.ToLower() && x.PersonId == personId)
                                       : dbContext.NotificationDetailedMessages.SingleOrDefault(x => x.Code == code && x.PersonId == personId);
                    return notification;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }

        public List<NotificationDetailedMessage> GetNotificationsByPerson(string personId)
        {
            XLogger.Info("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    var notifications = dbContext.NotificationDetailedMessages.Where(x => x.PersonId == personId).ToList();
                    return notifications;
                }

            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<NotificationDetailedMessage> GetNotificationsByTarget(string targetId)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    //var notification = context.NotificationDetailedMessages.Where( x => x.TargetId == targetId && x.Status == NotificationStatus.Pending ).ToList();
                    var notification = context.NotificationDetailedMessages.Where(x => x.TargetId == targetId).ToList();
                    return notification;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public int GetPendingNotificationsCountByPerson(string personId)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var count = context.NotificationDetailedMessages.Count(x => x.PersonId == personId && x.Status == NotificationStatus.Pending);
                    return count;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return 0;
            }
        }

        public bool MarkNotificationAsRead(Guid id)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var notification = context.NotificationDetailedMessages.FirstOrDefault(x => x.Id == id);
                    if (notification == null) return false;

                    notification.IsRead = true;
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }
        public bool MarkNotificationAsHandled(Guid id)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var notification = context.NotificationDetailedMessages.FirstOrDefault(x => x.Id == id);
                    if (notification == null) return false;

                    notification.Status = NotificationStatus.Handled;
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }
        public bool DeleteDetailedNotification(Guid id)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var notification = context.NotificationDetailedMessages.FirstOrDefault(x => x.Id == id);
                    if (notification == null) return false;

                    context.Delete(notification);
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }
        public bool MarkNotificationAsHandledByTarget(string targetId)
        {
            XLogger.Info("");

            try
            {
                using (var context = GetDataContext())
                {
                    var notifications = context.NotificationDetailedMessages.Where(x => x.TargetId == targetId).ToList();
                    if (notifications == null || notifications.Count == 0) return false;

                    foreach (NotificationDetailedMessage notification in notifications) notification.Status = NotificationStatus.Handled;
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        #endregion
        # region IWorkflowDataHandler

        public WorkflowInstance InstanceGet(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    //var dbContext = OpenDataSourceConnection();
                    var instance = dbContext.WorkflowInstances.FirstOrDefault(x => x.Id == id);
                    return InstanceDeserialize(dbContext.CreateDetachedCopy(instance));
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public WorkflowInstance InstanceGetByTarget(Guid targetId, Guid definitionId)
        {
            XLogger.Trace("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    //var dbContext = OpenDataSourceConnection();
                    var instance = dbContext.WorkflowInstances.FirstOrDefault(x => x.TargetId == targetId && x.DefinitionId == definitionId);
                    return InstanceDeserialize(instance);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<WorkflowInstance> InstancesGetByPerson(Guid personId)
        {
            XLogger.Trace("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    //var dbContext = OpenDataSourceConnection();
                    var instancesNative = dbContext.WorkflowInstances.Where(x => x.PersonId == personId).ToList();
                    var instances = instancesNative.Select(x => InstanceDeserialize(x)).ToList();

                    return instances;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<WorkflowInstance> InstancesGetByAffinity(string metadata)
        {
            XLogger.Trace("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    var instancesNative = IsOracle()
                                          ? dbContext.WorkflowInstances.Where(x => x.Metadata.ToLower().Contains(metadata.ToLower())).ToList()
                                          : dbContext.WorkflowInstances.Where(x => x.Metadata.Contains(metadata)).ToList();

                    var instances = instancesNative.Select(x => InstanceDeserialize(x)).ToList();
                    return instances;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return null;
            }
        }
        public List<WorkflowInstance> InstancesGetByAffinity( string metadata, WorkflowInstanceStatus status )
        {
            XLogger.Trace( "" );

            try
            {
                using ( var dbContext = GetDataContext() )
                {
                    List<WorkflowInstance> nativeInstances;

                    if ( IsOracle() )
                    {
                        nativeInstances = dbContext.WorkflowInstances.Where( x => x.Metadata.ToLower().Contains( metadata.ToLower() ) && x.Status == status ).ToList();
                    }
                    else
                    {
                        nativeInstances = dbContext.WorkflowInstances.Where( x => x.Metadata.Contains( metadata ) && x.Status == status ).ToList();
                    }

                    var instances = nativeInstances.Select( x => InstanceDeserialize( x ) ).ToList();
                    return instances;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : ", x );
                return null;
            }
        }

        public bool InstanceSave(WorkflowInstance instance)
        {
            XLogger.Trace("");

            try
            {
                instance = InstanceSerialize(instance);

                using (var context = GetDataContext())
                {
                    var instanceOld = context.WorkflowInstances.FirstOrDefault(x => x.Id == instance.Id);
                    if (instanceOld != null)
                    {
                        //context.AttachCopy(instance);

                        context.Delete(instanceOld);
                        context.SaveChanges();
                        context.Add(instance);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Add(instance);
                        context.SaveChanges();
                    }

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }
        public bool InstanceDelete(Guid id)
        {
            XLogger.Trace("");

            try
            {
                using (var context = GetDataContext())
                {
                    var instance = context.WorkflowInstances.FirstOrDefault(x => x.Id == id);
                    context.Delete(instance);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: ", x);
                return false;
            }
        }
        public bool InstanceDelete(WorkflowInstance instance)
        {
            return InstanceDelete(instance.Id);
        }
        public bool InstanceExists(Guid targetId, Guid definitionId)
        {
            XLogger.Trace("");

            try
            {
                using (var dbContext = GetDataContext())
                {
                    //var dbContext = OpenDataSourceConnection();
                    return dbContext.WorkflowInstances.Any(x => x.TargetId == targetId && x.DefinitionId == definitionId);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        #region Helpers

        private WorkflowInstance InstanceSerialize(WorkflowInstance instance)
        {
            if (instance == null) return null;
            if (instance.Id == Guid.Empty) instance.Id = Guid.NewGuid();

            instance.SerializedInstance = null;
            instance.SerializedInstance = XSerialize.Serialize(XSerialize.Mode.Xml, instance, instance.WorkflowSupportingTypes);
            instance.WorkflowSupportingTypesSerialized = XSerialize.Serialize(XSerialize.Mode.Xml, instance.WorkflowSupportingTypes);

            return instance;
        }
        private WorkflowInstance InstanceDeserialize(WorkflowInstance instance)
        {
            if (instance == null) return null;
            instance.WorkflowSupportingTypes = XSerialize.Deserialize<List<string>>(XSerialize.Mode.Xml, instance.WorkflowSupportingTypesSerialized);
            return instance = XSerialize.Deserialize<WorkflowInstance>(XSerialize.Mode.Xml, instance.SerializedInstance, instance.WorkflowSupportingTypes);
        }

        #endregion

        # endregion
        #region ISMSNotificationsDataHandler

        public bool CreateMessage(SMSMessage message)
        {
            try
            {
                using (var context = GetDataContext())
                {
                    context.Add(message);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
                return false;
            }
        }

        #endregion

        #endregion

        #region Helpers

        //private DomainDataContext OpenDataSourceConnectionX()
        //{
        //    try
        //    {
        //        if ( DataContext == null ) DataContext = GetDataContext();  // open the session connect ..
        //        var conn = DataContext.Connection;  // test the db connection ..

        //        if ( conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken )
        //        {
        //            throw new ObjectDisposedException( "DB Connection was closed or broken" );
        //        }

        //        return DataContext;
        //    }
        //    catch ( ObjectDisposedException )
        //    {
        //        DataContext = GetDataContext();  // open the session connect ..
        //        var conn = DataContext.Connection;  // test the db connection ..

        //        return DataContext;
        //    }
        //    catch ( Exception )
        //    {
        //        throw;
        //    }
        //}
        private bool IsOracle()
        {
            //return false;
            return Broker.Settings.Datastore.Type == DatastoreType.Oracle;
        }
        private bool IsOracle(DomainDataContext dbContext)
        {
            //return false;
            return dbContext.Metadata.BackendType != Telerik.OpenAccess.Metadata.Backend.MsSql;
        }
        private DomainDataContext GetDataContext()
        {
            var dbContext = new DomainDataContext();

            dbContext.BackendInfo.MaximumNumberOfInValues = Math.Max(Broker.Settings.Datastore.MaximumNumberOfInValues, dbContext.BackendInfo.MaximumNumberOfInValues);
            dbContext.BackendInfo.MaximumNumberOfQueryParameters = Math.Max(Broker.Settings.Datastore.MaximumNumberOfQueryParameters, dbContext.BackendInfo.MaximumNumberOfQueryParameters);

            return dbContext;
        }

        #endregion
    }
}