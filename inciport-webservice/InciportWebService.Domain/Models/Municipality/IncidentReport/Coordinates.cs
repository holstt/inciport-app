using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class Coordinates {
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }

    public Coordinates(double latitude, double longitude) {
      Latitude = IsValidLongitude(latitude) ? latitude : throw new ArgumentException($"Latitude '{latitude}' is invalid");
      Longitude = IsValidLongitude(longitude) ? longitude : throw new ArgumentException($"Longitude '{longitude}' is invalid");
    }

    protected Coordinates() { // For EF
    }

    public static bool IsValidLatitude(double latitude) {
      return latitude >= -90 && latitude <= 90;
    }

    public static bool IsValidLongitude(double longitude) {
      return longitude >= -180 && longitude <= 180;
    }
  }
}