using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamManagement.Entity;
using TeamManagement.Persistence;

namespace TeamManagement.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _dbContext;

        public EmployeeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEmployee(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }


        public IEnumerable<Employee> GetAllEmployees(string empNo = null)
        {
            if(string.IsNullOrEmpty(empNo))
            {
                return _dbContext.Employees;
            }
            else
            {
                return _dbContext.Employees.Where(e => e.EmployeeNo.StartsWith(empNo));
            }
            
        }

        public Employee GetById(int employeeId)
        {
            return _dbContext.Employees.Where(e => e.Id == employeeId).FirstOrDefault();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            _dbContext.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEmployeeById(int id)
        {
            var employee = GetById(id);
            _dbContext.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var employee = GetById(employeeId);
            _dbContext.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<EmployeeRole> GetPermissions()
        {
            return _dbContext.Employees.Select(x => new EmployeeRole {Id=x.Id, EmployeeNo = x.EmployeeNo, FullName = x.FullName, Role = x.Role.ToString() });

        }

        public IEnumerable<Employee> GetTerminatedEmployees()
        {
            return _dbContext.Employees.Where(e => e.EmploymentStatus == Status.Terminated);
        }

        public IEnumerable<Activity> GetActivities(string empNo = null)
        {
            if (string.IsNullOrEmpty(empNo))
            {
                return _dbContext.Activities;
            }
            else
            {
                return _dbContext.Activities.Where(e => e.EmployeeNo.StartsWith(empNo));
            }
        }


        public IEnumerable<string> GetManagers()
        {
            var nonManagerPositions = new List<Positions> { Positions.SoftwareEngineerI, Positions.SoftwareEngineerII, Positions.QAEngineer };
            return _dbContext.Employees.Where(e => !nonManagerPositions.Contains(e.Position)).Select(x => x.EmployeeNo);
            
        }

        public async Task TrackChanges(Employee current, Employee updated)
        {
            if(current.Position != updated.Position)
            {
                await _dbContext.Activities.AddAsync(new Activity()
                {
                    EmployeeNo = current.EmployeeNo,
                    PropertyName = "Position",
                    PreviousValue = current.Position.ToString(),
                    NewValue = updated.Position.ToString(),
                    ModifiedDateTime = DateTime.Now
                });
            }

            if (current.Role!= updated.Role)
            {
                await _dbContext.Activities.AddAsync(new Activity()
                {
                    EmployeeNo = current.EmployeeNo,
                    PropertyName = "Role",
                    PreviousValue = current.Role.ToString(),
                    NewValue = updated.Role.ToString(),
                    ModifiedDateTime = DateTime.Now
                });
            }

            if (current.ManagerId != updated.ManagerId)
            {
                await _dbContext.Activities.AddAsync(new Activity()
                {
                    EmployeeNo = current.EmployeeNo,
                    PropertyName = "ManagerId",
                    PreviousValue = current.ManagerId,
                    NewValue = updated.ManagerId,
                    ModifiedDateTime = DateTime.Now
                });
            }
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }



    }
}
