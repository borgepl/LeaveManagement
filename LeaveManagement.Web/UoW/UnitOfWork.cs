using AutoMapper;
using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Data;
using LeaveManagement.Web.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Web.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        public ILeaveTypeRepository LeaveType { get; private set;}

        public IEmployeeRepository Employee { get; private set;}

        public ILeaveAllocationRepository LeaveAllocation { get; private set;}

         public ILeaveRequestRepository LeaveRequest{ get; private set;}

        public UnitOfWork(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IMapper mapper, 
                AutoMapper.IConfigurationProvider configurationProvider)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.mapper = mapper;
            this.configurationProvider = configurationProvider;
            this.LeaveType = new LeaveTypeRepository(dbContext);
            this.LeaveAllocation = new LeaveAllocationRepository(dbContext, userManager, this.LeaveType, this.mapper, this.configurationProvider);
            this.Employee = new EmployeeRepository(dbContext);
            //this.LeaveRequest = new LeaveRequestRepository(dbContext, this.mapper,);
        }
        

        public void Save()
        {
            dbContext.SaveChangesAsync();
        }
    }
}