﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Opds4Net.Reflection
{
    /// <summary>
    /// Naming AdapterFactory helps to create and cache naming adapter instances. 
    /// For the same object type, the factory will returns the same instance of AdaptedPropertyAccessor from cache.
    /// </summary>
    [Export("Naming", typeof(IAccessorFactory))]
    public class AdaptedAccessorFactory : IAccessorFactory
    {
        private static IAccessorFactory instance = new AdaptedAccessorFactory();
        private static Dictionary<Type, IPropertyAccessor> propertyAdapters = new Dictionary<Type, IPropertyAccessor>();

        private AdaptedAccessorFactory() { }

        /// <summary>
        /// Gets the instance of AdaptedAccessorFactory.
        /// </summary>
        public static IAccessorFactory Instance { get { return instance; } }

        /// <summary>
        /// Gets an IPropertyAccessor by an object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>An instance of IPropertyAccessor.</returns>
        public IPropertyAccessor GetAccessor(object value)
        {
            return GetPropertyAccessor(value.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPropertyAccessor GetAccessor<T>()
        {
            var type = typeof(T);
            IPropertyAccessor adapter = null;
            lock (propertyAdapters)
            {
                if (!propertyAdapters.TryGetValue(type, out adapter))
                {
                    adapter = InitializeAccessor<T>();
                }
            }

            return adapter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IPropertyAccessor GetPropertyAccessor(Type type)
        {
            IPropertyAccessor adapter = null;
            lock (propertyAdapters)
            {
                if (!propertyAdapters.TryGetValue(type, out adapter))
                {
                    adapter = InitializeAccessor(type);
                }
            }

            return adapter;
        }

        private static IPropertyAccessor InitializeAccessor(Type type)
        {
            var adapter = Activator.CreateInstance(typeof(AdaptedPropertyAccessor<>).MakeGenericType(type)) as IPropertyAccessor;
            propertyAdapters.Add(type, adapter);

            return adapter;
        }

        private static IPropertyAccessor InitializeAccessor<T>()
        {
            var adapter = new AdaptedPropertyAccessor<T>();
            propertyAdapters.Add(typeof(T), adapter);

            return adapter;
        }
    }
}