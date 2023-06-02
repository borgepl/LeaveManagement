using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagement.Web.Data;

namespace LeaveManagement.Web.Contracts
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    {
         Task LeaveAllocation(int leaveTypeId);

         Task<bool> AllocationExists(string employeeId, int leaveTypeId, int period);
    }

   
}