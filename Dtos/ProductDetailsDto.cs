namespace Mise.Dtos;

public record ProductDetailsDto(Guid Id, string Name, decimal Price, List<Guid> TagsIds);