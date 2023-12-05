using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace InciportWebService.Api {

  public class GetIncidentReportDto {
    public int Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReportStatus Status { get; set; }
    public GetChosenMainCategoryDto ChosenMainCategory { get; set; }
    public LocationDto Location { get; set; }
    public string Description { get; set; }
    public ContactInformationDto ContactInformation { get; set; }
    public List<string> ImageUrls { get; set; }
    public DateTimeOffset TimestampCreatedUtc { get; set; }
    public DateTimeOffset TimestampModifiedUtc { get; set; }
    public GetWorkerTeamDto AssignedTeam { get; set; }

    public GetIncidentReportDto() {
      ImageUrls = new List<string>();
    }

    public static GetIncidentReportDto FromModel(IncidentReport model, Uri baseRessourcePath) {
      return new GetIncidentReportDto {
        Id = model.Id,
        Status = model.Status,
        ChosenMainCategory = GetChosenMainCategoryDto.FromModel(model.ChosenMainCategory),
        TimestampCreatedUtc = model.TimestampCreatedUtc,
        TimestampModifiedUtc = model.TimestampLastModifiedUtc,
        AssignedTeam = GetWorkerTeamDto.FromModel(model.AssignedTeam),
        ContactInformation = ContactInformationDto.FromModel(model.ContactInformation),
        Description = model.Description,
        ImageUrls = CreateImageReferenes(model.ImageReferences, baseRessourcePath),
        Location = LocationDto.FromModel(model.Location)
      };
    }

    public static GetIncidentReportDto FromEntity(IncidentReportEntity entity, MainCategory mainCategoryOptionMatch, Uri baseRessourcePath) {
      ChosenMainCategory chosenMainCategory = mainCategoryOptionMatch.ToChosenMainCategory(entity.ChosenMainCategoryEntity?.SubCategoryId);

      return new GetIncidentReportDto {
        Id = entity.Id,
        Status = entity.Status,
        ChosenMainCategory = GetChosenMainCategoryDto.FromModel(chosenMainCategory),
        TimestampCreatedUtc = entity.TimestampCreatedUtc,
        TimestampModifiedUtc = entity.TimestampLastModifiedUtc,
        AssignedTeam = entity.AssignedTeam != null ? new GetWorkerTeamDto {
          Id = entity.AssignedTeam.Id,
          Name = entity.AssignedTeam.Name
        } : null,
        ContactInformation = entity.ContactInformation != null ? new ContactInformationDto {
          Email = entity.ContactInformation.Email,
          Name = entity.ContactInformation.Name,
          PhoneNumber = entity.ContactInformation.PhoneNumber,
        } : null,
        Description = entity.Description,
        ImageUrls = CreateImageReferenes(entity.ImageReferences, baseRessourcePath),
        Location = LocationDto.FromModel(entity.Location)
      };
    }

    private static List<string> CreateImageReferenes(List<ImageReference> imageReferences, Uri baseRessourcePath) {
      return imageReferences.Select(i => new Uri(baseRessourcePath, "images/" + i.Id.ToString()).ToString()).ToList();
    }
  }
}