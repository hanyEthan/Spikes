using ADS.Common.Context;
using ADS.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Reports;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Models;
using ADS.Tamam.Clients.Web.Common.Reports.Contracts;
using Microsoft.Reporting.WebForms;
using System.IO;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Models.Enums;
using ADS.Common.Handlers;
using System.Web;
using ADS.Common.Handlers.Communication;
using ADS.Common;
using ADS.Tamam.Clients.Web.Common.Reports.Parameters;

namespace ADS.Tamam.Services.Reports
{
    // TODO :
    // Handle previously missed reports (due to service inactivity), only per Day.

    // Done : 
    // reports excution needs to be excuted sequentially.
    // GetScheduledReports : use a time period filtering mech. instead of a proximity time.
    // notify without person id


    public partial class ReportsService : ServiceBase
    {
        #region Fields

        private ScheduledReportsWorkers Workers;

        #endregion

        #region Cst..

        public ReportsService()
        {
//#if DEBUG
//            Debugger.Launch();
//#endif
            Initialize();

            Workers = new ScheduledReportsWorkers();
        }

        #endregion

        #region Events

        protected override void OnStart( string[] args )
        {
            XLogger.Settings.Sensitivity = XLogger.Enums.LogStatus.Info;
            XLogger.Info( "Reports Service started" );

            AppDomain.CurrentDomain.UnhandledException += currentDomain_UnhandledException;

            Workers.Start();
        }
        protected override void OnStop() { }
        private void currentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            XLogger.Error( ( ( Exception )e.ExceptionObject ).ToString() );
        }

        #endregion

        # region Operations

        private void Initialize()
        {
            InitializeComponent();
            InitializeServiceName();
        }

        # endregion

        # region internals

        private void InitializeServiceName()
        {
            this.ServiceName = ConfigurationManager.AppSettings["ServiceName"];
        }

