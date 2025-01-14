using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mise.Database;
using Mise.Entities;
using Mise.Errors;
using Mise.Services;

namespace Mise.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly CatalogService _catalogService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IErrorHandler _errorHandler;

    public TagsController(CatalogService catalogService, UserManager<ApplicationUser> userManager, IErrorHandler errorHandler)
    {
        _catalogService = catalogService;
        _userManager = userManager;
        _errorHandler = errorHandler;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = await _catalogService.GetAllTagsAsync();


        return Ok(tags);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTagById(Guid id)
    {
        var result = await _catalogService.GetTagByIdAsync(id);
        if (result.IsT1)
        {
            return NotFound(result.AsT1);
        }

        var tag = result.AsT0;

        return Ok(tag);
    }

    [HttpPost]
    [Authorize]
    [Route("")]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagDTO createTagsDTO)
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

        var result = await _catalogService.CreateTagsAsync(createTagsDTO, user.Id);
        if (result.IsT1)
        {
            var err = result.AsT1;
            return _errorHandler.Handle(err);
        }

        var createdTag = result.AsT0;


        return CreatedAtAction(nameof(GetTagById), new { id = createdTag.TagId }, createdTag);


    }

}




