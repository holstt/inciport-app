using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class LoginTokenDto {
    public string Value { get; }
    public DateTime Expiration { get; }

    public LoginTokenDto(string value, DateTime expiration) {
      Value = value;
      Expiration = expiration;
    }
  }
}