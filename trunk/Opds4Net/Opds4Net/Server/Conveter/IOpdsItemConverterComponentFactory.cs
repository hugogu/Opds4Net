using Opds4Net.Reflection;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpdsItemConverterComponentFactory
    {
        /// <summary>
        /// 
        /// </summary>
        IOpdsLinkGenerator LinkGenerator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IDataTypeDetector TypeDetector { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IAdapterFactory AdapterFactory { get; set; }
    }
}
