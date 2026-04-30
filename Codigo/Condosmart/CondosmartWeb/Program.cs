using Core;
using Core.Data;
using Core.Identity.Data;
using Core.Models;
using Core.Service;
using Core.Settings;
using CondosmartWeb.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service;

namespace Condosmart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();

            var connectionString = builder.Configuration.GetConnectionString("CondosmartConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Conexao com o banco de dados nao foi configurada corretamente.");

            builder.Services.AddDbContext<CondosmartContext>(options => options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)));

            var identityConnectionString = builder.Configuration.GetConnectionString("IdentityDatabase");
            if (string.IsNullOrEmpty(identityConnectionString))
                throw new InvalidOperationException("Conexao com o banco Identity nao foi configurada corretamente.");

            builder.Services.AddDbContext<IdentityContext>(options => options.UseMySql(
                identityConnectionString,
                ServerVersion.AutoDetect(identityConnectionString)));

            builder.Services.AddIdentity<UsuarioSistema, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AcessoNegado";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
            });

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(SmtpSettings.SectionName));

            builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
            builder.Services.AddScoped<ICondominioService, CondominioService>();
            builder.Services.AddScoped<ICnpjService, CnpjService>();
            builder.Services.AddScoped<ISindicoService, SindicoService>();
            builder.Services.AddScoped<IMoradorService, MoradorService>();
            builder.Services.AddScoped<IMoradorProvisionamentoService, MoradorProvisionamentoService>();
            builder.Services.AddScoped<IVisitanteService, VisitanteService>();
            builder.Services.AddScoped<IAtaService, AtaService>();
            builder.Services.AddScoped<IReservaService, ReservaService>();
            builder.Services.AddScoped<IUnidadesResidenciaisService, UnidadesResidenciaisService>();
            builder.Services.AddScoped<IAreaDeLazerService, AreaDeLazerService>();
            builder.Services.AddScoped<IPagamentoService, PagamentoService>();
            builder.Services.AddScoped<IMensalidadeService, MensalidadeService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddHttpClient<ICnpjService, CnpjService>();
            builder.Services.AddAutoMapper(configAction => { }, AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseMiddleware<SenhaTemporariaRedirectMiddleware>();
            app.UseAuthorization();

            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
