using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

namespace ADS.Tamam.Common.Data.ORM
{
    public class ADSBaseContext : OpenAccessContext
    {
        public ADSBaseContext( string connectionString , BackendConfiguration backendConfiguration , MetadataContainer metadataContainer ) : base( connectionString , backendConfiguration , metadataContainer ) { }
        public ADSBaseContext( string connectionString , BackendConfiguration backendConfiguration , MetadataSource metadataSource ) : base( connectionString , backendConfiguration , metadataSource ) { }

        /// <summary>
        /// Mg,5-2-2015.Intentionally hide base class 'ExecuteQuery' as it doesn't work correctly with oracle.
        /// Please use this overload to execute any Stored Procedures
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public new IList<T> ExecuteQuery<T>( string procedureName , params DbParameter[] parameters )
        {
            var dbType = this.Metadata.BackendType;
            if ( dbType == Telerik.OpenAccess.Metadata.Backend.MsSql )
            {
                return this.ExecuteQuery<T>( procedureName , System.Data.CommandType.StoredProcedure , parameters );
            }
            else if ( dbType == Telerik.OpenAccess.Metadata.Backend.Oracle )
            {
                return ExecuteQueryOracle<T>( procedureName , parameters );
            }
            else
                throw new Exception( "Unsupported DB Type" );
        }
        private IList<T> ExecuteQueryOracle<T>( string procedureName , params DbParameter[] parameters )
        {
                OracleParameter cursorParameter = new OracleParameter();
                cursorParameter.ParameterName = "cv_1";
                cursorParameter.Direction = ParameterDirection.Output;
                cursorParameter.OracleDbType = OracleDbType.RefCursor;

                foreach ( var paramter in parameters )
                {
                    if ( paramter.DbType == DbType.Guid )
                    {
                        paramter.DbType = DbType.String;
                    }
                }
                var newParams = parameters.ToList();
                newParams.Add( cursorParameter );
                return this.ExecuteQuery<T>( procedureName , System.Data.CommandType.StoredProcedure , newParams.ToArray() );
        }

        public new T ExecuteScalar<T>( string procedureName , params DbParameter[] parameters )
        {
            var dbType = this.Metadata.BackendType;
            if ( dbType == Telerik.OpenAccess.Metadata.Backend.MsSql )
            {
                return this.ExecuteScalar<T>( procedureName , parameters );
            }
            else if ( dbType == Telerik.OpenAccess.Metadata.Backend.Oracle )
            {
                return ExecuteScalarOracle<T>( procedureName , parameters );
            }
            else
                throw new Exception( "Unsupported DB Type" );
        }
        private T ExecuteScalarOracle<T>( string procedureName , DbParameter[] parameters )
        {
            OracleParameter cursorParameter = new OracleParameter();
            cursorParameter.ParameterName = "cv_1";
            cursorParameter.Direction = ParameterDirection.Output;
            cursorParameter.OracleDbType = OracleDbType.RefCursor;

            foreach ( var paramter in parameters )
            {
                if ( paramter.DbType == DbType.Guid )
                {
                    paramter.DbType = DbType.String;
                }
            }
            var newParams = parameters.ToList();
            newParams.Add( cursorParameter );

            var response = this.ExecuteQuery<T>( procedureName , System.Data.CommandType.StoredProcedure , newParams.ToArray() );
            return response != null && response is IList<T> ? response[0] : default( T );
        }
    }
}
