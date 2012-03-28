using System.IO;
using System.Linq;
using System.Web.Mvc;
using Opds4Net.Model;
using Opds4Net.Server.FileSystem;
using Opds4Net.Util;

namespace Opds4Net.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FileSystemOpdsController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Category(string id)
        {
            var request = new FSCategoryRequest(MvcApplication.Current.FileSystemBookFolder)
            {
                Id = id,
                PageIndex = 1,
                PageSize = 10,
                OrderBy = "LastUpdatedTime",
                OrderDirection = true,
            };
            var feed = new OpdsFeed(MvcApplication.Current.FileSystemOpds.GetItems(request).Items);

            return Content(feed.ToXml(), "text/xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            var request = new FSDetailRequest(MvcApplication.Current.FileSystemBookFolder) { Id = id };
            var item = MvcApplication.Current.FileSystemOpds.GetItems(request).Items.Single();

            return Content(item.ToXml(), "text/xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Download(string id)
        {
            // Given the file name to force download instead of opening within the browser.
            return File(Path.Combine("~/App_Data", id), OpdsHelper.DetectFileMimeType(id), Path.GetFileName(id));
        }
    }
}
