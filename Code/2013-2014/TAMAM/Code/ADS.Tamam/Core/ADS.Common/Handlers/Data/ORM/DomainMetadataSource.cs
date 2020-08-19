using System.Text;
using System.Configuration;
using System.Collections.Generic;
using ADS.Common.Models.Domain.Notification;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using Telerik.OpenAccess.Metadata.Fluent.Artificial;

using ADS.Common.Models.Domain;
using ADS.Common.Workflow.Models;
using ADS.Common.Models.Domain.Authorization;
using Role = ADS.Common.Models.Domain.Authorization.Role;
using Privilege = ADS.Common.Models.Domain.Authorization.Privilege;
using ADS.Common.Bases.MessageQueuing.Models;

namespace ADS.Common.Handlers.Data.ORM
{
    public partial class DomainMetadataSource : FluentMetadataSource
    {
        #region FluentMetadataSource

        protected override MetadataContainer CreateModel()
        {
            var model = base.CreateModel();

            bool useDelimitedSQL = true;
            bool.TryParse( ConfigurationManager.AppSettings["UseDelimitedSQL"] , out useDelimitedSQL );
            model.DefaultMapping.UseDelimitedSQL = useDelimitedSQL;
            return model;
        }
        protected override IList<MappingConfiguration> PrepareMapping()
        {
            var configurations = new List<MappingConfiguration>();

            var ConfigItemMap = MapConfigurationItem();
            var auditTrailActionMap = MapAuditTrailAction();
            var auditTrailModuleMap = MapAuditTrailModule();
            var auditTrailLogMap = MapAuditTrailLog();
            var masterCodeMap = MapMasterCode();
            var detailCodeMap = MapDetailCode();

            var workflowInstanceMap = MapWorkflowInstance();
            var actionMap = MapAuthorization_Action();
            var privilegeMap = MapAuthorization_Privilege();
            var roleMap = MapAuthorization_Role();
            var actorMap = MapAuthorization_Actor();
            

            // Notification
            var notificationMessageMap = MapNotificationMessage();
            var leaveApprovalNotificationMap = MapLeaveApprovalNotificationMessage();
            var excuseApprovalNotificationMap = MapExcuseApprovalNotificationMessage();
            var lateAttendanceNotificationMap = MapLateAttendanceNotificationMessage();
            var scheduledReportNotificationMap = MapScheduledReportNotificationMessage();
            var attendanceManualEditNotification = MapAttendanceManualEditNotification();

            var smsMessageMap = MapSMSMessage();
            var emailMessageMap = MapEmailMessage();

            var notificationDetailedMessageMap = MapNotificationDetailedMessage();
            var leaveApprovalNotificationDetailedMessage = MapLeaveApprovalNotificationDetailedMessage();
            var excuseApprovalNotificationDetailedMessage = MapExcuseApprovalNotificationDetailedMessage();
            var lateAttendanceNotificationDetailedMessage = MapLateAttendanceNotificationDetailedMessage();
            var scheduledReportNotificationDetailedMessage = MapScheduledReportNotificationDetailedMessage();
            var attendanceManualEditNotificationDetailedMessage = MapAttendanceManualEditNotificationDetailedMessage();

            var messageQueuePoolMap = MapMessageQueuePool();

            // Associations (Many-To- Many)
            roleMap.HasArtificialCollectionAssociation( "Privileges" , privilegeMap.ConfiguredType ).IsManaged( true ).MapJoinTable( "Authorization_RolePrivileges" , "RoleId" , "PrivilegeId" );
            actionMap.HasArtificialCollectionAssociation( "Privileges" , privilegeMap.ConfiguredType ).IsManaged( true ).MapJoinTable( "Authorization_ActionPrivileges" , "ActionId" , "PrivilegeId" );
            actorMap.HasArtificialCollectionAssociation( "Privileges" , privilegeMap.ConfiguredType ).IsManaged( true ).MapJoinTable( "Authorization_ActorPrivileges" , "ActorId" , "PrivilegeId" );
            actorMap.HasArtificialCollectionAssociation( "Roles" , roleMap.ConfiguredType ).IsManaged( true ).MapJoinTable( "Authorization_ActorRoles" , "ActorId" , "RoleId" );

            configurations.Add( ConfigItemMap );
            configurations.Add( auditTrailActionMap );
            configurations.Add( auditTrailModuleMap );
            configurations.Add( auditTrailLogMap );
            configurations.Add( masterCodeMap );
            configurations.Add( detailCodeMap );
            configurations.Add( workflowInstanceMap );
            

            // Authorization ...
            configurations.Add( actionMap );
            configurations.Add( privilegeMap );
            configurations.Add( roleMap );
            configurations.Add( actorMap );

            // Notifications..
            configurations.Add( notificationMessageMap );
            configurations.Add( leaveApprovalNotificationMap );
            configurations.Add( excuseApprovalNotificationMap );
            configurations.Add( lateAttendanceNotificationMap );
            configurations.Add( scheduledReportNotificationMap );
            configurations.Add( attendanceManualEditNotification );

            configurations.Add( smsMessageMap );
            configurations.Add( emailMessageMap );

            configurations.Add( notificationDetailedMessageMap );
            configurations.Add( leaveApprovalNotificationDetailedMessage );
            configurations.Add( excuseApprovalNotificationDetailedMessage );
            configurations.Add( lateAttendanceNotificationDetailedMessage );
            configurations.Add( scheduledReportNotificationDetailedMessage );
            configurations.Add( attendanceManualEditNotificationDetailedMessage );

            configurations.Add( messageQueuePoolMap );

            return configurations;
        }
        //protected override void SetContainerSettings(MetadataContainer container)
        //{
        //    container.Name = "DomainDataContext";
        //    container.DefaultNamespace = "ADS.Common.domain";
        //    container.NameGenerator.SourceStrategy = Telerik.OpenAccess.Metadata.NamingSourceStrategy.Property;
        //    container.NameGenerator.ResolveReservedWords = false;
        //    container.NameGenerator.RemoveCamelCase = false;
        //}

