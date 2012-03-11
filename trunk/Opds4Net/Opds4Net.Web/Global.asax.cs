using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Opds4Net.Server;

namespace Opds4Net.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class MvcApplication : HttpApplication
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
        public static MvcApplication Current
        {
            get { return HttpContext.Current.ApplicationInstance as MvcApplication; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static CompositionContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MvcApplication()
        {
            if (!initialized)
            {
                Container.ComposeExportedValue("BookFolder", HostingEnvironment.MapPath("~/App_Data"));
                Container.ComposeExportedValue("NavigationLinkPattern", "/Category?id={0}");
                Container.ComposeExportedValue("DetailLinkPattern", "/Detail?id={0}");
                Container.ComposeExportedValue("DownloadLinkPattern", "/Download?id={0}");
                Container.ComposeExportedValue("BuyLinkPattern", "/Buy?id={0}");
                initialized = true;
            }

            Container.ComposeParts(this);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "BookEditor",
                "Book/{action}/{id}",
                new { controller = "Book", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "CategoryEditor",
                "Category/{action}/{id}",
                new { controller = "Category", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "FSOpds",
                "FS/{action}/{id}",
                new { controller = "FileSystemOpds", action = "Category", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default",
                "{action}/{id}",
                new { controller = "DbOpds", action = "Category", id = UrlParameter.Optional }
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