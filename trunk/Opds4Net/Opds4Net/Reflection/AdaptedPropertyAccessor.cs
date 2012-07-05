using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static Func<T, string, object> memberGetter = null;
        private static Func<T, string, object, bool> memberSetter = null;

        static AdaptedPropertyAccessor()
        {
            InitializeMemberGetter();
            InitializeMemberSetter();
        }

        /// <summary>
        /// Gets property value from an object by an given propertyName.
        /// </summary>
        /// <param name="instance">The object to get value from.</param>
        /// <param name="propertyName">The property name by which to get value from.</param>
        /// <returns>Property value fetched from the given instance.</returns>
        public static object GetProperty(T instance, string propertyName)
        {
            return memberGetter(instance, propertyName);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetProperty(T instance, string propertyName, object value)
        {
            return memberSetter(instance, propertyName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetProperty(object instance, string propertyName, object value)
        {
            return SetProperty((T)instance, propertyName, value);
        }

        private static void InitializeMemberSetter()
        {
            var type = typeof(T);
            var instance = Expression.Parameter(type, "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var value = Expression.Parameter(typeof(object), "value");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));
            var cases = new List<SwitchCase>
                            {
                // Add a default case that do nothing.
                // In case of the class contains no writable properties.
                Expression.SwitchCase(Expression.Constant(false, typeof(bool)), Expression.Constant(-1, typeof(Int32))),
            };
            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(GetPropertyOrder))
            {
                // Property must be writeable.
                if (!propertyInfo.CanWrite)
                    continue;

                var propertyPairs = GetPropertyNamesHashes(propertyInfo).ToList();
                if (propertyPairs.Select(p => p.Key).Distinct().Count() < propertyPairs.Count())
                    throw new InvalidProgramException("Duplicated Property Name detected.");

                foreach (var propertyPair in propertyPairs)
                {
                    var property = BuildPropertyExpression(instance, propertyInfo, propertyPair);
                    var catchNullReference = Expression.Catch(typeof(NullReferenceException), Expression.Constant(false, typeof(bool)));
                    var tryRead = Expression.TryCatch(Expression.Block(typeof(bool), Expression.Assign(property, Expression.Convert(value, property.Type)), Expression.Constant(true, typeof(bool))), catchNullReference);
                    // case property.Name.GetHashCode():
                    //    try {
                    //        property[.Path][.Path] = (P)value;
                    //        return true;
                    //    // return null when any property in the path is null.
                    //    } catch(NullReferenceException ex) {
                    //        return false;
                    //    }
                    cases.Add(Expression.SwitchCase(tryRead, Expression.Constant(propertyPair.Key, typeof(int))));
                }
            }

            var switchEx = Expression.Switch(nameHash, Expression.Constant(false, typeof(bool)), cases.ToArray());
            var methodBody = Expression.Block(typeof(bool), new[] { nameHash }, calHash, switchEx);

            Debug.WriteLine(String.Format("Generate Accessor Method for class {0} defined {1} properties.", type.FullName, cases.Count));

            memberSetter = Expression.Lambda<Func<T, string, object, bool>>(methodBody, instance, memberName, value).Compile();
        }

        private static void InitializeMemberGetter()
        {
            var type = typeof(T);
            var instance = Expression.Parameter(type, "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            // var nameHash = memberName.GetHashCode();
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));

            // switch (memberName.GetHashCode()) {
            var cases = new List<SwitchCase>();
            // case: class.Name.GetHashCode():
            //      return instance;
            cases.Add(Expression.SwitchCase(Expression.Convert(instance, typeof(object)),
                GetClassNamesHashes(type).Select(h => Expression.Constant(h, typeof(int)))));
            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(GetPropertyOrder))
            {
                var propertyPairs = GetPropertyNamesHashes(propertyInfo).ToList();
                if (propertyPairs.Select(p => p.Key).Distinct().Count() < propertyPairs.Count())
                    throw new InvalidProgramException("Duplicated Property Name detected.");

                foreach (var propertyPair in propertyPairs)
                {
                    // Build the expression to fetch property value from complex type by a given property path.
                    var property = BuildPropertyExpression(instance, propertyInfo, propertyPair);
                    var catchNullReference = Expression.Catch(typeof(NullReferenceException), Expression.Constant(null, typeof(object)));
                    var tryRead = Expression.TryCatch(Expression.Convert(property, typeof(object)), catchNullReference);
                    // case property.Name.GetHashCode():
                    //    try {
                    //        return property[.Path][.Path] as object;
                    //    // return null when any property in the path is null.
                    //    } catch(NullReferenceException ex) {
                    //        return null;
                    //    }
                    cases.Add(Expression.SwitchCase(tryRead, Expression.Constant(propertyPair.Key, typeof(int))));
                }
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[] { nameHash }, calHash, switchEx);

            Debug.WriteLine(String.Format("Generate Accessor Method for class {0} defined {1} properties.", type.FullName, cases.Count));

            memberGetter = Expression.Lambda<Func<T, string, object>>(methodBody, instance, memberName).Compile();
        }

        private static MemberExpression BuildPropertyExpression(Expression instance, MemberInfo propertyInfo, KeyValuePair<int, string> propertyPair)
        {
            var property = Expression.Property(instance, propertyInfo.Name);
            if (!String.IsNullOrEmpty(propertyPair.Value))
            {
                var paths = propertyPair.Value.Split('.');
                foreach (var path in paths)
                {
                    property = Expression.Property(property, path);
                }
            }

            return property;
        }

        private static IEnumerable<KeyValuePair<int, string>> GetPropertyNamesHashes(PropertyInfo propertyInfo)
        {
            var isIgnored = propertyInfo.GetCustomAttributes(typeof(AdaptedNameIgnoreAttribute), true).Any();
            if (!isIgnored)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(AdaptedNameAttribute), true);
                // If an AdaptedNameAttribute is defined for a property. Then only the adaptedname could be used.
                // Otherwise, the adapted name may conflict with another existing property. For example
                // public class Dummy
                // {
                //     [AdaptedName("Value")]
                //     public Name { get; set }
                //     [AdaptedName("Name")]
                //     public Value { get; set }
                // }
                // The correct behavior is just switch the value of Name and Value property when reading them.
                // So, the original property name must be ignored when the AdaptedName attribute applied.
                if (attributes.Any())
                {
                    foreach (AdaptedNameAttribute attribute in attributes)
                    {
                        yield return new KeyValuePair<int, string>(attribute.Name.GetHashCode(), attribute.PropertyPath);
                    }
                }
                else
                {
                    yield return new KeyValuePair<int, string>(propertyInfo.Name.GetHashCode(), null);
                }
            }
        }

        private static int GetPropertyOrder(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes(typeof(AdaptedNameAttribute), true).Cast<AdaptedNameAttribute>().ToList();

            if (attributes.Any())
            {
                return attributes.Min(a => a.Order);
            }

            return Int32.MaxValue;
        }

        private static IEnumerable<int> GetClassNamesHashes(Type type)
        {
            yield return type.Name.GetHashCode();
            foreach (AdaptedNameAttribute attribute in type.GetCustomAttributes(typeof(AdaptedNameAttribute), true))
            {
                yield return attribute.Name.GetHashCode();
            }
        }
    }
}
