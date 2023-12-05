using InciportWebService.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public class MunicipalityEntity {
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<WorkerTeam> WorkerTeams { get; set; }
    public virtual List<MainCategory> MainCategories { get; set; }
    public virtual List<IncidentReportEntity> IncidentReports { get; set; }
    public virtual List<ApplicationUser> Users { get; set; }

    public MunicipalityEntity() {
      WorkerTeams = new List<WorkerTeam>();
      MainCategories = new List<MainCategory>();
      IncidentReports = new List<IncidentReportEntity>();
      Users = new List<ApplicationUser>();
    }
  }
}