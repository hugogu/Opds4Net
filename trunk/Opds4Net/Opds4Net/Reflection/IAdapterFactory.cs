namespace Opds4Net.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdapterFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IPropertyAdapter GetAdapter(object value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IPropertyAdapter GetAdapter<T>();
    }
}
