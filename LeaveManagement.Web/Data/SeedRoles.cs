using LeaveManagement.Web.UoW;
using LeaveManagement.Web.Constants;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Web.Data
{
    public class SeedRoles
    {
         public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            if (!roleManager.RoleExistsAsync(Roles.Administrator).GetAwaiter().GetResult()) {

                roleManager.CreateAsync( new IdentityRole(Roles.Administrator)).GetAwaiter().GetResult();
                roleManager.CreateAsync( new IdentityRole(Roles.User)).GetAwaiter().GetResult();

                // if roles are not created, then we craate admin user as well

                var adminEmail = "admin@localhost.com";
                await userManager.CreateAsync(new Employee {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Firstname = "Me",
                    Lastname = "Admin",
                    PhoneNumber = "123456789",
                    DateOfBirth = Convert.ToDateTime("1988/12/20"),
                    DateJoined = DateTime.Now,
                    EmailConfirmed = true
                 }, "Admin123!");

                 var userEmail = "user@localhost.com";
                  await userManager.CreateAsync(new Employee {
                    UserName = userEmail,
                    Email = userEmail,
                    Firstname = "Me",
                    Lastname = "User",
                    PhoneNumber = "123456789",
                    DateOfBirth = Convert.ToDateTime("1988/12/20"),
                    DateJoined = DateTime.Now
                 }, "User123!");

                Employee adminUser = await unitOfWork.Employee.GetAsync(u => u.Email == adminEmail);
                await userManager.AddToRoleAsync(adminUser, Roles.Administrator);

                Employee user = await unitOfWork.Employee.GetAsync(u => u.Email == userEmail);
                await userManager.AddToRoleAsync(user, Roles.User);

            }

        }
    }
}