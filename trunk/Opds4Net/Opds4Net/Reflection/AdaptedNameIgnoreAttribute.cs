using System;

namespace Opds4Net.Reflection
{
    /// <summary>
    /// Mark the applied property a non-opds related property. To speed up the adapter on the containing class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AdaptedNameIgnoreAttribute : Attribute
    {
    }
}
