namespace Opds4Net.Util.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyAccessor
    {
        object GetProperty(object instance, string propertyName);
    }
}
