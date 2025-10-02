using Library_Management_System.Services;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.AuthorService;
using Libray_Managment_System.Services.Role;
using Libray_Managment_System.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using Libray_Managment_System.DIContainer;
using Libray_Managment_System.MappingProfile;

internal class Program
{
   private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // DI Container
        builder.Services.ServiceAllInjection(builder.Configuration);

        // AutoMapper 
        builder.Services.AddAutoMapper(typeof(BookProfile));

        // DatabaseInjection
        builder.Services.DataBaseInjection(builder.Configuration);

        //JWT Config
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);

        //Add Controllers
        builder.Services.AddControllers();

        // Swagger + JWT
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management System", Version = "v1" });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securitySchema);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        { securitySchema, new[] { "Bearer" } }
            });
        });

        // Authentication & Authorization
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidAudience = jwtConfig["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddAuthorization();

        // Npgsql (DbContext)
        builder.Services.AddDbContext<LibraryManagmentSystemContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Custom Services (DI)
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<IUserService, UserService>();

        var app = builder.Build();

        // Middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}