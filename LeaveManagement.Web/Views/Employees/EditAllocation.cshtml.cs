using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LeaveManagement.Web.Views.Employees
{
    public class EditAllocation : PageModel
    {
        private readonly ILogger<EditAllocation> _logger;

        public EditAllocation(ILogger<EditAllocation> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}