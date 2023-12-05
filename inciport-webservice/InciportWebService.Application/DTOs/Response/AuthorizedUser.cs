using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class AuthorizedUser {
    public ApplicationUser Details { get; }
    public JwtSecurityToken LoginToken { get; }

    public AuthorizedUser(ApplicationUser user, JwtSecurityToken token) {
      Details = user;
      LoginToken = token;
    }
  }
}