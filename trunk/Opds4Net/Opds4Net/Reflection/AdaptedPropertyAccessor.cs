using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Opds4Net.Reflection
{
    /// <summary>
    /// Provides the capability to get property value from an object by a given name.
    /// </summary>
    public class AdaptedPropertyAccessor<T> : IPropertyAccessor
    {
        private static Func<T, string, object> memberAdaptor = null;

        static AdaptedPropertyAccessor()
        {
            InitializeMemberAdapter();
        }

        /// <summary>
        /// Gets property value from an object by an given propertyName.
        /// </summary>
        /// <param name="instance">The object to get value from.</param>
        /// <param name="propertyName">The property name by which to get value from.</param>
        /// <returns>Property value fetched from the given instance.</returns>
        public static object GetProperty(T instance, string propertyName)
        {
            return memberAdaptor(instance, propertyName);
        }

        /// <summary>
        /// Gets property value from an object by an given propertyName.
        /// </summary>
        /// <param name="instance">The object to get value from.</param>
        /// <param name="propertyName">The property name by which to get value from.</param>
        /// <returns>Property value fetched from the given instance.</returns>
        public object GetProperty(object instance, string propertyName)
        {
            return GetProperty((T)instance, propertyName);
        }

        private static void InitializeMemberAdapter()
        {
            var type = typeof(T);
            var instance = Expression.Parameter(typeof(T), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            // var nameHash = memberName.GetHashCode();
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));

            // switch (memberName.GetHashCode()) {
            var cases = new List<SwitchCase>();
            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyPairs = GetPropertyNamesHashes(propertyInfo);
                if (propertyPairs.Select(p => p.Key).Distinct().Count() < propertyPairs.Count())
                    throw new InvalidProgramException("Duplicated OpdsName detected.");

                foreach (var propertyPair in propertyPairs)
                {
                    // Build the expression to fetch property value from complex type by a given property path.
                    var property = Expression.Property(instance, propertyInfo.Name);
                    if (!String.IsNullOrEmpty(propertyPair.Value))
                    {
                        var paths = propertyPair.Value.Split('.');
                        foreach (var path in paths)
                        {
                            property = Expression.Property(property, path);
                        }
                    }
                    // case property.Name.GetHashCode():
                    //     return property.path.path as object;
                    cases.Add(Expression.SwitchCase(Expression.Convert(property, typeof(object)), Expression.Constant(propertyPair.Key, typeof(int))));
                }
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[] { nameHash }, calHash, switchEx);

            memberAdaptor = Expression.Lambda<Func<T, string, object>>(methodBody, instance, memberName).Compile();
        }

        private static IEnumerable<KeyValuePair<int, string>> GetPropertyNamesHashes(PropertyInfo propertyInfo)
        {
            var isIgnored = propertyInfo.GetCustomAttributes(typeof(AdaptedNameIgnoreAttribute), true).Any();
            if (!isIgnored)
            {
                yield return new KeyValuePair<int, string>(propertyInfo.Name.GetHashCode(), null);
                foreach (AdaptedNameAttribute attribute in propertyInfo.GetCustomAttributes(typeof(AdaptedNameAttribute), true))
                {
                    yield return new KeyValuePair<int, string>(attribute.Name.GetHashCode(), attribute.PropertyPath);
                }
            }
        }
    }
}
