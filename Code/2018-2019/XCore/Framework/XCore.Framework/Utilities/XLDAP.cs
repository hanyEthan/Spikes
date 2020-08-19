using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Framework.Utilities
{
    public static class XLDAP
    {
        public static bool Create( string domain , string username , string password , string adminUsername = null , string adminPassword = null )
        {
            try
            {
                using ( var context = string.IsNullOrWhiteSpace( adminUsername )
                                    ? new PrincipalContext( ContextType.Domain , domain )
                                    : new PrincipalContext( ContextType.Domain , domain , adminUsername , adminPassword ) )
                {
                    // check existing ...
                    var user = UserPrincipal.FindByIdentity( context , username );
                    if ( user != null ) return false;

                    // create new ...
                    using ( var principal = new UserPrincipal( context ) )
                    {
                        principal.SamAccountName = username;
                        principal.SetPassword( password );
                        principal.Enabled = true;
                        principal.PasswordNeverExpires = true;
                        principal.Save();
                    }
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public static bool Update( string domain , string username , string password , string passwordNew , string adminUsername = null , string adminPassword = null )
        {
            try
            {
                using ( var context = string.IsNullOrWhiteSpace( adminUsername )
                                    ? new PrincipalContext( ContextType.Domain , domain )
                                    : new PrincipalContext( ContextType.Domain , domain , adminUsername , adminPassword ) )
                {
                    // check existing ...
                    var user = UserPrincipal.FindByIdentity( context , username );
                    if ( user == null ) return false;

                    if ( !Check( domain , username , password ) ) return false;

                    //user.ChangePassword( password , passwordNew );
                    user.SetPassword( passwordNew );
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public static bool ResetPassword( string domain , string username , string passwordNew , string adminUsername = null , string adminPassword = null )
        {
            try
            {
                using ( var context = string.IsNullOrWhiteSpace( adminUsername )
                                    ? new PrincipalContext( ContextType.Domain , domain )
                                    : new PrincipalContext( ContextType.Domain , domain , adminUsername , adminPassword ) )
                {
                    // check existing ...
                    var user = UserPrincipal.FindByIdentity( context , username );
                    if ( user == null ) return false;

                    user.SetPassword( passwordNew );
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public static bool Check( string domain , string username , string password )
        {
            try
            {
                using ( var context = new PrincipalContext( ContextType.Domain , domain ) )
                {
                    return context.ValidateCredentials( username , password );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public static bool CheckByUserName( string domain , string username , string adminUsername = null , string adminPassword = null )
        {
            try
            {
                using ( var context = string.IsNullOrWhiteSpace( adminUsername )
                                    ? new PrincipalContext( ContextType.Domain , domain )
                                    : new PrincipalContext( ContextType.Domain , domain , adminUsername , adminPassword ) )
                {
                    // check existing ...
                    var user = UserPrincipal.FindByIdentity( context , username );
                    if ( user == null ) return false;
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public static bool XYZ( string domain , string username , string adminUsername = null , string adminPassword = null )
        {
            try
            {
                using ( var context = string.IsNullOrWhiteSpace( adminUsername )
                                    ? new PrincipalContext( ContextType.Domain , domain )
                                    : new PrincipalContext( ContextType.Domain , domain , adminUsername , adminPassword ) )
                {
                    // check existing ...
                    var user = UserPrincipal.FindByIdentity( context , username );
                    if ( user == null ) return false;



                    user.PasswordNeverExpires = true;
                    user.Save();
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
    }
}
