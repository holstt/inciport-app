using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class CreateIncidentReportDto {
    [Required]
    public ChosenMainCategoryDto ChosenMainCategory { get; set; }
    [Required]
    public LocationDto Location { get; set; }
    [Required]
    public string Description { get; set; }
    public ContactInformationDto ContactInformation { get; set; } // Can be null
    public List<string> ImagesBase64 { get; set; }
  }
}