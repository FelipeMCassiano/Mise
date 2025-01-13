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
        var resultTags = await _tagsService.GetMultipleTagsByNameAsync(createProductDto.TagsNames);
        if (resultTags.IsT1)
        {
            var err = resultTags.AsT1;

            return err;
        }
        var resultProduct = await _productsService.CreateProduct(createProductDto, resultTags.AsT0);
        if (resultProduct.IsT1)
        {
            var err = resultProduct.AsT1;

            return err;
        }
        var product = resultProduct.AsT0;

        return product;
    }

    public async Task<OneOf<ProductDetailsDto, NotFoundProductError>> GetProductbyId(Guid id)
    {
        return await _productsService.GetProductById(id);
    }

    public async Task<List<Tag>> GetAllTagsAsync() => await _tagsService.GetAllTagsAsync();

    public async Task<OneOf<Tag, NotFoundTagError>> GetTagByIdAsync(Guid id) => await _tagsService.GetTagAsync(id);


    public async Task<OneOf<List<ProductDto>, NotFoundProductError>> GetProductsByTagAsync(string[] tags) => await _productsService.GetProductsByTags(tags);

}