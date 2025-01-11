using Mise.Dtos;
using Mise.Entities;

namespace Mise.Mapping;

public static class ProductMapping
{

    public static ProductDto ToDto(this Product product)
    {
        var tagsNames = product.Tags.Select(t => t.Name).ToList();
        return new(product.Id,
                   product.Name,
                   product.Price,
                   tagsNames);
    }

    public static ProductDetailsDto ToDetailsDto(this Product product)
    {
        var tagsIds = product.Tags.Select(t => t.Id).ToList();
        return new(product.Id,
                   product.Name,
                   product.Price,
                   tagsIds);
    }
}