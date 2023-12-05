using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class ForbiddenAccessException : Exception {

    public ForbiddenAccessException(string message) : base(message) {
    }
  }
}