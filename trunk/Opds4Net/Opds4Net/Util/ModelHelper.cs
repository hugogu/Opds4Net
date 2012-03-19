using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Opds4Net.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelHelper<T>
    {
        private static Dictionary<string, Func<T, object>> keySelectors = new Dictionary<string, Func<T, object>>();

        static ModelHelper()
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="KeyType"></typeparam>
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
    }
}
