using Microsoft.AspNetCore.Mvc;
using Mise.Database;
using Mise.Services;

namespace Mise.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly CatalogService _catalogService;

    public TagsController(CatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = await _catalogService.GetAllTagsAsync();

        return Ok(tags);
    }
}




