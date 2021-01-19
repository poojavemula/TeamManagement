using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamManagement.Entity;

namespace TeamManagement.Models
{
    public class EmployeeCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Employee Number is required"),
            RegularExpression(@"^[A-Z]{3,3}[0-9]{3}$")]
        public string EmployeeNo { get; set; }

        [Required(ErrorMessage ="First Name is Required"),StringLength(50,MinimumLength =2),
            RegularExpression(@"^[A-Z][a-zA-Z""'\s-]*$"),Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50), Display(Name ="Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is Required"), StringLength(50, MinimumLength = 2),
            RegularExpression(@"^[A-Z][a-zA-Z""'\s-]*$"), Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName {
            get {
                return FirstName + (string.IsNullOrEmpty(MiddleName) ? " " : (" " + (char?)MiddleName[0] + ".").ToUpper()) + LastName;
            } 
        }

        [Display(Name ="Photo")]
        public IFormFile ImageUrl { get; set; }

        [DataType(DataType.EmailAddress),Display(Name ="Email Id")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber),Display(Name ="Mobile Number")]
        public string PreferredContactNumber { get; set; }

        [Display(Name ="EMployment Status")]
        public Status EmploymentStatus { get; set; }

        [Required(ErrorMessage ="Job Role is Required")]
        public Positions Position { get; set; }

        public Departments Department { get; set; }

        [DataType(DataType.Date),Display(Name ="Date Joined")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage ="Enter Employee Role")]
        public Roles Role { get; set; }

        public Shifts Shift { get; set; }

        [Display(Name ="Favourite Color")]
        public FavouriteColors FavouriteColor { get; set; }

        [Required, StringLength(150)]
        public string Address { get; set; }
        [Required, StringLength(50)]
        public string City { get; set; }
        [Required, StringLength(50),Display(Name ="Post Code")]
        public string PostCode { get; set; }
        [Required]
        public string ManagerId { get; set; }
    }
}
