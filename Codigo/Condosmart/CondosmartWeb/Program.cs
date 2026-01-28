using Core;
using Core.Data;
using Core.Service;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // Adicione este using

namespace Condosmart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("CondosmartConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Conexão com o banco de dados não foi configurada corretamente.");
            }
            builder.Services.AddDbContext<CondosmartContext>(
                options => options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                ));

            builder.Services.AddScoped<ICondominioService, CondominioService>();
            builder.Services.AddScoped<ISindicoService, SindicoService>();
            builder.Services.AddScoped<IVisitanteService, VisitanteService>();
            builder.Services.AddScoped<IAtaService, AtaService>();
            builder.Services.AddScoped<IReservaService, ReservaService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
