using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Domain {

  public class WorkerTeam {
    public int Id { get; private set; } // Set by EF
    public string Name { get; set; }
    /// <summary>
    /// If the worker team has been deleted/archived.
    /// </summary>
    public bool IsArchived { get; set; }

    public WorkerTeam(string name, bool isArchived = false) {
      Name = name;
      IsArchived = isArchived;
    }
  }
}