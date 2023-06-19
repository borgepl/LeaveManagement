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

        public ILeaveTypeRepository LeaveType { get; private set;}

        public IEmployeeRepository Employee { get; private set;}

        public ILeaveAllocationRepository LeaveAllocation { get; private set;}

        public UnitOfWork(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.mapper = mapper;
            this.LeaveType = new LeaveTypeRepository(dbContext);
            this.LeaveAllocation = new LeaveAllocationRepository(dbContext, userManager, this.LeaveType, this.mapper);
            this.Employee = new EmployeeRepository(dbContext);
        }
        

        public void Save()
        {
            dbContext.SaveChangesAsync();
        }
    }
}