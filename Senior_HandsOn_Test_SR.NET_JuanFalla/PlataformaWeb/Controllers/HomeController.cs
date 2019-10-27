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
        /// <summary>
        /// URL base for the data API
        /// </summary>
        private string apiURL { get; set; }

        public ActionResult Index()
        {
            return RedirectToAction("FindEmployees");
        }

        public ActionResult FindEmployees()
        {
            ViewBag.DataUrl = null;
            return View();
        }
        /// <summary>
        /// Returns a table with the employee or employees searched 
        /// </summary>
        /// <param name="name">Name of the employe to search, empty string to show all the employees</param>
        /// <returns>Partial view with the response for the requested name</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> FindEmployeesAsync(string name)
        {
            LoadUrlAPI();
            IEnumerable<EmployeeModel> employees = null;
            using (HttpClient client = new HttpClient())
            {
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
                        apiURL + "/?name=" + name
                        );
                }
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode.Equals(System.Net.HttpStatusCode.NotFound))
                        return RedirectToAction("NotFound");
                    return RedirectToAction("Error");
                }
                employees = await response.Content.ReadAsAsync<List<EmployeeModel>>();
                return PartialView(employees);
            }
        }
        /// <summary>
        /// Returns a table with the names found in the sistem that contains the searched text
        /// </summary>
        /// <param name="PartialName">Text to search</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ActionResult> GetPosibleNames(string PartialName)
        {
            LoadUrlAPI();
            IEnumerable<EmployeeModel> employees = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(
                        apiURL
                    );
                HttpResponseMessage response;
                response = await client.GetAsync(string.Empty);
                if (!response.IsSuccessStatusCode)
                    return null;

                employees = await response.Content.ReadAsAsync<List<EmployeeModel>>();
                return PartialView(employees.Where(
                    employee =>
                    employee.name.ToLowerInvariant().Contains(PartialName.ToLowerInvariant())
                    ));
            }
        }
        /// <summary>
        /// Partial viewfor errors
        /// </summary>
        /// <returns>Partial view</returns>
        public ActionResult Error()
        {
            return PartialView();
        }
        /// <summary>
        /// Partial view for content not found
        /// </summary>
        /// <returns>Partial view</returns>
        public ActionResult NotFound()
        {
            return PartialView();
        }
        /// <summary>
        /// Generate the base string for data method by API
        /// </summary>
        private void LoadUrlAPI()
        {
            if (string.IsNullOrEmpty(this.apiURL))
                this.apiURL = Url.Action("Get", "api/EmployeesAPI", null, Uri.UriSchemeHttp);
        }
    }
}