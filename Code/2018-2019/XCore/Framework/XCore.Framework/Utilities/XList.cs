using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Framework.Utilities
{
    public static class XList
    {
        public static IEnumerable<T> DistinctBy<T, TKey>( IEnumerable<T> items , Func<T , TKey> keyer )
        {
            if ( items == null ) return items;

            var set = new HashSet<TKey>();
            var list = new List<T>();

            foreach ( var item in items )
            {
                var key = keyer( item );
                if ( set.Contains( key ) ) continue;
                list.Add( item );
                set.Add( key );
            }
            return list;
        }
        public static List<List<T>> Divide<T>( List<T> items , int numberPerList )
        {
            var ListOfListsCount = ( items.Count / numberPerList ) + ( ( items.Count % numberPerList ) == 0 ? 0 : 1 );
            var ListOfLists = new List<List<T>>();
            for ( int i = 0 ; i < ListOfListsCount ; i++ )
            {
                ListOfLists.Add( items.Skip( i * numberPerList ).Take( numberPerList ).ToList() );
            }
            return ListOfLists;
        }
        public static string ListToString<T>( List<T> list )
        {
            if ( list != null ) return string.Join( "," , list.ToArray() );
            return string.Empty;
        }
    }
}
