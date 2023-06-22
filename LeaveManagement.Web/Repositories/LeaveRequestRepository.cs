using AutoMapper;
using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Data;
using LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace LeaveManagement.Web.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILeaveAllocationRepository leaveAllocationRepository;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly IEmailSender emailSender;
        private readonly UserManager<IdentityUser> userManager;
        
        public LeaveRequestRepository(ApplicationDbContext context, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ILeaveAllocationRepository leaveAllocationRepository,
            AutoMapper.IConfigurationProvider configurationProvider,
            IEmailSender emailSender,
            UserManager<IdentityUser> userManager) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.leaveAllocationRepository = leaveAllocationRepository;
            this.configurationProvider = configurationProvider;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await GetAsync(leaveRequestId);
            leaveRequest.Cancelled = true;
            UpdateAsync(leaveRequest);
            await context.SaveChangesAsync();

            var user = await userManager.FindByIdAsync(leaveRequest.RequestingEmployeeId);

            // await emailSender.SendEmailAsync(user.Email, $"Leave Request Cancelled", $"Your leave request from " +
            //     $"{leaveRequest.StartDate} to {leaveRequest.EndDate} has been Cancelled Successfully.");

        }

        public async Task ChangeApprovalStatus(int leaveRequestId, bool approved)
        {
            var leaveRequest = await GetAsync(leaveRequestId);
            leaveRequest.Approved = approved;

            if (approved)
            {
                var allocation = await leaveAllocationRepository
                                .GetEmployeeAllocationWithLeaveType(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                
                int daysBetween = GetWorkingDays((DateTime)leaveRequest.StartDate, (DateTime)leaveRequest.EndDate);
                int daysRequested = daysBetween+1;
                //int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                allocation.NumberOfDays -= daysRequested;

                leaveAllocationRepository.UpdateAsync(allocation);
                await context.SaveChangesAsync();
            } 
           
            UpdateAsync(leaveRequest);
            await context.SaveChangesAsync();

            var user = await userManager.FindByIdAsync(leaveRequest.RequestingEmployeeId);
            var approvalStatus = approved ? "Approved" : "Declined";

            // await emailSender.SendEmailAsync(user.Email, $"Leave Request {approvalStatus}", $"Your leave request from " +
            //     $"{leaveRequest.StartDate} to {leaveRequest.EndDate} has been {approvalStatus}");
        }

        public async Task<bool> CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor?.HttpContext?.User);

            var leaveAllocation = await leaveAllocationRepository.GetEmployeeAllocationWithLeaveType(user.Id, model.LeaveTypeId);

            if(leaveAllocation == null)
            {
                return false;
            }

            // int daysRequested = (int)(model.EndDate.Value - model.StartDate.Value).TotalDays;
            int daysBetween = GetWorkingDays((DateTime)model.StartDate.Value, (DateTime)model.EndDate.Value);
            int daysRequested = daysBetween+1;


            if(daysRequested > leaveAllocation.NumberOfDays)
            {
                return false;
            }

            var leaveRequest = mapper.Map<LeaveRequest>(model);
            leaveRequest.DateRequested = DateTime.Now;
            leaveRequest.RequestingEmployeeId = user.Id;

            await AddAsync(leaveRequest);
            await context.SaveChangesAsync();

            // await emailSender.SendEmailAsync(user.Email, "Leave Request Submitted Successfully", $"Your leave request from " +
            //     $"{leaveRequest.StartDate} to {leaveRequest.EndDate} has been submitted for approval");

            return true;
        }

        public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
        {
            var leaveRequests = await context.LeaveRequests.Include(q => q.LeaveType).ToListAsync();
            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(q => q.Approved == true),
                PendingRequests = leaveRequests.Count(q => q.Approved == null),
                RejectedRequests = leaveRequests.Count(q => q.Approved == false),
                LeaveRequests = mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };

            foreach (var leaveRequest in model.LeaveRequests)
            {
                leaveRequest.Employee = mapper.Map<EmployeeListVM>(await userManager.FindByIdAsync(leaveRequest.RequestingEmployeeId));
                int daysBetween = GetWorkingDays((DateTime)leaveRequest.StartDate, (DateTime)leaveRequest.EndDate);
                leaveRequest.NumberOfDays = daysBetween+1;
            }

            return model;
        }

        public async Task<List<LeaveRequestVM>> GetAllAsync(string employeeId)
        {
            var leaveRequestForEmployee = await context.LeaveRequests
                                                    .Where(q => q.RequestingEmployeeId == employeeId)
                                                    .ProjectTo<LeaveRequestVM>(configurationProvider)
                                                    .ToListAsync();
            return leaveRequestForEmployee;
        }

        public async Task<LeaveRequestVM> GetLeaveRequestAsync(int? id)
        {
             var leaveRequest = await context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (leaveRequest == null)
            {
                return null;
            }

            var model = mapper.Map<LeaveRequestVM>(leaveRequest);
            model.Employee = mapper.Map<EmployeeListVM>(await userManager.FindByIdAsync(leaveRequest?.RequestingEmployeeId));
            return model;
        }

        public async Task<EmployeeLeaveRequestViewVM> GetMyLeaveDetails()
        {
            var employeeId =  userManager.GetUserId(httpContextAccessor?.HttpContext?.User);
            var allocations = (await leaveAllocationRepository.GetEmployeeAllocations(employeeId)).LeaveAllocations;
            var requests = await GetAllAsync(employeeId);

            var model = new EmployeeLeaveRequestViewVM(allocations,requests);

            return model;
           
        }

        private int GetWorkingDays(DateTime from, DateTime to)
        {
        
            var dayDifference = (int)to.Subtract(from).TotalDays;
            return Enumerable
                .Range(1, dayDifference)
                .Select(x => from.AddDays(x))
                .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }
    }

   
}