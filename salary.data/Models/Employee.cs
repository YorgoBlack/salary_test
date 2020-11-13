using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace salary.data.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public DateTime HiringTime { get; set; }
        public float BaseRate { get; set; }
        public virtual Employee Chief { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
    }
}
