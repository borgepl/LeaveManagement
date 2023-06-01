using LeaveManagement.Web.Contracts;

namespace LeaveManagement.Web.UoW
{
    public interface IUnitOfWork
    {
        ILeaveTypeRepository LeaveType { get; }
        IEmployeeRepository Employee { get; }

        void Save();
    }
}