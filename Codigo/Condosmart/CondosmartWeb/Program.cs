using Core;
using Core.Data;
using Core.Identity.Data;
using Core.Models;
using Core.Service;
using Core.Settings;
using CondosmartWeb.Middleware;
using CondosmartWeb.Services;
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
            builder.Services.AddHttpContextAccessor();

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
            builder.Services.AddScoped<ICepService, CepService>();
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
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            builder.Services.AddScoped<IMoradorDashboardService, MoradorDashboardService>();
            builder.Services.AddScoped<ICondominioContextService, CondominioContextService>();
            builder.Services.AddScoped<IArquivoUploadService, ArquivoUploadService>();
            builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
            builder.Services.AddScoped<IAdminBootstrapService, AdminBootstrapService>();
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

            if (TryHandleAdminSeedCommand(args, app))
                return;

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

        private static bool TryHandleAdminSeedCommand(string[] args, WebApplication app)
        {
            if (!args.Contains("--seed-admin", StringComparer.OrdinalIgnoreCase))
                return false;

            var nome = GetArgumentValue(args, "--name") ?? "Administrador CondoSmart";
            var email = GetArgumentValue(args, "--email");
            var senha = GetArgumentValue(args, "--password");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                Console.WriteLine("Uso: dotnet run --project CondosmartWeb.csproj -- --seed-admin --name \"Nome\" --email admin@dominio.com --password \"SenhaForte123\"");
                return true;
            }

            using var scope = app.Services.CreateScope();
            var bootstrapService = scope.ServiceProvider.GetRequiredService<IAdminBootstrapService>();
            var criado = bootstrapService.CriarAdminAsync(nome, email, senha).GetAwaiter().GetResult();

            Console.WriteLine(criado
                ? $"Admin criado com sucesso para {email}."
                : $"Nao foi possivel criar o admin para {email}. Verifique se ele ja existe ou se a senha atende aos requisitos.");

            return true;
        }

        private static string? GetArgumentValue(string[] args, string option)
        {
            var index = Array.FindIndex(args, arg => string.Equals(arg, option, StringComparison.OrdinalIgnoreCase));
            if (index < 0 || index == args.Length - 1)
                return null;

            return args[index + 1];
        }
    }
}
