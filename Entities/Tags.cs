namespace Mise.Entities;

public class Tag
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    public List<Product> Products { get; set; } = [];
}