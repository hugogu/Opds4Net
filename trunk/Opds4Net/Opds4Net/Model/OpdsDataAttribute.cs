using System;

namespace Opds4Net.Model
{
    /// <summary>
    /// A decleration on a class to mark it as Opds data model, and also provide a few information about the data type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OpdsDataAttribute : Attribute
    {
        /// <summary>
        /// Default constructor fo OpdsDataAttribute
        /// </summary>
        /// <param name="type"></param>
        public OpdsDataAttribute(OpdsDataType type)
        {
            Type = type;
        }

        /// <summary>
        /// The data type the class represents.
        /// </summary>
        public OpdsDataType Type { get; internal set; }
    }
}
