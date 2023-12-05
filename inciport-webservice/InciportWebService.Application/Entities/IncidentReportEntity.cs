using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class IncidentReportEntity {
    public const int ARCHIVE_AFTER_DAYS = 30;
    public int Id { get; set; }
    public ReportStatus Status { get; set; }

    public virtual ChosenMainCategoryEntity ChosenMainCategoryEntity { get; set; }
    public virtual Location Location { get; set; }

    public string Description { get; set; }

    public virtual List<ImageReference> ImageReferences { get; set; }
    public virtual WorkerTeam AssignedTeam { get; set; }
    public virtual ContactInformation ContactInformation { get; set; }

    // Get as UTC manually, as EF converts to local time at mapping. NB! This requires EF to not use private fields.
    private DateTimeOffset _timestampCreatedUtc;

    public DateTimeOffset TimestampCreatedUtc {
      get { return _timestampCreatedUtc; }
      set {
        _timestampCreatedUtc = value.UtcDateTime;
      }
    }

    private DateTimeOffset _timestampLastModifiedUtc;
    public DateTimeOffset TimestampLastModifiedUtc {
      get { return _timestampLastModifiedUtc; }
      set {
        _timestampLastModifiedUtc = value.UtcDateTime;
      }
    }

    public IncidentReportEntity() {
      ImageReferences = new List<ImageReference>();
    }

    public static IncidentReportEntity FromModel(IncidentReport model) {
      return new IncidentReportEntity {
        Id = model.Id,
        Status = model.Status,
        ChosenMainCategoryEntity = ChosenMainCategoryEntity.FromChosenCategory(model.ChosenMainCategory),
        Location = model.Location,
        Description = model.Description,
        ContactInformation = model.ContactInformation,
        ImageReferences = model.ImageReferences,
        AssignedTeam = model.AssignedTeam,
        TimestampCreatedUtc = model.TimestampCreatedUtc,
        TimestampLastModifiedUtc = model.TimestampLastModifiedUtc
      };
    }

    public IncidentReport ToModel(ChosenMainCategory chosenMainCategory) {
      return new IncidentReport {
        Id = Id,
        Status = Status,
        ChosenMainCategory = chosenMainCategory,
        TimestampCreatedUtc = TimestampCreatedUtc,
        TimestampLastModifiedUtc = TimestampLastModifiedUtc,
        AssignedTeam = AssignedTeam,
        ContactInformation = ContactInformation,
        Description = Description,
        ImageReferences = ImageReferences,
        Location = Location,
      };
    }

    public void ArchiveIfEligible(DateTimeOffset currentTime) {
      if (IsStatusArchivable() && IsMoreThanDaysSinceModified(ARCHIVE_AFTER_DAYS, currentTime)) {
        Archive();
      }
    }

    private bool IsMoreThanDaysSinceModified(int days, DateTimeOffset currentTime) {
      TimeSpan daysOld = currentTime - TimestampLastModifiedUtc;
      return daysOld.TotalDays > days;
    }

    /// <summary>
    /// Archives the report.
    /// </summary>
    private void Archive() {
      Console.WriteLine("Archiving incident report " + Id);
      Status = ReportStatus.Archived;
    }

    private bool IsStatusArchivable() => Status == ReportStatus.Completed || Status == ReportStatus.Rejected;
  }
}