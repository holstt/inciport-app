using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class Municipality {
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<WorkerTeam> WorkerTeams { get; set; }
    public virtual List<MainCategory> MainCategory { get; set; }
    public virtual List<IncidentReport> IncidentReports { get; set; }
    public virtual List<ApplicationUser> Users { get; set; }
  }
}