using LeaveManagement.Web.Contracts;

namespace LeaveManagement.Web.UoW
{
    public interface IUnitOfWork
    {
        ILeaveTypeRepository LeaveType { get; }

        ILeaveAllocationRepository LeaveAllocation { get; }
        IEmployeeRepository Employee { get; }

        void Save();
    }
}