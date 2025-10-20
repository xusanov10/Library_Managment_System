using Library_Managment_System.Services;
using LibraryMS.Application;
using LibraryMS.Application.Seeders;
using LibraryMS.DataAccess.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);



        // RabbirMq Singltin
        // RabbitMQ addhostService<RabbitMQConsumer>();

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

        var jwtSettings = new JwtSettings();
        builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

        builder.Services
            .AddAuthentication(options =>
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
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services.AddAuthorization();
        builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection("MinioSettings"));

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
            var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();
            seeder.SeedPermissionsAsync();
        }
        app.MapControllers();

        app.Run();
    }
}