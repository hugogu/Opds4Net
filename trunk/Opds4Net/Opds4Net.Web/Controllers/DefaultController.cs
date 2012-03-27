using System.Web.Mvc;
using System.Web.Security;
using Opds4Net.Web.Models;

namespace Opds4Net.Web.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {
            return View();
        }
    }
}
