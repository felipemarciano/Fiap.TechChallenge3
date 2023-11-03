using Api.Model;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var user = new ApplicationUser { UserName = model?.Email, Email = model?.Email };
            var result = await _userManager.CreateAsync(user, model?.Password!);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                var msgError = "";

                foreach (var error in result.Errors)
                {
                    msgError += $" {error.Description}";
                }

                return BadRequest(msgError);
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (model.UserName == null || model.Password == null)
                return Unauthorized("Invalid username or password");

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && user.UserName != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var tokenHandler = new JwtSecurityTokenHandler();

                string? secretKey = _configuration.GetSection("JwtSettings:Secret").Value;

                if (secretKey == null)
                    throw new Exception("Not found jwtsettings secret");

                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName)
                };

                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(new { AccessToken = jwtToken });
            }

            return Unauthorized("Invalid username or password");
        }

    }
}
