namespace CondosmartAPI.Models;

public class JwtResponseViewModel
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = [];
    public DateTime Expiration { get; set; }
}
