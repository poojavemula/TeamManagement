using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamManagement.Entity;

namespace TeamManagement.Services
{
    public interface IEmployeeService
    {
        Task AddEmployee(Employee employee);

        Employee GetById(int employeeId);
        IEnumerable<Employee> GetAllEmployees(string empNo = null);

        Task UpdateEmployee(Employee employee);
        Task UpdateEmployeeById(int id);

        Task DeleteEmployee(int employeeId);

        IEnumerable<EmployeeRole> GetPermissions();

        IEnumerable<Employee> GetTerminatedEmployees();
        IEnumerable<string> GetManagers();
        IEnumerable<Activity> GetActivities(string empNo = null);
        Task TrackChanges(Employee current, Employee updated);
    }
}
