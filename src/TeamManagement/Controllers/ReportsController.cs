using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamManagement.Models;
using TeamManagement.Services;

namespace TeamManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _hosting;

        public ReportsController(IEmployeeService employeeService, IWebHostEnvironment hosting)
        {
            _employeeService = employeeService;
            _hosting = hosting;
        }

        [HttpGet]
        public IActionResult Permissions()
        {
            var employees = _employeeService.GetPermissions().Select(employee => new EmployeeRoleViewModel
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                Name = employee.FullName,
                Role = employee.Role
            }).ToList();

            return View(employees);

        }

        [HttpGet]
        public IActionResult Weekly()
        {

            var employeeDates = _employeeService.GetAllEmployees()
                .Select(x=> new { HireDate = StartOfWeek(x.StartDate, DayOfWeek.Monday) })
                .GroupBy(y=>y.HireDate).Select(z=>new { HireWeek = z.Key, HireCount = z.Count() }).ToList();

            var last24Weeks = new List<DateTime>();
            var now = DateTime.Now;
            for (int i = 0; i < 24; i++)
            {
                last24Weeks.Add(StartOfWeek(now.AddDays(-7 * i), DayOfWeek.Monday));
            }
            var weeks = last24Weeks.Select(x => new { WeekDate = x });
            var finalData = from week in weeks
                            join empWeeks in employeeDates on week.WeekDate equals empWeeks.HireWeek into gj
                            from subweek in gj.DefaultIfEmpty()
                            select new WeeklyHiresViewModel { WeekStartDate = week.WeekDate, HiredNumber = subweek?.HireCount ?? 0 };

            return View(finalData);

        }


        [HttpGet]
        public IActionResult Terminated()
        {
            var employees = _employeeService.GetTerminatedEmployees()
                .GroupBy(x => x.EndDate.Year)
                .Select(y => new TerminatedEmployeesViewModel() { TerminatedYear = y.Key, Total = y.Count() });

            return View(employees);

        }


        [HttpGet]
        [HttpPost]
        public IActionResult ActivityReport(string empNo = null)
        {
            IEnumerable<ActivityDetailsViewModel> data = new List<ActivityDetailsViewModel>();
            if (empNo == null)
            {
                data = _employeeService.GetActivities()
                    .OrderByDescending(x => x.ModifiedDateTime)
                    .Select(y => new ActivityDetailsViewModel()
                    {
                        EmployeeNo = y.EmployeeNo,
                        PropertyName = y.PropertyName,
                        PreviousValue = y.PreviousValue,
                        NewValue = y.NewValue,
                        ModifiedDateTime = y.ModifiedDateTime
                    }); ;
            }
            else
            {
                data = _employeeService.GetActivities()
                    .Where(x => x.EmployeeNo.IndexOf(empNo) >= 0)
                    .OrderByDescending(x => x.ModifiedDateTime)
                    .Select(y=>new ActivityDetailsViewModel()
                    {
                        EmployeeNo = y.EmployeeNo,
                        PropertyName = y.PropertyName,
                        PreviousValue = y.PreviousValue,
                        NewValue = y.NewValue,
                        ModifiedDateTime = y.ModifiedDateTime
                    });
            }
            ViewData["empNo"] = empNo;

            return View(data);

        }


        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
