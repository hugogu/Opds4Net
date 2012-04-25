using System.Collections;

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
            if (instance is IEnumerable)
            {
                return GetProperty(instance as IEnumerable, propertyName, accessor);
            }

            return (accessor ?? AdaptedAccessorFactory.Instance.GetAccessor(instance)).GetProperty(instance, propertyName);
        }

        /// <summary>
        /// Gets property value from a group of object by an given propertyName.
        /// The first property value found in the object group will be returned and other values will be ignore.
        /// </summary>
        /// <param name="instances">A group of object to get value from.</param>
        /// <param name="propertyName">The property name by which to get value from.</param>
        /// <param name="accessor"></param>
        /// <returns>Property value fetched from the given instances.</returns>
        public static object GetProperty(this IEnumerable instances, string propertyName, IPropertyAccessor accessor = null)
        {
            foreach (var instance in instances)
            {
                var value = GetProperty(instance, propertyName, accessor);
                if (value != null)
                    return value;
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
            return (accessor ?? AdaptedAccessorFactory.Instance.GetAccessor<T>()).GetProperty(instance, propertyName);
        }
    }
}
