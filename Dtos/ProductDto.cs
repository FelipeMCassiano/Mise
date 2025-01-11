namespace Mise.Dtos;

public record ProductDto(Guid Id, string Name, decimal Price, List<string> TagsNames);