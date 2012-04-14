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
        /// <returns></returns>
        public static object GetProperty(this object instance, string propertyName)
        {
            return NamingAdapterFactory.Instance.GetAdapter(instance).GetProperty(instance, propertyName);
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
            return NamingAdapterFactory.Instance.GetAdapter<T>().GetProperty(instance, propertyName);
        }
    }
}
