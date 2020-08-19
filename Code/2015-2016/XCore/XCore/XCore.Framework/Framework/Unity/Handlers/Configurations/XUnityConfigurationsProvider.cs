using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Framework.Unity.Models;
using XCore.Utilities.Logger;
using XCore.Utilities.Utilities;

namespace XCore.Framework.Framework.Unity.Handlers.Configurations
{
    public class XUnityConfigurationsProvider : IUnityConfigurationsProvider
    {
        #region props.

        public virtual bool Initialized { get; protected set; }
        public virtual List<string> InitializaionMessages { get; protected set; }

        #endregion
        #region cst.

        public XUnityConfigurationsProvider()
        {
            this.Initialized = true;
        }

        #endregion
        #region IUnityConfigurationsProvider

        public UnityConfig Get()
        {
            try
            {
                var config = GetConfigRaw();
                return XSerialize.JSON.Deserialize<UnityConfig>( config );
            }
            catch ( Exception x )
            {
                MemLog( "Error : couldn't load Unity config : " + x );
                throw;
            }
        }

        #endregion
        #region helpers.

        private string GetConfigRaw()
        {
            try
            {
                var query = @"SELECT C.Value FROM CfgConfiguration C WHERE C.[key] = '{0}'";

                using ( var conn = new SqlConnection( XDB.GetConnectionString( "XCore.Unity" ) ) )
                {
                    using ( var comm = conn.CreateCommand() )
                    {
                        comm.CommandText = string.Format( query , "XCore.Unity.Config" );

                        conn.Open();
                        var reader = comm.ExecuteReader();
                        while ( reader.Read() )
                        {
                            return XDB.ReadDBString( reader["Value"] );
                        }

                        //conn.Close();
                    }
                }

                return null;
            }
            catch ( Exception x )
            {
                XLogger.Error("Exception : " + x);
                MemLog( "Error : couldn't load Unity config : " + x );

                return null;
            }
        }
        private void MemLog( string message )
        {
            // ...
            InitializaionMessages = InitializaionMessages ?? new List<string>();

            // ...
            InitializaionMessages.Add( string.Format( "XUnity : [{0}] : {1}" , DateTime.Now.ToString() , message ) );
        }

        #endregion
    }
}
