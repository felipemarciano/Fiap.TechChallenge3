using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Configuration
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthenticationStateProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetToken(string jwtToken)
        {
            var identity = new ClaimsIdentity();

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(jwtToken, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"] ?? ""))
            }, out _);
            var authState = new AuthenticationState(principal);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwtToken = _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];

            if (jwtToken == null)
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            }

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(jwtToken, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"] ?? ""))
            }, out _);
            var authState = new AuthenticationState(principal);

            return Task.FromResult(authState);
        }
    }
}
