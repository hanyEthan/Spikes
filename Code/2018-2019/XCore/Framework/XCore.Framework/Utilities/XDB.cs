using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Framework.Utilities
{
    public static class XDB
    {
        #region props.

        private static XConfig _config { get; set; }

        #endregion
        #region cst.

        static XDB()
        {
            _config = new XConfig("appsettings.json");
        }

        #endregion

        #region Connection Strings

        public static string GetConnectionString(string connectionName)
        {
            try
            {
                var connectionstring = _config.GetConnectionString(connectionName);
                if (string.IsNullOrWhiteSpace(connectionstring))
                {
                    return connectionstring;
                }
                else if (connectionstring.ToLower().Contains("catalog"))
                {
                    return connectionstring;
                }
                else
                {
                    return XCrypto.DecryptFromAES(connectionstring);
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;
            }
        }

        #endregion
        #region Stored Procedures ...

        public static int ExecuteProcedure(DbContext EFContext, RawSqlString sql, IEnumerable<SqlParameter> parameters)
        {
            //parameters = parameters.Select(x => x ?? DBNull.Value).ToList();
            parameters.ToList().ForEach(x => x.Value = x.Value ?? DBNull.Value);
            return EFContext.Database.ExecuteSqlCommand(sql, parameters ?? throw new InvalidOperationException());
        }
        public static SqlParameter GetSqlParameter( string name , object value , SqlDbType? type , ParameterDirection? direction )
        {
            var parameter = new SqlParameter();

            parameter.ParameterName = name;
            parameter.SqlDbType = type ?? parameter.SqlDbType;
            parameter.Direction = direction ?? parameter.Direction;
            parameter.Value = value ?? DBNull.Value;

            return parameter;
        }

        #endregion
        #region Schema

        public static void EnsureDatabaseExists( string connectionString )
        {
            try
            {
                var builder = new SqlConnectionStringBuilder( connectionString );
                var database = builder.InitialCatalog;

                var masterConnection = connectionString.Replace( builder.InitialCatalog , "master" );

                using ( var connection = new SqlConnection( masterConnection ) )
                {
                    connection.Open();

                    using ( var command = connection.CreateCommand() )
                    {
                        command.CommandText = $@" if(db_id('{database}') is null)
                                                 create database [{database}]";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch ( Exception x )
            {
                throw;
            }
        }
        public static void ExecuteSql( string connectionString , string sql )
        {
            try
            {
                EnsureDatabaseExists( connectionString );

                using ( var connection = new SqlConnection( connectionString ) )
                {
                    connection.Open();

                    using ( var command = connection.CreateCommand() )
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch ( Exception x )
            {
                throw;
            }
        }
        public static void CreateSchema( string connectionString , string schema )
        {
            try
            {
                var sql = $@"if not exists (select  * from    sys.schemas where   name = N'{schema}')
                                exec('create schema {schema}');";
                ExecuteSql( connectionString , sql );
            }
            catch ( Exception x )
            {
                throw;
            }
        }

        #endregion
        #region Types

        /// <summary>
        /// Returns a safe date range to be used inside the data layer programming model
        /// </summary>
        public static object ConvertDateTimeToSqlDateTime( DateTime value )
        {
            if ( value < new DateTime( 1753 , 1 , 1 ) || value > new DateTime( 3000 , 12 , 31 ) )
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Returns DBNull safe objects
        /// </summary>
        public static object ReadDBObject( object value )
        {
            return ( value == DBNull.Value ) ? null : value;
        }

        /// <summary>
        /// Returns DBNull safe casted strings
        /// </summary>
        public static string ReadDBString( object value )
        {
            return ( value == DBNull.Value ) ? null : ( string ) value;
        }

        /// <summary>
        /// Returns DBNull safe casted ints
        /// </summary>
        public static int? ReadDBInt( object value )
        {
            return value == DBNull.Value ? null : new int?( ( value is decimal? ) ? ( int ) ( ( decimal ) value ) : ( int ) value );
        }

        /// <summary>
        /// Returns DBNull safe casted guids
        /// </summary>
        public static Guid ReadDBGuid( object value )
        {
            return ( value == DBNull.Value ) ? Guid.Empty : ( Guid ) value;
        }

        /// <summary>
        /// Returns DBNull safe casted dates
        /// </summary>
        public static DateTime? ReadDBDate( object value )
        {
            return ( value == DBNull.Value ) ? null : ( DateTime? ) value;
        }

        #endregion
    }
}
