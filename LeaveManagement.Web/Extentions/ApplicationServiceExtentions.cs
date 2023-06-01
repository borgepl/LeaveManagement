using LeaveManagement.Web.Configurations;
using LeaveManagement.Web.Data;
using LeaveManagement.Web.DBInit;
using LeaveManagement.Web.UoW;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Web.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddMyAppServices(this IServiceCollection services, 
                                                                 IConfiguration config)

        {
            
            services.AddDbContext<ApplicationDbContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
           
            // email smtp to use with PaperCut Smtp on localhost
            //services.AddTransient<IEmailSender>(s => new EmailSender("localhost", 25, "no-reply@leavemanagement.com"));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDBInitializer, DBInitializer>();

            services.AddAutoMapper(typeof(MapperConfig));
            
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                        // .WithOrigins("http://localhost:4200")
                        // .WithOrigins("https://localhost:4200")
                        // .WithOrigins("https://accounts.google.com");
                });
            });

            return services;
        }
    }
}