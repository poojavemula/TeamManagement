using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamManagement.Entity;

namespace TeamManagement.Models
{
    public class EmployeeDetailViewModel
    {
        public int Id { get; set; }

        public string EmployeeNo { get; set; }

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

        public FavouriteColors FavouriteColor { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string ManagerId { get; set; }
    }
}
