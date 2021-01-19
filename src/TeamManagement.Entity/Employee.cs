using System;
using System.ComponentModel.DataAnnotations;

namespace TeamManagement.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string EmployeeNo { get; set; }
        [Required,MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }

        public string Email { get; set; }
        public string PreferredContactNumber { get; set; }

        public Status EmploymentStatus { get; set; }
        public Positions Position { get; set; }
        public Departments Department { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Shifts Shift { get; set; }
        public Roles Role { get; set; }
        public string ManagerId { get; set; }
        public FavouriteColors FavouriteColor { get; set; }

        [Required,MaxLength(150)]
        public string Address { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, MaxLength(50)]
        public string PostCode { get; set; }
    }  
}
