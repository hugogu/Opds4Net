using System;

namespace Opds4Net.Reflection
{
    /// <summary>
    /// Give a Prorperty of existing Data Model another name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = true)]
    public class AdaptedNameAttribute : Attribute
    {
        /// <summary>
        /// Default constructor fo AdaptedNameAttribute
        /// </summary>
        /// <param name="name">Element name mapping to.</param>
        public AdaptedNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Element name mapping to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The path to read the value in the current property.
        /// Used to extract property value from custom types.
        /// </summary>
        public string PropertyPath { get; set; }

        /// <summary>
        /// Gets or sets the order value used in AdaptedPropertyAccessor to arrange the name in specified order.
        /// The order value larger, the later the property will be process.
        /// </summary>
        public int Order { get; set; }
    }
}
