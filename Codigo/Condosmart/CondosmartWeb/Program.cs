using Core.Data;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Service;

namespace Condosmart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            builder.Services.AddScoped<IMoradorService, MoradorService>();
            builder.Services.AddScoped<ICondominioService, CondominioService>();
            builder.Services.AddScoped<ISindicoService, SindicoService>();
            builder.Services.AddScoped<IVisitanteService, VisitanteService>();
            builder.Services.AddScoped<IAtaService, AtaService>();
            builder.Services.AddScoped<IReservaService, ReservaService>();
            builder.Services.AddScoped<IUnidadesResidenciaisService, UnidadesResidenciaisService>();
            builder.Services.AddScoped<IAreaDeLazerService, AreaDeLazerService>();
            builder.Services.AddScoped<IPagamentoService, PagamentoService>();
            builder.Services.AddScoped<IMensalidadeService, MensalidadeService>();


            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddHttpClient("CondoSmartAPI", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
            });

            builder.Services.AddTransient<IReservaService, ReservaService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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

            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
