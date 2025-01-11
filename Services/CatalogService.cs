using Mise.Dtos;
using Mise.Entities;
using Mise.Errors;
using OneOf;

namespace Mise.Services;

public class CatalogService
{
    private readonly ProductsService _productsService;
    private readonly TagsService _tagsService;
    public CatalogService(ProductsService productsService, TagsService tagsService)
    {
        _productsService = productsService;
        _tagsService = tagsService;
    }

    public async Task<OneOf<ProductDto, Error>> CreateProductWithTags(CreateProductDto createProductDto)
    {
        var tags = await _tagsService.GetOrCreateMultipleTagByNameAsync(createProductDto.TagsNames);
        var product = await _productsService.CreateProduct(createProductDto, tags);

        return product;
    }

    public async Task<OneOf<ProductDetailsDto, NotFoundProductError>> GetProductbyId(Guid id)
    {
        return await _productsService.GetProductById(id);
    }

    public async Task<List<Tag>> GetAllTagsAsync() => await _tagsService.GetAllTagsAsync();

}