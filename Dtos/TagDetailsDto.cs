using System.ComponentModel.DataAnnotations;

public record TagDetailsDto([Required] Guid TagId, [Required] string TagName);