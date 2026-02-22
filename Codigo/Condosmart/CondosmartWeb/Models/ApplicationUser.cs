using Microsoft.AspNetCore.Identity;

namespace CondosmartWeb.Models;

public class ApplicationUser : IdentityUser
{
    public string? NomeCompleto { get; set; }
}
