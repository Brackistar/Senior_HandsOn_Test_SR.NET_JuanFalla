using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using PlataformaWeb;

namespace PlataformaWeb.Models
{
    public class EmployeeModel
    {
        [Display(Name ="Código")]
        public int id { get; set; }
        [Display(Name = "Nombre")]
        public string name { get; set; }
        [Display(Name = "Tipo Contrato")]
        public string contractTypeName { get; set; }
        public int roleId { get; set; }
        [Display(Name = "Rol")]
        public string roleName { get; set; }
        [Display(Name = "Descripción del Rol")]
        public string roleDescription { get; set; }
        public double hourlySalary { get; set; }
        public double monthlySalary { get; set; }
        [Display(Name = "Salario Anual")]
        [DataType(DataType.Currency)]
        public double annualSalary { get; set; }
        public EmployeeModel(string name, double hourlySalary, double monthlySalary)
        {
            this.name = name;
            this.hourlySalary = hourlySalary;
            this.monthlySalary = monthlySalary;
            this.annualSalary = GetAnnualSalary();
        }
        protected virtual double GetAnnualSalary()
        {
            return 0;
        }
    }

    public class MonthlySalaryEmployee : EmployeeModel
    {
        public MonthlySalaryEmployee(string name, double hourlySalary, double monthlySalary)
            : base(name, hourlySalary, monthlySalary)
        {
        }
        protected override double GetAnnualSalary()
        {
            return monthlySalary * 12;
        }
    }

    public class WeeklySalaryEmployee : EmployeeModel
    {
        public WeeklySalaryEmployee(string name, double hourlySalary, double monthlySalary)
            : base(name, hourlySalary, monthlySalary)
        {
        }
        protected override double GetAnnualSalary()
        {
            return hourlySalary * 120 * 12;
        }
    }

    public class DataAccess : IDisposable
    {
        public List<EmployeeModel> Employees { get; set; }

        public DataAccess()
        {
            using (WebClient content = new WebClient())
            {
                Employees = new List<EmployeeModel>();
                IEnumerable<EmployeeModel> jsonEmployees = Newtonsoft
                    .Json
                    .JsonConvert
                    .DeserializeObject<IEnumerable<EmployeeModel>>(
                    content.DownloadString(
                        ConfigurationManager.AppSettings["APIUrl"])
                    );
                foreach (EmployeeModel employee in jsonEmployees)
                {
                    EmployeeModel newEmployee = null;
                    switch (employee.contractTypeName)
                    {
                        case "HourlySalaryEmployee":
                            newEmployee = new WeeklySalaryEmployee(
                                name: employee.name,
                                hourlySalary: employee.hourlySalary,
                                monthlySalary: employee.monthlySalary
                                )
                            {
                                id = employee.id,
                                roleId = employee.roleId,
                                roleDescription = employee.roleDescription,
                                roleName = employee.roleName,
                                contractTypeName = employee.contractTypeName
                            };
                            break;
                        case "MonthlySalaryEmployee":

                            newEmployee = new MonthlySalaryEmployee(
                                name: employee.name,
                                hourlySalary: employee.hourlySalary,
                                monthlySalary: employee.monthlySalary
                                )
                            {
                                id = employee.id,
                                roleId = employee.roleId,
                                roleDescription = employee.roleDescription,
                                roleName = employee.roleName,
                                contractTypeName = employee.contractTypeName
                            };
                            break;
                        default:
                            Employees.Add(
                                employee
                                );
                            break;
                    }
                    Employees.Add(
                                newEmployee
                                );
                }

            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~DataAccess()
        // {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}