using System;

namespace Opds4Net.Model
{
    /// <summary>
    /// Mapping a Prorperty of existing Data Model to OPDS entry element name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
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
    }
}
