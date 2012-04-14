using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Opds4Net.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    [Export("Naming", typeof(IAdapterFactory))]
    public class NamingAdapterFactory : IAdapterFactory
    {
        private static IAdapterFactory instance = new NamingAdapterFactory();
        private static Dictionary<Type, IPropertyAdapter> propertyAdapters = new Dictionary<Type, IPropertyAdapter>();

        private NamingAdapterFactory() { }

        /// <summary>
        /// 
        /// </summary>
        public static IAdapterFactory Instance { get { return instance; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IPropertyAdapter GetAdapter(object value)
        {
            return GetPropertyAdapter(value.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPropertyAdapter GetAdapter<T>()
        {
            var type = typeof(T);
            IPropertyAdapter adapter = null;
            lock (propertyAdapters)
            {
                if (!propertyAdapters.TryGetValue(type, out adapter))
                {
                    adapter = InitializeAdapter<T>();
                }
            }

            return adapter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IPropertyAdapter GetPropertyAdapter(Type type)
        {
            IPropertyAdapter adapter = null;
            lock (propertyAdapters)
            {
                if (!propertyAdapters.TryGetValue(type, out adapter))
                {
                    adapter = InitializeAdapter(type);
                }
            }

            return adapter;
        }

        private static IPropertyAdapter InitializeAdapter(Type type)
        {
            var adapter = Activator.CreateInstance(typeof(NamingAdapter<>).MakeGenericType(type)) as IPropertyAdapter;
            propertyAdapters.Add(type, adapter);

            return adapter;
        }

        private static IPropertyAdapter InitializeAdapter<T>()
        {
            var adapter = new NamingAdapter<T>();
            propertyAdapters.Add(typeof(T), adapter);

            return adapter;
        }
    }
}
