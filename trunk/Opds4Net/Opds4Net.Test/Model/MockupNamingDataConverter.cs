using System.ComponentModel.Composition;
using Opds4Net.Server;
using Opds4Net.Test.Common;

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
        [ImportingConstructor]
        public MockupNamingDataConverter()
            : base(TestInitializer.Container.GetExportedValue<IOpdsLinkGenerator>())
        {
        }
    }
}
