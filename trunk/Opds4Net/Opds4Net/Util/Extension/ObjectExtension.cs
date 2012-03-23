using System;
using System.Collections.Generic;

namespace Opds4Net.Util.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtension
    {
        private static Dictionary<Type, IPropertyAccessor> propertyAccessors = new Dictionary<Type, IPropertyAccessor>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IPropertyAccessor GetPropertyAccessor(this object instance)
        {
            return GetPropertyAccessor(instance.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPropertyAccessor GetPropertyAccessor<T>(this T instance)
        {
            var type = typeof(T);
            IPropertyAccessor accessor = null;
            lock (propertyAccessors)
            {
                if (!propertyAccessors.TryGetValue(type, out accessor))
                {
                    accessor = InitializeAccessor<T>();
                }
            }

            return accessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPropertyAccessor GetPropertyAccessor(this Type type)
        {
            IPropertyAccessor accessor = null;
            lock (propertyAccessors)
            {
                if (!propertyAccessors.TryGetValue(type, out accessor))
                {
                    accessor = InitializeAccessor(type);
                }
            }

            return accessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty(this object instance, string propertyName)
        {
            return GetPropertyAccessor(instance).GetProperty(instance, propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty<T>(this T instance, string propertyName)
        {
            return GetPropertyAccessor(instance).GetProperty(instance, propertyName);
        }

        private static IPropertyAccessor InitializeAccessor(Type type)
        {
            var accessor = Activator.CreateInstance(typeof(ModelHelper<>).MakeGenericType(type)) as IPropertyAccessor;
            propertyAccessors.Add(type, accessor);

            return accessor;
        }

        private static IPropertyAccessor InitializeAccessor<T>()
        {
            var accessor = new ModelHelper<T>();
            propertyAccessors.Add(typeof(T), accessor);

            return accessor;
        }
    }
}
