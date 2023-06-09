using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Web.Models
{
    public class AdminLeaveRequestViewVM
    {
        [Display(Name = "Total Number Of Requests")]
        public int TotalRequests { get; set; }
        
        [Display(Name = "Approved Requests")]
        public int ApprovedRequests { get; set; }
        
        [Display(Name = "Pending Requests")]
        public int PendingRequests { get; set; }
        
        [Display(Name = "Rejected Requests")]
        public int RejectedRequests { get; set; }

        [Display(Name = "Days")]
        public int Days { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }
    }
}