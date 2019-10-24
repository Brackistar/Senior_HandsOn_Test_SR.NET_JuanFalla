using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlataformaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FindEmployees()
        {
            ViewBag.DataUrl = null;
            return View();
        }

        [HttpPost]
        public ActionResult FindEmployees(string name)
        {
            ViewBag.DataUrl = null;
            if (string.IsNullOrEmpty(name))
            {

            }
        }
    }
}