using CondosmartAPI.Data;
using CondosmartAPI.Models;
using Core.Data;
using Core.Service;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Service;

namespace CondosmartAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "CondoSmart API", Version = "v1" });
            });

            var connectionString = builder.Configuration.GetConnectionString("CondosmartConnection")
                ?? throw new InvalidOperationException("Conexão com o banco de dados não foi configurada.");

            // Contexto de domínio (tabelas de negócio)
            builder.Services.AddDbContext<CondosmartContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Contexto do Identity (AspNetUsers, AspNetRoles, etc.)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // DataProtection compartilhado com CondosmartWeb
            // (permite que o cookie emitido pelo Web seja válido nesta API)
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(
                    new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "..", "keys")))
                .SetApplicationName("CondoSmart");

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
            });

            // CORS — permite chamadas do CondosmartWeb
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CondosmartWeb", policy =>
                    policy.WithOrigins("https://localhost:7290", "http://localhost:5280")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            });

            // Serviços de domínio
            builder.Services.AddScoped<IMoradorService, MoradorService>();

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeedService.SeedRolesAndAdminAsync(scope.ServiceProvider);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("CondosmartWeb");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