        # endregion
    }

    public class ScheduledReportsWorkers
    {
        public void Start()
        {
            _MainTask.Start();
        }

        private async void Process()
        {
            DateTime Xfrom = DateTime.Now;
            DateTime Xto;

            while ( true )
            {
                try
                {
                    XLogger.Info( "Main Task Started." );

                    GetTimeProximity( Xfrom, out Xfrom, out Xto );

                    // Data..
                    var reportsEvents = GetScheduledReports( Xfrom, Xfrom, Xto );
                    foreach ( var eventObj in reportsEvents ) HandleReportEvent( eventObj );

                    Xfrom = Xto;
                    if ( Xto >= DateTime.Now )
                    {
                        var interval = Xto - DateTime.Now;
                        XLogger.Info( "Main Task: Delay for: [{0}]", interval.ToString() );
                        XLogger.Info( "Planned next run at : [{0}]", ( DateTime.Now + interval ).ToString() );
                        await Task.Delay( interval );
                    }
                }
                catch ( Exception ex )
                {
                    XLogger.Error( "", ex );
                }
            }
        }

        #region props ...

        private Task _MainTask = null;
        private static string ReportsLocation;

        #endregion
        #region cst ...

        public ScheduledReportsWorkers()
        {
            if ( !TamamServiceBroker.Status.Initialized || !InitializeConfigurations() ) return;

            _MainTask = new Task( Process );
        }

        #endregion
        # region Helpers..

        private bool InitializeConfigurations()
        {
            try
            {
                XLogger.Info( "Loading Configurations..." );

                ReportsLocation = Broker.ConfigurationHandler.GetValue( "Tamam.Reports", "Reports.PhysicalLocation" );

                if ( string.IsNullOrWhiteSpace( ReportsLocation ) || !Directory.Exists( ReportsLocation ) )
                {
                    XLogger.Error( "Configuration Error : invalid Reports Location ({0})", ReportsLocation );
                    return false;
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Configurations Error : Exception : " + x );
                return false;
            }
        }

        private List<Person> GetSupervisors( Guid? departmentId )
        {
            List<Person> supervisors = new List<Person>();
            if ( departmentId.HasValue == false || departmentId.Value == Guid.Empty ) return supervisors;

            var response = SystemBroker.OrganizationHandler.GetSupervisors( departmentId.Value, SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success )
            {
                XLogger.Error( "Failed to get supervisors for department {0}", departmentId.ToString() );
                return supervisors;
            }

            supervisors = response.Result;
            return supervisors;
        }

        private List<ScheduledReportEvent> GetScheduledReports( DateTime date, DateTime timeFrom, DateTime timeTo )
        {
            try
            {
                //var x = new DateTime( 2015 , 9 , 30 , 0 , 0 , 0 );

                var criteria = new ScheduledReportEventsSearchCriteria() { Date = date.Date, TimeFrom = timeFrom, TimeTo = timeTo };
                var response = TamamServiceBroker.ReportingHandler.GetScheduledReportEvents( criteria, SystemRequestContext.Instance );
                var success = response.Type == ResponseState.Success;
                if ( success ) return response.Result;

                return new List<ScheduledReportEvent>();
            }
            catch ( Exception ex )
            {
                XLogger.Error( "", ex );
                return new List<ScheduledReportEvent>();
            }

        }

        //private void SendReport( ReportDefinition report , Guid personId , Department department )
        //{
        //    try
        //    {
        //        XLogger.Info( "start sending report {0} for PersonId {1} " ,
        //            report.Name ,
        //            personId.ToString() );

        //        // criteria
        //        BaseReportCriteria criteria = GetNewCriteria( department , report );

        //        // English File
        //        var dataGenerator = GetNewDataGenerator( report.ReportDataFullName , criteria , personId , "en" );
        //        var enFile = GenerateReportFile( report.ReportPath , dataGenerator.MainData , dataGenerator.HeaderData , dataGenerator.Parameters );

        //        // Arabic File
        //        dataGenerator = GetNewDataGenerator( report.ReportDataFullName , criteria , personId , "ar" );
        //        var arFile = GenerateReportFile( report.ReportPath , dataGenerator.MainData , dataGenerator.HeaderData , dataGenerator.Parameters );

        //        Notify( personId , report.Name , enFile , "" , "" );
        //        Notify( personId , report.NameCultureVariant , arFile , "" , "" );

        //        XLogger.Info( "sending report {0} completed for PersonId {1}" ,
        //            report.Name ,
        //            personId.ToString() );

        //    }
        //    catch ( Exception ex )
        //    {
        //        XLogger.Error( "" , ex );
        //    }
        //}

        private void SendReport( ScheduledReportEvent reportEvent, Guid personId, string mails, string ccs )
        {
            //ReportDefinition report , Department department, Repeates repeates

            try
            {
                byte[] enFile = null;
                byte[] arFile = null;

                #region LOG

                XLogger.Info( "sending report {0} completed to PersonId {1} emails {2} CCs {3}", reportEvent.ReportDefinition.Name, personId.ToString(), mails, ccs );

                #endregion

                // criteria
                BaseReportCriteria criteria = GetNewCriteria( reportEvent );

                // English File
                var dataGenerator = GetNewDataGenerator( reportEvent.ReportDefinition.ReportDataFullName, criteria, personId, "en" );
                if ( dataGenerator == null )
                {
                    XLogger.Error( " [{0}] Cannot be instantiated please review report definition id [{1}]", reportEvent.ReportDefinition.ReportDataFullName, reportEvent.ReportDefinition.Id.ToString() );
                }
                else
                {
                    enFile = GenerateReportFile( reportEvent.ReportDefinition.ReportPath, dataGenerator.MainData, dataGenerator.HeaderData, dataGenerator.Parameters );
                }

                // Arabic File
                dataGenerator = GetNewDataGenerator( reportEvent.ReportDefinition.ReportDataFullName, criteria, personId, "ar" );
                if ( dataGenerator == null )
                {
                    XLogger.Error( " [{0}] Cannot be instantiated please review report definition id [{1}]", reportEvent.ReportDefinition.ReportDataFullName, reportEvent.ReportDefinition.Id.ToString() );
                }
                else
                {
                    arFile = GenerateReportFile( reportEvent.ReportDefinition.ReportPathCultureVariant, dataGenerator.MainData, dataGenerator.HeaderData, dataGenerator.Parameters );
                }

                var fileName = reportEvent.ReportDefinition.Name.Replace( ' ', '_' ) + "-" + DateTime.Now.ToString( "dd-MM-yyyy" ) + ".pdf";
                var fileNameVariant = reportEvent.ReportDefinition.Name.Replace( ' ', '_' ) + "-" + DateTime.Now.ToString( "dd-MM-yyyy" ) + "_AR.pdf";
                //var fileNameVariant = report.NameCultureVariant.Replace( ' ' , '_' ) + "-" + DateTime.Now.ToString( "dd-MM-yyyy" ) + ".pdf";
                Notify( personId, fileName, fileNameVariant, enFile, arFile, mails, ccs );

                #region LOG
                
                XLogger.Info( "sending report {0} completed to PersonId {1} emails {2} CCs {3}", reportEvent.ReportDefinition.Name, personId.ToString(), mails, ccs );

                #endregion
            }
            catch ( Exception ex )
            {
                XLogger.Error( "", ex );
            }
        }

        private static BaseReportCriteria GetNewCriteria( ScheduledReportEvent reportEvent )
        {
            if ( string.IsNullOrWhiteSpace( reportEvent.ReportDefinition.ReportServiceCriteriaFullName ) ) return null;

            var departments = new List<IBaseModel>();
            if ( reportEvent.Department != null ) departments.Add( reportEvent.Department );

            DateTime startDate, endDate;
            switch ( reportEvent.Repeates )
            {
                case Repeates.Monthly:

                startDate = DateTime.Today.AddDays( -30 );
                endDate = DateTime.Today.AddDays( -1 );
                break;

                case Repeates.Weekly:

                startDate = DateTime.Today.AddDays( -7 );
                endDate = DateTime.Today.AddDays( -1 );
                break;

                case Repeates.Daily:
                default:

                startDate = DateTime.Today.AddDays( -1 );
                endDate = DateTime.Today.AddDays( -1 );
                break;
            }

            DatesParameter P = new DatesParameter( departments, startDate, endDate );
            BaseReportCriteria C = XReflector.GetInstance<BaseReportCriteria>( reportEvent.ReportDefinition.ReportServiceCriteriaFullName, P );
            return C;
        }

        private static BaseReportData GetNewDataGenerator( string generatorTypeName, BaseReportCriteria criteria, Guid? personId, string cultureName )
        {
            if ( string.IsNullOrWhiteSpace( generatorTypeName ) ) return null;

            var requestContext =
                 ( personId == null || personId == Guid.Empty )
                 ? SystemRequestContext.Instance
                 : new RequestContext() { PersonId = personId };

            requestContext.CultureName = cultureName;
            var dataGenerator = XReflector.GetInstance<BaseReportData>( generatorTypeName, criteria, requestContext );
            return dataGenerator;
        }

        private static void Notify( Guid personId, string fileName, string fileNameCultureVariant, byte[] file, byte[] fileCultureVariant, string emails, string ccs )
        {
            var ok = false;

            var message = Broker.ConfigurationHandler.GetValue( "Tamam.Reports", "Reports.Notifications.Message" );
            var messageCultureVariant = Broker.ConfigurationHandler.GetValue( "Tamam.Reports", "Reports.Notifications.Message.Arabic" );
            var messageHTML = Broker.ConfigurationHandler.GetValue( "Tamam.Reports", "Reports.Notifications.Message.HTML" );

            if ( personId == Guid.Empty ) ok = NotifyEmail( emails, ccs, fileName, fileNameCultureVariant, file, fileCultureVariant, message, messageCultureVariant, messageHTML );
            else ok = NotifyPerson( personId, fileName, fileNameCultureVariant, file, fileCultureVariant, message, messageCultureVariant, messageHTML );

            if ( !ok ) XLogger.Error( "Failed to Notify [{0}] with report file {1}", personId.ToString(), fileName );
        }

        private static bool NotifyPerson( Guid personId, string fileName, string fileNameCultureVariant, byte[] file, byte[] fileCultureVariant, string message, string messageCultureVariant, string messageHTML )
        {
            Guid id = Guid.NewGuid();

            NotificationMessage notification = new ScheduledReportNotificationMessage()
            {
                Id = id,
                Code = id.ToString(),
                Message = message,
                MessageCultureVariant = messageCultureVariant,
                MessageHTML = messageHTML,
                ActionName = string.Empty,
                ActionNameCultureVariant = string.Empty,
                ActionUrl = string.Empty,
                Type = NotificationType.Information,
                PersonId = personId.ToString(),
                TargetId = string.Empty,
                CreationTime = DateTime.Now
            };

            var attachements = new List<NotificationAttachment>();
            if ( file != null )
            {
                attachements.Add( new NotificationAttachment()
                    {
                        Name = fileName,
                        Report = file,
                        MIMEType = "application/pdf"
                    } );
            }
            if ( fileCultureVariant != null )
            {
                attachements.Add( new NotificationAttachment()
                {
                    Name = fileNameCultureVariant,
                    Report = fileCultureVariant,
                    MIMEType = "application/pdf"
                } );
            }

            notification.Attachments = attachements;
            return Broker.NotificationHandler.Notify( notification );
        }

        private static bool NotifyEmail( string emails, string ccs, string fileName, string fileNameCultureVariant, byte[] file, byte[] fileCultureVariant, string message, string messageCultureVariant, string messageHTML )
        {
            EmailHandler emailHandler = new EmailHandler();
            var tos = string.IsNullOrWhiteSpace( emails ) ? new string[0] : emails.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
            var toCcs = string.IsNullOrWhiteSpace( ccs ) ? new string[0] : ccs.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );

            string body = "";
            if ( string.IsNullOrWhiteSpace( messageHTML ) )
            {
                var msgTemplate = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection, Constants.EmailNotificationMessageTemplate );
                body = string.Format( msgTemplate, "", message, "", "", messageCultureVariant, "" );
            }
            else
            {
                body = messageHTML;
            }

            var files = new List<XMailAttachment>();
            if ( file != null ) files.Add( new XMailAttachment( fileName, new MemoryStream( file ) ) );
            if ( fileCultureVariant != null ) files.Add( new XMailAttachment( fileNameCultureVariant, new MemoryStream( fileCultureVariant ) ) );
            return emailHandler.Send( tos, toCcs, body, files );
        }

        private static byte[] GenerateReportFile( string reportPath, object data, object organizationDetails, Dictionary<string, string> parameters )
        {
            using ( var report = new LocalReport() )
            {
                #region report

                report.DataSources.Clear();

                report.ReportPath = reportPath.Replace( @"~/Reports", ReportsLocation.TrimEnd( '/' ).TrimEnd( '\\' ) );
                var array = ( parameters == null ) ? new ReportParameter[0] : parameters.Select( x => new ReportParameter( x.Key, x.Value ) ).ToArray();

                report.SetParameters( array );
                report.DataSources.Add( new ReportDataSource( "OrganizationDetails", organizationDetails ) );
                report.DataSources.Add( new ReportDataSource( "Data", data ) );

                #endregion
                #region Exporting

                Warning[] warnings;
                string[] streamids;
                string mimeType, encoding, filenameExtension;

                byte[] PDFBytes = report.Render( "PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings );
                return PDFBytes;

                #endregion
            }
        }

        private string ReportEventToString( ScheduledReportEvent reportEvent )
        {
            return string.Format( "Id [{0}],  Report [{1}],  PersonnelCount[{2}],  ExpectedTime[{3}], Department [{4}], IncludeSupervisor[{5}]",
                reportEvent.Id,
                reportEvent.ReportDefinition.Name,
                reportEvent.Personnel.Count,
                reportEvent.Time,
                reportEvent.Department == null ? "" : reportEvent.Department.Name,
                reportEvent.IncludeSupervisor );
        }

        private void HandleReportEvent( ScheduledReportEvent reportEvent )
        {
            try
            {
                XLogger.Info( "start creating Tasks for Report Event {0}", ReportEventToString( reportEvent ) );

                var personnel = reportEvent.Personnel;
                var emailTo = reportEvent.Email;
                var ccs = reportEvent.CCs;
                var sendSuperMail = !string.IsNullOrWhiteSpace( emailTo ) || !string.IsNullOrWhiteSpace( emailTo );

                var sendSupervisorsMail = reportEvent.IncludeSupervisor;
                var supervisors = sendSupervisorsMail ? GetSupervisors( reportEvent.DepartmentId ) : new List<Person>();

                #region Log event details

                XLogger.Info( "EventDetails Id [{0}] PersonnelCount [{1}], Names [{2}]", reportEvent.Id.ToString(), personnel.Count.ToString(), ( personnel.Count == 0 ) ? "" : string.Join( ",", personnel.Select( p => p.FullName ) ) );
                XLogger.Info( "EventDetails Id [{0}] emailTo [{1}], CCs [{2}]", reportEvent.Id.ToString(), emailTo, ccs );
                XLogger.Info( "EventDetails Id [{0}] Send Supervisors Mail [{1}], Supervisors [{2}]", reportEvent.Id.ToString(), sendSupervisorsMail.ToString(), ( supervisors.Count == 0 ) ? "" : string.Join( ",", supervisors.Select( p => p.FullName ) ) );

                #endregion

                // Super Mail
                if ( sendSuperMail ) SendReport( reportEvent, Guid.Empty, reportEvent.Email, reportEvent.CCs );

                // personnel & supervisors Mails (make a distinct list of personnel ids)..
                var personnelIds = new List<Guid>();
                personnelIds.AddRange( reportEvent.Personnel.Select( x => x.Id ) );
                personnelIds.AddRange( supervisors.Select( x => x.Id ) );
                personnelIds = personnelIds.Distinct().ToList();

                foreach ( var personId in personnelIds ) SendReport( reportEvent, personId, "", "" );
            }
            catch ( Exception ex )
            {
                XLogger.Error( "", ex );
            }
        }

        public static TimeSpan CalculateInterval( DateTime now )
        {
            // DateTime now = DateTime.Now;
            var smallerHalfHour = new DateTime( now.Year, now.Month, now.Day, now.Hour, now.Minute < 30 ? 0 : 31, 0 );
            var interval = smallerHalfHour.AddMinutes( 30 ) - now;
            return interval;
        }

        public static void GetTimeProximity( DateTime dateTime, out DateTime from, out DateTime to )
        {
            var isFirstHalf = dateTime.Minute < 30;

            from = new DateTime( dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, isFirstHalf ? 1 : 31, 0 );
            // to = new DateTime( dateTime.Year , dateTime.Month , dateTime.Day , dateTime.Minute >= 30 ? Math.Min(dateTime.Hour + 1 , 12) : dateTime.Hour , dateTime.Minute >= 30 ? 0 : 30 , 0 );
            to = from.AddMinutes( 29 );//isFirstHalf ? 30 : 31);
        }

        //public static DateTime RoundToSmallerHalfHour( DateTime dateTime )
        //{
        //    var nowMinute = dateTime.Minute >= 30 ? 30 : 0;
        //    var nowsec = 0;

        //    dateTime = new DateTime( dateTime.Year , dateTime.Month , dateTime.Day , dateTime.Hour , nowMinute , nowsec );
        //    return dateTime;
        //}
        //public static DateTime RoundToBiggerHalfHour( DateTime dateTime )
        //{
        //    int nowMinute;
        //    int nowHour;
        //    var nowsec = 0;

        //    if ( dateTime.Minute >= 30 )
        //    {
        //        nowHour = dateTime.AddHours( 1 ).Hour;
        //        nowMinute = 0;
        //    }
        //    else
        //    {
        //        nowHour = dateTime.Hour;
        //        nowMinute = 30;
        //    }

        //    dateTime = new DateTime( dateTime.Year , dateTime.Month , dateTime.Day , Hour , nowMinute , nowsec );
        //    return dateTime;
        //}

        # endregion
    }
}