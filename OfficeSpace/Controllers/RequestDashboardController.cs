using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeSpace.Controllers
{
    public class RequestDashboardController : Controller
    {
        // GET: RequestDashboard
        public ActionResult Index()
        {
            return View("RequestIndex");
        }
    }
}