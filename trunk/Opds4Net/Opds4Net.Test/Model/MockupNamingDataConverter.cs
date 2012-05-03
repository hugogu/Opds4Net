using System.ComponentModel.Composition;
using Opds4Net.Reflection;
using Opds4Net.Server;

namespace Opds4Net.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IOpdsItemConverterComponentFactory))]
    public class MockDataConverterComponentFactory : IOpdsItemConverterComponentFactory
    {
        /// <summary>
        /// 
        /// </summary>
        [ImportingConstructor]
        public MockDataConverterComponentFactory(
            [Import]IOpdsLinkGenerator linkGenerator,
            [Import("OpdsData")]IDataTypeDetector typeDetector,
            [Import("Naming")]IAccessorFactory adapterFactory)
        {
            LinkGenerator = linkGenerator;
            TypeDetector = typeDetector;
            AdapterFactory = adapterFactory;
            Names = new OpdsNames();
        }

        public IOpdsLinkGenerator LinkGenerator { get; set; }

        public IDataTypeDetector TypeDetector { get; set; }

        public IAccessorFactory AdapterFactory { get; set; }

        public OpdsNames Names { get; set; }
    }

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
        public MockupNamingDataConverter(IOpdsItemConverterComponentFactory factory)
            : base(factory)
        {
        }
    }
}
