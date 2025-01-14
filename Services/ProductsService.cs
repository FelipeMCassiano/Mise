using System.Formats.Tar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
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
                                                              List<Tag> tags, int userId)
    {
        try
        {
            var id = Guid.NewGuid();
            Product product = new()
            {
                Id = id,
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Tags = tags,
                UserId = userId
            };

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product.ToDto();
        }
        catch (Exception ex)
        {
            var err = new CreateProductError();
            return err with { Message = ex.Message };
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

    public async Task<OneOf<List<ProductDto>, NotFoundProductError>> GetProductsByTags(string[] tags)
    {
        if (tags is null || tags.Length == 0)
        {
            return new NotFoundProductError();

        }

        var products = await _dbContext.Products
                                                .Where(p => p.Tags.Any(t => tags.Contains(t.Name)))
                                                .Include(p => p.Tags)
                                                .Select(p => p.ToDto())
                                                .ToListAsync();
        if (products is null)
        {
            return new NotFoundProductError();
        }

        return products;

    }
    public async Task<List<ProductDetailsDto>> GetAllProductsAsync()
    {
        return await _dbContext.Products.Select(p => p.ToDetailsDto()).ToListAsync();

    }
}

