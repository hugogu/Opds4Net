using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Opds4Net.Reflection.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static object GetProperty(this object instance, string propertyName, IPropertyAccessor accessor = null)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            return HandleNullValue((accessor ?? AdaptedAccessorFactory.Instance.GetAccessor(instance)).GetProperty(instance, propertyName), instance, propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="accessor"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static object GetProperty(this object instance, IPropertyAccessor accessor = null, params string[] propertyNames)
        {
            if (propertyNames == null || propertyNames.Length == 0)
                throw new ArgumentNullException("propertyNames");

            accessor = accessor ?? AdaptedAccessorFactory.Instance.GetAccessor(instance);

            foreach (var propertyName in propertyNames)
            {
                // Need to Call Handle null value here.
                var value = instance.GetProperty(propertyName, accessor);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static object GetProperty<T>(this T instance, string propertyName, IPropertyAccessor accessor = null)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            return HandleNullValue((accessor ?? AdaptedAccessorFactory.Instance.GetAccessor<T>()).GetProperty(instance, propertyName), instance, propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static bool SetProperty<T>(this T instance, string propertyName, object value, IPropertyAccessor accessor = null)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            return (accessor ?? AdaptedAccessorFactory.Instance.GetAccessor<T>()).SetProperty(instance, propertyName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static bool SetProperty(this object instance, string propertyName, object value, IPropertyAccessor accessor = null)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException("propertyName");

            return (accessor ?? AdaptedAccessorFactory.Instance.GetAccessor(instance.GetType())).SetProperty(instance, propertyName, value);
        }

        private static object HandleNullValue(object result, object instance, string propertyName)
        {
            // 对象自身的属性具有更高的优先级。比如List的Count。
            if (result == null && instance is IEnumerable)
            {
                var values = new List<object>();
                var types = new List<Type>();
                foreach (var sub in instance as IEnumerable)
                {
                    if (sub == null)
                        continue;

                    types.Add(sub.GetType());
                    var value = GetProperty(sub, propertyName);
                    if (value == null)
                    {
                        continue;
                    }

                    var valueType = value.GetType();
                    // If the required propertyName result in a Primitive value or an IEnumerable.
                    // It represents a data part. Such as book info or category info set.
                    if (valueType.IsPrimitive ||
                        valueType == typeof(DateTime) ||
                        valueType == typeof(Guid) ||
                        typeof(IEnumerable).IsAssignableFrom(valueType))
                    {
                        return value;
                    }
                    // Any stand alone user type value. Such as an instance of Category Info or Author Info.
                    values.Add(value);
                }

                // Don't return empty set. Empty set wil not provide any real value.
                if (values.Any())
                {
                    // If all the value are of the same type.
                    // It will be take a value array. Such as Contributors and Categories.
                    if (types.GroupBy(t => t.FullName).Count() == 1)
                    {
                        return values;
                    }
                    // If the values are of difference typs.
                    // It is used as data combination. Such as book info and category info in a same set.
                    return values[0];
                }
            }

            return result;
        }
    }
}
