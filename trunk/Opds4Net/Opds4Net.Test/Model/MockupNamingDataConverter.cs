using System.ComponentModel.Composition;
using Opds4Net.Reflection;
using Opds4Net.Server;

namespace Opds4Net.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IOpdsItemConverter))]
    public class MockupNamingDataConverter : NamingDataOpdsItemConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        [ImportingConstructor]
        public MockupNamingDataConverter(
            [Import]IOpdsLinkGenerator linkGenerator,
            [Import("OpdsData")]IDataTypeDetector typeDetector,
            [Import("Naming")]IAdapterFactory adapterFactory)
            : base(linkGenerator, typeDetector, adapterFactory)
        {
        }
    }
}
