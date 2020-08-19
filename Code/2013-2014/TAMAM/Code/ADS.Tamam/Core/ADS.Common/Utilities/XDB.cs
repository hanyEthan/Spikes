using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using OpenAccessRuntime.Data;
using Telerik.OpenAccess.Data;
using Telerik.OpenAccess.Data.Common;
using LinqKit;

using ADS.Common.Handlers;
using System.Linq.Expressions;
using ADS.Common.Models.Enums;
using System.Collections.Generic;
using ADS.Common.Handlers.Data.ORM;

namespace ADS.Common.Utilities
{
    /// <summary>
    /// Utility : database utilities is a small set of quick code snippets for productivity support when coding against the underlying datalayer and its datasets
    /// </summary>
    public static class XDB
    {
        public static bool IsOracle()
        {
            //return false;
            return Broker.Settings.Datastore.Type == DatastoreType.Oracle;
        }
        public static bool IsOracle( DomainDataContext dbContext )
        {
            //return false;
            return dbContext.Metadata.BackendType != Telerik.OpenAccess.Metadata.Backend.MsSql;
        }
        public static DomainDataContext GetDataContext()
        {
            var dbContext = new DomainDataContext();

            dbContext.BackendInfo.MaximumNumberOfInValues = Math.Max( Broker.Settings.Datastore.MaximumNumberOfInValues , dbContext.BackendInfo.MaximumNumberOfInValues );
            dbContext.BackendInfo.MaximumNumberOfQueryParameters = Math.Max( Broker.Settings.Datastore.MaximumNumberOfQueryParameters , dbContext.BackendInfo.MaximumNumberOfQueryParameters );

            return dbContext;
        }

        public static Expression<Func<TModel , bool>> IsItemContained<TModel , TItem>( List<TItem> list , Func<TModel , TItem> prop )
        {
            var predicate = PredicateBuilder.False<TModel>();

            if ( list == null || !list.Any() ) return predicate.Or( x => true );

            if ( IsOracle() )
            {
                int PARTITIONSIZE = 500;

                // if it's a list of string, lower all items' cases
                list = typeof( TItem ) == typeof( string )
                     ? list.ConvertAll( x => EqualityComparer<TItem>.Default.Equals( x , default( TItem ) )
                     ? default( TItem )
                     : ( TItem ) ( object ) ( ( ( string ) ( object ) x ).ToLower() ) )
                     : list;

                var subListsCount = ( list.Count / PARTITIONSIZE ) + ( ( list.Count % PARTITIONSIZE ) == 0 ? 0 : 1 );
                var listOfLists = new List<List<TItem>>();

                for ( int i = 0 ; i < subListsCount ; i++ )
                {
                    listOfLists.Add( list.Skip( i * PARTITIONSIZE ).Take( PARTITIONSIZE ).ToList() );
                }

                for ( int i = 0 ; i < listOfLists.Count ; i++ )
                {
                    var subList = listOfLists[i];
                    //predicate = predicate.Or( x => subList.Contains( typeof( TItem ) == typeof( string ) ? ( ( string ) ( object ) prop( x ) ).ToLower() : prop( x ) ) );

                    if ( typeof( TItem ) == typeof( string ) )
                    {
                        predicate = predicate.Or( x => subList.Contains( ( TItem ) ( object ) ( ( ( string ) ( object ) prop( x ) ).ToLower() ) ) );
                    }
                    else
                    {
                        predicate = predicate.Or( x => subList.Contains( prop( x ) ) );
                    }
                }

                return predicate;
            }
            else
            {
                return predicate.Or( x => list.Contains( prop( x ) ) );
            }
        }
    }
}
