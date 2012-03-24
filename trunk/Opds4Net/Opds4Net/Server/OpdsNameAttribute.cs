using System;

namespace Opds4Net.Server
{
    /// <summary>
    /// Mapping a Prorperty of existing Data Model to OPDS entry element name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OpdsNameAttribute : Attribute
    {
        /// <summary>
        /// Default constructor fo OpdsNameAttribute
        /// </summary>
        /// <param name="name">Opds entry element name mapping to.</param>
        public OpdsNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Opds entry element name mapping to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The path to read the value in the current property.
        /// Used to extract OPDS property from custom types.
        /// </summary>
        public string PropertyPath { get; set; }
    }
}
