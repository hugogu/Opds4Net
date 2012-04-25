using System.Collections;
using System.Collections.Generic;

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
            return HandleNullValue((accessor ?? AdaptedAccessorFactory.Instance.GetAccessor(instance)).GetProperty(instance, propertyName), instance, propertyName);
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
            return HandleNullValue((accessor ?? AdaptedAccessorFactory.Instance.GetAccessor<T>()).GetProperty(instance, propertyName), instance, propertyName);
        }

        private static object HandleNullValue(object result, object instance, string propertyName)
        {
            // 对象自身的属性具有更高的优先级。比如List的Count。
            if (result == null && instance is IEnumerable)
            {
                foreach (var sub in instance as IEnumerable)
                {
                    var value = GetProperty(sub, propertyName);
                    if (value != null)
                    {
                        return value;
                    }
                }
            }

            return result;
        }
    }
}
