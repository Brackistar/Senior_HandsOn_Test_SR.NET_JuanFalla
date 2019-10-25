using PlataformaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PlataformaWeb.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("FindEmployees");
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
        public async System.Threading.Tasks.Task<ActionResult> FindEmployeesAsync(string name)
        {
            IEnumerable<EmployeeModel> employees = null;
            using (HttpClient client = new HttpClient())
            {
                string apiURL = Url.Action("Get", "api/EmployeesAPI",null,Request.Url.Scheme);
                client.BaseAddress = new Uri(
                        apiURL
                    );
                HttpResponseMessage response;
                if (string.IsNullOrEmpty(name))
                {
                    response = await client.GetAsync(string.Empty);

                }
                else
                {
                    response = await client.GetAsync(
                        apiURL+"/?name="+name
                        );
                }
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Error");
                }
                employees = await response.Content.ReadAsAsync<List<EmployeeModel>>();
                return PartialView(employees);
            }
        }

        [ChildActionOnly]
        public ActionResult Error()
        {
            return PartialView();
        }
    }
}