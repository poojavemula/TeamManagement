using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamManagement.Entity;

namespace TeamManagement.Persistence
{
    public class DataSeedingInitializer
    {
        private readonly AppDbContext _dbContext;
        private readonly IHostEnvironment _hostEnvironment;

        public DataSeedingInitializer(AppDbContext dbContext, IHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }

        public async Task SeedAsync()
        {
            
            if (!_dbContext.Employees.Any())
            {
                //need to create sample data
                var filePath = Path.Combine(_hostEnvironment.ContentRootPath, "SeedData/employees.json");
                var json = File.ReadAllText(filePath);
                var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(json);
                _dbContext.Employees.AddRange(employees);

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
