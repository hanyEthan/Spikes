using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using OpenAccessRuntime.Data;
using Telerik.OpenAccess.Data;
using Telerik.OpenAccess.Data.Common;

namespace ADS.Common.Utilities
{
    /// <summary>
    /// Utility : database utilities is a small set of quick code snippets for productivity support when coding against the underlying datalayer and its datasets
    /// </summary>
    public static class XDatabase
    {
        #region data types ...

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
        #region Stored Procedures ...

        /// <summary>
        /// a Sql Parameter wrapper snippet
        /// </summary>
        public static SqlParameter GetSqlParameter( string name , SqlDbType type , ParameterDirection direction , object value )
        {
            var parameter = new SqlParameter();
            parameter.ParameterName = name;
            parameter.SqlDbType = type;
            parameter.Direction = direction;
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// a telerik open access Parameter wrapper snippet
        /// </summary>
        public static OAParameter GetSqlParameter( string name , DbType type , ParameterDirection direction , object value )
        {
            var parameter = new OAParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Direction = direction;
            parameter.Value = value;

            return parameter;
        }

        #endregion

        #region nested

        public class StringToNullGuidConverter : AdoTypeConverter
        {
            internal static readonly object BoxedEmpty = Guid.Empty;
            private bool nullable;

            public override Type DefaultType
            {
                get
                {
                    return typeof( Guid? );
                }
            }
            public override AdoTypeConverter Initialize( IDataColumn user , Type clr , IAdoTypeConverterRegistry registry , bool secondary )
            {
                if ( clr == typeof( Guid ) || clr == typeof( Guid? ) )
                {
                    this.nullable = ( clr == typeof( Guid? ) );
                    return base.Initialize( user , clr , registry , secondary );
                }

                return AdoTypeConverterUtils.Unsupported( clr , this );
            }
            public override object Read( ref DataHolder data )
            {
                if ( data.Reader is IBufferingReader )
                {
                    data.NoValue = data.Reader.IsDBNull( data.Position );
                    data.ObjectValue = data.NoValue ? null : data.Reader.GetValue( data.Position );
                    if ( data.Box || this.nullable )
                    {
                        return data.ObjectValue;
                    }

                    data.GuidValue = data.NoValue ? Guid.Empty : ( Guid ) data.ObjectValue;

                    return null;
                }

                bool noValue = data.Reader.IsDBNull( data.Position );
                data.NoValue = noValue;
                if ( !noValue )
                {
                    Guid guid = data.Reader.GetGuid( data.Position );
                    if ( this.nullable )
                    {
                        data.ObjectValue = guid;
                    }
                    else
                    {
                        data.GuidValue = guid;
                    }
                }
                else
                {
                    if ( this.nullable )
                    {
                        data.ObjectValue = null;
                    }
                    else if ( data.Box )
                    {
                        data.ObjectValue = BoxedEmpty;
                    }
                    else
                    {
                        data.GuidValue = Guid.Empty;
                    }
                }

                return ( data.Box || this.nullable ) ? data.ObjectValue : null;
            }
            public override void Write( ref DataHolder data )
            {
                DbParameter parameter = data.Parameter;
                parameter.DbType = DbType.Guid;
                if ( data.NoValue )
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    if ( this.nullable || data.Box )
                    {
                        parameter.Value = AdoTypeConverterUtils.Check<Guid>( data.ObjectValue );
                    }
                    else
                    {
                        parameter.Value = data.GuidValue;
                    }
                }
            }
            public override bool CreateLiteralSql( ref DataHolder holder )
            {
                if ( holder.NoValue )
                {
                    holder.StringValue = "NULL";

                    return false;
                }
                else
                {
                    Guid guid;
                    if ( this.nullable || holder.Box )
                    {
                        guid = ( Guid ) AdoTypeConverterUtils.Check<Guid>( holder.ObjectValue );
                    }
                    else
                    {
                        guid = holder.GuidValue;
                    }

                    holder.StringValue = Convert.ToString( guid );

                    return false;
                }
            }
        }  

        #endregion
    }
}
