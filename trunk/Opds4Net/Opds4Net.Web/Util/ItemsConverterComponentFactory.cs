using System.ComponentModel.Composition;
using Opds4Net.Reflection;
using Opds4Net.Server;

namespace Opds4Net.Web.Util
{
    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IOpdsItemConverterComponentFactory))]
    public class ItemsConverterComponentFactory : IOpdsItemConverterComponentFactory
    {
        /// <summary>
        /// 
        /// </summary>
        [ImportingConstructor]
        public ItemsConverterComponentFactory(
            [Import]IOpdsLinkGenerator linkGenerator,
            [Import("OpdsData")]IDataTypeDetector typeDetector,
            [Import("Naming")]IAdapterFactory adapterFactory)
        {
            LinkGenerator = linkGenerator;
            TypeDetector = typeDetector;
            AdapterFactory = adapterFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        public IOpdsLinkGenerator LinkGenerator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDataTypeDetector TypeDetector { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IAdapterFactory AdapterFactory { get; set; }
    }
}