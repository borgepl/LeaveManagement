using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Data;
using LeaveManagement.Web.Repositories;

namespace LeaveManagement.Web.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        public ILeaveTypeRepository LeaveType { get; private set;}

        public IEmployeeRepository Employee { get; private set;}

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.LeaveType = new LeaveTypeRepository(dbContext);
            this.Employee = new EmployeeRepository(dbContext);
        }
        

        public void Save()
        {
            dbContext.SaveChangesAsync();
        }
    }
}