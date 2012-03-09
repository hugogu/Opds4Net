using System.Text;
using System.Web;
using System.Web.Mvc;
using Opds4Net.Model;

namespace Opds4Net.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CategoryController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Category(string id)
        {
            var items = (HttpContext.ApplicationInstance as MvcApplication).FileSystemOpds.GetItems(id);
            var feed = new OpdsFeed(items);

            return new ContentResult {
                ContentType = "text/xml",
                Content = feed.ToXml(),
                ContentEncoding = Encoding.UTF8
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            return new ContentResult
            {
                ContentType = "text/xml",
                Content = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><a></a>",
                ContentEncoding = Encoding.UTF8
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Download(string id)
        {
            return new FilePathResult("", "");
        }
    }
}
