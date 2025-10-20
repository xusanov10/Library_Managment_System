using LibraryMS.Application.Services;
using Libray_Managment_System.Services.Auth;
using Libray_Managment_System.Services.Role;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
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
    }

    private static void RegisterMapper(this IServiceCollection services)
    {
        services.AddMapster();
    }
}