        #endregion
        #region Mappings

        protected MappingConfiguration MapConfigurationItem()
        {
            var ConfigurationItemMappingConfiguration = new MappingConfiguration<ConfigurationItem>();
            ConfigurationItemMappingConfiguration.MapType( p => new
            {
                Id = p.Id ,
                ApplicationId = p.ApplicationId ,
                Key = p.Key ,
                Value = p.Value ,
                Active = p.Active ,
                //SomeProp = p.SomeProp ,
            } ).ToTable( "ConfigurationsData" );

            ConfigurationItemMappingConfiguration.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );
            ConfigurationItemMappingConfiguration.HasProperty(p => p.Value).IsUnicode();
            //ConfigurationItemMappingConfiguration.HasProperty( p => p.SomeProp ).HasFieldName( "_SomeProp" ).WithDataAccessKind( DataAccessKind.ReadWrite ).ToColumn( "SomeProp" ).IsNullable().HasColumnType( "nvarchar" ).HasLength( 50 );

            return ConfigurationItemMappingConfiguration;
        }
        protected MappingConfiguration MapAuditTrailAction()
        {
            var auditTrailActionMappingConfig = new MappingConfiguration<AuditTrailAction>();
            auditTrailActionMappingConfig.MapType( p => new
            {
                //Id = p.Id ,
                Id = p.Code ,
                Name = p.Name ,
            } ).ToTable( "LogAction" );

            auditTrailActionMappingConfig.HasProperty( p => p.Code ).IsIdentity( KeyGenerator.Autoinc );

            return auditTrailActionMappingConfig;
        }
        protected MappingConfiguration MapAuditTrailModule()
        {
            var auditTrailModuleMappingConfig = new MappingConfiguration<AuditTrailModule>();
            auditTrailModuleMappingConfig.MapType( p => new
            {
                //Id = p.Id ,
                Id = p.Code ,
                Name = p.Name ,
            } ).ToTable( "LogModule" );

            auditTrailModuleMappingConfig.HasProperty( p => p.Code ).IsIdentity( KeyGenerator.Autoinc );

            return auditTrailModuleMappingConfig;
        }
        protected MappingConfiguration MapAuditTrailLog()
        {
            var auditTrailLogMappingConfig = new MappingConfiguration<AuditTrailLog>();
            auditTrailLogMappingConfig.MapType( p => new
            {
                Id = p.Code ,
                IPAddress = p.IpAddress ,
                MachineName = p.MachineName ,
                ActionDate = p.ActionDate ,
                RefKey = p.RefKey ,
                Details = p.Details ,
                ModuleId = p.ModuleId ,
                //ModuleName = p.ModuleName,
                Action = p.ActionId ,
                //ActionName = p.ActionName,
                UserId = p.UserId ,
                UserName = p.Username
            } ).ToTable( "LogUserAction" );

            auditTrailLogMappingConfig.HasProperty( p => p.Code ).IsIdentity( KeyGenerator.Autoinc );

            // One Sided Association (Action & Module)
            auditTrailLogMappingConfig.HasAssociation( log => log.Action ).ToColumn( "Action" );
            auditTrailLogMappingConfig.HasAssociation( log => log.Module ).ToColumn( "ModuleId" );

            return auditTrailLogMappingConfig;
        }
        protected MappingConfiguration MapMasterCode()
        {
            var masterCodeMappingConfig = new MappingConfiguration<MasterCode>();
            masterCodeMappingConfig.MapType( p => new
            {
                Id = p.Id ,
                Code = p.Code ,
                Name = p.Name ,
                NameOther = p.NameCultureVariant ,
                IsActive = p.IsActive ,
                IsDeleted = p.IsDeleted ,
                ParentId = p.ParentId ,
                Field1EnTitle = p.FieldOneTitle ,
                Field1OtherTitle = p.FieldOneTitleCultureVariant ,
                Field1IsVisible = p.FieldOneIsVisible ,
                Field2EnTitle = p.FieldTwoTitle ,
                Field2OtherTitle = p.FieldTwoTitleCultureVariant ,
                Field2IsVisible = p.FieldTwoIsVisible ,
                Field3EnTitle = p.FieldThreeTitle ,
                Field3OtherTitle = p.FieldThreeTitleCultureVariant ,
                Field3IsVisible = p.FieldThreeIsVisible ,
                CreatedBy = p.CreatedBy ,
                CreatedOn = p.CreatedOn ,
                UpdatedBy = p.UpdatedBy ,
                UpdatedOn = p.UpdatedOn

            } ).ToTable( "ScMasterCode" );

            masterCodeMappingConfig.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Autoinc );

