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

    public class EmployeesController : ApiController
    {
        private DataAccess data = new DataAccess();
        public IEnumerable<EmployeeModel> Get()
        {

            return data.Employees;

        }
        //public IHttpActionResult GetAllEmployees()
        //{
        //    using (DataAccess data = new DataAccess())
        //    {
        //        IEnumerable<EmployeeModel> Employees = data.Employees;

        //        if (Employees != null)
        //            return Ok(Employees);

        //        return NotFound();
        //    }
        //}

        public IHttpActionResult GetEmployee(string name)
        {

            EmployeeModel employee = data.Employees.Where(
                Employee =>
                Employee.name.Equals(name)
                ).FirstOrDefault();
            if (employee != null)
                return Ok(employee);

            return NotFound();

        }
    }
}
