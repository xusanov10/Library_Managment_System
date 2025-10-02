using Library_Management_System.Services;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.AuthorService;
using Libray_Managment_System.Services.Book;
using Libray_Managment_System.Services.Category;
using Libray_Managment_System.Services.ReportService;
using Libray_Managment_System.Services.Role;
using Libray_Managment_System.Services.User;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.DIContainer;

public static class ServiceInjection
{
    public static IServiceCollection
        ServiceAllInjection(this IServiceCollection service , IConfiguration configuration)
    {

        // Token Service 
        service.AddScoped<ITokenService, TokenService>();
        // User Service
        service.AddScoped<IUserService , UserService>();
        //
        service.AddScoped<IRoleService, RoleService>();
        //
        service.AddScoped<IAuthorService, AuthorService>();
        // Book Service
        service.AddScoped<IBookService , BookService>();
        // Category Service
        service.AddScoped<ICategoryService, CategoryService>();
        // Report Service
        service.AddScoped<IReportService, ReportService>();

        return service;
    }

    public static IServiceCollection
        DataBaseInjection(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<LibraryManagmentSystemContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        return service;
    }
}