using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace Mise.Entities;

public class Product
{
    public Guid Id { get; init; }
    [MaxLength(50)]
    [MinLength(1)]
    public required string Name { get; set; }
    [Range(0, double.PositiveInfinity)]
    public decimal Price { get; set; }
    public List<Tag> Tags { get; set; } = [];
    public int UserId { get; init; }
    public ApplicationUser User { get; init; } = null!;

}

