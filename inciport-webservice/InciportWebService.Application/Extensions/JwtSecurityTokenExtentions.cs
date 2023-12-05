using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public static class JwtSecurityTokenExtentions {

    public static string AsTokenString(this JwtSecurityToken instance) => new JwtSecurityTokenHandler().WriteToken(instance);
  }
}