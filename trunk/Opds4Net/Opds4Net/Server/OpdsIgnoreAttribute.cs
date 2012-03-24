using System;

namespace Opds4Net.Server
{
    /// <summary>
    /// Mark the applied property a non-opds related property. To speed up the accessor on the containing class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OpdsIgnoreAttribute : Attribute
    {
    }
}
