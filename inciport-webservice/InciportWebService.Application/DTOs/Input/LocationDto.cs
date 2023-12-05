using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class LocationDto {
    [Required]
    public AddressDto Address { get; set; }

    [Required, Range(-90, 90)]
    public double? Latitude { get; set; }

    [Required, Range(-180, 180)]
    public double? Longitude { get; set; }

    public static LocationDto FromModel(Location model) {
      return new LocationDto() {
        Latitude = model.Coordinates.Latitude,
        Longitude = model.Coordinates.Longitude,
        Address = new AddressDto {
          City = model.Address.City,
          Street = model.Address.Street,
          ZipCode = model.Address.ZipCode,
          Country = model.Address.Country,
          Municipality = model.Address.Municipality
        }
      };
    }

    // Location and address will have no id if this is not created from an existing Location.
    public Location ToModel(int? locationId = null, int? addressId = null) {
      return new Location() {
        // Fill out id's as these are not specified by the user.
        Id = locationId.HasValue ? locationId.Value : 0,
        Coordinates = new Coordinates(Latitude.Value, Longitude.Value),
        Address = new Address(
          street: Address.Street,
          city: Address.City,
          zipCode: Address.ZipCode,
          country: Address.Country,
          municipality: Address.Municipality
,
          id: addressId.HasValue ? addressId.Value : 0) {
        }
      };
    }
  }
}