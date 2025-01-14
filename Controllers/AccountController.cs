using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mise.Entities;
namespace Mise.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUser)
    {

        var user = new ApplicationUser()
        {
            UserName = registerUser.Username,
            Email = registerUser.EmailAddress,

        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);
        if (!result.Succeeded)
        {
            return UnprocessableEntity(result.Errors);
        }

        return Redirect("/api/products");
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
    {

        var user = await _userManager.FindByEmailAsync(loginUser.EmailAddress);

        if (user == null)
        {
            return NotFound($"Unable to load user with email '{loginUser.EmailAddress}'.");
        }

        var isValid = await _signInManager.UserManager.CheckPasswordAsync(user, loginUser.Password);
        if (!isValid)
        {
            return Unauthorized();
        }

        await _signInManager.SignInAsync(user, loginUser.RememberMe, null);

        return Redirect("/api/products");
    }


    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/api/account/login");
    }


}