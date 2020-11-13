using System;
using System.Collections.Generic;
using System.Text;

namespace salary.data.Models
{
    public class EmployeeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float MaxRateInc { get; set; }
        public float YearRateInc { get; set; }
        public float SubordinatesRateInc { get; set; }
        public bool IsPrimary { get; set; }
    }
}
