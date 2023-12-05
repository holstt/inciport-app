using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class AddressDto {
    [Required, StringLength(256)]
    public string Street { get; set; }

    [Required, StringLength(256)]
    public string City { get; set; }

    [Required, StringLength(4, MinimumLength = 4)]
    public string ZipCode { get; set; }

    [Required, StringLength(256)]
    public string Country { get; set; }

    [Required, StringLength(256)]
    public string Municipality { get; set; }
  }
}