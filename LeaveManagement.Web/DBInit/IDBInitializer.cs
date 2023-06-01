using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Web.DBInit
{
    public interface IDBInitializer
    {
        Task InitializeAsync();
    }
}