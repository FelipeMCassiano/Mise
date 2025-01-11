
using System.ComponentModel.DataAnnotations;

namespace Mise.Dtos;
public record CreateProductDto([MaxLength(50)][MinLength(1)] string Name,
                               [Range(0, double.PositiveInfinity)] decimal Price,
                               List<string> TagsNames);