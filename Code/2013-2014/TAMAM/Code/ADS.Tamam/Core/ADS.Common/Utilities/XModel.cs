using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ADS.Common.Utilities
{
    public static class XModel
    {
        /// <summary>
        /// Compare between two objects from the same type on public, Primitive properties
        /// return a list with properties names that have different values
        /// </summary>
        public static List<string> GetDifferences<T>( T self, T to, params string[] ignore ) where T : class
        {
            var differences = new List<string>();
            if ( self == null && to == null ) return differences;

            var type = typeof ( T );
            var ignoreList = new List<string>( ignore );
            var properties = type.GetProperties( BindingFlags.Public | BindingFlags.Instance ).ToList();
            // filter : get primitives only
            properties = properties.Where( p => p.PropertyType.IsValueType || p.PropertyType == typeof ( string ) ).ToList();

            foreach ( var prop in properties )
            {
                if ( ignoreList.Contains( prop.Name ) ) continue;

                var selfValue = type.GetProperty( prop.Name ).GetValue( self, null );
                var toValue = type.GetProperty( prop.Name ).GetValue( to, null );

                if ( selfValue == null && toValue == null ) continue;

                if ( selfValue == null ^ toValue == null )
                {
                    differences.Add( prop.Name );
                    continue;
                }

                if ( !selfValue.Equals( toValue ) )
                {
                    differences.Add( prop.Name );
                }
            }

            return differences;
        }
        public static T Clone<T>( T model ) where T : class , new()
        {
            if ( model == null ) return default(T);

            T cloned = new T();

            var type = typeof( T );
            var properties = type.GetProperties( BindingFlags.Public | BindingFlags.Instance ).ToList();

            // filter : get primitives only
            //properties = properties.Where( p => p.PropertyType.IsValueType || p.PropertyType == typeof( string ) ).ToList();

            foreach ( var prop in properties )
            {
                var selfValue = type.GetProperty( prop.Name ).GetValue( model , null );

                if (type.GetProperty(prop.Name).CanWrite)
                {
                    type.GetProperty(prop.Name).SetValue(cloned, selfValue);
                }
            }

            return cloned;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(IEnumerable<T> items, Func<T, TKey> keyer)
        {
            if (items == null) return items;
            var set = new HashSet<TKey>();
            var list = new List<T>();
            foreach (var item in items)
            {
                var key = keyer(item);
                if (set.Contains(key))
                    continue;
                list.Add(item);
                set.Add(key);
            }
            return list;
        }

        public static List<List<T>> GetListOfLists<T>(List<T> items, int numberPerList)
        {
            var ListOfListsCount = (items.Count / numberPerList) + ((items.Count % numberPerList) == 0 ? 0 : 1);
            var ListOfLists = new List<List<T>>();
            for (int i = 0; i < ListOfListsCount; i++)
            {
                ListOfLists.Add(items.Skip(i * numberPerList).Take(numberPerList).ToList());
            }
            return ListOfLists;
        }

        public static List<string> SplitStringByLength(string input, int maxLength)
        {
            var ListOfString = new List<string>();
            for (int index = 0; index < input.Length; index += (maxLength + 1))
            {
                ListOfString.Add(input.Substring(index, Math.Min(maxLength, input.Length - index)));
            }
            return ListOfString;
        }


        public static string ListToString<T>(List<T> list)
        {
            if (list != null) return string.Join(",", list.ToArray());
            return string.Empty;
        }

        public static List<Guid> ToListOfGuid(List<string> stringList)
        {
            var listofGuid = new List<Guid>();
            foreach (var stringentry in stringList)
            {
                listofGuid.AddRange(stringentry.ToGuidList());
            }
            return listofGuid;
        }
    }
}
