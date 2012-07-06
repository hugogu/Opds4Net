using System.Collections.Generic;
using System.Linq;

namespace Opds4Net.Util.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            return source.OrderBy(ModelHelper<TSource>.FindKeySelector(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            return source.OrderByDescending(ModelHelper<TSource>.FindKeySelector(propertyName));
        }
    }
}