            // Self-Reference (Parent Master Code)
            masterCodeMappingConfig.HasAssociation( e => e.ParentMasterCode )
                .WithOpposite( e => e.ChildMasterCodes )
                .HasConstraint( ( e , r ) => e.ParentId == r.Id );

            masterCodeMappingConfig.HasProperty( p => p.Name ).IsUnicode();
            masterCodeMappingConfig.HasProperty( p => p.Code ).IsUnicode();
            masterCodeMappingConfig.HasProperty( p => p.NameCultureVariant ).IsUnicode();

            return masterCodeMappingConfig;
        }
        protected MappingConfiguration MapDetailCode()
        {
            var detailCodeMappingConfig = new MappingConfiguration<DetailCode>();
            detailCodeMappingConfig.MapType( p => new
            {
                Id = p.Id ,
                Code = p.Code ,
                Name = p.Name ,
                NameOther = p.NameCultureVariant ,
                ParentId = p.ParentId ,
                MasterCodeId = p.MasterCodeId ,
                IsActive = p.IsActive ,
                IsDeleted = p.IsDeleted ,
                Field1 = p.FieldOneValue ,
                Field2 = p.FieldTwoValue ,
                Field3 = p.FieldThreeValue ,
                CreatedBy = p.CreatedBy ,
                CreatedOn = p.CreatedOn ,
                UpdatedBy = p.UpdatedBy ,
                UpdatedOn = p.UpdatedOn
            } ).ToTable( "ScDetailCode" );

            detailCodeMappingConfig.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Autoinc );

            // Self-Reference (Parent Master Code)
            detailCodeMappingConfig.HasAssociation( e => e.ParentDetailCode )
                .WithOpposite( e => e.ChildDetailCodes )
                .HasConstraint( ( e , r ) => e.ParentId == r.Id );

            // One-To-Many Association - (MasterCode and DetailCode)
            detailCodeMappingConfig.HasAssociation( p => p.MasterCode )
                .WithOpposite( c => c.DetailCodes )
                .HasConstraint( ( p , c ) => p.MasterCodeId == c.Id );

            detailCodeMappingConfig.HasProperty( p => p.Name ).IsUnicode();
            detailCodeMappingConfig.HasProperty( p => p.Code ).IsUnicode();
            detailCodeMappingConfig.HasProperty( p => p.NameCultureVariant ).IsUnicode();

            return detailCodeMappingConfig;
        }
        protected MappingConfiguration MapWorkflowInstance()
        {
            var workflowInstanceMapping = new MappingConfiguration<WorkflowInstance>();
            workflowInstanceMapping.MapType( p => new
            {
                Id = p.Id ,
                TargetId = p.TargetId ,
                PersonId = p.PersonId ,
                DefinitionId = p.DefinitionId ,
                TimeCreated = p.TimeCreated ,
                Status = p.Status ,
                DataSerialized = p.SerializedInstance ,
                SupportingTypes = p.WorkflowSupportingTypesSerialized ,
                Metadata = p.Metadata ,

            } ).ToTable( "WorkflowInstances" );

            workflowInstanceMapping.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );
            workflowInstanceMapping.HasProperty( p => p.SerializedInstance ).IsUnicode().ToColumn( "DataSerialized" );

            return workflowInstanceMapping;
        }
        

        // authorization
        protected MappingConfiguration MapAuthorization_Action()
        {
            var actionMappingConfig = new MappingConfiguration<Action>();
            actionMappingConfig.MapType( p => new
            {
                Id = p.Id ,
                Code = p.Code ,
                Name = p.Name ,
                Description = p.Description ,
            } ).ToTable( "Authorization_Action" );

            actionMappingConfig.HasProperty( p => p.Name ).IsUnicode().ToColumn( "Name" );
            actionMappingConfig.HasProperty( p => p.Description ).IsUnicode().ToColumn( "Description" );
            actionMappingConfig.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return actionMappingConfig;
        }
        protected MappingConfiguration MapAuthorization_Privilege()
        {
            var privilegeMappingConfig = new MappingConfiguration<Privilege>();
            privilegeMappingConfig.MapType( p => new
            {
                Id = p.Id ,
                Code = p.Code ,
                Name = p.Name ,
                Description = p.Description ,
            } ).ToTable( "Authorization_Privilege" );

            privilegeMappingConfig.HasProperty( p => p.Name ).IsUnicode().ToColumn( "Name" );
            privilegeMappingConfig.HasProperty( p => p.Description ).IsUnicode().ToColumn( "Description" );
            privilegeMappingConfig.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return privilegeMappingConfig;
        }
        protected MappingConfiguration MapAuthorization_Role()
        {
            var roleMappingConfig = new MappingConfiguration<Role>();
            roleMappingConfig.MapType( p => new
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                SystemRole = p.SystemRole

            } ).ToTable( "Authorization_Role" );

            roleMappingConfig.HasProperty( p => p.Name ).IsUnicode().ToColumn( "Name" );
            roleMappingConfig.HasProperty( p => p.Description ).IsUnicode().ToColumn( "Description" );
            roleMappingConfig.HasProperty( p => p.Code ).IsUnicode().ToColumn( "Code" );
            roleMappingConfig.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return roleMappingConfig;
        }
        protected MappingConfiguration MapAuthorization_Actor()
        {
            var actorMappingConfig = new MappingConfiguration<Actor>();
            actorMappingConfig.MapType( p => new
            {
                Id = p.Id

            } ).ToTable( "Authorization_Actor" );

            actorMappingConfig.HasProperty( p => p.Id ).IsIdentity();

            return actorMappingConfig;
        }

        // Notification
        protected MappingConfiguration MapNotificationMessage()
        {
            var notificationMessageMappingConfiguration = new MappingConfiguration<NotificationMessage>();
            notificationMessageMappingConfiguration.MapType( p => new
            {
                Id = p.Id ,
                Code = p.Code ,
                Message = p.Message ,
                MessageCultureVariant = p.MessageCultureVariant ,
                MessageHTML = p.MessageHTML ,
                Type = p.Type ,
                ActionName = p.ActionName ,
                ActionNameCultureVariant = p.ActionNameCultureVariant ,
                ActionUrl = p.ActionUrl ,
                PersonId = p.PersonId ,
                TargetId = p.TargetId ,
                CreationTime = p.CreationTime ,
                SubscribersTokens = p.SubscribersTokens ,
                DelayTime = p.DelayTime,
                CCs = p.CCs ,
                AttachmentsSerialized = p.AttachmentsSerialized
            } ).ToTable( "NotificationMessage" );

            notificationMessageMappingConfiguration.HasProperty( p => p.MessageCultureVariant ).IsUnicode().ToColumn( "MessageCultureVariant" );
            notificationMessageMappingConfiguration.HasProperty( p => p.MessageHTML ).IsUnicode().ToColumn( "MessageHTML" );
            notificationMessageMappingConfiguration.HasProperty( p => p.ActionNameCultureVariant ).IsUnicode().ToColumn( "ActionNameCultureVariant" );
            notificationMessageMappingConfiguration.HasProperty( p => p.AttachmentsSerialized ).IsUnicode().ToColumn( "AttachmentsSerialized" );
            notificationMessageMappingConfiguration.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return notificationMessageMappingConfiguration;
        }
        protected MappingConfiguration MapLeaveApprovalNotificationMessage()
        {
            MappingConfiguration<LeaveApprovalNotification> mapping = new MappingConfiguration<LeaveApprovalNotification>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationMessage" );
            return mapping;
        }
        protected MappingConfiguration MapLateAttendanceNotificationMessage()
        {
            MappingConfiguration<LateAttendanceNotification> mapping = new MappingConfiguration<LateAttendanceNotification>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationMessage" );
            return mapping;
        }
        protected MappingConfiguration MapExcuseApprovalNotificationMessage()
        {
            MappingConfiguration<ExcuseApprovalNotification> mapping = new MappingConfiguration<ExcuseApprovalNotification>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationMessage" );
            return mapping;
        }
        protected MappingConfiguration MapScheduledReportNotificationMessage()
        {
            MappingConfiguration<ScheduledReportNotificationMessage> mapping = new MappingConfiguration<ScheduledReportNotificationMessage>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationMessage" );
            return mapping;
        }
        protected MappingConfiguration MapAttendanceManualEditNotification()
        {
            MappingConfiguration<AttendanceManualEditNotification> mapping = new MappingConfiguration<AttendanceManualEditNotification>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationMessage" );
            return mapping;
        }

        protected MappingConfiguration MapSMSMessage()
        {
            var notificationMessageMappingConfiguration = new MappingConfiguration<SMSMessage>();
            notificationMessageMappingConfiguration.MapType( x => new
            {
                Id = x.Id,
                CellNumber = x.CellNumber,
                Message = x.Message,
                Language = x.Language,
                CreationTime = x.CreationTime,

            } ).ToTable( "NotificationMessageSMS" );

            notificationMessageMappingConfiguration.HasProperty( p => p.Message ).IsUnicode().ToColumn( "Message" );
            notificationMessageMappingConfiguration.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return notificationMessageMappingConfiguration;
        }
        protected MappingConfiguration MapEmailMessage()
        {
            var mappings = new MappingConfiguration<EmailMessage>();
            mappings.MapType( x => new
            {
                Id = x.Id,
                Code = x.Code,
                EmailTo = x.To,
                CCs = x.CCs,
                Subject = x.Subject,
                Body = x.Body,
                AttachmentsSerialized = x.AttachmentsSerialized

            } ).ToTable( "NotificationMessageEmail" );

            mappings.HasProperty( p => p.Body ).IsUnicode().ToColumn( "Body" );
            mappings.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return mappings;
        }

        protected MappingConfiguration MapNotificationDetailedMessage()
        {
            var detailedMessageMappingConfiguration = new MappingConfiguration<NotificationDetailedMessage>();
            detailedMessageMappingConfiguration.MapType( p => new
            {
                Id = p.Id ,
                Code = p.Code ,
                Message = p.Message ,
                MessageCultureVariant = p.MessageCultureVariant ,
                Type = p.Type ,
                ActionName = p.ActionName ,
                ActionNameCultureVariant = p.ActionNameCultureVariant ,
                ActionUrl = p.ActionUrl ,
                PersonId = p.PersonId ,
                TargetId = p.TargetId ,
                CreationTime = p.CreationTime ,
                Status = p.Status ,
                IsRead = p.IsRead ,
                Metadata = p.Metadata
            } ).ToTable( "NotificationDetailedMessage" );

            detailedMessageMappingConfiguration.HasProperty( p => p.Message ).IsUnicode().ToColumn( "Message" );
            detailedMessageMappingConfiguration.HasProperty( p => p.MessageCultureVariant ).IsUnicode().ToColumn( "MessageCultureVariant" );
            detailedMessageMappingConfiguration.HasProperty( p => p.ActionNameCultureVariant ).IsUnicode().ToColumn( "ActionNameCultureVariant" );
            detailedMessageMappingConfiguration.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return detailedMessageMappingConfiguration;
        }
        protected MappingConfiguration MapLeaveApprovalNotificationDetailedMessage()
        {
            MappingConfiguration<LeaveApprovalNotificationDetailedMessage> mapping = new MappingConfiguration<LeaveApprovalNotificationDetailedMessage>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationDetailedMessage" );
            return mapping;
        }
        protected MappingConfiguration MapExcuseApprovalNotificationDetailedMessage()
        {
            MappingConfiguration<ExcuseApprovalNotificationDetailedMessage> mapping = new MappingConfiguration<ExcuseApprovalNotificationDetailedMessage>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationDetailedMessage" );
            return mapping;
        }
        protected MappingConfiguration MapLateAttendanceNotificationDetailedMessage()
        {
            MappingConfiguration<LateAttendanceNotificationDetailedMessage> mapping = new MappingConfiguration<LateAttendanceNotificationDetailedMessage>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationDetailedMessage" );
            return mapping;
        }
        protected MappingConfiguration MapScheduledReportNotificationDetailedMessage()
        {
            MappingConfiguration<ScheduledReportNotificationDetailedMessage> mapping = new MappingConfiguration<ScheduledReportNotificationDetailedMessage>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationDetailedMessage" );
            return mapping;
        }
        protected MappingConfiguration MapAttendanceManualEditNotificationDetailedMessage()
        {
            MappingConfiguration<AttendanceManualEditNotificationDetailedMessage> mapping = new MappingConfiguration<AttendanceManualEditNotificationDetailedMessage>();
            mapping.MapType().Inheritance( InheritanceStrategy.Flat ).ToTable( "NotificationDetailedMessage" );
            return mapping;
        }

        // MQ
        protected MappingConfiguration MapMessageQueuePool()
        {
            var messageQueuePoolMappingConfiguration = new MappingConfiguration<MQMessage>();
            messageQueuePoolMappingConfiguration.MapType( p => new
            {
                Id = p.Id ,
                Code = p.Code ,
                Type = p.Type ,
                Status = p.Status ,
                Priority = p.Priority ,
                Complexity = p.Complexity ,
                CreationTime = p.CreationTime ,
                ContentType = p.ContentType ,
                ContentSerialized = p.ContentSerialized,
                TargetCode = p.TargetCode ,
                TargetType = p.TargetType ,

            } ).ToTable( "MessageQueuePool" );

            messageQueuePoolMappingConfiguration.HasProperty( p => p.Code ).IsUnicode().ToColumn( "Code" );
            messageQueuePoolMappingConfiguration.HasProperty( p => p.Id ).IsIdentity( KeyGenerator.Guid );

            return messageQueuePoolMappingConfiguration;
        }

        #endregion
        #region Helpers

        protected void ManyManyAssociation( MappingConfiguration first , MappingConfiguration second , string firstListProperty , string secondListProperty , string joinTableName , string firstForeignKey , string secondForeignKey )
        {
            first.HasArtificialCollectionAssociation( firstListProperty , second.ConfiguredType ).IsManaged( true ).WithOppositeCollection( secondListProperty ).MapJoinTable( joinTableName , firstForeignKey , secondForeignKey );
            second.HasArtificialCollectionAssociation( secondListProperty , first.ConfiguredType ).WithOppositeCollection( firstListProperty ).MapJoinTable( joinTableName , secondForeignKey , firstForeignKey );
        }
        protected void ManyManyAssociationOneSided( MappingConfiguration first , MappingConfiguration second , string firstListProperty , string joinTableName , string firstForeignKey , string secondForeignKey )
        {
            first.HasArtificialCollectionAssociation( firstListProperty , second.ConfiguredType ).IsManaged( true ).MapJoinTable( joinTableName , firstForeignKey , secondForeignKey );
        }

        #endregion
    }
}