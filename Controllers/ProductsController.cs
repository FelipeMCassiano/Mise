using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mise.Dtos;
using Mise.Entities;
using Mise.Errors;
using Mise.Services;
namespace Mise.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly CatalogService _catalogService;
    private readonly IErrorHandler _errorHandler;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProductsController(CatalogService catalogService, IErrorHandler errorHandler, UserManager<ApplicationUser> userManager)
    {
        _catalogService = catalogService;
        _errorHandler = errorHandler;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("")]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var currentUser = User.Identity?.Name; ;
        if (currentUser is null)
        {
            return Unauthorized();
        }
        var user = await _userManager.FindByNameAsync(currentUser);
        if (user is null)
        {
            return Redirect("/api/account/login");
        }

        var result = await _catalogService.CreateProductWithTags(createProductDto, user.Id);
        if (result.IsT1)
        {
            var err = result.AsT1;

            return _errorHandler.Handle(err);
        }

        var product = result.AsT0;

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllProducts()
    {
        var result = await _catalogService.GetAllProductsAsync();

        return Ok(result);
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
    [Route("tags")]
    public async Task<IActionResult> GetProductByTags([FromQuery] string[] tags)
    {
        var result = await _catalogService.GetProductsByTagAsync(tags);

        return Ok(result.AsT0);
    }
}
