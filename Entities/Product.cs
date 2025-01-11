using System.ComponentModel.DataAnnotations;

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

}

