namespace Opds4Net.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object GetProperty(object instance, string propertyName);
    }
}
