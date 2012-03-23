using System.IO;
using System.Linq;
using System.Web.Mvc;
using Opds4Net.Model;
using Opds4Net.Server;
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
            var request = new DataItemsRequest()
            {
                Id = id,
                PageIndex = 1,
                PageSize = 10,
            };
            var items = MvcApplication.Current.FileSystemOpds.GetItems(request).Items.OrderByDescending(i => i.LastUpdatedTime);
            var feed = new OpdsFeed(items);

            return Content(feed.ToXml(), "text/xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            var item = MvcApplication.Current.FileSystemOpds.GetDetail(id);

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
