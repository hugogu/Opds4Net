using Opds4Net.Reflection;

namespace Opds4Net.Server
{
    /// <summary>
    /// A Factory to create the Components used in an IOpdsItemConverter.
    /// </summary>
    public interface IOpdsItemConverterComponentFactory
    {
        /// <summary>
        /// A syndication link generator, provides information of the OPDS Site.
        /// </summary>
        IOpdsLinkGenerator LinkGenerator { get; set; }

        /// <summary>
        /// A detector used to detect the data type of a given object.
        /// </summary>
        IDataTypeDetector TypeDetector { get; set; }

        /// <summary>
        /// A factory used to create property accessor according to a given object.
        /// </summary>
        IAccessorFactory AdapterFactory { get; set; }
    }
}
