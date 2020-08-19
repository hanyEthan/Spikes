using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace XCore.Framework.Utilities
{
    public static class XWeb
    {
        #region statics.

        public static RequestContext GetHTTPRequestEnvironment(HttpRequest httpRequest)
        {
            try
            {
                #region prep.

                GetPlatform(httpRequest, out string sourceOS, out string sourceBrowser);

                #endregion

                var audit = new RequestContext()
                {
                    SourceIP = GetSourceIP(httpRequest),
                    SourcePort = null,

                    DestinationIP = GetDestinationIP(httpRequest),
                    DestinationPort = GetDestinationPort(httpRequest),

                    WebServerAddress = null,
                    ConnectionMethod = null,

                    BrowserName = sourceBrowser,
                    OperatingSystem = sourceOS,
                };

                return audit;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return null;
            }
        }

        #endregion
        #region helpers.

        private static string GetSourceIP(HttpRequest httpRequest, bool tryUseXForwardHeader = true)
        {
            try
            {
                #region note.

                // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

                // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
                // for 99% of cases however it has been suggested that a better (although tedious)
                // approach might be to read each IP from right to left and use the first public IP.
                // http://stackoverflow.com/a/43554000/538763

                #endregion

                string ip = null;

                ip = !string.IsNullOrWhiteSpace(ip) || !tryUseXForwardHeader ? ip : GetHeaderValue(httpRequest, "X-Forwarded-For").SplitCSV().FirstOrDefault();
                ip = !string.IsNullOrWhiteSpace(ip) || httpRequest?.HttpContext?.Connection?.RemoteIpAddress == null ? ip : httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
                ip = !string.IsNullOrWhiteSpace(ip) ? ip : GetHeaderValue(httpRequest, "REMOTE_ADDR").SplitCSV().FirstOrDefault();

                return ip;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static string GetDestinationIP(HttpRequest httpRequest)
        {
            string ip = null;

            ip = !string.IsNullOrWhiteSpace(ip) ? ip : GetHeaderValue(httpRequest, "LOCAL_ADDR").SplitCSV().FirstOrDefault();

            return ip;
        }
        private static string GetDestinationPort(HttpRequest httpRequest)
        {
            string port = null;

            port = !string.IsNullOrWhiteSpace(port) ? port : GetHeaderValue(httpRequest, "SERVER_PORT").SplitCSV().FirstOrDefault();

            return port;
        }
        private static void GetPlatform(HttpRequest httpRequest, out string OS, out string browser)
        {
            OS = null;
            browser = null;

            try
            {
                var userAgent = GetHeaderValue(httpRequest, "User-Agent");
                if (string.IsNullOrWhiteSpace(userAgent)) return;

                var uaParser = UAParser.Parser.GetDefault();
                var clientInfo = uaParser.Parse(userAgent);

                OS = $"{clientInfo.OS.Family}.{clientInfo.OS.Major}";
                browser = $"{clientInfo.UA.Family}.{clientInfo.UA.Major}.{clientInfo.UA.Minor}";
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
            }
        }

        private static string GetHeaderValue(HttpRequest httpRequest, string headerName)
        {
            return httpRequest?.Headers?.TryGetValue(headerName, out StringValues values) ?? false
                 ? values.ToString()
                 : null;
        }

        #endregion

        #region nested.

        public class RequestContext
        {
            #region props.

            public string SourceIP { get; set; }
            public string SourcePort { get; set; }

            public string DestinationIP { get; set; }
            public string DestinationPort { get; set; }

            public string WebServerAddress { get; set; }
            public string ConnectionMethod { get; set; }

            public string BrowserName { get; set; }
            public string OperatingSystem { get; set; }

            #endregion
        }

        #endregion
    }
}
