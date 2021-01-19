using Microsoft.EntityFrameworkCore;
using System;
using TeamManagement.Entity;

namespace TeamManagement.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeRelationships> Relationships { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
