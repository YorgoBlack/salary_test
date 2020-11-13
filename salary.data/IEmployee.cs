using salary.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace salary.data
{
    public interface IEmployee
    {
        IEnumerable<EmployeeType> EmployeeTypes();
        Employee Add(Employee employee);
        void Delete(Employee employee);
        Employee AddSubordinates(Employee employee, Employee Subordinate);
        IEnumerable<Employee> GetSubordinates(Employee employee);
        IEnumerable<Employee> GetTopEmployees();
        Employee FindById(int id);
    }
}
