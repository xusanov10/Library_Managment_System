using LibraryMS.Application.Seeders;
using LibraryMS.Application.Services;
using Libray_Managment_System.Services.Auth;
using Libray_Managment_System.Services.Role;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryMS.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.RegisterMapper();

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBorrowService, BorrowService>();
            services.AddScoped<PermissionSeeder>();
        }

        private static void RegisterMapper(this IServiceCollection services)
        {
            services.AddMapster();
        }
    }
}
