using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using PlataformaWeb.Models;

namespace PlataformaWeb.Controllers
{

    public class EmployeesAPIController : ApiController
    {
        private DataAccess data = new DataAccess();
        /// <summary>
        /// Gets a list of all employees
        /// </summary>
        /// <returns>Result of the search and if success list of all employees</returns>
        public IHttpActionResult Get()
        {
            List<EmployeeModel> employees = data.Employees;
            if (employees == null || employees.Count == 0)
                return NotFound();

            return Ok(employees.AsEnumerable());

        }
        /// <summary>
        /// Returns list of employees with a name equal to se searched name
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <returns>Result of the search and if success list of the employees found</returns>
        public IHttpActionResult Get(string name)
        {
            List<EmployeeModel> employees = data.Employees.Where(
                Employee =>
                Employee.name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
            if (employees == null)
                return NotFound();


            return Ok(employees.AsEnumerable());

        }
    }
}
