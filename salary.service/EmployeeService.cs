using salary.data;
using salary.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace salary.Service
{
    public class EmployeeService : IEmployee
    {
        readonly List<EmployeeType> employeeTypes;
        readonly CompanyStorage _companyStorage;

        public EmployeeService(CompanyStorage companyStorage)
        {
            _companyStorage = companyStorage;
            employeeTypes = new List<EmployeeType>()
            {
                new EmployeeType() {
                    Id = 1,
                    Name = "Employee",
                    IsPrimary = true,
                    MaxRateInc = 0.3f,
                    YearRateInc = .03f,
                    SubordinatesRateInc = 0,
                },
                new EmployeeType() {
                    Id = 2,
                    Name = "Manager",
                    IsPrimary = false,
                    MaxRateInc = 0.4f,
                    YearRateInc = .05f,
                    SubordinatesRateInc = .005f
                },
                new EmployeeType() {
                    Id = 3,
                    Name = "Sales",
                    IsPrimary = false,
                    MaxRateInc = 0.35f,
                    YearRateInc = .01f,
                    SubordinatesRateInc = .003f
                }
            };
        }

        public Employee Add(Employee employee)
        {
            if (employee == null) return null;
            employee.EmployeeType = employeeTypes
                .FirstOrDefault(x => x.Id == (employee.EmployeeType == null ? -1 : employee.EmployeeType.Id) );
            if (employee.EmployeeType == null) return null;
            
            employee.BaseRate = 1000;
            return _companyStorage.Add(employee);
        }

        public Employee AddSubordinates(Employee employee, Employee child)
        {
            var parent = _companyStorage.GetEmployeeById(employee == null ? -1 : employee.Id);
            if (parent == null) return null;
            if (parent.EmployeeType.IsPrimary) return null;

            child.EmployeeType = employeeTypes
                .FirstOrDefault(x => x.Id == (employee.EmployeeType == null ? -1 : employee.EmployeeType.Id));
            if (child.EmployeeType == null) return null;
            
            employee.BaseRate = 1000;
            return _companyStorage.AddChild(employee, child);
        }

        public void Delete(Employee employee)
        {
            _companyStorage.Delete(employee);
        }

        public IEnumerable<EmployeeType> EmployeeTypes()
        {
            return employeeTypes;
        }

        public IEnumerable<Employee> GetSubordinates(Employee employee)
        {
            return _companyStorage.GetChildren(employee);
        }

        public IEnumerable<Employee> GetTopEmployees()
        {
            return _companyStorage.GetTop();
        }

        public Employee FindById(int id)
        {
            return _companyStorage.GetEmployeeById(id);
        }
    }
}
