using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Opds4Net.Server;

namespace Opds4Net.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static bool initialized = false;
        private static CompositionContainer container = new CompositionContainer(new AggregateCatalog(new DirectoryCatalog(@".\bin")));

        /// <summary>
        /// 
        /// </summary>
        [Import("FileSystem")]
        public IOpdsDataSource FileSystemOpds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MvcApplication()
        {
            if (!initialized)
            {
                container.ComposeExportedValue("BookFolder", HostingEnvironment.MapPath("~/App_Data"));
                container.ComposeExportedValue("NavigationLinkPattern", "/Category?id={0}");
                container.ComposeExportedValue("DetailLinkPattern", "/Detail?id={0}");
                container.ComposeExportedValue("DownloadLinkPattern", "/Download?id={0}");
                initialized = true;
            }

            container.ComposeParts(this);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{action}/{id}", // URL with parameters
                new { controller = "Category", action = "Category", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}