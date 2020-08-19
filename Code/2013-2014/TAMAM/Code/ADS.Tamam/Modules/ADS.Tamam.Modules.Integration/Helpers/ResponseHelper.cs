using System.Text;

using ADS.Common.Context;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;

namespace ADS.Tamam.Modules.Integration.Helpers
{
    public class ResponseHelper
    {
        public static void HandleResponse<T>(ExecutionResponse<T> response, ILoggable loggable, string callerActionKey) where T : new()
        {
            var message = BuildMessage<T>(response, loggable, callerActionKey);
            if ( response != null && response.Type == ResponseState.Success )
            {
                XLogger.Info(message);
            }
            else
            {
                XLogger.Error(message);
            }
        }

        private static string BuildMessage<T>(ExecutionResponse<T> response, ILoggable loggable, string callerActionKey) where T : new()
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("-------------------------------------------------------------");
            messageBuilder.AppendLine("[" + loggable.Reference + "]");
            messageBuilder.AppendLine("ActionKey " + callerActionKey);

            messageBuilder.AppendLine( "Response.Type " + response != null ? response.Type.ToString() : "Failed (will try next iteration)" );
            messageBuilder.AppendLine("Integration data " + loggable.GetLoggingData());

            if (response != null && response.Exception != null)
            {
                messageBuilder.AppendLine(string.Format("Exception {0}", response.Exception.ToString()));
            }

            if (response != null && response.MessageDetailed != null && response.MessageDetailed.Count > 0)
            {
                messageBuilder.AppendLine("Errors ");
                for (int i = 0; i < response.MessageDetailed.Count; i++)
                {
                    var err = response.MessageDetailed[i];
                    messageBuilder.AppendLine(string.Format("{0} - [{1}] - [{2}]", i, err.PropertyName, err.Meta));
                }
            }

            messageBuilder.AppendLine("-------------------------------------------------------------");  
            return messageBuilder.ToString();
        }

        #region DetailCodes

        public static void HandleResponse(DetailCode response, ILoggable loggable, string callerActionKey)
        {
            // TODO: George: 
            // Notify
            var message = BuildMessage(response, loggable, callerActionKey);
            if ( response != null && response.Id > 0 )
            {
                XLogger.Info(message);
            }
            else
            {
                XLogger.Error(message);
            }
        }
        private static string BuildMessage(DetailCode response, ILoggable loggable, string callerActionKey)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("-------------------------------------------------------------");
            messageBuilder.AppendLine("[" + loggable.Reference + "]");
            messageBuilder.AppendLine("ActionKey " + callerActionKey);

            messageBuilder.AppendLine( "Response.Type " + ( response != null && response.Id > 0 ? "Success" : "Failure" ) );
            messageBuilder.AppendLine("Integration data " + loggable.GetLoggingData());

            messageBuilder.AppendLine("-------------------------------------------------------------");
            return messageBuilder.ToString();
        }

        public static void HandleResponse( DetailCode response, ILoggable loggable, string callerActionKey, string validationError )
        {
            // TODO: George: 
            // Notify
            var message = BuildMessage( response, loggable, callerActionKey, validationError );
            if ( response != null && response.Id > 0 )
            {
                XLogger.Info( message );
            }
            else
            {
                XLogger.Error( message );
            }
        }
        private static string BuildMessage( DetailCode response, ILoggable loggable, string callerActionKey, string validationError )
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine( "-------------------------------------------------------------" );
            messageBuilder.AppendLine( "[" + loggable.Reference + "]" );
            messageBuilder.AppendLine( "ActionKey " + callerActionKey );

            messageBuilder.AppendLine( "Response.Type " + ( response != null && response.Id > 0 ? "Success" : "Failure" ) );
            messageBuilder.AppendLine( "Integration data " + loggable.GetLoggingData() );
            if ( !string.IsNullOrWhiteSpace( validationError ) ) messageBuilder.AppendLine( validationError );
            messageBuilder.AppendLine( "-------------------------------------------------------------" );
            return messageBuilder.ToString();
        }

        #endregion

    }
}
