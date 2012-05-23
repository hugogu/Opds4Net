using System;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Opds4Net.Model;
using Opds4Net.Server;
using Opds4Net.Util;
using Opds4Net.Web.Models;

namespace Opds4Net.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DbOpdsController : Controller
    {
        private BookDBContext db = new BookDBContext();

        /// <summary>
        /// 
        /// </summary>
        public static DBOpdsDataSource DbOpdsData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbOpdsController()
        {
            if (DbOpdsData == null)
            {
                DbOpdsData = new DBOpdsDataSource(db, MvcApplication.Container.GetExport<IOpdsItemConverter>("DataModel").Value);
            }

            DbOpdsData.DbContext = db;
        }

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
            var items = DbOpdsData.GetItems(request).Items.OrderByDescending(i => i.Value.LastUpdatedTime);
            var feed = new OpdsFeed(items.Select(i => i.Value));
            feed.SearchUri = WebRequestHelper.CurrentHostUri + "/Opds/Search/{keyword}";

            return Content(feed.ToXml(), "text/xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult Search(string keyword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            var item = DbOpdsData.GetDetail(id);
            if (item == null)
            {
                return new HttpNotFoundResult();
            }

            return Content(item.ToXml(), "text/xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Download(string id)
        {
            var book = db.Books.Find(new Guid(id));
            if (book == null)
            {
                return new HttpNotFoundResult();
            }
            var extName = OpdsHelper.GetExtensionName(book.MimeType);
            return new RangeFileResult(Request, HostingEnvironment.MapPath("~/App_Data/Uploaded/") + id + extName, book.Name + extName , book.MimeType);
        }
    }
}
