using Library_Managment_System1;
using LibraryMS.DataAccess.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options; 

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryManagmentSystemContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<JwtSettings>(options =>
                   configuration.GetSection("JwtSettings")
                                .Bind(options));

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
