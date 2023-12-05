using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.Serialization;

namespace InciportWebService.Domain {

  public class NotFoundException : Exception {

    public NotFoundException(string ressourceName, string ressourceId) : base($"{ressourceName} with id '{ressourceId}' not found") {
    }

    public NotFoundException(string ressourceName, int ressourceId) : this(ressourceName, ressourceId.ToString()) {
    }

    public NotFoundException(string message) : base(message) {
    }
  }
}