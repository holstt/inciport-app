using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class AuthorizationPolicyNames {
    public const string REQUIRE_INTERNAL_ACCESS = "REQUIRE_INTERNAL_ACCESS";
    public const string REQUIRE_ELEVATED_RIGHTS = "REQUIRE_ELEVATED_RIGHTS";
    public const string REQUIRE_SUPER_USER = "REQUIRE_SUPER_USER";
  }
}