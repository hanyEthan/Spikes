using System;
using System.Linq;
using System.Net;

namespace XCore.Utilities.Utilities
{
    public static class XNet
    {
        /// <summary>
        /// Get Current Machine Name
        /// </summary>
        public static string GetMachineName()
        {
            //Return Environment.MachineName;
            //To unify the machine name between web and desktop application
            var ipAdress = GetIP();
            var machineName = string.Empty;
            try
            {
                System.Net.IPHostEntry hostEntry = Dns.GetHostEntry( ipAdress );

                machineName = hostEntry.HostName;
            }
            catch ( Exception x )
            {
                //XLogger.Error( "Exception : " + x );
                //return "";
                throw;
            }
            return machineName;
        }

        /// <summary>
        /// Get Internet Protocol Number (IP)
        /// </summary>
        public static string GetIP()
        {
            try
            {
                string strHostName = Dns.GetHostName();

                var ipEntry = Dns.GetHostEntry( strHostName );
                if ( ipEntry != null )
                {
                    var ipAddressList = ipEntry.AddressList;
                    if ( ipAddressList.Any() )
                    {
                        foreach ( var iPAddress in ipAddressList )
                        {
                            if ( iPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
                            {
                                return iPAddress.ToString();
                            }
                        }
                    }
                }
                return "";
            }
            catch ( Exception x )
            {
                //XLogger.Error( "Exception : " + x );
                //return "";
                throw;
            }

        }
    }
}
