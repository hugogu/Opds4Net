using System;
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
                OrderDirection = true,
                OrderBy = "LastUpdatedTime",
            };
            var feed = new OpdsFeed(MvcApplication.Current.OpdsDataConverter.GetItems(request.Process()).Items.Select(i => i.Value));
            feed.SearchUri = WebRequestHelper.CurrentHostUri + "/FS/Search/{keyword}";

            return Content(feed.ToXml(), "text/xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Search(string id)
        {
            var request = new FSSearchRequest(MvcApplication.Current.FileSystemBookFolder)
            {
                PageIndex = 1,
                PageSize = 10,
                OrderDirection = true,
                OrderBy = "LastUpdatedTime",
                KeyWord = String.Format("*{0}*", id),
            };
            var feed = new OpdsFeed(MvcApplication.Current.OpdsDataConverter.GetItems(request.Process()).Items.Select(i => i.Value));

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
            var item = MvcApplication.Current.OpdsDataConverter.GetItems(request.Process()).Items.Single();

            return Content(item.Value.ToXml(), "text/xml");
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
