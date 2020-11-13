using salary.data;
using salary.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace salary.Service
{
    public class SalaryService : ISalary
    {
        readonly IEmployee _employeeService;
        public SalaryService(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        public float CalcSalary(int employeeId)
        {
            float sum = 0;
            var emp = _employeeService.FindById(employeeId);
            if( emp != null )
            {
                int years = DateTime.Now.Year - emp.HiringTime.Year;
                years = years < 0 ? 0 : years;

                float add = emp.EmployeeType.YearRateInc * years;
                add = add > emp.EmployeeType.MaxRateInc ? emp.EmployeeType.MaxRateInc : add;
                sum += emp.BaseRate + add * emp.BaseRate;
                var subordinates = _employeeService.GetSubordinates(emp);
                if( subordinates != null )
                {
                    foreach (var n in  subordinates)
                    {
                        sum += emp.EmployeeType.SubordinatesRateInc * CalcSalary(n.Id);
                    }
                }
            }
            return sum;
        }

        public float CalcSalaryTotal()
        {
            float sum = 0;
            foreach(var n in _employeeService.GetTopEmployees() )
            {
                sum += CalcSalary(n.Id);
            }
            return sum;
        }
    }
}
