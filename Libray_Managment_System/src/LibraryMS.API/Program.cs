using Library_Management_System.Services;
using Library_Managment_System;
using Library_Managment_System.Services;
using Libray_Managment_System.Data;
using Libray_Managment_System.Services.Auth;
using Libray_Managment_System.Services.Role;
using Libray_Managment_System.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IFileStorageService, MinioFileStorageService>();



        builder.Services.AddCors(options =>
        {
            // "CorsPolicy" nomli yangi policy yaratamiz
            options.AddPolicy("CorsPolicy", policy =>
            {
                // AllowAnyOrigin() - BARCHA domenlardan so'rovlarga ruxsat beradi
                // ⚠️ DIQQAT: Production muhitida bu xavfli!
                // Production uchun aniq domenlarni ko'rsating:
                // policy.WithOrigins("https://myapp.com", "https://admin.myapp.com")
                policy.AllowAnyOrigin()

                // AllowAnyMethod() - barcha HTTP metodlarga ruxsat (GET, POST, PUT, DELETE va h.k.)
                .AllowAnyMethod()

                // AllowAnyHeader() - barcha HTTP headerslarga ruxsat 
                // (Content-Type, Authorization va boshqalar)
                .AllowAnyHeader();
            });

            // QOSHIMCHA: Xavfsizroq CORS sozlamasi namunasi (o'quvchilarga ko'rsatish uchun)
            // Production muhiti uchun tavsiya etiladi:

            options.AddPolicy("StrictCorsPolicy", policy =>
            {
                // Faqat aniq domenlardan so'rovlarga ruxsat
                policy.WithOrigins(
                    "https://myapp.com",
                    "https://www.myapp.com",
                    "http://localhost:3000", // Development uchun
                    "http://127.0.0.1:5500"
                )
                // Faqat kerakli metodlarga ruxsat
                .WithMethods("GET", "POST", "PUT", "DELETE")
                // Faqat kerakli headerlarga ruxsat
                .WithHeaders("Content-Type", "Authorization")
                // Credentials (Cookie, Authorization headers) bilan ishlash
                .AllowCredentials();
            });
        });

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
        builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection("MinioSettings"));

        // Npgsql (DbContext)
        builder.Services.AddDbContext<LibraryManagmentSystemContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddSingleton<IMinioClient>(sp =>
        {
            var minioSettings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

            // MinioClient obyektini yaratish
            var client = new MinioClient()
                .WithEndpoint(minioSettings.Endpoint)
                .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey);

            // Agar SSL yoqilgan bo'lsa
            if (minioSettings.UseSsl)
            {
                client = client.WithSSL();
            }

            return client.Build(); // MinioClient ni qurish
        });

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

        // ============================================
        // MIDDLEWARE PIPELINE TARTIBI MUHIM!
        // ============================================
        // CORS middleware Authentication va Authorization dan OLDIN chaqirilishi kerak
        // Chunki preflight OPTIONS so'rovlari autentifikatsiyadan o'tmaydi
        app.UseCors("StrictCorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<LibraryManagmentSystemContext>();
                var seeder = new PermissionSeeder(context);
                seeder.SeedPermissionsAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
        app.MapControllers();

        app.Run();
    }
}