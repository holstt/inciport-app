using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class Address {
    public int Id { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string ZipCode { get; private set; }
    public string Country { get; private set; }
    public string Municipality { get; private set; }

    public Address(string street, string city, string zipCode, string country, string municipality, int id = 0) {
      if (string.IsNullOrEmpty(street)) {
        throw new ArgumentException($"'{nameof(street)}' cannot be null or empty.", nameof(street));
      }

      if (string.IsNullOrEmpty(city)) {
        throw new ArgumentException($"'{nameof(city)}' cannot be null or empty.", nameof(city));
      }

      if (string.IsNullOrEmpty(zipCode)) {
        throw new ArgumentException($"'{nameof(zipCode)}' cannot be null or empty.", nameof(zipCode));
      }

      if (string.IsNullOrEmpty(country)) {
        throw new ArgumentException($"'{nameof(country)}' cannot be null or empty.", nameof(country));
      }

      if (string.IsNullOrEmpty(municipality)) {
        throw new ArgumentException($"'{nameof(municipality)}' cannot be null or empty.", nameof(municipality));
      }

      Id = id;
      Street = street;
      City = city;
      ZipCode = zipCode;
      Country = country;
      Municipality = municipality;
    }

    protected Address() { // For EF
    }
  }
}