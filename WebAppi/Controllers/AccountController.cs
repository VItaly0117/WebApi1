using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppi.DTOs;
using WebAppi.Models;
using WebAppi.Services;

namespace WebAppi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(UserManager<User> userManager, IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<OperationResult<UserDto>>> Register(RegisterDto registerDto)
    {
        var user = new User { UserName = registerDto.Username, Email = registerDto.Email };
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(OperationResult<UserDto>.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description))));
        }

        return Ok(OperationResult<UserDto>.SuccessResult(new UserDto
        {
            Email = user.Email,
            Token = CreateToken(user)
        }));
    }

    [HttpPost("login")]
    public async Task<ActionResult<OperationResult<UserDto>>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Login) ?? await userManager.FindByNameAsync(loginDto.Login);

        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized(OperationResult<UserDto>.FailureResult("Invalid login or password."));
        }

        return Ok(OperationResult<UserDto>.SuccessResult(new UserDto
        {
            Email = user.Email,
            Token = CreateToken(user)
        }));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<OperationResult<UserDto>>> GetProfile()
    {
        var user = await userManager.GetUserAsync(User);
        return Ok(OperationResult<UserDto>.SuccessResult(new UserDto { Email = user.Email }));
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
