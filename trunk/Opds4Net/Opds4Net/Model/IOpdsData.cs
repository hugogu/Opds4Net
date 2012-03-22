namespace Opds4Net.Model
{
    /// <summary>
    /// Provides the ability to provide opds data type information.
    /// </summary>
    public interface IOpdsData
    {
        /// <summary>
        /// Gets the data type of the current data object.
        /// </summary>
        OpdsDataType DataType { get; }
    }
}
