using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Hackathon.AuthService.Dtos;
using Hackathon.AuthService.Interfaces;
using Hackathon.AuthService.Entities;


namespace Hackathon.AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IUserInfoService _userInfoService;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, IUserInfoService userInfoService, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _userInfoService = userInfoService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ValidationProblemDetails(ModelState));

        var appUser = new AppUser
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            RefreshToken = _tokenService.CreateRefreshToken(),
            RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30)
        };

        var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
        if (!createdUser.Succeeded)
        {
            var errors = createdUser.Errors.ToDictionary(e => e.Code, e => new[] { e.Description });
            return BadRequest(new ValidationProblemDetails(errors));
        }

        await _userManager.AddToRoleAsync(appUser, "User");

        var roles = await _userManager.GetRolesAsync(appUser);

        return Ok(new NewUserDto
        {
            UserName = appUser.UserName,
            Email = appUser.Email,
            AccessToken = _tokenService.CreateAccessToken(appUser, roles.ToList()),
            RefreshToken = appUser.RefreshToken
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ValidationProblemDetails(ModelState));

        var user = await _userManager.FindByNameAsync(loginDto.Username);

        if (user == null)
            return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
            { { "Login", new[] { "Username not found and/or password incorrect." } } }));

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
            { { "Login", new[] {"Username not found and/or password incorrect."} } }));

        var roles = await _userManager.GetRolesAsync(user);

        if (user.RefreshTokenExpiryDate < DateTime.UtcNow)
        {
            user.RefreshToken = _tokenService.CreateRefreshToken();
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30);
            await _userManager.UpdateAsync(user);
        }

        return Ok(new
        {
            user.UserName,
            user.Email,
            AccessToken = _tokenService.CreateAccessToken(user, roles.ToList()),
            RefreshToken = user.RefreshToken
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenDto.RefreshToken))
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            { { "RefreshToken", new[] { "Refresh token is required." } } }));
        }

        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken && u.RefreshTokenExpiryDate > DateTime.UtcNow);

        if (user == null)
        {
            return Unauthorized(new ValidationProblemDetails(new Dictionary<string, string[]>
            { { "RefreshToken", new[] { "Invalid or expired refresh token." } } }));
        }

        var roles = await _userManager.GetRolesAsync(user);

        var newAccessToken = _tokenService.CreateAccessToken(user, roles.ToList());

        user.RefreshToken = _tokenService.CreateRefreshToken();
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30);
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            AccessToken = newAccessToken,
            RefreshToken = user.RefreshToken
        });
    }

    [Authorize]
    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID is missing in token");
        }

        var appUser = await _userManager.FindByIdAsync(userId);

        if (appUser == null)
        {
            return NotFound("User not found");
        }

        var userInfo = await _userInfoService.GetUserInfo(appUser);

        return Ok(userInfo);
    }
}