using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace CondosmartWeb.Middleware
{
    public class SenhaTemporariaRedirectMiddleware
    {
        private static readonly string[] AllowedPaths =
        {
            "/Identity/Account/TrocarSenhaTemporaria",
            "/Identity/Account/Logout",
            "/Identity/Account/AcessoNegado"
        };

        private readonly RequestDelegate _next;

        public SenhaTemporariaRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<UsuarioSistema> userManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var path = context.Request.Path.Value ?? string.Empty;
                var allowed = AllowedPaths.Any(allowedPath => path.StartsWith(allowedPath, StringComparison.OrdinalIgnoreCase));

                if (!allowed)
                {
                    var usuario = await userManager.GetUserAsync(context.User);
                    if (usuario?.SenhaTemporaria == true)
                    {
                        context.Response.Redirect("/Identity/Account/TrocarSenhaTemporaria");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
