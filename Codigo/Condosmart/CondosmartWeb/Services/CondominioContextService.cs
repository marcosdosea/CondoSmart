using Core.Identity;
using Core.Models;
using Core.Service;

namespace CondosmartWeb.Services
{
    public class CondominioContextService : ICondominioContextService
    {
        private const string CookieName = "cs_condominio_admin";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICondominioService _condominioService;
        private readonly IMoradorService _moradorService;
        private readonly IUnidadesResidenciaisService _unidadesService;

        public CondominioContextService(
            IHttpContextAccessor httpContextAccessor,
            ICondominioService condominioService,
            IMoradorService moradorService,
            IUnidadesResidenciaisService unidadesService)
        {
            _httpContextAccessor = httpContextAccessor;
            _condominioService = condominioService;
            _moradorService = moradorService;
            _unidadesService = unidadesService;
        }

        public int? GetCondominioAtualId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
                return null;

            if (user.IsInRole(Perfis.Morador))
            {
                var email = user.Identity?.Name;
                if (string.IsNullOrWhiteSpace(email))
                    return null;

                var morador = _moradorService.GetByEmail(email);
                var unidade = morador is null ? null : _unidadesService.GetByMoradorId(morador.Id);
                return morador?.CondominioId ?? unidade?.CondominioId;
            }

            if (user.IsInRole(Perfis.Admin))
            {
                var cookie = httpContext?.Request.Cookies[CookieName];
                if (int.TryParse(cookie, out var condominioCookieId) && _condominioService.GetById(condominioCookieId) is not null)
                    return condominioCookieId;

                return _condominioService.GetAll().OrderBy(c => c.Nome).FirstOrDefault()?.Id;
            }

            return null;
        }

        public Condominio? GetCondominioAtual()
        {
            var id = GetCondominioAtualId();
            return id.HasValue ? _condominioService.GetById(id.Value) : null;
        }

        public string GetCondominioAtualNome()
        {
            return GetCondominioAtual()?.Nome ?? "Condominio nao selecionado";
        }

        public List<Condominio> GetCondominiosDisponiveis()
        {
            return _condominioService.GetAll()
                .OrderBy(c => c.Nome)
                .ToList();
        }

        public void SelecionarCondominio(int condominioId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.IsInRole(Perfis.Admin) != true)
                return;

            var condominio = _condominioService.GetById(condominioId);
            if (condominio is null)
                return;

            httpContext.Response.Cookies.Append(
                CookieName,
                condominioId.ToString(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = false,
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                });
        }
    }
}
