using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TeamManagement.Entity
{
    public class Activity
    {
        public int Id { get; set; }
        public string EmployeeNo { get; set; }
        public string PropertyName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
