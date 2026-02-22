namespace CondosmartWeb.Infrastructure;

/// <summary>
/// Propaga o cookie de autenticação do usuário corrente nas chamadas
/// HTTP internas do MVC para a Web API hospedada no mesmo processo.
/// </summary>
public class CookieAuthDelegatingHandler : DelegatingHandler
{
    private const string CookieName = ".AspNetCore.Identity.Application";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieAuthDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[CookieName];
        if (!string.IsNullOrEmpty(cookie))
            request.Headers.Add("Cookie", $"{CookieName}={cookie}");

        return base.SendAsync(request, cancellationToken);
    }
}
