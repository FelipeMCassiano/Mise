using Microsoft.AspNetCore.Mvc;
using Mise.Dtos;
using Mise.Errors;
using Mise.Services;
namespace Mise.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly CatalogService _catalogService;
    private readonly IErrorHandler _errorHandler;

    public ProductsController(CatalogService catalogService, IErrorHandler errorHandler)
    {
        _catalogService = catalogService;
        _errorHandler = errorHandler;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {

        var result = await _catalogService.CreateProductWithTags(createProductDto);
        if (result.IsT1)
        {
            var err = result.AsT1;

            return _errorHandler.Handle(err);
        }

        var product = result.AsT0;

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {

        var result = await _catalogService.GetProductbyId(id);
        if (result.IsT1)
        {
            var err = result.AsT1;
            return _errorHandler.Handle(err);
        }

        return Ok(result.AsT0);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetProductByTags([FromQuery] string[] tags)
    {
        var result = await _catalogService.GetProductsByTagAsync(tags);

        return Ok(result.AsT0);
    }
}
