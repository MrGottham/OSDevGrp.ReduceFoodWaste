using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = string.Format("{0} {1}", Texts.WelcomeTo, Texts.ReduceFoodWasteProject);

            return View();
        }
    }
}
