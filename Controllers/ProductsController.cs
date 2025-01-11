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

    public ProductsController(CatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {

        var result = await _catalogService.CreateProductWithTags(createProductDto);
        if (result.IsT1)
        {
            var err = result.AsT1;

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { status = "error", message = err.Message, errorType = err.ErrorType.ToString() });
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
            return NotFound(new { status = "error", message = err.Message, errorType = err.ErrorType.ToString() });
        }

        return Ok(result.AsT0);
    }
}
