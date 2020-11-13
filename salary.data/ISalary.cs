using salary.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace salary.data
{
    public interface ISalary
    {
        float CalcSalary(int employeeId);
        float CalcSalaryTotal();
    }
}
