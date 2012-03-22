namespace Opds4Net.Util.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyAccessor
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
