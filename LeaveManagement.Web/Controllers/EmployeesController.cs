using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagement.Web.UoW;
using LeaveManagement.Web.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using LeaveManagement.Web.Models;
using LeaveManagement.Web.Data;

namespace LeaveManagement.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeesController(ILogger<EmployeesController> logger, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            //var employees = await userManager.GetUsersInRoleAsync(Roles.User);
            //

            var employees = await unitOfWork.Employee.GetAllAsync();

            var model = mapper.Map<List<EmployeeListVM>>(employees);

            var modelToView = new List<EmployeeListVM>();
            foreach (var employee in employees)
            {
                if (await userManager.IsInRoleAsync(employee,Roles.User)) {
                    modelToView.Add( mapper.Map<EmployeeListVM>(employee));
                }
            }
            
            return View(modelToView);
        }

        public async Task<ActionResult> ViewAllocations(string id)
        {
            var model = await unitOfWork.LeaveAllocation.GetEmployeeAllocations(id);
            return View(model);
        }

        // GET: EmployeesController/EditAllocation/5
        public async Task<ActionResult> EditAllocation(int id)
        {
            var model = await unitOfWork.LeaveAllocation.GetEmployeeAllocation(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

         // POST: EmployeesController/EditAllocation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAllocation(int id, LeaveAllocationEditVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(await unitOfWork.LeaveAllocation.UpdateEmployeeAllocation(model))
                    {
                        unitOfWork.Save();
                        return RedirectToAction(nameof(ViewAllocations), new { id = model.EmployeeId });
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An Error Has Occurred. Please Try Again Later");
            }
            model.Employee = mapper.Map<EmployeeListVM>(await userManager.FindByIdAsync(model.EmployeeId));
            model.LeaveType = mapper.Map<LeaveTypeVM>(await unitOfWork.LeaveType.GetAsync(model.LeaveTypeId));
            return View(model);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}