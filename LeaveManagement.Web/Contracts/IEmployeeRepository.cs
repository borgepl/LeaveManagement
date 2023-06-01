using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagement.Web.Data;

namespace LeaveManagement.Web.Contracts
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        
    }
}