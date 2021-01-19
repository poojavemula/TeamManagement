using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeamManagement.Entity;
using TeamManagement.Models;
using TeamManagement.Services;

namespace TeamManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _hosting;

       

        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment hosting)
        {
            _employeeService = employeeService;
            _hosting = hosting;
        }
        [HttpGet]
        [HttpPost]
        public IActionResult Index(string empNo, string sortOrder)
        {
            ViewData["EmployeeNoSort"] = string.IsNullOrEmpty(sortOrder) ? "Emp_No" : "";
            ViewData["NameSort"] = sortOrder == "Name_Asc_Sort" ? "Name_Desc_Sort" : "Name_Asc_Sort";

            List<EmployeeIndexViewModel> employees;
            if(string.IsNullOrEmpty(empNo))
            {
                 employees = _employeeService.GetAllEmployees(empNo).Select(employee => new EmployeeIndexViewModel
                {
                    Id = employee.Id,
                    EmployeeNo = employee.EmployeeNo,
                    FullName = employee.FullName,
                    ImageUrl = employee.ImageUrl
                }).ToList();
                switch(sortOrder)
                {
                    case "Emp_No":
                        employees = employees.OrderByDescending(e => e.EmployeeNo).ToList();
                        break;
                    case "Name_Asc_Sort":
                        employees = employees.OrderBy(e => e.FullName).ToList();
                        break;
                    case "Name_Desc_Sort":
                        employees = employees.OrderByDescending(e => e.FullName).ToList();
                        break;
                    default:
                        employees = employees.OrderBy(e => e.EmployeeNo).ToList();
                        break;
                }
                return View(employees);
            }
            else
            {
                employees = _employeeService.GetAllEmployees(empNo).Select(employee => new EmployeeIndexViewModel
                {
                    Id = employee.Id,
                    EmployeeNo = employee.EmployeeNo,
                    FullName = employee.FullName,
                    ImageUrl = employee.ImageUrl
                }).ToList();
                return View(employees);
            }

            
        }

        [HttpGet]
        public IActionResult Create()
        {
            var managers = _employeeService.GetManagers();
            ViewBag.Managers = managers.Select(x => new SelectListItem { Value = x, Text = x });
            var model = new EmployeeCreateViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid)
            {

                var employee = new Employee
                {
                    Id = model.Id,
                    EmployeeNo = model.EmployeeNo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    FullName = model.FullName,
                    Email = model.Email,
                    PreferredContactNumber = model.PreferredContactNumber,
                    EmploymentStatus = model.EmploymentStatus,
                    Position = model.Position,
                    Department = model.Department,
                    StartDate = model.StartDate,
                    Shift = model.Shift,
                    FavouriteColor = model.FavouriteColor,
                    Address = model.Address,
                    City = model.City,
                    PostCode = model.PostCode,
                    Role= model.Role
                };
                if(model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webRootPath = _hosting.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("yyyymmssfff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;
                }
                await _employeeService.AddEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetById(id);
            var managers = _employeeService.GetManagers();
            ViewBag.Managers = managers.Select(x=> new SelectListItem {Value = x, Text= x });
            if(employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeUpdateViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Email = employee.Email,
                PreferredContactNumber = employee.PreferredContactNumber,
                EmploymentStatus = employee.EmploymentStatus,
                Position = employee.Position,
                Department = employee.Department,
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                Shift = employee.Shift,
                FavouriteColor = employee.FavouriteColor,
                Address = employee.Address,
                City = employee.City,
                PostCode = employee.PostCode,
                Role = employee.Role,
                ManagerId = employee.ManagerId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeUpdateViewModel model)
        {
            if(ModelState.IsValid)
            {
                var employee = _employeeService.GetById(model.Id);
                var current = JsonConvert.DeserializeObject<Employee>(JsonConvert.SerializeObject(employee));
                
                if (employee == null)
                {
                    return NotFound();
                }
                employee.EmployeeNo = model.EmployeeNo;
                employee.FirstName = model.FirstName;
                employee.MiddleName = model.MiddleName;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                employee.PreferredContactNumber = model.PreferredContactNumber;
                employee.EmploymentStatus = model.EmploymentStatus;
                employee.Position = model.Position;
                employee.Department = model.Department;
                employee.StartDate = model.StartDate;
                employee.EndDate = model.EndDate;
                employee.Shift = model.Shift;
                employee.FavouriteColor = model.FavouriteColor;
                employee.Address = model.Address;
                employee.City = model.City;
                employee.PostCode = model.PostCode;
                employee.ManagerId = model.ManagerId;
                employee.Role = model.Role;
                if(model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webRootPath = _hosting.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("yyyymmssfff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;
                }
                await _employeeService.UpdateEmployee(employee);
                await _employeeService.TrackChanges(current, employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var employee = _employeeService.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }
            EmployeeDetailViewModel model = new EmployeeDetailViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
                ImageUrl = employee.ImageUrl,
                Email = employee.Email,
                PreferredContactNumber = employee.PreferredContactNumber,
                EmploymentStatus = employee.EmploymentStatus,
                Position = employee.Position,
                Department = employee.Department,
                Shift = employee.Shift,
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                FavouriteColor = employee.FavouriteColor,
                Address = employee.Address,
                City = employee.City,
                PostCode = employee.PostCode,
                ManagerId = employee.ManagerId
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _employeeService.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeDeleteViewModel()
            {
                Id = employee.Id,
                FullName = employee.FullName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeDeleteViewModel model)
        {
            await _employeeService.DeleteEmployee(model.Id);
            return RedirectToAction(nameof(Index));
        }


    }
}
