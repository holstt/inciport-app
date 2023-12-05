using InciportWebService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class AuthencicationService : IAuthencicationService {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthencicationService(UserManager<ApplicationUser> userManager, IConfiguration configuration) {
      _userManager = userManager;
      _configuration = configuration;
    }

    public async Task<AuthorizedUser> LoginAsync(string email, string password) {
      ApplicationUser user = await _userManager.FindByEmailAsync(email);

      if (user is null) {
        throw new NotFoundException($"No user with email '{email}'");
      }

      bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, password);
      if (!isCorrectPassword) {
        throw new ValidationException($"Wrong password for user with email '{user.UserName}'");
      }

      JwtSecurityToken token = await CreateJwtTokenAsync(user);

      return new AuthorizedUser(user, token);
    }

    /// Creates a valid login token.
    private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user) {
      List<Claim> claims = await CreateUserClaims(user);

      // Create key from secret.
      SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
      // Create JWT token with claims.
      JwtSecurityToken token = new JwtSecurityToken(
          issuer: _configuration["JWT:ValidIssuer"],
          audience: _configuration["JWT:ValidAudience"],
          expires: DateTimeOffset.UtcNow.AddHours(_configuration.GetSection("JWT:TokenExpirationHours").Get<int>()).UtcDateTime,
          claims: claims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
          );

      return token;
    }

    private async Task<List<Claim>> CreateUserClaims(ApplicationUser user) {
      // Get roles of user.
      IList<string> userRoles = await _userManager.GetRolesAsync(user);

      // Create claims
      List<Claim> claims = new List<Claim>{
          new Claim(ClaimTypes.Name, user.UserName),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

      // Add user roles to claims
      foreach (string userRole in userRoles) {
        claims.Add(new Claim(ClaimTypes.Role, userRole));
      }

      return claims;
    }
  }
}