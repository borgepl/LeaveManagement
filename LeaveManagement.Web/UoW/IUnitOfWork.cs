using LeaveManagement.Web.Contracts;

namespace LeaveManagement.Web.UoW
{
    public interface IUnitOfWork
    {
        ILeaveTypeRepository LeaveType { get; }

        void Save();
    }
}