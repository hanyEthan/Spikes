using System;
using System.DirectoryServices;

namespace ADS.Common.Utilities
{
    [Serializable]
    public class XLDAP
    {
        #region Models

        private class LDAP
        {
            #region props ...

            public bool IsValid { get; set; }
            public string Domain { get; set; }
            public string Server { get; set; }
            public string Username { get; set; }
            public string DomainUser { get; set; }
            
            #endregion
            #region cst ...

            public LDAP( string domain , string username )
            {
                if ( string.IsNullOrWhiteSpace( domain ) || string.IsNullOrWhiteSpace( username ) ) return;

                domain = domain.Trim();
                username = username.Trim();

                this.Server = domain.StartsWith( "LDAP://" ) ? domain : string.Format( "LDAP://{0}" , domain );
                this.Domain = domain.StartsWith( "LDAP://" ) ? domain.Remove( 0 , 7 ) : domain;
                this.Username = username;
                this.DomainUser = string.Format( "{0}\\{1}" , domain , username );

                IsValid = true;
            }
            
            #endregion
        }

        #endregion

        public static bool Authenticate( string domain , string username , string password )
        {
            var ldap = new LDAP( domain , username );
            if ( !ldap.IsValid ) return false;

            try
            {
                using ( var entry = new DirectoryEntry( ldap.Server , ldap.DomainUser , password ) )
                {
                    Object obj = entry.NativeObject;
                    using ( var searcher = new DirectorySearcher( entry ) )
                    {
                        searcher.Filter = "(sAMAccountName=" + ldap.Username + ")";
                        searcher.PropertiesToLoad.Add( "cn" );

                        var result = searcher.FindOne();
                        return result != null;

                        //string displayName = ( String ) result.Properties["cn"][0];
                    }
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw x;
            }
        }
        public static bool Exists( string domain , string username )
        {
            var ldap = new LDAP( domain , username );
            if ( !ldap.IsValid ) return false;

            try
            {
                using ( var entry = new DirectoryEntry( ldap.Server ) )
                {
                    using ( var searcher = new DirectorySearcher( entry ) )
                    {
                        searcher.Filter = "(sAMAccountName=" + ldap.Username + ")";
                        var result = searcher.FindOne();

                        return result != null;
                    }
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw x;
            }
        }
    }
}
