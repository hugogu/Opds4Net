using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Opds4Net.Test.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TestInitializer
    {
        private static CompositionContainer container = new CompositionContainer(new AggregateCatalog(new DirectoryCatalog(".")), true);

        static TestInitializer()
        {
            Container.ComposeExportedValue("BuyLinkPattern", "/Buy?id={0}");
            Container.ComposeExportedValue("DetailLinkPattern", "/Detail?id={0}");
            Container.ComposeExportedValue("DownloadLinkPattern", "/Download?id={0}");
            Container.ComposeExportedValue("NavigationLinkPattern", "/Category?id={0}");
        }

        /// <summary>
        /// 
        /// </summary>
        public static CompositionContainer Container
        {
            get { return container; }
        }
    }
}
