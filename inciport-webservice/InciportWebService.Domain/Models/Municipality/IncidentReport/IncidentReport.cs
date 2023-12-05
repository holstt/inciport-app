using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InciportWebService.Domain {

  public class IncidentReport {
    public int Id { get; set; }

    public ReportStatus Status { get; set; }
    public virtual ChosenMainCategory ChosenMainCategory { get; set; }
    public virtual Location Location { get; set; }
    public string Description { get; set; }
    public virtual ContactInformation ContactInformation { get; set; }
    public virtual WorkerTeam AssignedTeam { get; set; }
    public virtual List<ImageReference> ImageReferences { get; set; }
    public DateTimeOffset TimestampCreatedUtc { get; set; }
    public DateTimeOffset TimestampLastModifiedUtc { get; set; }

    public IncidentReport() {
      ImageReferences = new List<ImageReference>();
    }
  }
}