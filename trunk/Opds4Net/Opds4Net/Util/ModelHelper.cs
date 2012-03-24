using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Opds4Net.Server;
using Opds4Net.Util.Extension;

namespace Opds4Net.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelHelper<T> : IPropertyAccessor
    {
        private static Dictionary<string, Func<T, object>> keySelectors = new Dictionary<string, Func<T, object>>();
        private static Func<T, string, object> memberAccessor = null;

        static ModelHelper()
        {
            InitializeKeySelectors();
            InitializeMemberAccessor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Func<T, object> FindKeySelector(string propertyName)
        {
            Func<T, object> result = null;
            keySelectors.TryGetValue(propertyName ?? "__DefaultSelector", out result);
            if (result == null)
            {
                keySelectors.TryGetValue("__DefaultSelector", out result);

                if (result == null)
                {
                    throw new InvalidOperationException("Neigher the propertyName nor the default property as key selector is specified.");
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty(T instance, string propertyName)
        {
            return memberAccessor(instance, propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetProperty(object instance, string propertyName)
        {
            return GetProperty((T)instance, propertyName);
        }

        private static void InitializeKeySelectors()
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var instance = Expression.Parameter(typeof(T));
                var keySelector = Expression.Lambda(typeof(Func<T, object>), Expression.Convert(Expression.PropertyOrField(instance, propertyInfo.Name), typeof(object)), instance).Compile() as Func<T, object>;

                keySelectors.Add(propertyInfo.Name, keySelector);

                var attributes = propertyInfo.GetCustomAttributes(typeof(DefaultKeySelectorAttribute), true);
                if (attributes.Length > 0)
                {
                    keySelectors["__DefaultSelector"] = keySelector;
                }
            }
        }

        private static void InitializeMemberAccessor()
        {
            var type = typeof(T);
            var instance = Expression.Parameter(typeof(T), "instance");
            var memberName = Expression.Parameter(typeof(string), "memberName");
            var nameHash = Expression.Variable(typeof(int), "nameHash");
            // var nameHash = memberName.GetHashCode();
            var calHash = Expression.Assign(nameHash, Expression.Call(memberName, typeof(object).GetMethod("GetHashCode")));

            // switch (memberName.GetHashCode()) {
            var cases = new List<SwitchCase>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var propertyHash = GetPropertyNamesHashes(propertyInfo).Select(i => Expression.Constant(i, typeof(int)));
                if (propertyHash.Any())
                {
                    var property = Expression.Property(instance, propertyInfo.Name);
                    // case property.Name.GetHashCode():
                    //     return property as object;
                    cases.Add(Expression.SwitchCase(Expression.Convert(property, typeof(object)), propertyHash));
                }
            }
            var switchEx = Expression.Switch(nameHash, Expression.Constant(null), cases.ToArray());
            var methodBody = Expression.Block(typeof(object), new[] { nameHash }, calHash, switchEx);

            memberAccessor = Expression.Lambda<Func<T, string, object>>(methodBody, instance, memberName).Compile();
        }

        private static IEnumerable<int> GetPropertyNamesHashes(PropertyInfo propertyInfo)
        {
            var isIgnored = propertyInfo.GetCustomAttributes(typeof(OpdsIgnoreAttribute), true).Any();
            if (!isIgnored)
            {
                yield return propertyInfo.Name.GetHashCode();
                foreach (OpdsNameAttribute attribute in propertyInfo.GetCustomAttributes(typeof(OpdsNameAttribute), true))
                {
                    yield return attribute.Name.GetHashCode();
                }
            }
        }
    }
}
