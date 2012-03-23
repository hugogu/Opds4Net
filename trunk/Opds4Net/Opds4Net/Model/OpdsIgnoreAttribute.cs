using System;

namespace Opds4Net.Model
{
    /// <summary>
    /// Mark the applied property a non-opds related property. To speed up the accessor on the containing class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OpdsIgnoreAttribute : Attribute
    {
    }
}
