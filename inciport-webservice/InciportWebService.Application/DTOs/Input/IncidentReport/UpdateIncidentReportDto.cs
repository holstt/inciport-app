using InciportWebService.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InciportWebService.Application {

  public class UpdateIncidentReportDto {
    [Required]
    public int Id { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReportStatus Status { get; set; }

    [Required]
    public ChosenMainCategoryDto ChosenMainCategory { get; set; }
    [Required]
    public LocationDto Location { get; set; }

    [Required]
    public string Description { get; set; }

    // Optionals
    public ContactInformationDto ContactInformation { get; set; }
    public List<string> ImageUrls { get; set; }
    public GetWorkerTeamDto AssignedTeam { get; set; }
  }
}