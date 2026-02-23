using Microsoft.AspNetCore.Identity;

namespace CondosmartAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string? NomeCompleto { get; set; }
}
