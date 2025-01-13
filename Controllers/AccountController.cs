using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Mise.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUser)
    {

        var user = new IdentityUser()
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

        var signInResult = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, false);
        if (!signInResult.Succeeded)
        {
            return Unauthorized();

        }

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