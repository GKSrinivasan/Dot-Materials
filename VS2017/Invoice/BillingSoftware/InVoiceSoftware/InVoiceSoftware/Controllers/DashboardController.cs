using System.Web.Mvc;

namespace InVoiceSoftware.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}