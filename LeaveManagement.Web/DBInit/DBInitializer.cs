using LeaveManagement.Web.Data;
using LeaveManagement.Web.DBInit;
using LeaveManagement.Web.UoW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Web.DBInit
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer( UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext, IUnitOfWork unitOfWork,
            ILogger<DBInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            // migrations if they are not yet applied
            try
            {
                if(_dbContext.Database.GetPendingMigrations().Count() > 0) {
                    await _dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex) {
                    _logger.LogError(ex, "An error occured during migrations");

            }

            // create roles if they are not created  
            await SeedRoles.SeedRolesAsync(_roleManager, _userManager, _unitOfWork);
            
        }
    }
}