using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using salary.data;
using salary.data.Models;
using salary.Service;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Salary_NUnitTest
{
    public class SalaryTest
    {
        ServiceProvider sp;
        IEmployee company;
        ISalary salary;

        public SalaryTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<CompanyStorage, CompanyStorage>();
            services.AddScoped<IEmployee, EmployeeService>();
            services.AddScoped<ISalary, SalaryService>();
            sp = services.BuildServiceProvider();
        }

        [SetUp]
        public void Setup()
        {
            company = sp.GetService<IEmployee>();
            salary = sp.GetService<ISalary>();
        }

        [Test]
        public void SalaryCalc()
        {
            foreach(var e in company.GetTopEmployees() )
            {
                var s1 = salary.CalcSalary(e.Id);
                System.Diagnostics.Debug.WriteLine(e.Fio + " " + salary.CalcSalary(e.Id));
            }
        }

        [Test]
        public void CompanyCreate()
        {
            _CompanyCreate();
        }
    
        void _CompanyCreate()
        {
            var empTypes = company.EmployeeTypes();
            var rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                var tp = empTypes.ElementAt(rnd.Next(3));
                var s0 = company.Add(
                    new Employee()
                    {
                        Fio = tp.Name + "_" + (i + 1),
                        EmployeeType = tp,
                        HiringTime = DateTime.Now.AddMonths(-rnd.Next(10, 500))
                    });

                Assert.NotNull(s0, "Error create user");

                for (int j = 0; j < 10; j++)
                {
                    if (s0.EmployeeType.IsPrimary) continue;
                    tp = empTypes.ElementAt(rnd.Next(3));
                    var s1 = company.AddSubordinates(s0,
                        new Employee()
                        {
                            Fio = tp.Name + "_" + (j + 1),
                            EmployeeType = tp,
                            HiringTime = DateTime.Now.AddMonths(-rnd.Next(10, 500))
                        });
                    Assert.NotNull(s1, "Error create user");

                    for (int k = 0; k < 5; k++)
                    {
                        if (s1.EmployeeType.IsPrimary) continue;
                        tp = empTypes.ElementAt(rnd.Next(3));
                        var s2 = company.AddSubordinates(s1,
                            new Employee()
                            {
                                Fio = tp.Name + "_" + (k + 1),
                                EmployeeType = tp,
                                HiringTime = DateTime.Now.AddMonths(-rnd.Next(10, 500))
                            });

                        Assert.NotNull(s2, "Error create user");
                    }
                }

            }

        }
    }
}