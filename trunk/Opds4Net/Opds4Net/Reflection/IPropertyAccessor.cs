namespace Opds4Net.Reflection
{
    /// <summary>
    /// Provides the capability to get property value from an object.
    /// </summary>
    public interface IPropertyAccessor
    {
        /// <summary>
        /// Gets property value from an object by an given propertyName.
        /// </summary>
        /// <param name="instance">The object to get value from.</param>
        /// <param name="propertyName">The property name by which to get value from.</param>
        /// <returns>Property value fetched from the given instance.</returns>
        object GetProperty(object instance, string propertyName);

        /// <summary>
        /// Sets property value to an object by an given propertyName and property Value
        /// </summary>
        /// <param name="instance">The object to set value to.</param>
        /// <param name="propertyName">The property name by which to set value to.</param>
        /// <param name="value">the newPropertyValue</param>
        /// <returns>True is the property is set successfully. False if the proeprty not found.</returns>
        bool SetProperty(object instance, string propertyName, object value);
    }
}
