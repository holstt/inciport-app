using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace InciportWebService.Domain {

  public class ValidationException : Exception {
    public string Title { get; }
    public List<string> Errors { get; } = new List<string>();

    public ValidationException(string title, List<string> validationErrors) {
      Errors = validationErrors;
      Title = title;
    }

    public ValidationException(string message) : base(message) {
      Errors.Add(message);
    }
  }
}