using Microsoft.EntityFrameworkCore;
using Mise.Database;
using Mise.Dtos;
using Mise.Entities;
using Mise.Errors;
using Mise.Mapping;
using OneOf;

namespace Mise.Services;
public class ProductsService
{

    private readonly AppDbContext _dbContext;
    public ProductsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<OneOf<ProductDto, Error>> CreateProduct(CreateProductDto createProductDto,
                                                              List<Tag> tags)
    {
        try
        {
            var id = Guid.NewGuid();
            Product product = new()
            {
                Id = id,
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Tags = tags
            };

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product.ToDto();
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ErrorType.ServerError);
        }

    }

    public async Task<OneOf<ProductDetailsDto, NotFoundProductError>> GetProductById(Guid id)
    {
        var product = await _dbContext.Products.Include(x => x.Tags)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return new NotFoundProductError();
        }

        return product.ToDetailsDto();
    }
}