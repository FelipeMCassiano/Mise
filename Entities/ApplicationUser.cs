using Microsoft.AspNetCore.Identity;

namespace Mise.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public List<Product> Products { get; } = new();
}