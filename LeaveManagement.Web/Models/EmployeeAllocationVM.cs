using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Web.Models
{
    public class EmployeeAllocationVM : EmployeeListVM
    {
       public List<LeaveAllocationVM> LeaveAllocations { get; set; }
    }
}