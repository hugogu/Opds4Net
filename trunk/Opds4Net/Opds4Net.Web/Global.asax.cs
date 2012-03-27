using System;
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
        private static CompositionContainer container = new CompositionContainer(new AggregateCatalog(new DirectoryCatalog(@".\bin")), true);

        /// <summary>
        /// 
        /// </summary>
        [Import("FileSystem")]
        public IOpdsDataSource FileSystemOpds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Import("Local")]
        public IContentStorage ContentSaver { get; set; }

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
                Container.ComposeExportedValue("BuyLinkPattern", "/opds/Buy?id={0}");
                Container.ComposeExportedValue("DetailLinkPattern", "/opds/Detail?id={0}");
                Container.ComposeExportedValue("DownloadLinkPattern", "/opds/Download?id={0}");
                Container.ComposeExportedValue("NavigationLinkPattern", "/opds/Category?id={0}");
                Container.ComposeExportedValue("BookFolder", HostingEnvironment.MapPath("~/App_Data"));
                Container.ComposeExportedValue("LocalStorageFolder", HostingEnvironment.MapPath("~/App_Data/Uploaded"));
                initialized = true;
            }

            Container.ComposeParts(this);

            AuthenticateRequest += OnAuthenticateRequest;
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
                "Opds",
                "Opds/{action}/{id}",
                new { controller = "DbOpds", action = "Category", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Account",
                "account/{action}",
                new { controller = "Account", action = "LogOn" }
            );

            routes.MapRoute(
                "Default",
                "{action}",
                new { controller = "Default", action = "Index" }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                var id = Context.User.Identity;
                if (id.IsAuthenticated)
                {
                    //var roles = new UserRepository().GetRoles(id.Name);
                    //Context.User = new GenericPrincipal(id, roles);
                }
            }
            else
            {
                // TODO: 跳转登录？
            }
        }
    }
}